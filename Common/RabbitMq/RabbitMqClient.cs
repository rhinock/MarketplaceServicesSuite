using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;

namespace Common.RabbitMq;

public class RabbitMqClient
{
    private const string Exchange = "item_exchange";
    private const string ItemUpdateQueue = "item_update_queue";
    private const string ItemUpdateRoutingKey = "item_update_routing_key";
    private const string ItemDeleteQueue = "item_delete_queue";
    private const string ItemDeleteRoutingKey = "item_delete_routing_key";
    private const string DeadLetterSuffix = "_dead_letter";
    private const string RetryDelayHeader = "x-delay-in-ms";
    private readonly string _deadLetterExchange;
    private readonly string _deadLetterItemUpdateQueue;
    private readonly string _deadLetterItemDeleteQueue;
    private IModel _channel;
    private readonly ILogger<RabbitMqClient> _logger;
    private readonly RabbitMqConfiguration _configuration;

    public RabbitMqClient(IOptions<RabbitMqConfiguration> options, ILogger<RabbitMqClient> logger)
    {
        _configuration = options.Value;
        ArgumentNullException.ThrowIfNull(_configuration, nameof(_configuration));
        ArgumentException.ThrowIfNullOrWhiteSpace(_configuration.HostName, nameof(_configuration.HostName));
        ArgumentNullException.ThrowIfNull(_configuration.Port, nameof(_configuration.Port));
        ArgumentException.ThrowIfNullOrWhiteSpace(_configuration.UserName, nameof(_configuration.UserName));
        ArgumentException.ThrowIfNullOrWhiteSpace(_configuration.Password, nameof(_configuration.Password));

        _logger = logger;

        var policy = Policy
            .Handle<BrokerUnreachableException>()
            .WaitAndRetryForever(
                retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                (Exception exception, TimeSpan timeSpan) =>
                {
                    _logger.LogWarning(
                        exception,
                        "RabbitMq connection failed. Retrying in {seconds} seconds",
                        timeSpan.TotalSeconds.ToString());
                }
            );

        policy.Execute(() => Connect(_configuration));

        _deadLetterExchange = $"{Exchange}{DeadLetterSuffix}";
        _deadLetterItemUpdateQueue = $"{ItemUpdateQueue}{DeadLetterSuffix}";
        _deadLetterItemDeleteQueue = $"{ItemDeleteQueue}{DeadLetterSuffix}";

        SetupExchangeAndQueues();
        SetupDeadLetterExchangeAndQueues();
    }

    public void Publish(ItemUpdateMessage message)
    {
        var messageString = JsonSerializer.Serialize(message);
        SendMessage(ItemUpdateRoutingKey, messageString);
        LogPublishInformation(message, messageString);
    }

    public void Publish(ItemDeleteMessage message)
    {
        var messageString = JsonSerializer.Serialize(message);
        SendMessage(ItemDeleteRoutingKey, messageString);
        LogPublishInformation(message, messageString);
    }

    public void Consume(Action<ItemUpdateMessage> onReceived)
    {
        ConsumeMessage(ItemUpdateQueue, onReceived);
    }

    public void ConsumeDeadLetter(Action<ItemUpdateMessage> onReceived)
    {
        ConsumeMessage(_deadLetterItemUpdateQueue, onReceived);
    }

    public void Consume(Action<ItemDeleteMessage> onReceived)
    {
        ConsumeMessage(ItemDeleteQueue, onReceived);
    }

    public void ConsumeDeadLetter(Action<ItemDeleteMessage> onReceived)
    {
        ConsumeMessage(_deadLetterItemDeleteQueue, onReceived);
    }

    private void Connect(RabbitMqConfiguration configuration)
    {
        var factory = new ConnectionFactory
        {
            HostName = configuration.HostName,
            Port = configuration.Port,
            UserName = configuration.UserName,
            Password = configuration.Password,
        };

        var connection = factory.CreateConnection();
        _channel = connection.CreateModel();
    }

    private void SetupExchangeAndQueues()
    {
        _channel.ExchangeDeclare(Exchange, ExchangeType.Direct, true);

        var args = new Dictionary<string, object>
        {
            { "x-dead-letter-exchange", _deadLetterExchange },
            { "x-message-ttl", 5000 }
        };

        _channel.QueueDeclare(ItemUpdateQueue, true, false, false, args);
        _channel.QueueBind(ItemUpdateQueue, Exchange, ItemUpdateRoutingKey, null);

        _channel.QueueDeclare(ItemDeleteQueue, true, false, false, args);
        _channel.QueueBind(ItemDeleteQueue, Exchange, ItemDeleteRoutingKey, null);
    }

    private void SetupDeadLetterExchangeAndQueues()
    {
        _channel.ExchangeDeclare(_deadLetterExchange, ExchangeType.Direct, true);

        _channel.QueueDeclare(_deadLetterItemUpdateQueue, true, false, false);
        _channel.QueueBind(_deadLetterItemUpdateQueue, _deadLetterExchange, ItemUpdateRoutingKey, null);

        _channel.QueueDeclare(_deadLetterItemDeleteQueue, true, false, false);
        _channel.QueueBind(_deadLetterItemDeleteQueue, _deadLetterExchange, ItemDeleteRoutingKey, null);
    }

    private void SendMessage(string routingKey, string message)
    {
        var body = Encoding.UTF8.GetBytes(message);
        _channel.BasicPublish(Exchange, routingKey, null, body);
    }

    private void ConsumeMessage<T>(string queue, Action<T> onReceived)
    {
        var consumer = new EventingBasicConsumer(_channel);

        consumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var messageString = Encoding.UTF8.GetString(body);

            try
            {
                var message = JsonSerializer.Deserialize<T>(messageString);
                ArgumentNullException.ThrowIfNull(message, nameof(message));

                onReceived(message);
                _channel.BasicAck(ea.DeliveryTag, false);

                _logger.LogInformation(
                    "Successfully consumed {messageType} from {queue}: {message}",
                    typeof(T).Name, queue, messageString);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to process {messageType}: {message}", typeof(T).Name, messageString);

                var properties = _channel.CreateBasicProperties();
                properties.Headers = new Dictionary<string, object>();

                if (ea.BasicProperties.Headers.TryGetValue(RetryDelayHeader, out var value))
                {
                    properties.Headers[RetryDelayHeader] = (long)value * 2;
                }
                else
                {
                    properties.Headers[RetryDelayHeader] = 1000;
                }

                _channel.BasicPublish(_deadLetterExchange, queue, properties, body);
                _channel.BasicNack(ea.DeliveryTag, false, false);
            }
        };

        _channel.BasicConsume(queue, false, consumer);
    }

    private void LogPublishInformation<T>(T message, string messageString)
    {
        _logger.LogInformation("Successfully published {messageType}: {message}", typeof(T).Name, messageString);
    }
}

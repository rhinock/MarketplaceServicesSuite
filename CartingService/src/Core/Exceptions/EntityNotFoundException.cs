namespace Carting.Core.Exceptions;

public class EntityNotFoundException(string message) : Exception(message)
{
}
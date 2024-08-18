namespace Carting.Core.Exceptions;

public class EntityExistsException(string message) : Exception(message)
{
}
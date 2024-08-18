namespace Catalog.Core.Exceptions;

public class EntityExistsException(string message) : Exception(message)
{
}
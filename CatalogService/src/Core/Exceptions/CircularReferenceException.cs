namespace Catalog.Core.Exceptions;

public class CircularReferenceException(string message) : Exception(message)
{
}
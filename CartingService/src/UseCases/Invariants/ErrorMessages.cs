namespace Carting.UseCases.Invariants;

public static class ErrorMessages
{
    public static string CartNotFound => "Cart with Id = {0} not found";
    public static string ItemExists => "Item with Id = {0} already exists";
    public static string ItemNotFound => "Item with Id = {0} not found";
    public static string InvalidIdFormat => "Cart Id should be in GUID format";
}
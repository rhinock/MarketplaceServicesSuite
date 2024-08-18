namespace Catalog.UseCases.Invariants;

public static class ErrorMessages
{
    public static string CategoryExists => "Category with Id = {0} already exists";
    public static string CategoryNotFound => "Category with Id = {0} not found";
    public static string CategoryCircularReference => "Category circular reference error: Id = {0}, ParentId = {1}";
    public static string ItemExists => "Item with Id = {0} already exists";
    public static string ItemNotFound => "Item with Id = {0} not found";
}
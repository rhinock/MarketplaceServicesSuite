using MongoDB.Bson;

namespace Carting.UseCases.Helpers;

public static class ObjectIdHelper
{
    public static bool IsValidObjectId(string objectId)
    {
        return ObjectId.TryParse(objectId, out _);
    }
}
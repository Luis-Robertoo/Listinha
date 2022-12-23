using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace ToDoAPI.Models
{
    public class TodoList
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public string Titulo { get; set; }
        public string Conteudo { get; set; }
    }
}

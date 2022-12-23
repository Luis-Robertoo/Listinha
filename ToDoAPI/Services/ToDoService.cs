using Microsoft.Extensions.Options;
using MongoDB.Driver;
using ToDoAPI.Models;

namespace ToDoAPI.Services
{
    public class ToDoService
    {
        private readonly IMongoCollection<TodoList> _ToDoCollection;
        public ToDoService(
        IOptions<ToDoDBSettings> toDoDBSettings)
        {
            var mongoClient = new MongoClient(
                toDoDBSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                toDoDBSettings.Value.DatabaseName);

            _ToDoCollection = mongoDatabase.GetCollection<TodoList>(
                toDoDBSettings.Value.ToDoCollectionName);
        }

        public async Task<List<TodoList>> GetAsync() =>
        await _ToDoCollection.Find(_ => true).ToListAsync();

        public async Task<TodoList?> GetAsync(string id) =>
            await _ToDoCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(TodoList newToDo) =>
            await _ToDoCollection.InsertOneAsync(newToDo);

        public async Task UpdateAsync(string id, TodoList updatedToDo) =>
            await _ToDoCollection.ReplaceOneAsync(x => x.Id == id, updatedToDo);

        public async Task RemoveAsync(string id) =>
            await _ToDoCollection.DeleteOneAsync(x => x.Id == id);
    }
}


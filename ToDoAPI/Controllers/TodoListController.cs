using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using ToDoAPI.Domain;
using ToDoAPI.Models;
using ToDoAPI.Services;

namespace ToDoAPI.Controllers
{
    [ApiController]
    [Route("api/v1/todo")]
    public class TodoListController : ControllerBase
    {
        private readonly ToDoService _toDoService;
        private readonly string mensagemError = $"Não foi encontrado essa nota!!";

        public TodoListController(ToDoService toDoService) =>
            _toDoService = toDoService;

        [HttpGet]
        public async Task<List<TodoList>> Get() =>
            await _toDoService.GetAsync();

        [HttpGet("{id}")]
        public async Task<ActionResult<TodoList>> Get(string id)
        {
            var todo = await _toDoService.GetAsync(id);

            if (todo is null)
            {
                return NotFound(mensagemError);
            }

            return Ok(todo);
        }

        [HttpPost]
        public async Task<IActionResult> Post(
            [FromBody]TodoList newToDo)
        {
            await _toDoService.CreateAsync(newToDo);

            return CreatedAtAction(nameof(Get), new { Id = newToDo.Id }, newToDo);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, 
            [FromBody] TodoList updatedToDo)
        {
            var todo = await _toDoService.GetAsync(id);

            if (todo is null)
            {
                return NotFound(mensagemError);
            }

            updatedToDo.Id = todo.Id;

            await _toDoService.UpdateAsync(id, updatedToDo);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var todo = await _toDoService.GetAsync(id);

            if (todo is null)
            {
                return NotFound(mensagemError);
            }

            await _toDoService.RemoveAsync(id);

            return NoContent();
        }


        /*

        [HttpGet()]
        public List<TodoList> Get()
        {
            var client = getClient();
            var database = client.GetDatabase("tododb");
            var collection = database.GetCollection<TodoList>("todo");

            var dados = collection.Find(new BsonDocument()).ToList();

            return dados;

            throw new ArgumentNullException("deu merda");
        }

        [HttpPost()]
        public TodoList Post(TodoListDBO request)
        {
            var client = getClient();
            var database = client.GetDatabase("tododb");
            var collection = database.GetCollection<TodoList>("todo");

            var datas = new TodoList()
            {
                _id = Guid.NewGuid().ToString(),
                Titulo = request.Titulo,
                Conteudo = request.Conteudo
                
            };
            
            collection.InsertOneAsync(datas);


            return datas;
        }


        public MongoClient getClient()
        {

            var settings = MongoClientSettings.FromConnectionString("mongodb+srv://user-supremo-42:PamonhaDoce42@todo.ve60tfe.mongodb.net/?retryWrites=true&w=majority");
            settings.ServerApi = new ServerApi(ServerApiVersion.V1);
            var client = new MongoClient(settings);

            return client;

            //var database = client.GetDatabase("tododb");
            //var collection = database.GetCollection<TodoList>("todo");
            //var dados = collection.Find(new BsonDocument()).FirstOrDefault();
            //return dados.ToJson();
        }*/
    }
}

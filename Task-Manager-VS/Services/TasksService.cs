using Task_Manager_VS.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Task_Manager_VS.Services;

public class TasksService
{
    private readonly IMongoCollection<Models.Task> _tasksCollection;

    public TasksService(
        IOptions<TasksDatabaseSettings> tasksDatabaseSettings)
    {
        var mongoClient = new MongoClient(
            tasksDatabaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            tasksDatabaseSettings.Value.DatabaseName);

        _tasksCollection = mongoDatabase.GetCollection<Models.Task>(
            tasksDatabaseSettings.Value.TasksCollectionName);
    }

    public async Task<List<Models.Task>> GetAsync() =>
        await _tasksCollection.Find(_ => true).ToListAsync();

    public async Task<Models.Task?> GetAsync(string id) =>
        await _tasksCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async System.Threading.Tasks.Task CreateAsync(Models.Task newBook) =>
        await _tasksCollection.InsertOneAsync(newBook);

    public async System.Threading.Tasks.Task UpdateAsync(string id, Models.Task updatedBook) =>
        await _tasksCollection.ReplaceOneAsync(x => x.Id == id, updatedBook);

    public async System.Threading.Tasks.Task RemoveAsync(string id) =>
        await _tasksCollection.DeleteOneAsync(x => x.Id == id);
}
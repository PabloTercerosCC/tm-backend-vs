using Task_Manager_VS.Models;
using Task_Manager_VS.Services;
using Microsoft.AspNetCore.Mvc;

namespace Task_Manager_VS.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    private readonly TasksService _tasksService;

    public TasksController(TasksService booksService) =>
        _tasksService = booksService;

    [HttpGet]
    public async Task<List<Task_Manager_VS.Models.Task>> Get() =>
        await _tasksService.GetAsync();

    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<Task_Manager_VS.Models.Task>> Get(string id)
    {
        var task = await _tasksService.GetAsync(id);

        if (task is null)
        {
            return NotFound();
        }

        return task;
    }

    [HttpPost]
    public async Task<IActionResult> Post(Task_Manager_VS.Models.Task newTask)
    {
        await _tasksService.CreateAsync(newTask);

        return CreatedAtAction(nameof(Get), new { id = newTask.Id }, newTask);
    }

    [HttpPut("{id:length(24)}")]
    public async Task<IActionResult> Update(string id, Task_Manager_VS.Models.Task updatedTask)
    {
        var task = await _tasksService.GetAsync(id);

        if (task is null)
        {
            return NotFound();
        }

        updatedTask.Id = task.Id;

        await _tasksService.UpdateAsync(id, updatedTask);

        return NoContent();
    }

    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> Delete(string id)
    {
        var task = await _tasksService.GetAsync(id);

        if (task is null)
        {
            return NotFound();
        }

        await _tasksService.RemoveAsync(id);

        return NoContent();
    }
}
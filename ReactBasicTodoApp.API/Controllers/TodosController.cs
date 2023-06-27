using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReactBasicTodoApp.API.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class TodosController : ControllerBase
{
	private readonly TodoDbContext _dbContext;

	public TodosController(TodoDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	[HttpGet]
	public async Task<ActionResult<IEnumerable<Todo>>> GetTodos()
	{
		return await _dbContext.Todos.ToListAsync();
	}

	[HttpGet("count")]
	public async Task<IActionResult> GetCount()
	{
		return Ok(await _dbContext.Todos.LongCountAsync());
	}

	[HttpGet("{id}")]
	public async Task<ActionResult<Todo>> GetTodoById(int id)
	{
		var todo = await _dbContext.Todos.FindAsync(id);
		if (todo == null)
		{
			return NotFound();
		}
		return todo;
	}

	[HttpPost]
	public async Task<IActionResult> CreateTodo([FromBody] Todo todo)
	{
		_dbContext.Todos.Add(todo);
		await _dbContext.SaveChangesAsync();

		return CreatedAtAction(nameof(GetTodoById), new { id = todo.Id }, todo);
	}

	[HttpPut("{id}")]
	public async Task<IActionResult> UpdateTodo(int id, [FromBody] Todo updatedTodo)
	{
		var todo = await _dbContext.Todos.FindAsync(id);
		if (todo == null)
		{
			return NotFound();
		}

		todo.Title = updatedTodo.Title;
		todo.IsCompleted = updatedTodo.IsCompleted;

		await _dbContext.SaveChangesAsync();

		return NoContent();
	}

	[HttpPut("complete/{id}")]
	public async Task<IActionResult> Complete(int id)
	{
		var todo = await _dbContext.Todos.FindAsync(id);
		if (todo == null)
		{
			return NotFound();
		}

		todo.IsCompleted = !todo.IsCompleted;

		await _dbContext.SaveChangesAsync();

		return NoContent();
	}

	[HttpDelete("{id}")]
	public async Task<IActionResult> DeleteTodo(int id)
	{
		var todo = await _dbContext.Todos.FindAsync(id);
		if (todo == null)
		{
			return NotFound();
		}

		_dbContext.Todos.Remove(todo);
		await _dbContext.SaveChangesAsync();

		return NoContent();
	}
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MyTodoApp.Models;

namespace MyTodoApp.Pages
{
  public class IndexModel : PageModel
  {
    private readonly AppDbContext _context;

    public IndexModel(AppDbContext context)
    {
      _context = context;
    }

    public List<TodoItem> TodoItems { get; set; } = new();
    public string NewTaskTitle { get; set; } = string.Empty;
    public async Task OnGetAsync()
    {
      TodoItems = await _context.TodoItems.OrderByDescending(task => task.CreatedAt).ToListAsync();
    }
    public async Task<IActionResult> OnPostCreateTaskAsync(string taskName)
    {
      if (string.IsNullOrWhiteSpace(taskName))
      {
        return BadRequest("Název úkolu nesmí být prázdný.");
      }

      var newItem = new TodoItem
      {
        Title = taskName,
        isDone = false,
        CreatedAt = DateTime.UtcNow,
      };

      _context.TodoItems.Add(newItem);
      await _context.SaveChangesAsync();

      return new JsonResult(new { id = newItem.Id, name = newItem.Title });
    }

    public async Task<IActionResult> OnPostUpdateStatusAsync(int id, bool isDone)
    {
      var todoItem = await _context.TodoItems.FindAsync(id);

      if (todoItem == null)
      {
        return NotFound("Úkol s tímto ID nebyl v databázi nalezen.");
      }

      todoItem.isDone = isDone;
      await _context.SaveChangesAsync();

      return new JsonResult(new { success = true });
    }
  }
}
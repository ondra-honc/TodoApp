using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MyTodoApp.Pages
{
  public class IndexModel : PageModel
  {
    public void OnGet()
    {
    }

    public IActionResult OnPostCreateTask()
    {
      string taskName = Request.Form["taskName"];

      if (string.IsNullOrWhiteSpace(taskName))
      {
        return BadRequest("Název úkolu nesmí být prázdný.");
      }

      var newTask = new { name = taskName };
      return new JsonResult(newTask);
    }
  }
}
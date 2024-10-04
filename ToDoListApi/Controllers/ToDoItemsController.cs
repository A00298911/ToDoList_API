using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoListApi.Data;
using TodoListApi.Models;

namespace TodoListApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToDoItemsController : ControllerBase
    {
        private readonly DataContext _context;

        public ToDoItemsController(DataContext context)
        {
            _context = context;
        }

        // Get all ToDoItems without CompletedDate
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ToDoItem>>> GetIncompleteItems()
        {
            return await _context.ToDoItems.Where(item => item.CompletedDate == null).ToListAsync();
        }

        // Get ToDoItem by Id
        [HttpGet("{id}")]
        public async Task<ActionResult<ToDoItem>> GetToDoItem(int id)
        {
            var item = await _context.ToDoItems.FindAsync(id);

            if (item == null)
            {
                return NotFound();
            }

            return item;
        }

        // Post a new ToDoItem
        [HttpPost]
        public async Task<ActionResult<ToDoItem>> CreateToDoItem(ToDoItem newItem)
        {
            _context.ToDoItems.Add(newItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetToDoItem), new { id = newItem.Id }, newItem);
        }

        // Put (Update) ToDoItem by Id, marking it as complete
        [HttpPut("{id}")]
        public async Task<IActionResult> MarkAsComplete(int id, ToDoItem updatedItem)
        {
            var existingItem = await _context.ToDoItems.FindAsync(id);

            if (existingItem == null)
            {
                return NotFound();
            }

            existingItem.IsCompleted = true;
            existingItem.CompletedDate = DateTime.Now;

            _context.Entry(existingItem).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Delete ToDoItem by Id
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteToDoItem(int id)
        {
            var item = await _context.ToDoItems.FindAsync(id);

            if (item == null)
            {
                return NotFound();
            }

            _context.ToDoItems.Remove(item);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}

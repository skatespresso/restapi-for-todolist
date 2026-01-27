using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using todoApi.Models;

namespace todoApi.Controllers
{
    [Route("api/todoitem")]
    [ApiController]
    public class TodoItemController : ControllerBase
    {
        private readonly TodoDbContext _context;

        public TodoItemController (TodoDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task <ActionResult<IEnumerable<TodoItem>>> GetTodoItems()
        {
            return await _context.TodoItems.Where(i => !i.IsDeleted).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItem>> GetTodoItemById(long id)
        {
            var todoItem = await _context.TodoItems.Where(i => !i.IsDeleted).FirstOrDefaultAsync(t => t.Id == id);

            if (todoItem == null)
            {
                return NotFound();
            }

            return todoItem;
        }


        [HttpPost]
        public async Task<ActionResult<TodoItem>> PostTodoItem(TodoItem todoItem)
        {
            _context.TodoItems.Add(todoItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTodoItemById", new {id = todoItem.Id} , todoItem); 
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutTodoItem(long id, TodoItem todoItem)
        {
            if (id!=todoItem.Id)
            {
                return BadRequest("Något gick fel.");
            }
            _context.Entry(todoItem).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(todoItem);
        }

        public class PatchIsDoneDto
        {
            public bool IsDone {get; set;}
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchTodoItemDone(long id, [FromBody] PatchIsDoneDto patchDto)
        {
            var todoItem = await _context.TodoItems.FindAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            todoItem.IsDone = patchDto.IsDone;
            await _context.SaveChangesAsync();

            return Ok(todoItem);
        }

        [HttpDelete ("{id}")]
        public async Task<IActionResult> DeleteTodoItem(long id)
        {
            var todoItem = await _context.TodoItems.FindAsync(id);
           
            if (todoItem == null)
            {
                return NotFound();
            }

            todoItem.IsDeleted = true;

           // _context.TodoItems.Remove(todoItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

       
    }
}

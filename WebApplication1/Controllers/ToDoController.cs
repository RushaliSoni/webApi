using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToDoController : ControllerBase
    {
        private readonly TodoContext context;

        public ToDoController(TodoContext context)
        {
            this.context = context;
            if (context.TodoItems.Count() == 0)
            {
                context.TodoItems.Add(new TodoItem { Name = "Item1" });
                context.SaveChanges();

            }

        }
        //Get : api/ToDo
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItem>>> GetToDoitems()
        {
            return await context.TodoItems.ToListAsync();
        }

        //Get : api/ToDo/7

        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItem>> GetToDoitem(long id)
        {
            var todoitem = await context.TodoItems.FindAsync(id);
            if (todoitem == null)
            {
                return NotFound();

            }
            return todoitem;
        }

        //Post : api/Todo
        [HttpPost]

        public async Task<ActionResult<TodoItem>> PostToDoitem(TodoItem item)
        {
            // add new item
            context.TodoItems.Add(item);
            // save changes 
            await context.SaveChangesAsync();
            // pass three parmeter  first action , route value id and object
            // create new item

            return CreatedAtAction(nameof(GetToDoitem), new { id = item.Id }, item);
        }

        // PUT: api/Todo/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTodoItem(long id, TodoItem item)
        {
            if (id != item.Id)
            {
                return BadRequest();
            }

            context.Entry(item).State = EntityState.Modified;
            await context.SaveChangesAsync();

            return NoContent();
        }
        //Delete : api/todo/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteToDoItem(long id)
        {
            var todoitem = await context.TodoItems.FindAsync(id);
            if (todoitem == null)
            {
                return NotFound();
            }
            context.TodoItems.Remove(todoitem);
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}

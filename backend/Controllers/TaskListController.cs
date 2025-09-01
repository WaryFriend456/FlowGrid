using backend.Data;
using backend.Dtos;
using backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace backend.Controllers
{
    [Route("/api/board/{boardId}/tasklist")]
    [ApiController]
    [Authorize]
    public class TaskListController(ApplicationDbContext context, UserManager<AppUser> userManager) : ControllerBase
    {
        private readonly ApplicationDbContext _context = context;
        private readonly UserManager<AppUser> _userManager = userManager;

        /// <summary>
        /// Gets all task lists for a specific board owned by the currently authenticated user.
        /// </summary>
        /// <param name="boardId">The ID of the board.</param>
        /// <returns>Returns a list of task lists for the specified board.</returns>
        /// <response code="200">Returns the list of task lists.</response>
        /// <response code="401">User is not authenticated</response>
        /// <response code="403">User is not authorized to access this board.</response>
        /// <response code="404">Board not found.</response>
        [HttpGet]
        public async Task<IActionResult> GetTaskLists(int boardId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var board = await _context.Boards.FindAsync(boardId);

            if(board == null)
            {
                return NotFound(new { message = "Board not found" });
            }
            if(board.AppUserId != userId)
            {
                return Forbid();
            }
            //var lists = await _context.TaskLists
            //    .Where(l => l.BoardId == boardId)
            //    .ToListAsync();

            var taskList = await _context.TaskLists
                .Where(l => l.BoardId == boardId)
                .OrderBy(l => l.Order)
                .Select(l => new TaskListDto
                {
                    Id = l.Id,
                    Title = l.Title,
                    Order = l.Order,
                    BoardId = l.BoardId,
                    Cards = l.Cards.OrderBy(c => c.Order).Select(c => new CardDto
                    {
                        Id = c.Id,
                        Title = c.Title,
                        Description = c.Description,
                        Order = c.Order,
                        ListId = c.ListId
                    }).ToList()
                }).ToListAsync();

            return Ok(taskList);
        }

        /// <summary>
        /// Creates a new task list in a specific board owned by the currently authenticated user.
        /// </summary>
        /// <param name="boardId">ID of the board</param>
        /// <param name="listDto">Body of the request containing the task list details</param>
        /// <returns>Returns the created task list</returns>
        /// <response code="201">Returns the created task list</response>
        /// <response code="401">User is not authenticated</response>
        /// <response code="403">User is not authorized to access this board.</response>
        /// <response code="404">Board not found.</response>
        [HttpPost]
        public async Task<IActionResult> CreateTaskList(int boardId, [FromBody] CreateTaskListDto listDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var board = await _context.Boards.FindAsync(boardId);

            if(board == null)
            {
                return NotFound(new { message = "Board not found" });
            }

            if(board.AppUserId != userId)
            {
                return Forbid();
            }

            //var maxOrder = await _context.TaskLists
            //    .Where(l => l.BoardId == boardId)
            //    .Select(l => (int?)l.Order) // Use (int?) to allow for nulls (empty lists)
            //    .MaxAsync();

            var newOrder = await _context.TaskLists
                .CountAsync(l => l.BoardId == boardId);

            var taskList = new TaskList
            {
                Title = listDto.Title,
                BoardId = boardId,
                Order = newOrder
            };

            await _context.TaskLists.AddAsync(taskList);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTaskLists), new { boardId = board.Id }, new TaskListDto
            {
                Id = taskList.Id,
                Title = taskList.Title,
                BoardId = taskList.BoardId,
                Order = taskList.Order
            });
        }

        /// <summary>
        /// Updates an existing task list in a specific board owned by the currently authenticated user.
        /// </summary>
        /// <param name="boardId">The ID of the board.</param>
        /// <param name="listId">The ID of the task list.</param>
        /// <param name="listDto">The updated task list details.</param>
        /// <returns>No content since the task list was updated successfully.</returns>
        /// <response code="204">No content since the task list was updated successfully.</response>
        /// <response code="401">User is not authenticated</response>
        /// <response code="403">User is not authorized to access this board.</response>
        /// <response code="404">Board or task list not found.</response>
        [HttpPut("{listId}")]
        public async Task<IActionResult> UpdateTaskList(int boardId, int listId, [FromBody] CreateTaskListDto listDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var board = await _context.Boards.FindAsync(boardId);

            if(board == null)
            {
                return NotFound(new { message = "Board not found" });
            }

            if(board.AppUserId != userId)
            {
                return Forbid();
            }

            var taskList = await _context.TaskLists.FirstOrDefaultAsync(l => l.Id == listId && l.BoardId == boardId);

            if(taskList == null)
            {
                return NotFound(new { message = "Task list not found" });
            }

            taskList.Title = listDto.Title;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Deletes a task list from a specific board owned by the currently authenticated user.
        /// </summary>
        /// <param name="boardId">ID of the board</param>
        /// <param name="listId">ID of the task list</param>
        /// <returns>No content if the task list was deleted successfully</returns>
        /// <response code="204">No content if the task list was deleted successfully</response>
        /// <response code="401">User is not authenticated</response>
        /// <response code="403">User is not authorized to access this board.</response>
        /// <response code="404">Board or task list not found.</response>
        [HttpDelete("{listId}")]
        public async Task<IActionResult> DeleteTaskList(int boardId, int listId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var board = await _context.Boards.FindAsync(boardId);

            if(board == null)
            {
                return NotFound(new { message = "Board not found" });
            }

            if(board.AppUserId != userId)
            {
                return Forbid();
            }

            var taskList = await _context.TaskLists.FirstOrDefaultAsync(l => l.Id == listId && l.BoardId == board.Id);

            if(taskList == null)
            {
                return NotFound(new { message = "Task list not found" });
            }

            _context.TaskLists.Remove(taskList);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}

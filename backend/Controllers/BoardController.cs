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
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BoardController(ApplicationDbContext context, UserManager<AppUser> userManager) : ControllerBase
    {
        private readonly ApplicationDbContext _context = context;
        private readonly UserManager<AppUser> _userManager = userManager;

        /// <summary>
        /// Gets all boards owned by the currently authenticated user.
        /// </summary>
        /// <returns>A list of user's boards</returns>
        /// <response code="200">Returns the list of boards</response>
        /// <response code="401">If the user is not authenticated</response>
        [HttpGet]
        public async Task<IActionResult> GetUserBoards()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if(userId == null)
            {
                return Unauthorized(new { message = "User not found" });
            }

            //var boards = await _context.Boards
            //    .Where(b => b.AppUserId == userId)
            //    .ToListAsync();

            var boards = await _context.Boards
                .Include(b => b.Lists)
                .ThenInclude(l => l.Cards)
                .Where(b => b.AppUserId == userId)
                .Select(b => new BoardDto
                {
                    Id = b.Id,
                    Title = b.Title,
                    Lists = b.Lists.Select(l => new TaskListDto
                    {
                        Id = l.Id,
                        Title = l.Title,
                        BoardId = l.BoardId,
                        Cards = l.Cards.Select(c => new CardDto
                        {
                            Id = c.Id,
                            Title = c.Title,
                            Description = c.Description,
                            ListId = c.ListId
                        }).ToList()
                    }).ToList()
                }).ToListAsync();

            return Ok(boards);
        }

        /// <summary>
        /// Creates a new board for the currently authenticated user.
        /// </summary>
        /// <param name="boardDto">The detail's for the new board</param>
        /// <returns>The newly created board</returns>
        /// <response code="201">Returns the created board</response>
        /// <response code="401">If the user is not authenticated</response>
        [HttpPost]
        public async Task<IActionResult> CreateBoard([FromBody] CreateBoardDto boardDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized(new { message = "User not found" });
            }

            var board = new Board
            {
                Title = boardDto.Title,
                AppUserId = userId // Associate the board with the current user
            };

            await _context.Boards.AddAsync(board);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUserBoards), new { id = board.Id }, board);
        }

        /// <summary>
        /// Updates an existing board owned by the currently authenticated user.
        /// </summary>
        /// <param name="id">Id of the board to update</param>
        /// <param name="boardDto">Updated board details</param>
        /// <returns>No Content if the update is successful</returns>
        /// <response code="204">No Content if the update is successful</response>
        /// <response code="401">User is not authenticated</response>
        /// <response code="403">Forbidden if the user is not the owner of the board</response>
        /// <response code="404">Board not found</response>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBoard(int id, [FromBody] UpdateBoardDto boardDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var board = await _context.Boards.FirstOrDefaultAsync(b => b.Id == id);

            if(board == null)
            {
                return NotFound(new { message = "Board not found" });
            }

            // Ensure the board belongs to the current user
            if (board.AppUserId != userId)
            {
                return Forbid(); // 403 Forbidden
            }

            board.Title = boardDto.Title;
            await _context.SaveChangesAsync();

            return NoContent(); // Standard 204 No Content for successful update (PUT)
        }

        /// <summary>
        /// Deletes an existing board owned by the currently authenticated user.
        /// </summary>
        /// <param name="id">The Id of the board to delete</param>
        /// <returns>No Content if the deletion is successful</returns>
        /// <response code="204">No Content if the deletion is successful</response>
        /// <response code="401">User is not authenticated</response>
        /// <response code="403">Forbidden if the user is not the owner of the board</response>
        /// <response code="404">Board not found</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBoard(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var board = await _context.Boards.FirstOrDefaultAsync(b => b.Id == id);

            // Check if the board exists
            if(board == null)
            {
                return NotFound(new {message = "Board not found" });
            }

            // Ensure the board belongs to the current user
            if(board.AppUserId != userId)
            {
                return Forbid(); // 403 Forbidden
            }

            _context.Boards.Remove(board);
            await _context.SaveChangesAsync();

            return NoContent(); // Standard 204 No Content for successful deletion
        }
    }
}

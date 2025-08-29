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
    [Route("/api/tasklist/{listId}/card")]
    [ApiController]
    [Authorize]
    public class CardController(ApplicationDbContext context, UserManager<AppUser> userManager) : ControllerBase
    {
        private readonly ApplicationDbContext _context = context;
        private readonly UserManager<AppUser> _userManager = userManager;

        /// <summary>
        /// Gets all cards for a specific task list owned by the currently authenticated user.
        /// </summary>
        /// <param name="listId">The ID of the task list.</param>
        /// <returns>A list of cards for the specified task list.</returns>
        /// <remarks>
        /// This endpoint requires authentication and authorization.
        /// </remarks>
        /// <response code="200">Returns a list of cards for the specified task list.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="403">If the user is not authorized to access the task list.</response>
        /// <response code="404">If the task list is not found.</response>
        [HttpGet]
        public async Task<IActionResult> GetCards(int listId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            // Verify the list exists and belongs to a board owned by the user
            var taskList = await _context.TaskLists.Include(l => l.Board)
                .FirstOrDefaultAsync(l => l.Id == listId);

            if (taskList == null)
            {
                return NotFound(new { message = "Task list not found" });
            }

            if (taskList.Board.AppUserId != userId)
            {
                return Forbid();
            }

            var cards = await _context.Cards
                .Where(c => c.ListId == listId)
                .Select(c => new CardDto
                {
                    Id = c.Id,
                    Title = c.Title,
                    Description = c.Description,
                    ListId = c.ListId
                })
                .ToListAsync();

            return Ok(cards);
        }

        /// <summary>
        /// Creates a new card in a specific task list owned by the currently authenticated user.
        /// </summary>
        /// <param name="listId">The ID of the task list.</param>
        /// <param name="cardDto">The data transfer object containing the card details.</param>
        /// <returns>A IActionResult representing the result of the operation.</returns>
        /// <remarks>
        /// This endpoint requires authentication and authorization.
        /// </remarks>
        /// <response code="201">Returns the created card.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="403">If the user is not authorized to access the task list.</response>
        /// <response code="404">If the task list is not found.</response>
        [HttpPost]
        public async Task<IActionResult> CreateCard(int listId, [FromBody] CreateCardDto cardDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            // Verify the list exists and belongs to a board owned by the user
            var taskList = await _context.TaskLists.Include(l => l.Board)
                .FirstOrDefaultAsync(l => l.Id == listId);

            if (taskList == null)
            {
                return NotFound(new { message = "Task list not found" });
            }

            if (taskList.Board.AppUserId != userId)
            {
                return Forbid();
            }

            var card = new Card
            {
                Title = cardDto.Title,
                Description = cardDto.Description,
                ListId = listId
            };

            await _context.Cards.AddAsync(card);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCards), new { listId = taskList.Id }, new CardDto
            {
                Id = card.Id,
                Title = card.Title,
                Description = card.Description,
                ListId = card.ListId
            });
        }

        /// <summary>
        /// Updates an existing card in a specific task list owned by the currently authenticated user.
        /// </summary>
        /// <param name="listId">The ID of the task list.</param>
        /// <param name="cardId">The ID of the card to update.</param>
        /// <param name="cardDto">The data transfer object containing the updated card details.</param>
        /// <returns>A IActionResult representing the result of the operation.</returns>
        /// <remarks>
        /// This endpoint requires authentication and authorization.
        /// </remarks>
        /// <response code="204">If the update is successful.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="403">If the user is not authorized to access the task list.</response>
        /// <response code="404">If the task list or card is not found.</response>
        [HttpPut("{cardId}")]
        public async Task<IActionResult> UpdateCard(int listId, int cardId, [FromBody] UpdateCardDto cardDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            // Verify the list exists and belongs to a board owned by the user
            var taskList = await _context.TaskLists.Include(l => l.Board)
                .FirstOrDefaultAsync(l => l.Id == listId);

            if (taskList == null)
            {
                return NotFound(new { message = "Task list not found" });
            }

            if (taskList.Board.AppUserId != userId)
            {
                return Forbid();
            }

            var card = await _context.Cards.FirstOrDefaultAsync(c => c.Id == cardId && c.ListId == listId);

            if(card == null)
            {
                return NotFound(new { message = "Card not found" });
            }

            card.Title = cardDto.Title;
            card.Description = cardDto.Description;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Deletes a specific card from a task list owned by the currently authenticated user.
        /// </summary>
        /// <param name="listId">The ID of the task list.</param>
        /// <param name="cardId">The ID of the card to delete.</param>
        /// <returns>A IActionResult representing the result of the operation.</returns>
        /// <remarks>
        /// This endpoint requires authentication and authorization.
        /// </remarks>
        /// <response code="204">If the deletion is successful.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="403">If the user is not authorized to access the task list.</response>
        /// <response code="404">If the task list or card is not found.</response>
        [HttpDelete("{cardId}")]
        public async Task<IActionResult> DeleteCard(int listId, int cardId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            // Verify the list exists and belongs to a board owned by the user
            var taskList = await _context.TaskLists.Include(l => l.Board)
                .FirstOrDefaultAsync(l => l.Id == listId);

            if (taskList == null)
            {
                return NotFound(new { message = "Task list not found" });
            }

            if (taskList.Board.AppUserId != userId)
            {
                return Forbid();
            }

            var card = await _context.Cards.FirstOrDefaultAsync(c => c.Id == cardId && c.ListId == listId);

            if (card == null)
            {
                return NotFound(new { message = "Card not found" });
            }

            _context.Cards.Remove(card);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}

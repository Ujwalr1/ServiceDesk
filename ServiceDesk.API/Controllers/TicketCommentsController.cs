using Microsoft.AspNetCore.Mvc;
using ServiceDesk.API.DTOs.TicketComments;
using ServiceDesk.API.Services.Interfaces;

namespace ServiceDesk.API.Controllers
{
    /// <summary>
    /// Provides operations for managing comments associated with service desk tickets.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class TicketCommentsController : ControllerBase
    {
        private readonly ITicketCommentService _ticketCommentService;

        public TicketCommentsController(
            ITicketCommentService ticketCommentService)
        {
            _ticketCommentService = ticketCommentService;
        }

        /// <summary>
        /// Gets all comments associated with a specific ticket.
        /// </summary>
        /// <param name="ticketId">The ID of the ticket.</param>
        [HttpGet("ticket/{ticketId}")]
        [ProducesResponseType(
            StatusCodes.Status200OK,
            Type = typeof(IEnumerable<TicketCommentResponseDto>))]
        public async Task<ActionResult<IEnumerable<TicketCommentResponseDto>>> GetByTicketId(
            int ticketId)
        {
            var comments = await _ticketCommentService
                .GetByTicketIdAsync(ticketId);

            return Ok(comments);
        }

        /// <summary>
        /// Gets a comment by its ID.
        /// </summary>
        /// <param name="id">The comment ID.</param>
        [HttpGet("{id}")]
        [ProducesResponseType(
            StatusCodes.Status200OK,
            Type = typeof(TicketCommentResponseDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TicketCommentResponseDto>> GetById(int id)
        {
            var comment = await _ticketCommentService.GetByIdAsync(id);

            if (comment == null)
            {
                return NotFound();
            }

            return Ok(comment);
        }

        /// <summary>
        /// Creates a new comment for a ticket.
        /// </summary>
        /// <param name="dto">Comment creation information.</param>
        [HttpPost]
        [ProducesResponseType(
            StatusCodes.Status201Created,
            Type = typeof(TicketCommentResponseDto))]
        [ProducesResponseType(
            StatusCodes.Status400BadRequest,
            Type = typeof(ProblemDetails))]
        public async Task<ActionResult<TicketCommentResponseDto>> Create(
            TicketCommentCreateDto dto)
        {
            // Temporary until authentication is implemented
            int userId = 1;

            var comment = await _ticketCommentService.CreateAsync(
                dto,
                userId);

            return CreatedAtAction(
                nameof(GetById),
                new { id = comment.Id },
                comment);
        }

        /// <summary>
        /// Deletes a comment created by the current user.
        /// </summary>
        /// <param name="id">The comment ID.</param>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            // Temporary until authentication is implemented
            int userId = 1;

            var deleted = await _ticketCommentService.DeleteAsync(
                id,
                userId);

            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}

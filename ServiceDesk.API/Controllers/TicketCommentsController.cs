using Microsoft.AspNetCore.Mvc;
using ServiceDesk.API.DTOs.TicketComments;
using ServiceDesk.API.Services.Interfaces;

namespace ServiceDesk.API.Controllers
{
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

        [HttpGet("ticket/{ticketId}")]
        public async Task<ActionResult<IEnumerable<TicketCommentResponseDto>>> GetByTicketId(
            int ticketId)
        {
            var comments = await _ticketCommentService
                .GetByTicketIdAsync(ticketId);

            return Ok(comments);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TicketCommentResponseDto>> GetById(int id)
        {
            var comment = await _ticketCommentService.GetByIdAsync(id);

            if (comment == null)
            {
                return NotFound();
            }

            return Ok(comment);
        }

        [HttpPost]
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

        [HttpDelete("{id}")]
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

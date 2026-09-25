using Microsoft.AspNetCore.Mvc;
using ServiceDesk.API.DTOs.Tickets;
using ServiceDesk.API.Services.Interfaces;

namespace ServiceDesk.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TicketsController : ControllerBase
    {
        private readonly ITicketService _ticketService;

        public TicketsController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TicketResponseDto>>> GetAll()
        {
            var tickets = await _ticketService.GetAllAsync();

            return Ok(tickets);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TicketResponseDto>> GetById(int id)
        {
            var ticket = await _ticketService.GetByIdAsync(id);

            if (ticket == null)
            {
                return NotFound();
            }

            return Ok(ticket);
        }

        [HttpPost]
        public async Task<ActionResult<TicketResponseDto>> Create(
            TicketCreateDto dto)
        {
            // Temporary until authentication is implemented
            int createdByUserId = 1;

            var ticket = await _ticketService.CreateAsync(
                dto,
                createdByUserId);

            return CreatedAtAction(
                nameof(GetById),
                new { id = ticket.Id },
                ticket);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
            int id,
            TicketUpdateDto dto)
        {
            var updated = await _ticketService.UpdateAsync(id, dto);

            if (!updated)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}

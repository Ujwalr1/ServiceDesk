using Microsoft.AspNetCore.Mvc;
using ServiceDesk.API.DTOs;
using ServiceDesk.API.DTOs.Tickets;
using ServiceDesk.API.Services.Interfaces;

namespace ServiceDesk.API.Controllers
{
    /// <summary>
    /// Provides operations for managing service desk tickets.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class TicketsController : ControllerBase
    {
        private readonly ITicketService _ticketService;

        public TicketsController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        /// <summary>
        /// Gets a filtered and paginated list of service desk tickets.
        /// </summary>
        /// <param name="query">
        /// Filtering, searching, and pagination parameters.
        /// </param>
        [HttpGet]
        [ProducesResponseType(
            StatusCodes.Status200OK,
            Type = typeof(PagedResultDto<TicketResponseDto>))]
        public async Task<ActionResult<PagedResultDto<TicketResponseDto>>> GetAll([FromQuery] TicketQueryDto query)
        {
            var tickets = await _ticketService.GetAllAsync(query);

            return Ok(tickets);
        }

        /// <summary>
        /// Gets a service desk ticket by its ID.
        /// </summary>
        /// <param name="id">The ticket ID.</param>
        [HttpGet("{id}")]
        [ProducesResponseType(
            StatusCodes.Status200OK,
            Type = typeof(TicketResponseDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TicketResponseDto>> GetById(int id)
        {
            var ticket = await _ticketService.GetByIdAsync(id);

            if (ticket == null)
            {
                return NotFound();
            }

            return Ok(ticket);
        }

        /// <summary>
        /// Creates a new service desk ticket.
        /// </summary>
        /// <param name="dto">Ticket creation information.</param>
        [HttpPost]
        [ProducesResponseType(
            StatusCodes.Status201Created,
            Type = typeof(TicketResponseDto))]
        [ProducesResponseType(
            StatusCodes.Status400BadRequest,
            Type = typeof(ProblemDetails))]
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

        /// <summary>
        /// Updates an existing service desk ticket.
        /// </summary>
        /// <param name="id">The ticket ID.</param>
        /// <param name="dto">Updated ticket information.</param>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest,
            Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

using Microsoft.AspNetCore.Mvc;
using ServiceDesk.API.DTOs.TicketStatuses;
using ServiceDesk.API.Services.Interfaces;

namespace ServiceDesk.API.Controllers
{
    /// <summary>
    /// Provides operations for retrieving ticket statuses.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class TicketStatusesController : ControllerBase
    {
        private readonly ITicketStatusService _ticketStatusService;

        public TicketStatusesController(
            ITicketStatusService ticketStatusService)
        {
            _ticketStatusService = ticketStatusService;
        }

        /// <summary>
        /// Gets all available ticket statuses.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(
            StatusCodes.Status200OK,
            Type = typeof(IEnumerable<TicketStatusResponseDto>))]
        public async Task<ActionResult<IEnumerable<TicketStatusResponseDto>>> GetAll()
        {
            var statuses = await _ticketStatusService.GetAllAsync();

            return Ok(statuses);
        }

        /// <summary>
        /// Gets a ticket status by its ID.
        /// </summary>
        /// <param name="id">The ticket status ID.</param>
        [HttpGet("{id}")]
        [ProducesResponseType(
            StatusCodes.Status200OK,
            Type = typeof(TicketStatusResponseDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TicketStatusResponseDto>> GetById(int id)
        {
            var status = await _ticketStatusService.GetByIdAsync(id);

            if (status == null)
            {
                return NotFound();
            }

            return Ok(status);
        }
    }
}

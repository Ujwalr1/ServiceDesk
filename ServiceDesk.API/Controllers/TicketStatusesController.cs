using Microsoft.AspNetCore.Mvc;
using ServiceDesk.API.DTOs.TicketStatuses;
using ServiceDesk.API.Services.Interfaces;

namespace ServiceDesk.API.Controllers
{
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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TicketStatusResponseDto>>> GetAll()
        {
            var statuses = await _ticketStatusService.GetAllAsync();

            return Ok(statuses);
        }

        [HttpGet("{id}")]
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

using Microsoft.EntityFrameworkCore;
using ServiceDesk.API.DTOs.TicketStatuses;
using ServiceDesk.API.Services.Interfaces;
using ServiceDesk.Data.Data;

namespace ServiceDesk.API.Services
{
    public class TicketStatusService : ITicketStatusService
    {
        private readonly ServiceDeskDbContext _context;

        public TicketStatusService(ServiceDeskDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TicketStatusResponseDto>> GetAllAsync()
        {
            return await _context.TicketStatuses
                .Select(s => new TicketStatusResponseDto
                {
                    Id = s.Id,
                    Name = s.Name
                })
                .ToListAsync();
        }

        public async Task<TicketStatusResponseDto?> GetByIdAsync(int id)
        {
            return await _context.TicketStatuses
                .Where(s => s.Id == id)
                .Select(s => new TicketStatusResponseDto
                {
                    Id = s.Id,
                    Name = s.Name
                })
                .FirstOrDefaultAsync();
        }
    }
}

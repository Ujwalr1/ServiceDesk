using ServiceDesk.API.DTOs.TicketStatuses;

namespace ServiceDesk.API.Services.Interfaces
{
    public interface ITicketStatusService
    {
        Task<IEnumerable<TicketStatusResponseDto>> GetAllAsync();

        Task<TicketStatusResponseDto?> GetByIdAsync(int id);
    }
}

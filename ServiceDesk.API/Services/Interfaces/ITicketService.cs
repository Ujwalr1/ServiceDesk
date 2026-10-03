using ServiceDesk.API.DTOs;
using ServiceDesk.API.DTOs.Tickets;

namespace ServiceDesk.API.Services.Interfaces
{
    public interface ITicketService
    {
        Task<PagedResultDto<TicketResponseDto>> GetAllAsync(TicketQueryDto query);

        Task<TicketResponseDto?> GetByIdAsync(int id);

        Task<TicketResponseDto> CreateAsync(TicketCreateDto dto, int createdByUserId);

        Task<bool> UpdateAsync(int id, TicketUpdateDto dto);

        //Task<bool> DeleteAsync(int id);
    }
}

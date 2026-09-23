using ServiceDesk.API.DTOs.Tickets;

namespace ServiceDesk.API.Services.Interfaces
{
    public interface ITicketService
    {
        Task<IEnumerable<TicketResponseDto>> GetAllAsync();

        Task<TicketResponseDto?> GetByIdAsync(int id);

        Task<TicketResponseDto> CreateAsync(TicketCreateDto dto, int createdByUserId);

        Task<bool> UpdateAsync(int id, TicketUpdateDto dto);

        //Task<bool> DeleteAsync(int id);
    }
}

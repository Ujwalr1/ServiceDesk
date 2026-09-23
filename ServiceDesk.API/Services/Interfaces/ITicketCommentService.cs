using ServiceDesk.API.DTOs.TicketComments;

namespace ServiceDesk.API.Services.Interfaces
{
    public interface ITicketCommentService
    {
        Task<IEnumerable<TicketCommentResponseDto>> GetByTicketIdAsync(int ticketId);

        Task<TicketCommentResponseDto?> GetByIdAsync(int id);

        Task<TicketCommentResponseDto> CreateAsync(
            TicketCommentCreateDto dto,
            int userId);

        Task<bool> DeleteAsync(int id, int userId);
    }
}

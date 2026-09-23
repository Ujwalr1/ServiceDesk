using Microsoft.EntityFrameworkCore;
using ServiceDesk.API.DTOs.TicketComments;
using ServiceDesk.API.Services.Interfaces;
using ServiceDesk.Data.Data;
using ServiceDesk.Data.Entities;

namespace ServiceDesk.API.Services
{
    public class TicketCommentService : ITicketCommentService
    {
        private readonly ServiceDeskDbContext _context;

        public TicketCommentService(ServiceDeskDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TicketCommentResponseDto>> GetByTicketIdAsync(
            int ticketId)
        {
            return await _context.TicketComments
                .Where(c => c.TicketId == ticketId)
                .OrderBy(c => c.CreatedAt)
                .Select(c => new TicketCommentResponseDto
                {
                    Id = c.Id,
                    TicketId = c.TicketId,
                    UserId = c.UserId,
                    UserName = c.User.FullName,
                    CommentText = c.CommentText,
                    CreatedAt = c.CreatedAt
                })
                .ToListAsync();
        }

        public async Task<TicketCommentResponseDto?> GetByIdAsync(int id)
        {
            return await _context.TicketComments
                .Where(c => c.Id == id)
                .Select(c => new TicketCommentResponseDto
                {
                    Id = c.Id,
                    TicketId = c.TicketId,
                    UserId = c.UserId,
                    UserName = c.User.FullName,
                    CommentText = c.CommentText,
                    CreatedAt = c.CreatedAt
                })
                .FirstOrDefaultAsync();
        }

        public async Task<TicketCommentResponseDto> CreateAsync(
            TicketCommentCreateDto dto,
            int userId)
        {
            var comment = new TicketComment
            {
                TicketId = dto.TicketId,
                UserId = userId,
                CommentText = dto.CommentText,
                CreatedAt = DateTime.UtcNow
            };

            _context.TicketComments.Add(comment);

            await _context.SaveChangesAsync();

            var createdComment = await GetByIdAsync(comment.Id);

            return createdComment!;
        }

        public async Task<bool> DeleteAsync(int id, int userId)
        {
            var comment = await _context.TicketComments
                        .FirstOrDefaultAsync(c => c.Id == id);

            if (comment == null)
            {
                return false;
            }

            if (comment.UserId != userId)
            {
                return false;
            }

            _context.TicketComments.Remove(comment);

            await _context.SaveChangesAsync();

            return true;
        }
    }
}

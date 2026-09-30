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
        private readonly ILogger<TicketCommentService> _logger;

        public TicketCommentService(ServiceDeskDbContext context, ILogger<TicketCommentService> logger)
        {
            _context = context;
            _logger = logger;
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
            // Validate comment text
            if (string.IsNullOrWhiteSpace(dto.CommentText))
            {
                _logger.LogWarning(
                    "Comment creation failed. Comment text is empty. TicketId: {TicketId}, UserId: {UserId}",
                    dto.TicketId,
                    userId);

                throw new ArgumentException("Comment cannot be empty.");
            }

            // Validate ticket
            var ticketExists = await _context.Tickets
                .AnyAsync(t => t.Id == dto.TicketId);

            if (!ticketExists)
            {
                _logger.LogWarning(
                    "Comment creation failed. Ticket not found. TicketId: {TicketId}, UserId: {UserId}",
                    dto.TicketId,
                    userId);

                throw new ArgumentException("Invalid ticket.");
            }

            // Validate user
            var userExists = await _context.Users
                .AnyAsync(u => u.Id == userId && u.IsActive);

            if (!userExists)
            {
                _logger.LogWarning(
                    "Comment creation failed. User is invalid or inactive. UserId: {UserId}",
                    userId);

                throw new ArgumentException("Invalid or inactive user.");
            }

            var comment = new TicketComment
            {
                TicketId = dto.TicketId,
                UserId = userId,
                CommentText = dto.CommentText,
                CreatedAt = DateTime.UtcNow
            };

            _context.TicketComments.Add(comment);

            await _context.SaveChangesAsync();

            _logger.LogInformation(
                   "Comment created successfully. CommentId: {CommentId}, TicketId: {TicketId}, UserId: {UserId}",
                   comment.Id,
                   comment.TicketId,
                   comment.UserId);

            var createdComment = await GetByIdAsync(comment.Id);

            return createdComment!;
        }

        public async Task<bool> DeleteAsync(int id, int userId)
        {
            var comment = await _context.TicketComments
                        .FirstOrDefaultAsync(c => c.Id == id);

            if (comment == null)
            {
                _logger.LogWarning(
                    "Comment deletion failed. Comment not found. CommentId: {CommentId}, UserId: {UserId}",
                    id,
                    userId);

                return false;
            }

            if (comment.UserId != userId)
            {

                _logger.LogWarning(
                    "Comment deletion failed. User is not the comment owner. CommentId: {CommentId}, UserId: {UserId}",
                    id,
                    userId);

                return false;
            }

            _context.TicketComments.Remove(comment);

            await _context.SaveChangesAsync();

            _logger.LogInformation(
                "Comment deleted successfully. CommentId: {CommentId}, UserId: {UserId}",
                id,

            userId);
            return true;
        }
    }
}

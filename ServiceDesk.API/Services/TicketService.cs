using Microsoft.EntityFrameworkCore;
using ServiceDesk.API.DTOs.Tickets;
using ServiceDesk.API.Services.Interfaces;
using ServiceDesk.Data.Constants;
using ServiceDesk.Data.Data;
using ServiceDesk.Data.Entities;

namespace ServiceDesk.API.Services
{
    public class TicketService : ITicketService
    {
        private readonly ServiceDeskDbContext _context;

        public TicketService(ServiceDeskDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TicketResponseDto>> GetAllAsync()
        {
            return await _context.Tickets
                .Select(t => new TicketResponseDto
                {
                    Id = t.Id,
                    Title = t.Title,
                    Description = t.Description,

                    CreatedByUserId = t.CreatedByUserId,
                    CreatedByUserName = t.CreatedByUser.FullName,

                    AssignedToUserId = t.AssignedToUserId,
                    AssignedToUserName = t.AssignedToUser != null
                        ? t.AssignedToUser.FullName
                        : null,

                    CategoryId = t.CategoryId,
                    CategoryName = t.Category.Name,

                    StatusId = t.StatusId,
                    StatusName = t.Status.Name,

                    Priority = t.Priority,

                    CreatedAt = t.CreatedAt,
                    UpdatedAt = t.UpdatedAt
                })
                .ToListAsync();
        }


        public async Task<TicketResponseDto?> GetByIdAsync(int id)
        {
            return await _context.Tickets
                .Where(t => t.Id == id)
                .Select(t => new TicketResponseDto
                {
                    Id = t.Id,
                    Title = t.Title,
                    Description = t.Description,

                    CreatedByUserId = t.CreatedByUserId,
                    CreatedByUserName = t.CreatedByUser.FullName,

                    AssignedToUserId = t.AssignedToUserId,
                    AssignedToUserName = t.AssignedToUser != null
                        ? t.AssignedToUser.FullName
                        : null,

                    CategoryId = t.CategoryId,
                    CategoryName = t.Category.Name,

                    StatusId = t.StatusId,
                    StatusName = t.Status.Name,

                    Priority = t.Priority,

                    CreatedAt = t.CreatedAt,
                    UpdatedAt = t.UpdatedAt
                })
                .FirstOrDefaultAsync();
        }


        public async Task<TicketResponseDto> CreateAsync(
            TicketCreateDto dto,
            int createdByUserId)
        {
            var categoryExists = await _context.Categories
                    .AnyAsync(c => c.Id == dto.CategoryId && c.IsActive);

            if (!categoryExists)
            {
                // validation failure
                throw new ArgumentException("Invalid or inactive category.");
            }

            var ticket = new Ticket
            {
                Title = dto.Title,
                Description = dto.Description,

                CreatedByUserId = createdByUserId,

                CategoryId = dto.CategoryId,

                // New tickets start as Open
                // Update this later
                StatusId = TicketStatusIds.Open,

                Priority = dto.Priority,

                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Tickets.Add(ticket);

            await _context.SaveChangesAsync();

            var createdTicket = await GetByIdAsync(ticket.Id);

            return createdTicket!;

            //return await GetByIdAsync(ticket.Id);
        }


        public async Task<bool> UpdateAsync(
            int id,
            TicketUpdateDto dto)
        {
            var ticket = await _context.Tickets.FindAsync(id);

            if (ticket == null)
            {
                return false;
            }

            ticket.Title = dto.Title;
            ticket.Description = dto.Description;
            ticket.CategoryId = dto.CategoryId;
            ticket.AssignedToUserId = dto.AssignedToUserId;
            ticket.StatusId = dto.StatusId;
            ticket.Priority = dto.Priority;

            ticket.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return true;
        }


        //public async Task<bool> DeleteAsync(int id)
        //{
        //    var ticket = await _context.Tickets.FindAsync(id);

        //    if (ticket == null)
        //    {
        //        return false;
        //    }

        //    _context.Tickets.Remove(ticket);

        //    await _context.SaveChangesAsync();

        //    return true;
        //}
    }
}

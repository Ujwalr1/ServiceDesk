using Microsoft.EntityFrameworkCore;
using ServiceDesk.API.DTOs;
using ServiceDesk.API.DTOs.Tickets;
using ServiceDesk.API.Enums;
using ServiceDesk.API.Services.Interfaces;
using ServiceDesk.Data.Constants;
using ServiceDesk.Data.Data;
using ServiceDesk.Data.Entities;
using ServiceDesk.Data.Enums;

namespace ServiceDesk.API.Services
{
    public class TicketService : ITicketService
    {
        private readonly ServiceDeskDbContext _context;
        private readonly ILogger<TicketService> _logger;

        public TicketService(ServiceDeskDbContext context, ILogger<TicketService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<PagedResultDto<TicketResponseDto>> GetAllAsync(TicketQueryDto query)
        {
            if (query.Page < 1)
            {
                throw new ArgumentException(
                    "Page number must be greater than or equal to 1.");
            }

            if (query.StatusId.HasValue)
            {
                var statusExists = await _context.TicketStatuses
                    .AnyAsync(s => s.Id == query.StatusId.Value);

                if (!statusExists)
                {
                    throw new ArgumentException(
                        "Invalid ticket status.");
                }
            }

            if (query.CategoryId.HasValue)
            {
                var categoryExists = await _context.Categories
                    .AnyAsync(c =>
                        c.Id == query.CategoryId.Value &&
                        c.IsActive);

                if (!categoryExists)
                {
                    throw new ArgumentException(
                        "Invalid or inactive category.");
                }
            }

            if (query.AssignedToUserId.HasValue)
            {
                var userExists = await _context.Users
                    .AnyAsync(u =>
                        u.Id == query.AssignedToUserId.Value &&
                        u.IsActive);

                if (!userExists)
                {
                    throw new ArgumentException(
                        "Invalid or inactive assigned user.");
                }
            }

            if (query.Search?.Length > 200)
            {
                throw new ArgumentException(
                    "Search text cannot exceed 200 characters.");
            }

            IQueryable<Ticket> tickets = _context.Tickets.AsNoTracking();

            // Search
            if (!string.IsNullOrWhiteSpace(query.Search))
            {
                var search = query.Search.Trim();

                tickets = tickets.Where(t =>
                    t.Title.Contains(search) ||
                    t.Description.Contains(search));
            }

            // Filter by status
            if (query.StatusId.HasValue)
            {
                tickets = tickets.Where(t =>
                    t.StatusId == query.StatusId.Value);
            }

            // Filter by category
            if (query.CategoryId.HasValue)
            {
                tickets = tickets.Where(t =>
                    t.CategoryId == query.CategoryId.Value);
            }

            // Filter by priority
            if (query.Priority.HasValue)
            {
                tickets = tickets.Where(t =>
                    t.Priority == query.Priority.Value);
            }

            // Filter by assigned user
            if (query.AssignedToUserId.HasValue)
            {
                tickets = tickets.Where(t =>
                    t.AssignedToUserId == query.AssignedToUserId.Value);
            }

            // Apply sorting
            tickets = query.SortBy switch
            {
                TicketSortField.Title => query.SortDescending
                    ? tickets.OrderByDescending(t => t.Title)
                    : tickets.OrderBy(t => t.Title),

                TicketSortField.Priority => query.SortDescending
                    ? tickets.OrderByDescending(t => t.Priority)
                    : tickets.OrderBy(t => t.Priority),

                TicketSortField.Status => query.SortDescending
                    ? tickets.OrderByDescending(t => t.Status.Name)
                    : tickets.OrderBy(t => t.Status.Name),

                TicketSortField.Category => query.SortDescending
                    ? tickets.OrderByDescending(t => t.Category.Name)
                    : tickets.OrderBy(t => t.Category.Name),

                TicketSortField.CreatedAt => query.SortDescending
                    ? tickets.OrderByDescending(t => t.CreatedAt)
                    : tickets.OrderBy(t => t.CreatedAt),

                _ => throw new ArgumentException("Invalid sort field.")
            };

            // Total count BEFORE pagination
            var totalCount = await tickets.CountAsync();

            // Validate pagination
            var page = query.Page < 1
                ? 1
                : query.Page;

            const int pageSize = 10;

            // Calculate total pages
            var totalPages = (int)Math.Ceiling(
                totalCount / (double)pageSize);

            // Apply pagination and projection
            var items = await tickets
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
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

            return new PagedResultDto<TicketResponseDto>
            {
                Items = items,
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = totalPages
            };
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
            // Validate input text
            if (string.IsNullOrWhiteSpace(dto.Title))
            {
                throw new ArgumentException("Ticket title cannot be empty.");
            }

            if (string.IsNullOrWhiteSpace(dto.Description))
            {
                throw new ArgumentException("Ticket description cannot be empty.");
            }

            // Validate priority
            if (!Enum.IsDefined(typeof(TicketPriority), dto.Priority))
            {
                throw new ArgumentException("Invalid ticket priority.");
            }

            // Validate category
            var categoryExists = await _context.Categories
                .AnyAsync(c => c.Id == dto.CategoryId && c.IsActive);

            if (!categoryExists)
            {
                _logger.LogWarning(
                    "Ticket creation failed. Invalid or inactive CategoryId: {CategoryId}",
                    dto.CategoryId);

                throw new ArgumentException("Invalid or inactive category.");
            }

            // Validate creating user
            var userExists = await _context.Users
                .AnyAsync(u => u.Id == createdByUserId && u.IsActive);

            if (!userExists)
            {
                throw new ArgumentException("Invalid or inactive user.");
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

            _logger.LogInformation(
                    "Ticket created successfully. TicketId: {TicketId}, CreatedByUserId: {UserId}",
                    ticket.Id,
                    createdByUserId);

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

            // Validate input text
            if (string.IsNullOrWhiteSpace(dto.Title))
            {
                throw new ArgumentException("Ticket title cannot be empty.");
            }

            if (string.IsNullOrWhiteSpace(dto.Description))
            {
                throw new ArgumentException("Ticket description cannot be empty.");
            }

            // Validate priority
            if (!Enum.IsDefined(typeof(TicketPriority), dto.Priority))
            {
                throw new ArgumentException("Invalid ticket priority.");
            }

            // Validate category
            var categoryExists = await _context.Categories
                .AnyAsync(c => c.Id == dto.CategoryId && c.IsActive);

            if (!categoryExists)
            {
                _logger.LogWarning(
                    "Ticket update failed. Invalid or inactive CategoryId: {CategoryId}",
                    dto.CategoryId);

                throw new ArgumentException("Invalid or inactive category.");
            }

            // Validate status
            var statusExists = await _context.TicketStatuses
                .AnyAsync(s => s.Id == dto.StatusId);

            if (!statusExists)
            {
                throw new ArgumentException("Invalid ticket status.");
            }

            // Validate assigned user if one was provided
            if (dto.AssignedToUserId.HasValue)
            {
                var assignedUserExists = await _context.Users
                    .AnyAsync(u =>
                        u.Id == dto.AssignedToUserId.Value &&
                        u.IsActive);

                if (!assignedUserExists)
                {
                    throw new ArgumentException(
                        "Invalid or inactive assigned user.");
                }
            }

            ticket.Title = dto.Title;
            ticket.Description = dto.Description;
            ticket.CategoryId = dto.CategoryId;
            ticket.AssignedToUserId = dto.AssignedToUserId;
            ticket.StatusId = dto.StatusId;
            ticket.Priority = dto.Priority;

            ticket.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            _logger.LogInformation(
                "Ticket updated successfully. TicketId: {TicketId}",
                id);

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

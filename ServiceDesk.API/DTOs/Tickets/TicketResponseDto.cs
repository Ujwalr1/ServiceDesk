using ServiceDesk.Data.Enums;

namespace ServiceDesk.API.DTOs.Tickets
{
    public class TicketResponseDto
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public int CreatedByUserId { get; set; }

        public string CreatedByUserName { get; set; } = string.Empty;

        public int? AssignedToUserId { get; set; }

        public string AssignedToUserName { get; set; } = string.Empty;

        public int CategoryId { get; set; }

        public string CategoryName { get; set; } = string.Empty;

        public int StatusId { get; set; }

        public string StatusName { get; set; } = string.Empty;

        public TicketPriority Priority { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}

using ServiceDesk.API.Enums;
using ServiceDesk.Data.Enums;

namespace ServiceDesk.API.DTOs
{
    public class TicketQueryDto
    {
        public string? Search { get; set; } 

        public int? StatusId { get; set; }

        public int? CategoryId { get; set; }

        public TicketPriority? Priority { get; set; }

        public int? AssignedToUserId { get; set; }

        public int Page { get; set; } = 1;

        public TicketSortField SortBy { get; set; } = TicketSortField.CreatedAt;

        public bool SortDescending { get; set; } = true;
    }
}

using ServiceDesk.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace ServiceDesk.API.DTOs.Tickets
{
    public class TicketUpdateDto
    {
        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [MaxLength(5000)]
        public string Description { get; set; } = string.Empty;

        public int CategoryId { get; set; }

        public int? AssignedToUserId { get; set; }

        public int StatusId { get; set; }

        public TicketPriority Priority { get; set; }
    }
}

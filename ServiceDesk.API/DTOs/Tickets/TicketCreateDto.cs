using ServiceDesk.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace ServiceDesk.API.DTOs.Tickets
{
    public class TicketCreateDto
    {
        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [MaxLength(5000)]
        public string Description { get; set; } = string.Empty;

        [Required]
        public int CategoryId { get; set; }

        [Required]
        public TicketPriority Priority { get; set; }
    }
}

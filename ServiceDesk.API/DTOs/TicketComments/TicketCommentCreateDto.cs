using System.ComponentModel.DataAnnotations;

namespace ServiceDesk.API.DTOs.TicketComments
{
    public class TicketCommentCreateDto
    {
        [Required]
        public int TicketId { get; set; }

        [Required]
        [MaxLength(2000)]
        public string CommentText { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace ServiceDesk.API.DTOs.TicketComments
{
    public class TicketCommentUpdateDto
    {
        [Required]
        [MaxLength(2000)]
        public string CommentText { get; set; }
    }
}

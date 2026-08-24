namespace ServiceDesk.API.DTOs.TicketComments
{
    public class TicketCommentResponseDto
    {
        public int Id { get; set; }

        public int TicketId { get; set; }

        public int UserId { get; set; }

        public string UserName { get; set; }

        public string CommentText { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}

namespace ServiceDesk.API.DTOs.TicketComments
{
    public class TicketCommentCreateDto
    {
        public int TicketId { get; set; }

        public string CommentText { get; set; }
    }
}

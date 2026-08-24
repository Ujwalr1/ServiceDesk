namespace ServiceDesk.API.DTOs.Tickets
{
    public class TicketCreateDto
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public int CategoryId { get; set; }

        public string Priority { get; set; }
    }
}

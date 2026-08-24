namespace ServiceDesk.API.DTOs.Tickets
{
    public class TicketUpdateDto
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public int CategoryId { get; set; }

        public int? AssignedToUserId { get; set; }

        public int StatusId { get; set; }

        public string Priority { get; set; }
    }
}

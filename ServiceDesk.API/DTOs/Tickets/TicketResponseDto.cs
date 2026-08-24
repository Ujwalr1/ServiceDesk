namespace ServiceDesk.API.DTOs.Tickets
{
    public class TicketResponseDto
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public int CreatedByUserId { get; set; }

        public string CreatedByUserName { get; set; }

        public int? AssignedToUserId { get; set; }

        public string AssignedToUserName { get; set; }

        public int CategoryId { get; set; }

        public string CategoryName { get; set; }

        public int StatusId { get; set; }

        public string StatusName { get; set; }

        public string Priority { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}

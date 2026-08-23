using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceDesk.Data.Entities
{
    public class Ticket
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public int CreatedByUserId { get; set; }

        public int? AssignedToUserId { get; set; }

        public int CategoryId { get; set; }

        public int StatusId { get; set; }

        public string Priority { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        // Navigation properties

        public User CreatedByUser { get; set; }

        public User AssignedToUser { get; set; }

        public Category Category { get; set; }

        public TicketStatus Status { get; set; }

        public ICollection<TicketComment> Comments { get; set; }
    }
}

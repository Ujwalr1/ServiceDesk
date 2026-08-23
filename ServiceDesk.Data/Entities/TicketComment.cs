using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceDesk.Data.Entities
{
    public class TicketComment
    {
        public int Id { get; set; }

        public int TicketId { get; set; }

        public int UserId { get; set; }

        public string CommentText { get; set; }

        public DateTime CreatedAt { get; set; }

        // Navigation properties

        public Ticket Ticket { get; set; }

        public User User { get; set; }
    }
}

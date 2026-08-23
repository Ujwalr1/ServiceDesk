using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ServiceDesk.Data.Entities
{
    public class User
    {
        public int Id { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public string PasswordHash { get; set; }

        public string Role { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }

        // Navigation properties
        public ICollection<Ticket> CreatedTickets { get; set; }

        public ICollection<Ticket> AssignedTickets { get; set; }

        public ICollection<TicketComment> TicketComments { get; set; }
    }
}

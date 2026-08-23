using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ServiceDesk.Data.Entities
{
    public class TicketStatus
    {
        public int Id { get; set; }

        public string Name { get; set; }

        // Navigation property
        public ICollection<Ticket> Tickets { get; set; }
    }
}

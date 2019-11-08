using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Models
{
    public class TicketStatus
    {
        public TicketStatus()
        {
            Tickets = new HashSet<Ticket>();
        }


        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }


        public virtual ICollection<Ticket> Tickets { get; set; }

    }
}
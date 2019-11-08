using BugTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.ViewModels
{
    public class IndexTicketViewModel
    {
        public ICollection<Ticket> Tickets { get; set; }
        public ICollection<ApplicationUser> Users { get; set; }
    }
}
using System;
using System.Collections.Generic;
using BugTracker.Models;
using System.Linq;
using System.Web;

namespace BugTracker.ViewModels
{
    public class DashBoardVewModel
    {
        public ICollection<TicketNotification> Notifications { get; set; }
        public ICollection<ApplicationUser> Users { get; set; }
    }
}
using System;
using System.Collections.Generic;
using BugTracker.Models;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BugTracker.ViewModels
{
    [Authorize]
    public class DashBoardVewModel
    {
        public ICollection<TicketNotification> Notifications { get; set; }
        public ICollection<ApplicationUser> Users { get; set; }
    }
}
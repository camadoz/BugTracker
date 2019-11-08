using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BugTracker.Models;

namespace BugTracker.Helpers
{
    public class Utilities
    {
        private static ApplicationDbContext db = new ApplicationDbContext();

        public static string MakeReadable(string property, string value)
        {
            switch (property)
            {
                case "TicketStatusId":
                    return db.TicketStatus.Find(Convert.ToInt32(value)).Name;
                case "TicketPriorityId":
                    return db.TicketPriorities.Find(Convert.ToInt32(value)).Name;
                case "TicketTypeId":
                    return db.TicketTypes.Find(Convert.ToInt32(value)).Name;
                case "AssignedToUserId":
                case "OwnerUserId":
                    return db.Users.Find(value).FullName;
                default:
                    return value;

            }

        }
    }

}
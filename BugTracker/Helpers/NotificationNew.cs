using BugTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Helpers
{
    public class NotificationNew
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public void ManageNotification(Ticket oldTicket, Ticket newTicket)
        {
            var ticketHasBeenAssigned = oldTicket.AssignedToUserId == null && newTicket.AssignedToUserId != null;
            var ticketHasBeenUnAssigned = oldTicket.AssignedToUserId != null && newTicket.AssignedToUserId != newTicket.AssignedToUserId;
            var ticketHasBeenReAssigned = oldTicket.AssignedToUserId != null && newTicket.AssignedToUserId != null && oldTicket.AssignedToUserId != newTicket.AssignedToUserId;

            if (ticketHasBeenAssigned)
            {
                AddAssignentNotification(oldTicket, newTicket);

            }
            else if (ticketHasBeenUnAssigned)
            {
                AddUnAssignmentNotification(oldTicket, newTicket);
            }
            else if (ticketHasBeenReAssigned)
            {
                AddReAssignmentNotificatin(oldTicket, newTicket);
            }

        }


         private void AddAssignentNotification(Ticket oldTicket,Ticket newTicket)
         {
            var notification = new TicketNotification
            {
                TicketId = newTicket.Id,
                isRead = false,
                RecipientId = newTicket.AssignedToUserId,
                NotificationBody = $"You have been assigned to a ticket id {newTicket.Id} on project {newTicket.Project.Name}. The ticket Title is {newTicket.Title}"
            };

            db.TicketNotifications.Add(notification);
            db.SaveChanges();
         }

        private void AddUnAssignmentNotification(Ticket oldTicket, Ticket newTicket)
        {
            var notification = new TicketNotification
            {
                TicketId = newTicket.Id,
                isRead = false,
                RecipientId = newTicket.AssignedToUserId,
                NotificationBody = $"You have been unassigned from ticket id {newTicket.Id} on project {newTicket.Project.Name}. The ticket Title is {newTicket.Title}"
            };

            db.TicketNotifications.Add(notification);
            db.SaveChanges();
        }

        private void AddReAssignmentNotificatin(Ticket oldTicket, Ticket newTicket)
        {
            var notification = new TicketNotification
            {
                TicketId = newTicket.Id,
                isRead = false,
                RecipientId = newTicket.AssignedToUserId,
                NotificationBody = $"You have been reunassigned to ticket id {newTicket.Id} on project {newTicket.Project.Name}. The ticket Title is {newTicket.Title}"
            };

            db.TicketNotifications.Add(notification);
            db.SaveChanges();
        }


    }
}
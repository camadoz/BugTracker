using BugTracker.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Helpers
{
    public class TicketHistoryHelper
    {
        ApplicationDbContext db = new ApplicationDbContext();

        public void RecordHistoricalChanges(Ticket oldTicket, Ticket newTicket)
        { 
            if(oldTicket.TicketStatusId != newTicket.TicketStatusId)
          {
             var newHistoryRecord = new TicketHistory() { 
             
              PropertyName = "TicketStatusId",
              OldValue = oldTicket.TIcketStatus.Name,
              NewValue = newTicket.TIcketStatus.Name,
              Updated = (DateTime)newTicket?.Updated,
              UserId=HttpContext.Current.User.Identity.GetUserId(),
                 TicketId = newTicket.Id

             };

             db.TicketHistories.Add(newHistoryRecord);
          }
            
            if (oldTicket.TicketPriorityId != newTicket.TicketPriorityId)
            {
                var newHistoryRecord = new TicketHistory()
                {

                    PropertyName = "TicketPriorityId",
                    OldValue = oldTicket.TicketPriority.Name,
                    NewValue = newTicket.TicketPriority.Name,
                    Updated = (DateTime)newTicket.Updated,
                    UserId = HttpContext.Current.User.Identity.GetUserId(),
                    TicketId = newTicket.Id

                };

                db.TicketHistories.Add(newHistoryRecord);
            }
            
            if (oldTicket.TicketTypeId != newTicket.TicketTypeId)
            {
                var newHistoryRecord = new TicketHistory()
                {

                    PropertyName = "TicketTypeId",
                    OldValue = oldTicket.TicketType.Name,
                    NewValue = newTicket.TicketType.Name,
                    Updated = (DateTime)newTicket.Updated,
                    UserId = HttpContext.Current.User.Identity.GetUserId(),
                    TicketId = newTicket.Id

                };

                db.TicketHistories.Add(newHistoryRecord);
            }

            if (oldTicket.Title != newTicket.Title)
            {
                var newHistoryRecord = new TicketHistory()
                {

                    PropertyName = "Title",
                    OldValue = oldTicket.Title,
                    NewValue = newTicket.Title,
                    Updated = (DateTime)newTicket.Updated,
                    UserId = HttpContext.Current.User.Identity.GetUserId(),
                    TicketId = newTicket.Id

                };

                db.TicketHistories.Add(newHistoryRecord);
            }

            if (oldTicket.Description != newTicket.Description)
            {
                var newHistoryRecord = new TicketHistory()
                {

                    PropertyName = "Description",
                    OldValue = oldTicket.Description,
                    NewValue = newTicket.Description,
                    Updated = (DateTime)newTicket.Updated,
                    UserId = HttpContext.Current.User.Identity.GetUserId(),
                    TicketId = newTicket.Id

                };

                db.TicketHistories.Add(newHistoryRecord);
            }
            
            if (oldTicket.AssignedToUserId != newTicket.AssignedToUserId)
            {
                var newHistoryRecord = new TicketHistory()
                {

                    PropertyName = "AssignedToUserId",
                    OldValue = oldTicket.AssignedToUser?.FullName == null ? "UnAssigned" : oldTicket.AssignedToUser.FullName,
                    NewValue = newTicket.AssignedToUser?.FullName == null ? "UnAssigned" : newTicket.AssignedToUser.FullName,
                    Updated = (DateTime)newTicket?.Updated,
                    UserId = HttpContext.Current.User.Identity.GetUserId(),
                    TicketId = newTicket.Id
                    
                };

                db.TicketHistories.Add(newHistoryRecord);
            }

            db.SaveChanges();
        }
    }
}
﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using BugTracker.Helpers;
using BugTracker.Models;
using BugTracker.ViewModels;
using Microsoft.AspNet.Identity;

namespace BugTracker.Controllers
{
     [Authorize]
    public class TicketsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserRolesHelper roleHelper = new UserRolesHelper();
        ProjectsHelper projectHelper = new ProjectsHelper();
        private TicketHistoryHelper ticketHistoryHeler = new TicketHistoryHelper();
        private NotificationHelper notificationHelper = new NotificationHelper();


        // GET: Tickets
       
        public ActionResult Index()
        {
            
            if (User.Identity.GetUserId() == null) return RedirectToAction("Index", "Home");

            var tickets = new List<Ticket>();
            var ticketsVM = new IndexTicketViewModel();
            //var users = roleHelper.UsersInRole("Developer");
            //var userOnCurrentProject = projectHelper.UsersInProject()
            ViewBag.AssignedToUserId = new SelectList(roleHelper.UsersInRole("Developer"), "Id", "FirstName");
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name");
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name");
            ViewBag.TicketStatusId = new SelectList(db.TicketStatus, "Id", "Name");
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name");


            ViewBag.AssignUsers = new SelectList(db.Users, "Id", "FullName");
            ViewBag.UsersIds = new MultiSelectList(db.Users, "Id", "FullName");
            var loggedInUser = User.Identity.GetUserId();
            
            if (User.IsInRole("Admin") || User.IsInRole("DemoAdmin"))
            {
                //tickets = db.Tickets.Include(t => t.AssignedToUser).Include(t => t.Project).Include(t => t.TicketPriority).Include(t => t.TIcketStatus).Include(t => t.TicketType).ToList();
                tickets = db.Tickets.OrderByDescending(t => t.TicketPriorityId).ToList();
            }
            else if (User.IsInRole("Submitter") || User.IsInRole("DemoSubmitter"))
            {
                tickets = db.Tickets.Where(t => t.OwnerUserId == loggedInUser).ToList();
            }
            else if (User.IsInRole("Developer") || User.IsInRole("DemoDeveloper"))
            {
                tickets = db.Tickets.Where(t => t.AssignedToUserId == loggedInUser).ToList();
            }
            else if(User.IsInRole("Project Manager") || User.IsInRole("Admin"))
            {
                var userId = User.Identity.GetUserId();

                var user = db.Users.Find(userId);

                tickets.AddRange(user.Projects.SelectMany(p => p.Tickets));

                //ICollection<Project> projects = projectHelper.ListUserProjects(loggedInUser);

                //foreach (var project in projects)
                //{
                //    foreach (var tick in project.Tickets)
                //    {
                //        tickets.Add(tick);
                //    }
                //}
            }

            ticketsVM.Tickets = tickets;
            ticketsVM.Users = db.Users.ToList();

            return View(ticketsVM);
        }

        // GET: Tickets/Details/5
        public ActionResult Details(int? id)
        {

            ViewBag.AssignedToUserId = new SelectList(roleHelper.UsersInRole("Developer"), "Id", "FirstName");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }

        // GET: Tickets/Create
        public ActionResult Create()
        {
            ViewBag.AssignedToUserId = new SelectList(db.Users, "Id", "FirstName");
            //ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name");
            ViewBag.ProjectId = new SelectList(projectHelper.ListUserProjects(User.Identity.GetUserId()), "Id", "Name");
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name");
            ViewBag.TicketStatusId = new SelectList(db.TicketStatus, "Id", "Name");
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name");
            return View();
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ProjectId,TicketTypeId,TicketStatusId,TicketPriorityId,Title,Description,Created,Updated,OwnerUserId,AssignedToUserId")] Ticket ticket)
        {

            if (ModelState.IsValid)
            {
               

                ticket.Created = DateTime.Now;
                ticket.TicketStatusId = db.TicketStatus.FirstOrDefault(t => t.Name == "open").Id;
                ticket.OwnerUserId = User.Identity.GetUserId();
               
                db.Tickets.Add(ticket);
                db.SaveChanges();
                //return RedirectToAction("Index");
                return RedirectToAction("Details", new { id = ticket.Id });
            }

            //ViewBag.AssignedToUserId = new SelectList(db.Users, "Id", "FirstName");
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", ticket.ProjectId);
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name", ticket.TicketPriorityId);
            ViewBag.TicketStatusId = new SelectList(db.TicketStatus, "Id", "Name", ticket.TicketStatusId);
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name", ticket.TicketTypeId);
             return View(ticket);

           
        }

        // GET: Tickets/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            ViewBag.AssignedToUserId = new SelectList(roleHelper.UsersInRole("Developer"), "Id", "FirstName", ticket.AssignedToUserId);
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", ticket.ProjectId);
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name", ticket.TicketPriorityId);
            ViewBag.TicketStatusId = new SelectList(db.TicketStatus, "Id", "Name", ticket.TicketStatusId);
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name", ticket.TicketTypeId);
            return View(ticket);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ProjectId,TicketTypeId,TicketStatusId,TicketPriorityId,Title,Description,Created,Updated,OwnerUserId,AssignedToUserId")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                var oldTicket = db.Tickets.AsNoTracking().FirstOrDefault(t => t.Id == ticket.Id);
                //Ticket updateTicket = db.Tickets.Find(ticket.Id);
                ticket.Updated = DateTime.Now;
                db.Entry(ticket).State = EntityState.Modified;
                db.SaveChanges();

                var newTicket = db.Tickets.AsNoTracking().FirstOrDefault(t => t.Id == ticket.Id);
                ticketHistoryHeler.RecordHistoricalChanges(oldTicket, newTicket);
                notificationHelper.CreateChangeNotification(oldTicket, newTicket);



                return RedirectToAction("Index");

            }

            ViewBag.AssignedToUserId = new SelectList(roleHelper.UsersInRole("Developer"), "Id", "FirstName", ticket.AssignedToUserId);
            //ViewBag.AssignedToUserId = new SelectList(, "Id", "FirstName", ticket.AssignedToUserId);
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", ticket.ProjectId);
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name", ticket.TicketPriorityId);
            ViewBag.TicketStatusId = new SelectList(db.TicketStatus, "Id", "Name", ticket.TicketStatusId);
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name", ticket.TicketTypeId);
            return View(ticket);
        }


        [HttpPost]
        public ActionResult EditTicket(int ticketID, int ticketTYpeValue,int ticketStatusValue, int ticketPriotityValue,string ticketTitleValue,string ticketDescritionValue)
        {

            if (ModelState.IsValid)
            {


                var oldTicket = db.Tickets.AsNoTracking().FirstOrDefault(t => t.Id == ticketID);
                //Ticket updateTicket = db.Tickets.Find(ticket.Id);
                var ticket = db.Tickets.Find(ticketID);

                ticket.Updated = DateTime.Now;
                ticket.TicketTypeId = ticketTYpeValue;
                ticket.TicketStatusId = ticketStatusValue;
                ticket.TicketPriorityId = ticketPriotityValue;
                ticket.Title = ticketTitleValue;
                ticket.Description = ticketDescritionValue;

                db.Entry(ticket).State = EntityState.Modified;
                db.SaveChanges();

                var newTicket = db.Tickets.AsNoTracking().FirstOrDefault(t => t.Id == ticket.Id);
                ticketHistoryHeler.RecordHistoricalChanges(oldTicket, newTicket);
                notificationHelper.CreateChangeNotification(oldTicket, newTicket);



                return RedirectToAction("Index");

            }


            return View();
        }





        // GET: Tickets/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Ticket ticket = db.Tickets.Find(id);
            db.Tickets.Remove(ticket);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        public ActionResult AssignToTicket(int? id)
        {
            AssignTicketUserViewModel ticketInfo = new AssignTicketUserViewModel();
            Ticket ticket = db.Tickets.Find(id);
            ICollection<ApplicationUser> userToDisplay = new List<ApplicationUser>();

            if (User.IsInRole("Project Manager"))
            { 
            
            var usersInTheProject = projectHelper.UsersOnProject(ticket.ProjectId);
           
            foreach(var user in usersInTheProject)
            {
                if (roleHelper.IsUserInRole(user.Id, "Developer"))
                {
                   userToDisplay.Add(user);
                }
             }
           }
           else
           {
                userToDisplay = db.Users.ToList();
           }


            ticketInfo.ticket = ticket;
            ticketInfo.projectDescription = db.Projects.Find(ticketInfo.ticket.ProjectId).Name;

            //ViewBag.developers = new SelectList(roleHelper.UsersInRole("Developer"), "Id", "FirstName");
            ViewBag.developers = new SelectList(userToDisplay, "Id", "FirstName");
            return View(ticketInfo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AssignToTicket(AssignTicketUserViewModel ticketToAssigned,string developers)
        {
            Ticket ticket = db.Tickets.Find(ticketToAssigned.ticket.Id);
            ticket.AssignedToUserId = developers;

            db.SaveChanges();
        
            return RedirectToAction("Index", "Tickets");
        }

        [HttpPost]
        [Authorize]
        public async Task<bool> UnassignTickerFromUser(int ticketId, string userId)
        {
            var oldTicket = db.Tickets.AsNoTracking().FirstOrDefault(t => t.Id == ticketId);

            Ticket ticket = db.Tickets.Find(ticketId);
            if (userId != null && ticket != null)
            {
                ticket.AssignedToUserId = null;
                ticket.Updated = DateTime.Now;
                ticket.TicketStatusId = db.TicketStatus.FirstOrDefault(t => t.Name == "open").Id;
                db.SaveChanges();
                notificationHelper.ManageNotifications(oldTicket, ticket);
                ticketHistoryHeler.RecordHistoricalChanges(oldTicket, ticket);
                var callbackUrl = Url.Action("Details", "Tickets", new { id = ticket.Id }, protocol: Request.Url.Scheme);

                try
                {
                    EmailService ems = new EmailService();
                    IdentityMessage msg = new IdentityMessage();
                    ApplicationUser user = db.Users.Find(userId);
                    msg.Body = "You have been assigned a new Ticket." + Environment.NewLine + "Please click the following link to view the details" +
                        "<a href=\"" + callbackUrl + "\">NEW TICKET</a>";
                    msg.Destination = user.Email;
                    msg.Subject = "Invite to Household";
                    await ems.SendMailAsync(msg);
                }
                catch (Exception ex)
                {
                    await Task.FromResult(0);
                }

                
            }
            return true;
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<int> AssignTicketToUser(int ticketId,string userId )
        {

           
            var oldTicket = db.Tickets.AsNoTracking().FirstOrDefault(t => t.Id == ticketId);

            Ticket ticket = db.Tickets.Find(ticketId);
            if (userId != null && ticket != null )
            { 
                ticket.AssignedToUserId = userId;
                ticket.Updated = DateTime.Now;
                ticket.TicketStatusId =db.TicketStatus.FirstOrDefault(t => t.Name == "Assigned").Id;
                db.SaveChanges();
                notificationHelper.ManageNotifications(oldTicket, ticket);
                ticketHistoryHeler.RecordHistoricalChanges(oldTicket, ticket);
                var callbackUrl = Url.Action("Details","Tickets",new{ id = ticket.Id },protocol: Request.Url.Scheme);

                try
                {
                    EmailService ems =  new EmailService();
                    IdentityMessage msg = new IdentityMessage();
                    ApplicationUser user = db.Users.Find(userId);
                    msg.Body = "You have been assigned a new Ticket."  + Environment.NewLine + "Please click the following link to view the details"+
                        "<a href=\""+callbackUrl +"\">NEW TICKET</a>";
                    msg.Destination = user.Email;
                    msg.Subject = "Ticket Assign Notice";
                    await ems.SendMailAsync(msg);
                }
                catch(Exception ex)
                {
                    await Task.FromResult(0);
                }

               
            }

            return 4;
        }

        
    }
}

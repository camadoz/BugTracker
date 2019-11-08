namespace BugTracker.Migrations
{
    using BugTracker.Helpers;
    using BugTracker.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Web.Configuration;

    internal sealed class Configuration : DbMigrationsConfiguration<BugTracker.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(BugTracker.Models.ApplicationDbContext context)
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            #region Create roles

            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                roleManager.Create(new IdentityRole { Name = "Admin" });

            }

            if (!context.Roles.Any(r => r.Name == "Project Manager"))
            {
                roleManager.Create(new IdentityRole { Name = "Project Manager" });

            }

            if (!context.Roles.Any(r => r.Name == "Developer"))
            {
                roleManager.Create(new IdentityRole { Name = "Developer" });

            }


            if (!context.Roles.Any(r => r.Name == "Submitter"))
            {
                roleManager.Create(new IdentityRole { Name = "Submitter" });

            }

            if (!context.Roles.Any(r => r.Name == "DemoAdmin"))
            {
                roleManager.Create(new IdentityRole { Name = "DemoAdmin" });

            }

            if (!context.Roles.Any(r => r.Name == "DemoPM"))
            {
                roleManager.Create(new IdentityRole { Name = "DemoPM" });

            }

            if (!context.Roles.Any(r => r.Name == "DemoDeveloper"))
            {
                roleManager.Create(new IdentityRole { Name = "DemoDeveloper" });

            }

            if (!context.Roles.Any(r => r.Name == "DemoSubmitter"))
            {
                roleManager.Create(new IdentityRole { Name = "DemoSubmitter" });

            }
            #endregion

            #region Create users
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            if (!context.Users.Any(u => u.Email == "madozc@gmail.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "madozc@gmail.com",
                    Email = "madozc@gmail.com",
                    FirstName = "christophe",
                    LastName = "madoz",
                    DisplayName = "Administrator"
                }, WebConfigurationManager.AppSettings["my password"]);
            }


            if (!context.Users.Any(u => u.Email == "madozsolutions@gmail.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "madozsolutions@gmail.com",
                    Email = "madozsolutions@gmail.com",
                    FirstName = "Pascal",
                    LastName = "madoz",
                    DisplayName = "Project Manager"
                }, "Abc&123");
            }

            if (!context.Users.Any(u => u.Email == "madozchristophe@gmail.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "madozchristophe@gmail.com",
                    Email = "madozchristophe@gmail.com",
                    FirstName = "Alain",
                    LastName = "madoz",
                    DisplayName = "Developer"
                }, "Abc&123");
            }


            if (!context.Users.Any(u => u.Email == "submitter@gmail.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "submitter@gmail.com",
                    Email = "submitter@gmail.com",
                    FirstName = "John",
                    LastName = "madoz",
                    DisplayName = "Submitter"
                }, "Abc&123");
            }

            if (!context.Users.Any(u => u.Email == "regularuser@gmail.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "regularuser@gmail.com",
                    Email = "regularuser@gmail.com",
                    FirstName = "Steve",
                    LastName = "clayton",
                    DisplayName = "steve Clayton"
                }, "Abc&123");
            }

            #endregion

            #region Add users to roles
            var adminId = userManager.FindByEmail("madozc@gmail.com").Id;
            userManager.AddToRole(adminId, "Admin");

            var projectMangerId = userManager.FindByEmail("madozsolutions@gmail.com").Id;
            userManager.AddToRole(projectMangerId, "Project Manager");

            var developerId = userManager.FindByEmail("madozchristophe@gmail.com").Id;
            userManager.AddToRole(developerId, "Developer");

            var submitterId = userManager.FindByEmail("submitter@gmail.com").Id;
            userManager.AddToRole(submitterId, "Submitter");


            var userId = userManager.FindByEmail("regularuser@gmail.com").Id;
            userManager.AddToRole(userId, "Submitter");
            #endregion

            #region loading lookup date

            context.TicketPriorities.AddOrUpdate(item => item.Name,

                new TicketPriority() { Name = "High", Description = "The issue needs to be resolve within 3 days." },
                new TicketPriority() { Name = "Low", Description = "The issue can be resolved within 2 weeks." },
                new TicketPriority() { Name = "Medium", Description = "The issue can be resolved within 1 week." },
                new TicketPriority() { Name = "Urgent", Description = "Crutial problem with the system which needs to be resolve within 1 day." }
                );

            context.TicketStatus.AddOrUpdate(t => t.Name,
                new TicketStatus() { Name = "open", Description = "A newly created or simply unassinged ticket" },
                new TicketStatus() { Name = "Assigned", Description = "A ticket that has been assigned but not yet started" },
                new TicketStatus() { Name = "In Progress", Description = "A ticket that has been assigned and is currently being working on" },
                new TicketStatus() { Name = "Resolved", Description = "A ticket that has been resolved." },
                new TicketStatus() { Name = "Archived", Description = "A ticket that has been archived." }
                );

            context.TicketTypes.AddOrUpdate(t => t.Name,
                 new TicketType() { Name = "Defect", Description = "A defect in the software has been identified" },
                 new TicketType() { Name = "Feature Request", Description = "I user has made a request to add a feature to the software." },
                 new TicketType() { Name = "Training request", Description = "A new employee has made a request to get a training for a specific feature in the system." }
                );

            #endregion

            #region Project Creation
            context.Projects.AddOrUpdate(
               p => p.Name,
                   new Project() { Name = "Blog Project", Description = "This is the Blog project ready for use.", Created = DateTime.Now },
                   new Project() { Name = "Portfolio", Description = "This is the Portfolio project which is ready for use.", Created = DateTime.Now },
                  new Project() { Name = "BugTracker", Description = "This is the BugTracker project.", Created = DateTime.Now }
               );

            context.SaveChanges();
            #endregion


            #region Project Assignment
            var blogProjectId = context.Projects.FirstOrDefault(p => p.Name == "Blog Project").Id;
            var bugTrackerProjectId = context.Projects.FirstOrDefault(p => p.Name == "BugTracker").Id;

            var projectHelper = new ProjectsHelper();

            //Assign all three users to the Blog project
            projectHelper.AddUserToProject(adminId, blogProjectId);
            projectHelper.AddUserToProject(projectMangerId, blogProjectId);
            projectHelper.AddUserToProject(developerId, blogProjectId);

            //Assign all three users to the Blog project
            projectHelper.AddUserToProject(adminId, bugTrackerProjectId);
            projectHelper.AddUserToProject(projectMangerId, bugTrackerProjectId);
            projectHelper.AddUserToProject(developerId, bugTrackerProjectId);
            #endregion






            #region Ticket creation          
            context.Tickets.AddOrUpdate(
               p => p.Title,
                new Ticket
                {
                    ProjectId = blogProjectId,
                    OwnerUserId = submitterId,
                    Title = "Ticket Test one",
                    Description = "Ticket project one",
                    Created = DateTime.Now,
                    TicketPriorityId = context.TicketPriorities.FirstOrDefault(t => t.Name == "High").Id,
                    TicketStatusId = context.TicketStatus.FirstOrDefault(t => t.Name == "open").Id,
                    TicketTypeId = context.TicketTypes.FirstOrDefault(t => t.Name == "Defect").Id,
                },
                new Ticket
                {
                    ProjectId = blogProjectId,
                    OwnerUserId = submitterId,
                    AssignedToUserId = developerId,
                    Title = "Ticket Test Two",
                    Description = "Ticket project one with issues",
                    Created = DateTime.Now,
                    TicketPriorityId = context.TicketPriorities.FirstOrDefault(t => t.Name == "Medium").Id,
                    TicketStatusId = context.TicketStatus.FirstOrDefault(t => t.Name == "In Progress").Id,
                    TicketTypeId = context.TicketTypes.FirstOrDefault(t => t.Name == "Defect").Id,
                },


                new Ticket
                {
                    ProjectId = bugTrackerProjectId,
                    OwnerUserId = submitterId,
                    Title = "Ticket Test Three",
                    Description = "Ticket project one with issues",
                    Created = DateTime.Now,
                    TicketPriorityId = context.TicketPriorities.FirstOrDefault(t => t.Name == "Urgent").Id,
                    TicketStatusId = context.TicketStatus.FirstOrDefault(t => t.Name == "open").Id,
                    TicketTypeId = context.TicketTypes.FirstOrDefault(t => t.Name == "Defect").Id,
                },
                new Ticket
                {
                    ProjectId = bugTrackerProjectId,
                    OwnerUserId = submitterId,
                    AssignedToUserId = developerId,
                    Title = "Ticket Test Four",
                    Description = "Ticket project one with issues",
                    Created = DateTime.Now,
                    TicketPriorityId = context.TicketPriorities.FirstOrDefault(t => t.Name == "Low").Id,
                    TicketStatusId = context.TicketStatus.FirstOrDefault(t => t.Name == "Resolved").Id,
                    TicketTypeId = context.TicketTypes.FirstOrDefault(t => t.Name == "Defect").Id,
                });









            #endregion



        }
    }
}

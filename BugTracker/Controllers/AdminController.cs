using BugTracker.Helpers;
using BugTracker.Models;
using BugTracker.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BugTracker.Controllers
{
    public class AdminController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserRolesHelper roleHelper = new UserRolesHelper();
        private ProjectsHelper projectHelper = new ProjectsHelper();
        // GET: Admin
        public ActionResult ManageRoles()
        {
            ViewBag.UsersIds = new MultiSelectList(db.Users,"Id", "FullName");
            ViewBag.Role = new SelectList(db.Roles,"Name","Name");
            ViewBag.Developer = db.Users.ToList();
            var users = new List<ManageRolesViewModel>();
            foreach(var user in db.Users.ToList())
            {
                users.Add(new ManageRolesViewModel
                {
                    UserName = $"{ user.LastName},{ user.FirstName}",
                    RoleName=roleHelper.ListUserRoles(user.Id).FirstOrDefault(),
                    email = user.Email

                });
            }
            return View(users);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ManageRoles(List<string> usersIds,string role)
        {
            if (usersIds != null)
            {

           
            foreach(var userId in usersIds)
            {

                var userRole = roleHelper.ListUserRoles(userId).FirstOrDefault();
                if(userRole != null)
                {
                    roleHelper.RemoveUserFromRole(userId,userRole);
                }
            }

            if(!string.IsNullOrEmpty(role))
            {
                 foreach(var userId in usersIds)
                 {
                    roleHelper.AddUserToRole(userId, role);
                 }
            }
        }

            return RedirectToAction("ManageRoles","Admin");
        }




        public ActionResult GetUsers()
        {
            var users = new List<ManageRolesViewModel>();
            foreach (var user in db.Users.ToList())
            {
                users.Add(new ManageRolesViewModel
                {
                    UserName = $"{ user.LastName},{ user.FirstName}",
                    RoleName = roleHelper.ListUserRoles(user.Id).FirstOrDefault(),
                    userId = user.Id

                });
            }
            return Json(new { data = users }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UsersIndex()
        {
            var usersInfo = new List<IndexUsersViewModel>();
            var allUsers = db.Users.ToList();
            foreach (var user in allUsers)
            {
                var userInfo = new IndexUsersViewModel();
                userInfo.User = user;
                userInfo.Projects = projectHelper.ListUserProjects(user.Id);
                userInfo.role = roleHelper.ListUserRoles(user.Id);
                usersInfo.Add(userInfo);

            }
            return View(usersInfo.ToList());
        }


        public ActionResult UsersAndProjects()
        {
            ViewBag.ProjecstIds = new MultiSelectList(db.Projects, "Id", "Name");
            var usersInfo = new List<IndexUsersViewModel>();
            var allUsers = db.Users.ToList();
            foreach (var user in allUsers)
            {
                var userInfo = new IndexUsersViewModel();
                userInfo.User = user;
                userInfo.Projects = projectHelper.ListUserProjects(user.Id);
                userInfo.role = roleHelper.ListUserRoles(user.Id);
                usersInfo.Add(userInfo);

            }
            return View("ManageUsersAndProjects", usersInfo.ToList());
        }

        public ActionResult AssignUserProject(string userId, int projectID)
        {
            projectHelper.AddUserToProject(userId, projectID);

            ViewBag.ProjecstIds = new MultiSelectList(db.Projects, "Id", "Name");
            var usersInfo = new List<IndexUsersViewModel>();
            var allUsers = db.Users.ToList();
            foreach (var user in allUsers)
            {
                var userInfo = new IndexUsersViewModel();
                userInfo.User = user;
                userInfo.Projects = projectHelper.ListUserProjects(user.Id);
                userInfo.role = roleHelper.ListUserRoles(user.Id);
                usersInfo.Add(userInfo);

            }
            var project = db.Projects.Find(projectID);
            
            //return RedirectToAction("ManageUsersAndProjects", usersInfo.ToList());
            return Json(new { Name = project.Name, Description = project.Description, Created = project.Created.ToString("MMM dd,yyyy"),projectID,userId });
        }

        public ActionResult RemoveUser(string id,int projectID)
        {
            projectHelper.RemoveUserFromProject(id, projectID);
            ViewBag.ProjecstIds = new MultiSelectList(db.Projects, "Id", "Name");

            var usersInfo = new List<IndexUsersViewModel>();
            var allUsers = db.Users.ToList();
            foreach (var user in allUsers)
            {
                var userInfo = new IndexUsersViewModel();
                userInfo.User = user;
                userInfo.Projects = projectHelper.ListUserProjects(user.Id);
                userInfo.role = roleHelper.ListUserRoles(user.Id);
                usersInfo.Add(userInfo);

            }
            return View("ManageUsersAndProjects", usersInfo.ToList());

         }

        [HttpPost]
        public ActionResult RemoveUserFromProject(string userID, int projectID)
        {
            projectHelper.RemoveUserFromProject(userID, projectID);
            ViewBag.ProjecstIds = new MultiSelectList(db.Projects, "Id", "Name");

            var usersInfo = new List<IndexUsersViewModel>();
            var allUsers = db.Users.ToList();
            foreach (var user in allUsers)
            {
                var userInfo = new IndexUsersViewModel();
                userInfo.User = user;
                userInfo.Projects = projectHelper.ListUserProjects(user.Id);
                userInfo.role = roleHelper.ListUserRoles(user.Id);
                usersInfo.Add(userInfo);

            }
            return View("ManageUsersAndProjects", usersInfo.ToList());

        }




        [Authorize(Roles ="Admin")]
        public ActionResult ManageUserProjects()
        {
            ViewBag.ProjecstIds = new MultiSelectList(db.Projects, "Id", "Name");
            ViewBag.UsersIds = new MultiSelectList(db.Users, "Id", "FullName");
            if (User.IsInRole("Admin"))
            {
                ViewBag.UsersIds = new MultiSelectList(db.Users, "Id", "FullName");
            }
            else if(User.IsInRole("Developer"))
            {
                ViewBag.UsersIds = new MultiSelectList(roleHelper.UsersInRole("Developer"), "Id", "FullName");
            }
            
            //ViewBag.UsersIds = new MultiSelectList(roleHelper.UsersInRole("Developer"), "Id", "FullName");

            //if(User.IsInRole("Admin"))
            //{
            //    ViewBag.ProjectManagerId = new SelectList(roleHelper.UsersInRole("Project Manager"), "Id", "Email");
            //}


           // var myData = new List<UserProjectListViewModel>();
           // UserProjectViewModel userVm = null;
           //foreach(var user in db.Users.ToList())
           // {
           //     userVm = new USerProjectListViewModel
           //     {
           //         Name = ${ user.LastName},{ user.FirstName}"," +
           //         PRojectNames = projectHelper.ListOfProjects(user.Id).Select(p => p.Name).Count() == 0 ? "N/A" : projectHelper.ListOfProjects(user.Id).Select(p => p.Name).ToList();
           //     };

           // myData.Add(userVm);
           // }


            var users = new List<ManageProjecstUsersViewModel>();
            foreach (var user in db.Users.ToList())
            {
                users.Add(new ManageProjecstUsersViewModel
                {
                    FullName = $"{ user.LastName},{ user.FirstName}",
                    RoleName = roleHelper.ListUserRoles(user.Id).FirstOrDefault(),
                    Email = user.Email,
                    ProjectName = projectHelper.ListOfProjects(user.Id)

                }); ;
            }
            return View(users);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ManageUserProjects(List<string> UsersIds, List<int> ProjecstIds)
        {
            if (UsersIds != null && ProjecstIds != null)
            {

                foreach(var userId in UsersIds)
                {
                    foreach(var projectId in ProjecstIds)
                    {
                        projectHelper.AddUserToProject(userId, projectId);
                    }
                }

            }

            return RedirectToAction("ManageUserProjects", "Admin");
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ManageProjectUsers(int projectId, List<string> ProjectManagers, List<string> Developers, List<string> Submitters)
        {
            //Step 1: Remove all users from the project
            foreach (var user in projectHelper.UsersOnProject(projectId).ToList())
            {
                projectHelper.RemoveUserFromProject(user.Id, projectId);
            }

            //Step 2: Adds back all the selected PM's
            if (ProjectManagers != null)
            {
                foreach (var projectManagerId in ProjectManagers)
                {
                    projectHelper.AddUserToProject(projectManagerId, projectId);
                }
            }

            //Step 3: Adds back all the selected Developers
            if (Developers != null)
            {
                foreach (var developerId in Developers)
                {
                    projectHelper.AddUserToProject(developerId, projectId);
                }
            }

            //Step 4: Adds back all the selected Submitters
            if (Submitters != null)
            {
                foreach (var submitterId in Submitters)
                {
                    projectHelper.AddUserToProject(submitterId, projectId);
                }
            }

            //Step 4: Redirect the user somewhere
            return RedirectToAction("Details", "Projects", new { id = projectId });
        }

    }
}
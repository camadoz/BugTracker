using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BugTracker.Helpers;
using BugTracker.Models;

namespace BugTracker.Controllers
{
    public class ProjectsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserRolesHelper roleHelper = new UserRolesHelper();
        private ProjectsHelper projectHelper = new ProjectsHelper();

        // GET: Projects
        public ActionResult Index()
        {
            return View(db.Projects.ToList());
        }

        // GET: Projects/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            DetailProjectViewModel projectDetail = new DetailProjectViewModel();
            projectDetail.projectDetail = project;
            projectDetail.tickets = db.Tickets.Where(t => t.ProjectId == project.Id).ToList();
            projectDetail.users = projectHelper.UsersOnProject(project.Id);

            //var allProjectManagers = roleHelper.UsersInRole("Project Manager");
            //var currentProjectManagers = projectHelper.UsersInRoleOnProject(project.Id, "ProjectManager");
            //ViewBag.ProjectManagers = new MultiSelectList(allProjectManagers, "Id", "FirstName", currentProjectManagers);

            //var allSubmitters = roleHelper.UsersInRole("Submitter");
            //var currentSubmitters = projectHelper.UsersInRoleOnProject(project.Id, "Submitter");
            //ViewBag.Submitters = new MultiSelectList(allSubmitters, "Id", "FirstName", currentSubmitters);

            //var allDevelopers = roleHelper.UsersInRole("Developer");
            //var currentDevelopers = projectHelper.UsersInRoleOnProject(project.Id, "Developer");
            //ViewBag.Developers = new MultiSelectList(allDevelopers, "Id", "FirstName", currentDevelopers);

            return View(projectDetail);
        }

        // GET: Projects/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Projects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Description,Created,Updated")] Project project)
        {
            if (ModelState.IsValid)
            {
                project.Created = DateTime.Now;
                db.Projects.Add(project);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(project);
        }

        // GET: Projects/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            var currentUserAssigned = projectHelper.UsersInProject(project.Id);

            ViewBag.AllUsers = new MultiSelectList(db.Users, "Id", "FullName", currentUserAssigned);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Description,Created,Updated")] Project project, List<string> AllUsers)
        {
            if (ModelState.IsValid)
            {

                foreach (var user in projectHelper.UsersOnProject(project.Id).ToList())
                {
                    projectHelper.RemoveUserFromProject(user.Id, project.Id);
                }


                foreach (var userId in AllUsers)
                {
                    projectHelper.AddUserToProject(userId,project.Id);
                }


                db.Entry(project).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(project);
        }

        // GET: Projects/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Project project = db.Projects.Find(id);
            db.Projects.Remove(project);
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
    }
}

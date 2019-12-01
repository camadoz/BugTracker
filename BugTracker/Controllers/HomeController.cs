using BugTracker.Models;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using BugTracker.ViewModels;
using BugTracker.Helpers;
using Newtonsoft.Json;
using Microsoft.AspNet.Identity;

namespace BugTracker.Controllers
{
    [Authorize]
    [RequireHttps]
    public class HomeController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        private ApplicationSignInManager _signInManager;

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }
        [AllowAnonymous]
        public ActionResult DemoUser()
        {
            return View();
        }


        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Dashboard", "Home");
        }


        //================================== added ======================================



        public ActionResult Index()
        {
            return View();
        }
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        public ActionResult DataStats()
        {
            ApplicationDbContext db = new ApplicationDbContext();

            var ret = new int[10];
            ret[0] = db.Tickets.Where(t => t.TIcketStatus.Name == "open").Count();
            ret[1] = db.Tickets.Where(t => t.TIcketStatus.Name == "Assigned").Count(); ;
            ret[2] = db.Tickets.Where(t => t.TIcketStatus.Name == "In Progress").Count();
            ret[3] = db.Tickets.Where(t => t.TIcketStatus.Name == "Resolved").Count();
            ret[4] = db.Tickets.Where(t => t.TIcketStatus.Name == "Archived").Count();
            // return (ret);
            return Json(ret);

            //return Json(new { Name = project.Name, Description = project.Description, Created = project.Created.ToString("MMM dd,yyyy"), projectID, userId });
        }

        public ActionResult DataStatsPerUser()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            UserRolesHelper userRoleHelper = new UserRolesHelper();

            var allDeveloper = userRoleHelper.UsersInRole("Developer");
            string[,] usersTicket = new String[2, allDeveloper.Count()];


            for(var i = 0 ; i < allDeveloper.Count() ; i++)
            {
                usersTicket[0,i] = allDeveloper.ElementAt(i).FullName;

                ApplicationUser developer= allDeveloper.ElementAt(i);

                usersTicket[1, i] = Convert.ToString( db.Tickets.Where(t => t.AssignedToUserId == developer.Id).ToList().Count());
               
            }
            string output = JsonConvert.SerializeObject(usersTicket);

            return Json(output);
            //return Json(new { Name = project.Name, Description = project.Description, Created = project.Created.ToString("MMM dd,yyyy"), projectID, userId });
        }

        public JsonResult GetTicketsPerProjectData()
        {
            ProjectsHelper projectsHelper = new ProjectsHelper();
            var dataSet = new List<TicketPerProjectData>();
            var projects = projectsHelper.ListUserProjects(User.Identity.GetUserId());
            foreach (var project in projects)
            { 
                dataSet.Add(new TicketPerProjectData { Label = project.Name, Value = project.Tickets.Count() });
            
            }
           

            return Json(dataSet);
        }



        public long GetJavascriptTimestamp(DateTime input)
        {
            TimeSpan span = new TimeSpan(DateTime.Parse("1/1/1970").Ticks);
            DateTime time = input.Subtract(span);
            return (long)(time.Ticks / 10000);
        }



        public JsonResult GetTicketsPerDay()
        {
            var dataSet = new List<TicketsPerDay>();
           
            DateTime cutOfDate = DateTime.Now.AddDays(-30);
            DateTime currDate;
            var count = 0;

            var tickets = db.Tickets.Where(t => t.Created > cutOfDate).ToList();
            while (cutOfDate <= DateTime.Now)
            {
                currDate = cutOfDate.Date;
                count = 0;
                foreach (var ticket in tickets)
                { 
                    if(DateTime.Compare(ticket.Created.Date,currDate) == 0)
                    {
                        count++;
                    }
                
                }

                dataSet.Add(new TicketsPerDay { DayTimeOccurence = GetJavascriptTimestamp(currDate), occurences = count });
                cutOfDate = cutOfDate.AddDays(1);
            }

            return Json(dataSet);
        }



        public ActionResult Dashboard()
        {

            DashBoardVewModel dashboard = new DashBoardVewModel();
            ApplicationDbContext db = new ApplicationDbContext();
            dashboard.Notifications = db.TicketNotifications.Where(t => t.isRead == false).ToList();

            return View(dashboard);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Invalid login attempt.");
                return View(model);
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:








                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model); ;
            }
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
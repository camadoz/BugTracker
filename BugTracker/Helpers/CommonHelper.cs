using BugTracker.Enumerations;
using BugTracker.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Helpers
{
    public class CommonHelper
    {
        protected ApplicationDbContext db = new ApplicationDbContext();
        protected UserRolesHelper RoleHelper = new UserRolesHelper();
        protected ProjectsHelper ProjectHelper = new ProjectsHelper();
        protected ApplicationUser CurrentUser = null;
        protected SystemRole CurrentRole = SystemRole.None;

        protected CommonHelper()
        {
            var userId = HttpContext.Current.User.Identity.GetUserId();
            if (userId != null)
                CurrentUser = db.Users.Find(userId);

            //"Submitter" ==> SystemRole.Submitter          
            var stringRole = RoleHelper.ListUserRoles(userId).FirstOrDefault();
            stringRole = stringRole == "Project Manager" ? "ProjectManager" : stringRole;

            if (!string.IsNullOrEmpty(stringRole))
                CurrentRole = (SystemRole)Enum.Parse(typeof(SystemRole), stringRole);
        }
    }
}
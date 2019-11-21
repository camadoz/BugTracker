using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BugTracker.Controllers
{
    public class SeedController : Controller
    {
        // GET: Seed
        public ActionResult Run()
        {
            //var configuration = new Configuration();
            //var migrator = new DbMigrator(configuration);
            //migrator.Update();

            return View();
        }
    }
}
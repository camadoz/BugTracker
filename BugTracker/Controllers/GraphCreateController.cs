using BugTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BugTracker.Controllers
{
    [Authorize]
    [RequireHttps]
    public class GraphCreateController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: GraphCreate
        public ActionResult Index()
        {
            return View();
        }

        
        public JsonResult GetTicketsPerDays(int days)
        {


                var dataSet = new List<TicketsPerDay>();

                DateTime cutOfDate = DateTime.Now.AddDays(-1*days);
                DateTime currDate;
                var count = 0;

                var tickets = db.Tickets.Where(t => t.Created > cutOfDate).ToList();
                while (cutOfDate <= DateTime.Now)
                {
                    currDate = cutOfDate.Date;
                    count = 0;
                    foreach (var ticket in tickets)
                    {
                        if (DateTime.Compare(ticket.Created.Date, currDate) == 0)
                        {
                            count++;
                        }

                    }

                    dataSet.Add(new TicketsPerDay { DayTimeOccurence = GetJavascriptTimestamp(currDate), occurences = count });
                    cutOfDate = cutOfDate.AddDays(1);
                }

                return Json(dataSet);
        }

        public long GetJavascriptTimestamp(DateTime input)
        {
            TimeSpan span = new TimeSpan(DateTime.Parse("1/1/1970").Ticks);
            DateTime time = input.Subtract(span);
            return (long)(time.Ticks / 10000);
        }


    }
}
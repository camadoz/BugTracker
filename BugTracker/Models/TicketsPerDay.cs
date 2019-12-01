using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Models
{
    public class TicketsPerDay
    {
        public long DayTimeOccurence { get; set; }
        public int occurences { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BugTracker.Models;

namespace BugTracker.ViewModels
{
    public class IndexUsersViewModel
    {
        public ICollection<string> role { get; set; }
        public ICollection<Project>  Projects { get; set; }
        public ApplicationUser User { get; set; }
    }
}
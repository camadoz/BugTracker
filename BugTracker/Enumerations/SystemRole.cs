using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Enumerations
{
    public enum SystemRole
    {
        Admin,
        ProjectManager,
        Submitter,
        Developer,
        DemoAdmin,
        DemoDeveloper,
        DemoPM,
        DemoSubmitter,
        None
    }
}
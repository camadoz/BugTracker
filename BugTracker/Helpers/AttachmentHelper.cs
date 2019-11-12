using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace BugTracker.Helpers
{
    public class AttachmentHelper
    {
        public static string GetIconPath(string filePath)
        {
            string path = Path.GetExtension(filePath);

            var keyValue = WebConfigurationManager.AppSettings[path];
            var defaultValue = "/Images/Icons/blank.png";
            return string.IsNullOrEmpty(keyValue) ? defaultValue : keyValue;
        }
    }
}
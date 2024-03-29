﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BugTracker.Models
{
    public class TicketComment
    {
        public int Id { get; set; }
        public string AuthorId { get; set; }
        public int TicketId { get; set; }
        [AllowHtml]
        public string CommentBody { get; set; }
        public DateTime Created { get; set; }


        public virtual Ticket Ticket { get; set; }
        public virtual ApplicationUser Author { get; set; }
    }
}
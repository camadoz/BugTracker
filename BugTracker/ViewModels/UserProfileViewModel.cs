using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BugTracker.ViewModels
{
    public class UserProfileViewModel
    {
        public string Id { get; set; }

        [Required]
        [MaxLength(40, ErrorMessage = "First Name cannot be greater than 40 characters")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(40, ErrorMessage = "Last Name cannot be greater than 40 characters")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [MaxLength(20, ErrorMessage = "Display Name cannot be greater than 20 characters")]
        [Display(Name = "Display Name")]
        public string DisplayName { get; set; }

        public string PhoneNumber { get; set; }

        [Display(Name = "Avatar path")]
        public string AvatarUrl { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public HttpPostedFileBase Avatar { get; set; }
    }
}
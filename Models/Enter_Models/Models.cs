using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ExampleB.Models
{
    public class LoginModel
    {
        public string Name { get; set; }
        public string Password { get; set; }
    }
    public class RegisterModel
    {
        public int Id { get; set; }

        
        public string Name { get; set; }

        
        public string Email { get; set; }


        public bool? Subscription { get; set; }

        [Required]
        public string Password { get; set; }


        [Required(ErrorMessage = "Confirm Password required")]
        [Compare("Password")]
        [Display( Name = "Confirm")]
        public string ConfirmPassword { get; set; }
    }
}
﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;


namespace Group20_IoT.Models
{
    public class Login
    {
        [Required(ErrorMessage ="Please enter your Email")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Your email is invalid.")]
        [EmailAddress(ErrorMessage ="Your email is invalid")]
        public string Email { get; set; }

        [Required(ErrorMessage ="Please enter your Password")]
        public string Password { get; set; }

        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; }

    }
}
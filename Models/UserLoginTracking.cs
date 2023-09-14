using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Group20_IoT.Models
{
    public class UserLoginTracking
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        [Display(Name = "Login Date/Time")]
        public DateTime? UserLoginDateTime { get; set; }

        [Display(Name = "Logout Date/Time")]
        public DateTime? UserLogoutDateTime { get; set; }

        public double? Duration { get; set; }

        [ForeignKey("Users")]
        public int UserId { get; set; }

        public Users Users { get; set; }

    }
}
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

        public int UserId { get; set; }

        public DateTime? UserLoginDateTime { get; set; }

        public DateTime? UserLogoutDateTime { get; set; }

        public double? Duration { get; set; } 
    }
}
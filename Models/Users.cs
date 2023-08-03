using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.EnterpriseServices.Internal;

namespace Group20_IoT.Models
{
    public class Users
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        [Required]
        [MinLength(3,ErrorMessage ="Please enter a Name with atleast 3 characters"), MaxLength(40, ErrorMessage ="Your name cannot exceed 40 characters")]
        public string Name { get; set; }


        [Required]
        [Index(IsUnique = true)]
        [MaxLength(255)]
        [EmailAddress]
        public string Email { get; set; }


        [Required]
        [MinLength(8,ErrorMessage ="Please enter a Password with atleast 8 characters"), MaxLength(128, ErrorMessage ="Your Password cannot exceed 128 characters")]
        public string Password { get; set; }

        [Required]
        public bool Access { get; set; }

        [ForeignKey("Role")]
        [Required]
        public int RoleId { get; set; }

        public Role Role { get; set; }

    }
}
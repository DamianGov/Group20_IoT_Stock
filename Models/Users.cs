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
    [SessionChecker("Admin")]
    public class Users
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter a First Name")]
        [StringLength(40, ErrorMessage = "The First Name must not exceed 40 characters")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please enter a Surname")]
        [StringLength(40, ErrorMessage = "The Surname must not exceed 40 characters")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Please enter an Email")]
        [Index(IsUnique = true)]
        [StringLength(255, ErrorMessage = "The Email must not exceed 255 characters")]
        [EmailAddress(ErrorMessage = "Please enter a valid Email Address")]
        public string Email { get; set; }

        public string Password { get; set; }

        [Required(ErrorMessage = "Please enter a Study Year")]
        [Range(1,10,ErrorMessage = "The Study Year should be between 1 and 10")]
        [Display(Name = "Study Year")]
        public int StudyYear { get; set; }

        [Required(ErrorMessage = "Please enter a Qualification")]
        [StringLength(255, ErrorMessage = "The Qualification must not exceed 255 characters")]
        public string Qualification { get; set; }

        public bool Access { get; set; } = true;

        [ForeignKey("Role")]
        [Required(ErrorMessage ="Please select a Role")]
        public int RoleId { get; set; }

        public Role Role { get; set; }

        [Display(Name = "Created By")]
        public int CreatedBy { get; set; }


        public string GetFullName()
        {
            return FirstName + " " + Surname;
        }
    }
}
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
    [SessionCheckerAdmin]
    public class Users
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter a User Name")]
        [StringLength(40, ErrorMessage ="The User Name must not exceed 40 characters")]
        [Display(Name = "User Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter an Email")]
        [Index(IsUnique = true)]
        [StringLength(255, ErrorMessage = "The Email must not exceed 255 characters")]
        [EmailAddress(ErrorMessage = "Please enter a valid Email Address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter a Password")]
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

    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Group20_IoT.Models
{
    public class Room
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        [Required]
        [Index(IsUnique = true)]
        [Display(Name = "Room Number")]
        [StringLength(255, ErrorMessage = "The Room Number must not exceed 255 characters")]
        public string Room_Number { get; set; }

        [Required]
        [Display(Name = "Room Description")]
        [MinLength(5,ErrorMessage = "The Description must be atleast 5 characters"), StringLength(25,ErrorMessage = "The Description must not exceed 25 characters")]
        public string Room_Description { get; set; }

        public bool Active { get; set; } = true;
        [Display(Name = "Created By")]
        public int CreatedBy { get; set; }
    }
}
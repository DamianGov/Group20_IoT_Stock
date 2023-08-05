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
        [MaxLength(255)]
        public string Room_Number { get; set; }

        [Required]
        [Display(Name = "Room Description")]
        [MinLength(5,ErrorMessage = "The Description must be atleast 5 characters"),MaxLength(20,ErrorMessage = "The Description cannot exceed 20 characters")]
        public string Room_Description { get; set; }

        public bool Active { get; set; } = true;

    }
}
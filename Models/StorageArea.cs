using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Group20_IoT.Models
{
    public class StorageArea
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter a Area Name")]
        [StringLength(255, ErrorMessage = "The Area Name must not exceed 255 characters")]
        [Display(Name = "Area Name")]
        public string Area_Name { get; set; }

        public bool Active { get; set; } = true;

        [ForeignKey("Room")]
        [Required(ErrorMessage = "Please select a Room")]
        public int RoomId { get; set; }

        public Room Room { get; set; }

        [Display(Name = "Created By")]
        public int CreatedBy { get; set; }
    }
}
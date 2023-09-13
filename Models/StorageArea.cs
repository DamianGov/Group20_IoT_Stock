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

        [Required]
        [MaxLength(255)]
        public string Area_Name { get; set; }

        public bool Active { get; set; } = true;

        [ForeignKey("Room")]
        [Required]
        public int RoomId { get; set; }

        public Room Room { get; set; }

        public int CreatedBy { get; set; }
    }
}
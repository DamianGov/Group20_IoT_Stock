using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Group20_IoT.Models
{
    public class Role
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        [Index(IsUnique = true)]
        [MaxLength(50)]
        [Required(ErrorMessage = "Please enter the Role")]
        public string Type { get; set; }
    }
}
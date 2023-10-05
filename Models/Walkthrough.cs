using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Group20_IoT.Models
{
    public class Walkthrough
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter the Walkthrough Date")]
        [Display(Name = "Walkthrough Date")]
        public DateTime WalkthroughDate { get; set; }

        [Required(ErrorMessage = "Please enter a Description")]
        [StringLength(250, ErrorMessage = "The Description must not exceed 250 characters")]
        [Display(Name = "Description")]
        public string WalkthroughDescription { get;set; }

        public DateTime DateCreated { get; set; } = DateTime.Now;

        [ForeignKey("Users")]
        public int CreatedBy { get; set; }

        public bool StockStillAllocated { get; set; } = true;

        public virtual Users Users { get; set; }
    }
}
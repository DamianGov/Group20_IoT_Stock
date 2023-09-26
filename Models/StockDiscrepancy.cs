using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Group20_IoT.Models
{
    public class StockDiscrepancy
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Please select Stock")]
        [ForeignKey("Stock")]
        [Display(Name = "Stock")]
        public int StockId { get; set; }

        [Required(ErrorMessage = "Please enter the Quantity")]
        [Range(1,int.MaxValue,ErrorMessage = "Please enter a Quantity more than 0")]
        [Display(Name = "Quantity")]
        public int Quantity { get; set; }

        [Display(Name = "Discrepancy Category")]
        public eDiscrepancyCategory DiscrepancyCategory { get; set; }

        public string Note { get; set; }

        [Required(ErrorMessage = "Please enter the Date the Discrepancy was Found")]
        [Display(Name = "Date Discrepancy Found")]
        public DateTime DiscrepancyFound { get; set; }

        [ForeignKey("Users")]
        public int CreatedBy { get; set; }

        [Display(Name = "Date Created")]
        public DateTime CreationDate { get; set; } = DateTime.Now;

        public bool Resolved { get; set; } = false;

        [Display(Name = "Resolved Date")]
        public DateTime? ResolvedDate { get; set; }

        public virtual Users Users { get; set; }

        public virtual Stock Stock { get; set; }

        public enum eDiscrepancyCategory
        {
            [Display(Name = "Minor Defect")]
            Minor_Defect,
            [Display(Name = "Mediocre Defect")]
            Mediocre_Defect,
            [Display(Name = "Major Defect")]
            Major_Defect,
            [Display(Name = "Missing/Stolen")]
            Missing
        }
    }
}
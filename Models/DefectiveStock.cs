using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Group20_IoT.Models
{
    public class DefectiveStock
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Please select Stock")]
        [ForeignKey("Stock")]
        [Display(Name = "Stock")]
        public int StockId { get; set; }

        [Required(ErrorMessage = "Please enter the Quantity Defective")]
        [Range(1,int.MaxValue,ErrorMessage = "Please enter a Quantity more than 0")]
        [Display(Name = "Quantity Defective")]
        public int Quantity { get; set; }

        [Display(Name = "Defective Category")]
        public eDefectiveCategory DefectiveCategory { get; set; }

        public string Note { get; set; }

        [Required(ErrorMessage = "Please enter the Date the Defect was Found")]
        [Display(Name = "Date Defect Found")]
        public DateTime DefectFound { get; set; }

        [ForeignKey("Users")]
        public int CreatedBy { get; set; }

        [Display(Name = "Date Created")]
        public DateTime CreationDate { get; set; } = DateTime.Now;

        public bool Resolved { get; set; } = false;

        [Display(Name = "Resolved Date")]
        public DateTime? ResolvedDate { get; set; }

        public virtual Users Users { get; set; }

        public virtual Stock Stock { get; set; }

        public enum eDefectiveCategory
        {
            [Display(Name = "Minor Defect")]
            Minor,
            [Display(Name = "Major Defect")]
            Major
        }
    }
}
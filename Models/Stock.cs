using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Group20_IoT.Models
{
    public class Stock
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter the Stock Code")]
        [Index(IsUnique = true)]
        [StringLength(255, ErrorMessage = "The Stock Code must not exceed 255 characters")]
        [Display(Name = "Stock Code")]
        public string StockCode { get; set; }

        [Required(ErrorMessage = "Please enter the Stock Name")]
        [StringLength(100,ErrorMessage ="The Stock Name must not exceed 100 characters")]
        [Display(Name = "Stock Name")]
        public string Name { get; set; }

        [Display(Name="Total Quantity")]
        [Range(0,int.MaxValue,ErrorMessage = "Please enter a valid Total Quantity")]
        public int TotalQuantity { get; set; } = 0;

        [Display(Name = "Quantity on Loan")]
        public int QuantityOnLoan { get; set; } = 0;

        public bool Loanable { get; set; } = true;

        [Range(0.00,double.MaxValue,ErrorMessage ="Please enter a valid Unit Price")]
        [Display(Name = "Unit Price")]
        public decimal UnitPrice { get; set; } = 0;

        [ForeignKey("StorageArea")]
        [Required(ErrorMessage = "Please select a Storage Area")]
        [Display(Name = "Storage Area")]
        public int StorageAreaId { get; set; }

        public StorageArea StorageArea { get; set; }
        [Display(Name = "Created By")]
        public int CreatedBy { get; set; }

        [Display(Name = "Stock Image")]
        public string ImageFile { get; set; }

    }
}
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

        [Required]
        [Index(IsUnique = true)]
        [MaxLength(255)]
        [Display(Name = "Stock Code")]
        public string StockCode { get; set; }

        [Required]
        [MinLength(1),MaxLength(100)]
        public string Name { get; set; }

        public int TotalQuantity { get; set; } = 0;
        public int QuantityOnLoan { get; set; } = 0;

        public bool Loanable { get; set; } = true;

        [ForeignKey("StorageArea")]
        [Required(ErrorMessage = "Please select a Storage Area")]
        [Display(Name = "Storage Area")]
        public int StorageAreaId { get; set; }

        public StorageArea StorageArea { get; set; }

        public int CreatedBy { get; set; }

        [Display(Name = "Stock Image")]
        public string ImageFile { get; set; }

    }
}
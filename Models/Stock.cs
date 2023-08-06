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

        public int Quantity { get; set; } = 0;

        [Display(Name = "Quantity Borrowed")]
        public int QuantityBorrowed { get; set; } = 0;

        public bool Active { get; set; } = true;

        //public DateTime DateCreated { get; set; }


        [ForeignKey("StorageArea")]
        [Required(ErrorMessage = "Please select a Storage Area")]
        [Display(Name = "Storage Area")]
        public int StorageAreaId { get; set; }

        public StorageArea StorageArea { get; set; }

        public int CreatedBy { get; set; }

    }
}
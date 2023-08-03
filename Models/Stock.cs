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
        [MinLength(1),MaxLength(100)]
        public string Name { get; set; }

        public int Quantity { get; set; } = 0;

        public int QuantityBorrowed { get; set; } = 0;

        public DateTime? LastBorrowedDate { get; set; }

        public DateTime? LastReturnedDate { get; set; }

        [ForeignKey("StorageArea")]
        [Required]
        public int StorageAreaId { get; set; }

        public StorageArea StorageArea { get; set; }

    }
}
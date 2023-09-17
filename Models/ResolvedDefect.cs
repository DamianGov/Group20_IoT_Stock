using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Group20_IoT.Models
{
    public class ResolvedDefect
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("DefectiveStock")]
        public int DefStockId { get; set; }

        public int Quantity { get; set; }

        public string ResolvedNote { get; set; }

        public virtual DefectiveStock DefectiveStock { get; set; }

    }
}
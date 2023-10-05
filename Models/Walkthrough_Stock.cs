using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Group20_IoT.Models
{
    public class Walkthrough_Stock
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("Walkthrough")]
        public int WalkthroughId { get; set; }

        [ForeignKey("Stock")]
        public int StockId { get; set;}

        public int Quantity { get; set; }   

        public virtual Walkthrough Walkthrough { get; set; }

        public virtual Stock Stock { get; set; }
    }
}
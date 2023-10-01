using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Group20_IoT.Models
{
    public class LoanStatus
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("RequestLoanStock")]
        public int RequestId { get;set; }

        [ForeignKey("Users")]
        public int AccBy { get; set; }

        public string Note { get; set; }    

        public DateTime AccOn { get; set; } = DateTime.Now;

        public bool RequestExtension { get; set; } = false;

        public LoanStatusStock Status { get; set; }

        public virtual RequestLoanStock RequestLoanStock { get; set; }

        public virtual Users Users { get; set; }

        public enum LoanStatusStock
        {
            [Display(Name = "Awaiting Pickup")]
            Awaiting_Pickup,
            [Display(Name = "Picked up")]
            Picked_Up,
            Returned,
            Overdue,
            Cancelled
       }
        
    }
}
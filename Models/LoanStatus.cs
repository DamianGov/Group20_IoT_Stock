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

        public string Note { get; set; }

        [ForeignKey("Users")]
        public int AccRejBy { get; set; }

        public virtual RequestLoanStock RequestLoanStock { get; set; }

        public virtual Users Users { get; set; }


        
    }
}
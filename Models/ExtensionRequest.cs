using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Group20_IoT.Models
{
    public class ExtensionRequest
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("LoanStatus")]
        public int LoanStatusId { get; set; }

        public string Reason { get; set; }  

        public ExtensionStatus Status { get; set; }

        public virtual LoanStatus LoanStatus { get; set; }

        public DateTime ExtensionSubmitted { get; set; } = DateTime.Now;

        public enum ExtensionStatus
        {
            Pending,
            Accepted,
            Rejected
        }
    }
}
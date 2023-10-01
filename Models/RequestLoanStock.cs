using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Group20_IoT.Models
{
    public class RequestLoanStock
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("Stock")]
        public int StockId { get; set; }

        [ForeignKey("Users")]
        public int UserId { get; set; }

        public int Quantity { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime DueDate { get; set; } // Populate automatically

        public DateTime DateRequested { get; set; } = DateTime.Now;

        public RequestStatus Status { get; set; } = RequestStatus.Pending;

        public virtual Stock Stock { get; set; }

        public virtual Users Users { get; set; }

        public enum RequestStatus
        {
            Pending,
            Accepted,
            Rejected,
            Cancelled
        }

    }
}
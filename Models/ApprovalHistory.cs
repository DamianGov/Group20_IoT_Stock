using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Group20_IoT.Models
{
    public class ApprovalHistory
    {
        // TODO: Sort out the bottom
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        [ForeignKey("RequestStock")]
        public int RequestId { get; set; }

        [ForeignKey("Users")]
        public int AdminId { get; set; }

        public DateTime ApprovalDate { get; set; } = DateTime.Now;

        public virtual RequestStock RequestStock { get; set; }
        public virtual Users Users { get; set; }
    }
}
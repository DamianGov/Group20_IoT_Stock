using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Group20_IoT.Models
{
    public class RequestStock
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        // Created By
        [ForeignKey("Users")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Please enter the Stock Name")]
        [Display(Name = "Stock Name")]
        [StringLength(255,ErrorMessage = "The Stock Name must not exceed 255 characters")]
        public string StockName { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Please enter a valid Quantity")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Please enter the Stock Price")]
        [Display(Name = "Stock Price")]
        [Range(0.05,double.MaxValue, ErrorMessage = "Please enter a valid Stock Price")]
        public double StockPrice { get; set; }

        [Required(ErrorMessage = "Please enter the link to the Stock")]
        [Display(Name = "Stock Link")]
        [StringLength(2000, ErrorMessage = "The Stock Link should not exceed 2000 characters")]
        [RegularExpression(@"^(https?://)?([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?$", ErrorMessage = "Please enter a valid Stock Link")]
        public string StockLink { get; set; }

        public string StockImage { get; set; }
        [Display(Name = "Request Date")]
        public DateTime RequestDate { get; set; } = DateTime.Now;
        [Display(Name = "Approved")]
        public bool IsApproved { get; set; } = false;

        public virtual Users Users { get; set; }
    }
}
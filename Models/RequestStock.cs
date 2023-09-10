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

        [ForeignKey("Users")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Please enter the name of the Stock")]
        [Display(Name = "Stock Name")]
        [MaxLength(255)]
        public string StockName { get; set; }

        [Required(ErrorMessage = "Please enter the Stock Price")]
        [Display(Name = "Stock Price")]
        [Range(0.1,double.MaxValue)]
        public double StockPrice { get; set; }

        [Required(ErrorMessage = "Please enter the link to the Stock ")]
        [Display(Name = "Link To Stock Item")]
        [MaxLength(255)]
        [RegularExpression(@"^(https?://)?([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?$", ErrorMessage = "Please enter a valid URL")]
        public string StockLink { get; set; }

        public string StockImage { get; set; }

        public DateTime RequestDate { get; set; } = DateTime.Now;

        public bool IsApproved { get; set; } = false;

        public virtual Users Users { get; set; }
    }
}
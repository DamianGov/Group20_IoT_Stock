using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Group20_IoT.Models.ViewModel
{
    public class LoanStockDetailsViewModel
    {
        public int StockId { get; set; }

        public string StockCode { get; set; }

        public string Name { get; set; }    

        public string Description { get; set; }

        public int QuantityAva { get; set; }

        public string Image { get; set; }

        [Required(ErrorMessage = "Please enter the Quantity you want to loan")]
        [Range(1, int.MaxValue,ErrorMessage ="Please enter a valid Quantity")]
        public int QuantityWantToLoan { get; set; }

        [Required(ErrorMessage = "Please choose a Borrow Start Date")]
        public DateTime BorrowStartDate { get; set; }


    }
}
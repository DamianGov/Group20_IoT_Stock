using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Group20_IoT.Models.ViewModel
{
    public class PendingLoanStockViewModel
    {
        public int Id { get; set; }
        public string UserName { get; set; }   
        public string UserRole { get; set; }
        public string StockCode { get; set; }   
        public string StockName { get; set;}
        public int AmntAva { get; set; }
        public int QuantityWant { get; set; }
        public string DateRequested { get; set; }
        public string BorrowDate { get; set; }
        public string DueDate { get; set; }
    }
}
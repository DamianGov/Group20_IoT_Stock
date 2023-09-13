using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Group20_IoT.Models.ViewModel
{
    public class StockViewModel
    {
        public int? Id { get; set; }
        public string StockCode { get; set; }
        public string Name { get; set; }
        public string UnitPrice { get; set; }
        public string Loanable { get; set; }
        public int TotalQuantity { get; set; }  
        public int QuantityOnLoan { get; set; }
        public string Room { get; set; }
        public string StorageArea { get; set; }
        public string Image { get; set; }
    }
}
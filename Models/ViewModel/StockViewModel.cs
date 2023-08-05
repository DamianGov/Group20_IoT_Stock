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
        public int Quantity { get; set; }   
        public int QuantityBorrowed { get; set; }
        public string Room { get; set; }
        public string StorageArea { get; set; }
    }
}
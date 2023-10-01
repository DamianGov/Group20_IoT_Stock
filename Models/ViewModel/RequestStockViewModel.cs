using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Group20_IoT.Models.ViewModel
{
    public class RequestStockViewModel
    {
        public int? Id { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string StockName { get; set; }
        public int Quantity { get; set; }
        public string StockPrice { get; set; }
        public string StockLink { get; set; }
        public string StockImage { get; set;}
        public string RequestDate { get; set; }
        public string Approved { get; set; }
        public string ApprovedBy { get; set; }
    }
}
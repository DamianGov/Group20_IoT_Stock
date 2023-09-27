using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Group20_IoT.Models.ViewModel
{
    public class LoanStockViewModel
    {
        public int Id { get; set; }

        public string StockCode { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string ImageFile { get; set; }

        public int QuantityCanLoan { get; set; }
    }
}
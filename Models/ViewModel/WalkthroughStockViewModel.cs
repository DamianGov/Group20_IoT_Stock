using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Group20_IoT.Models.ViewModel
{
    public class WalkthroughStockViewModel
    {
        public Walkthrough Walkthrough { get; set; }
        public List<StockItemsWalkthrough> Stocks { get; set; }
    }

    public class StockItemsWalkthrough
    {
        public int Id { get; set; }
        public string Stockcode { get; set; }   
        public string Name { get; set; }
        public int QuantityAvailable { get; set; }
        public int Quantity { get; set; } = 1;
        public bool Selected { get; set; } = false;
    }
}
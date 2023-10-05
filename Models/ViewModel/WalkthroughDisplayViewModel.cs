using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Group20_IoT.Models.ViewModel
{
    public class WalkthroughDisplayViewModel
    {
        public Walkthrough Walkthrough { get; set; }
        public List<Walkthrough_Stock> Walkthrough_Stocks { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Group20_IoT.Models.ViewModel
{
    public class StorageAreaViewModel
    {
        public int Id { get; set; }
        public string Area_Name { get; set; }
        public string Active { get; set; }
        public string RoomName { get; set; }
    }
}
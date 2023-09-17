using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Group20_IoT.Models.ViewModel
{
    public class UserTrackingViewModel
    {
        public int? Id { get; set; }

        public string Name { get; set; }    

        public string Email { get; set; }

        public string LoggedIn { get; set; }

        public string LoggedOut { get; set; }

        public string Duration { get; set; }

        public string UserType { get; set; }

        public string UsedRememberMe { get; set; }
    }
}
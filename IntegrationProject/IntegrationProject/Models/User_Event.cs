using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IntegrationProject.Models
{
    public class User_Event
    {
        [Key]
        public int ID { get; set; }

        public User User { get; set; }
        public int UserID { get; set; }

        public VolunteerEvent VolunteerEvent { get; set; }
        public int VolunteerEventID { get; set; }

    }
}
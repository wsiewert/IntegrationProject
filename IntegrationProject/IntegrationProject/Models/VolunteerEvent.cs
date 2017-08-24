using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IntegrationProject.Models
{
    public class VolunteerEvent
    {
        [Key]
        public int ID { get; set; }

        [Display (Name="Event Name")]
        public string EventName { get; set; }

        public string HostID { get; set; }

        public string Description { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        [RegularExpression(@"\d{5}$", ErrorMessage = "Invalid Zip Code")]
        public string Zip { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
    }
}
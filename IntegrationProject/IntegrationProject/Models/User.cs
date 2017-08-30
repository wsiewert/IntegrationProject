using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace IntegrationProject.Models
{
    public class User
    {
        [Key]
        public int ID { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [RegularExpression(@"\d{10}$", ErrorMessage = "Invalid Phone Number")]
        public string Phone { get; set; }

        public string Description { get; set; }

        [Display (Name = "Up Vote")]
        public int VolunteerUpVotes { get; set; }

        [Display(Name = "Down Vote")]
        public int VolunteerDownVotes { get; set; }

        [Display(Name = "Up Vote")]
        public int EventUpVotes { get; set; }

        [Display(Name = "Down Vote")]
        public int EventDownVotes { get; set; }

        [Display(Name = "No Show")]
        public int NoShowCount { get; set; }

        
    }
}
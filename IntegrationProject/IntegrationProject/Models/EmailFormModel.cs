using System.ComponentModel.DataAnnotations;

namespace MVCEmail.Models
{
    public class EmailFormModel
    {
        [Required, Display(Name = "Your Name")]
        public string FromName { get; set; }
        [Required, Display(Name = "Your Email"), EmailAddress]
        public string FromEmail { get; set; }
        [Required]
        public string Message { get; set; }
    }
}
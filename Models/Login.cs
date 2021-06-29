using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryClient.Models
{
    public class Login
    {
        [Key]
        [Display(Name = "Enter User Name")]

        [Required(ErrorMessage = "{0} is required")]
        public string userName { get; set; }

        [DataType(DataType.Password)]
 
        [Display(Name = "Enter Password")]
 
        [Required(ErrorMessage = "{0} is required")]
        public string pass { get; set; }
        [Required]
        [Display(Name = "Select User Type")]
        public string type { get; set; }
    }
}

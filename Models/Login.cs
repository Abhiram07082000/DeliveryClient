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
        [RegularExpression(@"^[a-zA-Z\d]+$", ErrorMessage = "Invalid User Name")]
        [Required(ErrorMessage = "{0} is required")]
        public string userName { get; set; }

        [DataType(DataType.Password)]
        [StringLength(15, ErrorMessage = "Please Enter 8 - 15 characters", MinimumLength = 8)]
        [Display(Name = "Enter Password")]
        [RegularExpression(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[\W]).*$", ErrorMessage = "Invalid Password")]
        [Required(ErrorMessage = "{0} is required")]
        public string pass { get; set; }
        [Required]
        public string type { get; set; }
    }
}

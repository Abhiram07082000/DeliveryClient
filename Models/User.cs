using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryClient.Models
{
    public class User
    {
        [Key]
        [Required]
        public int UserId { get; set; }
        
        [DataType(DataType.Text)]
        [Display(Name = "Name")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Special Characters and spaces are not allowed")]
        [Required(ErrorMessage = "{0} is required")]
        public string Name { get; set; }

        [Display(Name = "User Name")]
        [RegularExpression(@"^[a-zA-Z\d]+$", ErrorMessage = "Special Characters and spaces are not allowed")]
        [Required(ErrorMessage = "{0} is required")]
        public string UserName { get; set; }

        [DataType(DataType.Password)]
        [StringLength(15, ErrorMessage = "Please Enter 8 - 15 characters", MinimumLength = 8)]
        [Display(Name = "Password")]
        [RegularExpression(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[\W]).*$", ErrorMessage = "Password must have an Uppercase, Lowercase,Special character and a number")]
        [Required(ErrorMessage = "{0} is required")]
        public string Password { get; set; }
        
        [Required]
        [RegularExpression(@"^[+]?([0-9]+(?:[\.][1-9]*)?|\.[1-9]+)$", ErrorMessage = "Enter a Positive Number")]
        public int Age { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Phone Number")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Not a number")]
        [Required(ErrorMessage = "{0} is required")]
        [StringLength(10,ErrorMessage = "Please 10 digits")]
        public string Phone { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string UserType { get; set; }

    }
}

using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryClient.Models
{
    [Authorize]
    public class DeliveryBooking
    {
        
        [Key]
        public int BookingId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        
        [DataType(DataType.DateTime)]
        [Display(Name = "Enter the Date and Time")]
        public DateTime DateAndTime { get; set; }
        
        [Display(Name = "Enter the Weight(in Kgs)")]
        public float Weight { get; set; }
        public string Address { get; set; }
        public string City { get; set; }

        [Display(Name = "Choose a Delivery Executive of your choice")]
        public string DeliveryExecutive { get; set; }
        public string BookingStatus { get; set; }
        public float Price { get; set; }
        
    }
}

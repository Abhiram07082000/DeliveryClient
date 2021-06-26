using DeliveryClient.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace DeliveryClient.Controllers
{
    public class StatusCheckController : Controller
    {
            public StatusCheckController()
            {

            }
            public IActionResult StatusCheck()
            {
                return View();
            }
        
            [HttpPost]
            public IActionResult StatusCheck(StatusCheck d)
            {
                return RedirectToAction("StatusCheck", "DeliveryBooking", new { id = d.BookingId });
                    
            }
                  
    }
}

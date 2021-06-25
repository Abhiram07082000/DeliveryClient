using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryClient.Controllers
{
    public class UserDashboardController : Controller
    {
        public IActionResult Index()
        {
            string msg = HttpContext.Session.GetString("userName");
            if (msg != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
            
        }
    }
}

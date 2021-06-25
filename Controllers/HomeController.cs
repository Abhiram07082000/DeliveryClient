using DeliveryClient.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryClient.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        string Baseurl = "https://localhost:44372/";
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        public IActionResult Index()
        {
           
            return View();
        }
        [HttpPost]
         public async Task<IActionResult> Login(Login l)
         {
            List<User> User = new List<User>();

            using (var client = new HttpClient())
            {
                
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = await client.GetAsync("api/Users/GetAllUsers");
                if (Res.IsSuccessStatusCode)
                {
                    var UserResponse = Res.Content.ReadAsStringAsync().Result;
                    User = JsonConvert.DeserializeObject<List<User>>(UserResponse);
                }


                User UserInfo = (from i in User
                                  where i.UserName == l.userName && i.Password == l.pass &&i.UserType==l.type
                             select i).FirstOrDefault();
                
                if(UserInfo!= null)
                {
                    String Username = UserInfo.UserName;
                    HttpContext.Session.SetString("username", Username);
                    return RedirectToAction("CreateBooking","DeliveryBooking");
                }
                else
                {
                    return View();
                }

            }     
            
         }


        public IActionResult CreateBooking()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateBooking(DeliveryBooking p)
        {
            DeliveryBooking Pobj = new DeliveryBooking();
            //  HttpClient obj = new HttpClient();
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(Baseurl);
                StringContent content = new StringContent(JsonConvert.SerializeObject(p), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PostAsync("api/DeliveryBooking", content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    Pobj = JsonConvert.DeserializeObject<DeliveryBooking>(apiResponse);
                }
            }
            return View();
        }



        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> Create()
        {
            return await Task.Run(() => View());
        }
        [HttpPost]
        public async Task<IActionResult> Create(User p)
        {
            User Uobj = new User();
            //  HttpClient obj = new HttpClient();
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(Baseurl);
                StringContent content = new StringContent(JsonConvert.SerializeObject(p), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PostAsync("api/Users", content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    Uobj = JsonConvert.DeserializeObject<User>(apiResponse);
                }
            }
            return View();
        }

    }
}

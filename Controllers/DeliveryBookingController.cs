using DeliveryClient.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryClient.Controllers
{
    public class DeliveryBookingController : Controller
    {
        // GET: DeliveryBookingController
        string Baseurl = "https://localhost:44372/";
        public async Task<IActionResult> GetAllBookings()
        {
            List<DeliveryBooking> BookingInfo = new List<DeliveryBooking>();


            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = await client.GetAsync("api/DeliveryBookings");

                if (Res.IsSuccessStatusCode)
                {
                    var ProductResponse = Res.Content.ReadAsStringAsync().Result;
                    BookingInfo = JsonConvert.DeserializeObject<List<DeliveryBooking>>(ProductResponse);

                }

                return View(BookingInfo);
            }
        }

        public async Task<IActionResult> ListByUser()
        {
            //List<User> User = new List<User>();
            List<DeliveryBooking> BookingInfo = new List<DeliveryBooking>();
            List<DeliveryBooking> UserBookings = new List<DeliveryBooking>();

            using (var client = new HttpClient())
            {
                string Baseurl = "https://localhost:44372/";
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //HttpResponseMessage Res = await client.GetAsync("api/Users");
                HttpResponseMessage Response = await client.GetAsync("api/DeliveryBookings");
                //if (Res.IsSuccessStatusCode)
                //{
                //    var UserResponse = Res.Content.ReadAsStringAsync().Result;
                //    User = JsonConvert.DeserializeObject<List<User>>(UserResponse);
                //}
                if (Response.IsSuccessStatusCode)
                {
                    var UserRes = Response.Content.ReadAsStringAsync().Result;
                    BookingInfo = JsonConvert.DeserializeObject<List<DeliveryBooking>>(UserRes);
                }
                string Username = HttpContext.Session.GetString("username");
                foreach (var i in BookingInfo)
                {
                    int res = String.Compare(i.UserName, Username);
                    if (res == 0)
                    {
                        UserBookings.Add(i);
                    }
                }


                return View(UserBookings);
            }
        }

        [HttpGet]
            public async Task<IActionResult> StatusCheck(int id)
            {
                //TempData["BookingId"] = id;
                DeliveryBooking p = new DeliveryBooking();
                
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.GetAsync("https://localhost:44372/api/DeliveryBookings/" + id))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        p = JsonConvert.DeserializeObject<DeliveryBooking>(apiResponse);
                    }
                }
                return View(p);
            }


        public async Task<IActionResult> GetById(string name)
        {
            string usname = HttpContext.Session.GetString("username");
            //List<User> User = new List<User>();
            List<DeliveryBooking> BookingInfo = new List<DeliveryBooking>();
            List<DeliveryBooking> DelList = new List<DeliveryBooking>();
           
            using (var client = new HttpClient())
            {
                string Baseurl = "https://localhost:44372/";
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //HttpResponseMessage Res = await client.GetAsync("api/Users");
                HttpResponseMessage Response = await client.GetAsync("api/DeliveryBookings");
                //if (Res.IsSuccessStatusCode)
                //{
                //    var UserResponse = Res.Content.ReadAsStringAsync().Result;
                //    User = JsonConvert.DeserializeObject<List<User>>(UserResponse);
                //}
                if (Response.IsSuccessStatusCode)
                {
                    var UserRes = Response.Content.ReadAsStringAsync().Result;
                    BookingInfo = JsonConvert.DeserializeObject<List<DeliveryBooking>>(UserRes);
                }

                DelList = (from s in BookingInfo
                                where s.DeliveryExecutive == usname
                                select s).ToList();

                return View(DelList);
            }
        }

         public async Task<IActionResult> CreateBooking()
            {
            List<User> UserList = new List<User>();
            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = await client.GetAsync("api/Users");

                if (Res.IsSuccessStatusCode)
                {
                    var UserResponse = Res.Content.ReadAsStringAsync().Result;
                    UserList = JsonConvert.DeserializeObject<List<User>>(UserResponse);

                }
            }
                string Username = HttpContext.Session.GetString("username");

                var createobj = (from k in UserList
                                 where k.UserName == Username
                                 select k).FirstOrDefault();
                

                if (createobj != null)
                {
                    ViewBag.Message = createobj;
                }

                var usercity = createobj.City;
                //List<User> executives = new List<User>();
                var executives = (from j in UserList
                                  where j.City == usercity && j.UserType == "Delivery Executive"
                                  select j.UserName).ToList();
                ViewBag.executives = new SelectList(executives);
                return View();
            }
            
            [HttpPost]
            public async Task<IActionResult> CreateBooking(DeliveryBooking p)
            {
                DeliveryBooking Pobj = new DeliveryBooking();
                //  HttpClient obj = new HttpClient();
                using (var httpClient = new HttpClient())
                {
                    string Baseurl = "https://localhost:44372/";
                    httpClient.BaseAddress = new Uri(Baseurl);
                    StringContent content = new StringContent(JsonConvert.SerializeObject(p), Encoding.UTF8, "application/json");

                    using (var response = await httpClient.PostAsync("api/DeliveryBookings", content))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        Pobj = JsonConvert.DeserializeObject<DeliveryBooking>(apiResponse);
                        //Pobj.Price = Pobj.Weight * 30;
                    }
                }
                
                                
                   return RedirectToAction("ListByUser");
            }

           

            //[HttpPost]
            //public async Task<IActionResult> PriceCalc(float wt)
            //{
            //    return await View();
            //}
            public async Task<IActionResult> EditBooking(int id)
            {
                DeliveryBooking p = new DeliveryBooking();
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.GetAsync("https://localhost:44372/api/DeliveryBookings/" + id))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        p = JsonConvert.DeserializeObject<DeliveryBooking>(apiResponse);
                    }
                }
                return View(p);

            }

            [HttpPost]
            public async Task<IActionResult> EditBooking(DeliveryBooking p)
            {
            DeliveryBooking p1 = new DeliveryBooking();
                using (var httpClient = new HttpClient())
                {
                    int id = p.BookingId;
                    StringContent content1 = new StringContent(JsonConvert.SerializeObject(p), Encoding.UTF8, "application/json");
                    using (var response = await httpClient.PutAsync("https://localhost:44372/api/DeliveryBookings/" + id, content1))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        ViewBag.Result = "Success";
                        p1 = JsonConvert.DeserializeObject<DeliveryBooking>(apiResponse);
                    }
                }
                return RedirectToAction("GetById");
            }

            [HttpGet]
            public async Task<ActionResult> DeleteBooking(int id)
            {
                TempData["BookingId"] = id;
                DeliveryBooking e = new DeliveryBooking();
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.GetAsync("https://localhost:44372/api/DeliveryBookings/" + id))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        e = JsonConvert.DeserializeObject<DeliveryBooking>(apiResponse);
                    }
                }
                return View(e);

            }

            [HttpPost]
            public async Task<ActionResult> DeleteBooking(DeliveryBooking p)
            {
                int bkid = Convert.ToInt32(TempData["BookingId"]);
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.DeleteAsync("https://localhost:44372/api/DeliveryBookings/" + bkid))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                    }
                }

                return RedirectToAction("GetAllBookings");
            }
      }
}


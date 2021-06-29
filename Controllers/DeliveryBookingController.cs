using DeliveryClient.Models;
using Microsoft.AspNetCore.Authorization;
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

        //Get all the Bookings of Users
        public async Task<IActionResult> GetAllBookings()
        {
            try
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
                }
                return View(BookingInfo);
            }
            catch(Exception)
            {
                ModelState.AddModelError("", "Invalid Operation");
                return View();
            }
        }

        //Get all the Bookings of a Particular User by UserName
        public async Task<IActionResult> ListByUser()
        {
            try
            {


                List<DeliveryBooking> BookingInfo = new List<DeliveryBooking>();
                List<DeliveryBooking> UserBookings = new List<DeliveryBooking>();
                using (var client = new HttpClient())
                {
                    string Baseurl = "https://localhost:44372/";
                    client.BaseAddress = new Uri(Baseurl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage Response = await client.GetAsync("api/DeliveryBookings");
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
            catch (Exception)
            {
                ModelState.AddModelError("", "Invalid Operation");
                return View();
            }

         }
     


        //Check the status of a particular booking by Booking Id
        [HttpGet]
            public async Task<IActionResult> StatusCheck(int id)
            {
                try
                {
                    string usname = HttpContext.Session.GetString("username");
                    DeliveryBooking p = new DeliveryBooking();
                    List<DeliveryBooking> BookingInfo = new List<DeliveryBooking>();
                    List<DeliveryBooking> DelList = new List<DeliveryBooking>();
                    using (var client = new HttpClient())
                    {
                        string Baseurl = "https://localhost:44372/";
                        client.BaseAddress = new Uri(Baseurl);
                        client.DefaultRequestHeaders.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        HttpResponseMessage Response = await client.GetAsync("api/DeliveryBookings");
                        if (Response.IsSuccessStatusCode)
                        {
                            var UserRes = Response.Content.ReadAsStringAsync().Result;
                            BookingInfo = JsonConvert.DeserializeObject<List<DeliveryBooking>>(UserRes);
                        }
                    }

                    DelList = (from k in BookingInfo
                               where k.BookingId == id && k.UserName == usname
                               select k).ToList();

                    if (DelList.Count() != 0)
                    {
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
                    else
                    {
                        ViewBag.errormsg = "Invalid Booking Id";
                        return View();
                    }
                }
            catch(Exception)
            {
                ModelState.AddModelError("", "Invalid Operation");
                return View();
            }
                                            
            }


        //Get the details of Bookings received by a particular Delivery Executive
        public async Task<IActionResult> GetById(string name)
        {
            try
            {


                string usname = HttpContext.Session.GetString("username");
                List<DeliveryBooking> BookingInfo = new List<DeliveryBooking>();
                List<DeliveryBooking> DelList = new List<DeliveryBooking>();
                using (var client = new HttpClient())
                {
                    string Baseurl = "https://localhost:44372/";
                    client.BaseAddress = new Uri(Baseurl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage Response = await client.GetAsync("api/DeliveryBookings");
                    if (Response.IsSuccessStatusCode)
                    {
                        var UserRes = Response.Content.ReadAsStringAsync().Result;
                        BookingInfo = JsonConvert.DeserializeObject<List<DeliveryBooking>>(UserRes);
                    }

                    //Get the list of executives, compare it with username entered during login and store the appropriate data in a list
                    DelList = (from s in BookingInfo
                               where s.DeliveryExecutive == usname
                               select s).ToList();

                    return View(DelList);
                }
            }
            catch(Exception)
            {
                ModelState.AddModelError("", "Invalid Operation");
                return View();
            }
        }

        //Create a Booking
        public async Task<IActionResult> CreateBooking()
        {
            try
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
                //Code to get the complete details of a logged in user
                var createobj = (from k in UserList
                                 where k.UserName == Username
                                 select k).FirstOrDefault();

                if (createobj != null)
                {
                    ViewBag.Message = createobj;
                }

                //Extracting the city of the user
                var usercity = createobj.City;

                //Code to get the list of delivery executives living in the particular city of a user
                var executives = (from j in UserList
                                  where j.City == usercity && j.UserType == "Delivery Executive"
                                  select j.UserName).ToList();

                //Store the list in a ViewBag
                ViewBag.executives = new SelectList(executives);
                return View();
            }
            catch(Exception)
            {
                ModelState.AddModelError("", "Invalid Operation");
                return View();
            }
            
        }
        


        [HttpPost]
            public async Task<IActionResult> CreateBooking(DeliveryBooking p)
            {
            try
            {
                DeliveryBooking Pobj = new DeliveryBooking();
                using (var httpClient = new HttpClient())
                {
                    string Baseurl = "https://localhost:44372/";
                    httpClient.BaseAddress = new Uri(Baseurl);
                    StringContent content = new StringContent(JsonConvert.SerializeObject(p), Encoding.UTF8, "application/json");

                    using (var response = await httpClient.PostAsync("api/DeliveryBookings", content))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        Pobj = JsonConvert.DeserializeObject<DeliveryBooking>(apiResponse);
                    }
                }
                return RedirectToAction("ListByUser");
            }
            catch(Exception)
            {
                ModelState.AddModelError("", "Invalid Operation");
                return View();
            }
                                               
                   
            }

           
           //Edit the booking details
            public async Task<IActionResult> EditBooking(int id)
            {
            try
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
            catch(Exception)
            {
                ModelState.AddModelError("", "Invalid Operation");
                return View();
            }
            }


            [HttpPost]
            public async Task<IActionResult> EditBooking(DeliveryBooking p)
            {
            try
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
            catch(Exception)
            {
                ModelState.AddModelError("", "Invalid Operation");
                return View();
            }
                
            }

        //Delete a booking
            [HttpGet]
            public async Task<ActionResult> DeleteBooking(int id)
            {
            try
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
             catch(Exception)
            {
                ModelState.AddModelError("", "Invalid Operation");
                return View();
            }

            }

            [HttpPost]
            public async Task<ActionResult> DeleteBooking(DeliveryBooking p)
            {
            try
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
            catch (Exception)
            {
                ModelState.AddModelError("", "Invalid Operation");
                return View();
            }
            }
      }
}


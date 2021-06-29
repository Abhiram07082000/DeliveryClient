using DeliveryClient.Models;
using DeliveryClient.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Dynamic;
using Microsoft.AspNetCore.Authorization;

namespace DeliveryClient.Controllers
{
    
    public class LoginController : Controller
    {
        
        public LoginController()
        {

        }
        public IActionResult Login()
        {
            return View();
        }
       
        [HttpPost]
         public async Task<IActionResult> Login(Login l)
         {
            try
            {


                List<User> UserList = new List<User>();
                List<DeliveryBooking> BookingInfo = new List<DeliveryBooking>();
                List<DeliveryBooking> bookinglist = new List<DeliveryBooking>();

                using (var client = new HttpClient())
                {
                    string Baseurl = "https://localhost:44372/";
                    client.BaseAddress = new Uri(Baseurl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage Res = await client.GetAsync("api/Users");
                    HttpResponseMessage Response = await client.GetAsync("api/DeliveryBookings");
                    if (Res.IsSuccessStatusCode)
                    {
                        var UserResponse = Res.Content.ReadAsStringAsync().Result;
                        UserList = JsonConvert.DeserializeObject<List<User>>(UserResponse);
                    }
                    if (Response.IsSuccessStatusCode)
                    {
                        var BookingRes = Response.Content.ReadAsStringAsync().Result;
                        BookingInfo = JsonConvert.DeserializeObject<List<DeliveryBooking>>(BookingRes);
                    }


                    User UserInfo = (from i in UserList
                                     where i.UserName == l.userName && i.Password == l.pass && i.UserType == l.type
                                     select i).FirstOrDefault();

                    DeliveryBooking BookingInf = (from k in BookingInfo
                                                  where k.UserName == l.userName
                                                  select k).LastOrDefault();


                    if (UserInfo != null)
                    {


                        if (l.type == "Customer")
                        {
                            //int bookingid = BookingInf.BookingId;
                            String Username = UserInfo.UserName;
                            HttpContext.Session.SetString("username", Username);
                            String Usertype = UserInfo.UserType;
                            HttpContext.Session.SetString("usertype", Usertype);
                            return RedirectToAction("CreateBooking", "DeliveryBooking");

                        }
                        else if (l.type == "Delivery Executive")
                        {

                            String Username = UserInfo.UserName;
                            HttpContext.Session.SetString("username", Username);
                            String Usertype = UserInfo.UserType;
                            HttpContext.Session.SetString("usertype", Usertype);

                            return RedirectToAction("GetById", "DeliveryBooking");

                        }
                        else if (l.type == "Admin")
                        {
                            String Username = UserInfo.UserName;
                            HttpContext.Session.SetString("username", Username);
                            String Usertype = UserInfo.UserType;
                            HttpContext.Session.SetString("usertype", Usertype);
                            return RedirectToAction("GetAllUsers", "User");
                        }
                        else
                        {
                            ModelState.AddModelError("", "Unauthorized Access");
                            return View();
                        }
                    }



                    else
                    {
                        ModelState.AddModelError("", "Invalid Username or Password");
                        return View();
                    }

                }
            }
            catch(Exception)
            {
                ModelState.AddModelError("", "Invalid Operation");
                return View();
            }
            
         }
        public ActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}


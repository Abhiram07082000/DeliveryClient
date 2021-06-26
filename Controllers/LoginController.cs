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
                                  where i.UserName == l.userName && i.Password == l.pass &&i.UserType==l.type
                             select i).FirstOrDefault();
                //if (UserInfo != null && UserInfo.UserType == "Customer")
                //{
                //    User createobj = (from k in User
                //                      where k.UserName == l.userName
                //                      select k).FirstOrDefault();

                //    if (createobj != null)
                //    {

                //        ViewBag.Message = createobj;
                //    }

                //    var usercity = createobj.City;
                //    if (UserInfo != null)
                //    {
                //        List<User> executives = new List<User>();
                //        executives = (from j in User
                //                      where j.City == usercity && j.UserType == "Delivery Executive"
                //                      select j).ToList();
                //        ViewBag.Cities = executives;
                        
                        
                        
                //    }
                //}
                
                

                
                DeliveryBooking BookingInf = (from k in BookingInfo
                                              where k.UserName == l.userName
                                              select k).LastOrDefault();

                //var filtered = (from s in BookingInfo
                //                           where s.DeliveryExecutive == l.userName
                //                           select s).ToList();



                //if(DeliveryBooking.)
                //&& i.Password == l.pass && i.UserType == l.type



                if (UserInfo!= null)
                {
                    if (l.type == "Customer")
                    {
                        //int bookingid = BookingInf.BookingId;
                        String Username = UserInfo.UserName;
                        HttpContext.Session.SetString("username", Username);
                        return RedirectToAction("CreateBooking", "DeliveryBooking");
                        //return RedirectToAction("StatusCheck", "DeliveryBooking",new {id= bookingid});
                    }
                    else if (l.type == "Delivery Executive")
                    { 
                        
                        String Username = UserInfo.UserName;
                        HttpContext.Session.SetString("username", Username);
                        //var result = new DeliveryBookingController().GetById(Username);  ,new {name=Username }
                        return RedirectToAction("GetById","DeliveryBooking");
                        
                    }
                    else
                    {
                        String Username = UserInfo.UserName;
                        HttpContext.Session.SetString("username", Username);
                        return RedirectToAction("GetAllUsers", "User");
                    }
                }
                
                else
                {
                    return View();
                }

            }     
            
         }
    }
}

//tagline, core values, - hyphen, handshake 
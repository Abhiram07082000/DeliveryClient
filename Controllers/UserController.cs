using DeliveryClient.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
    public class UserController : Controller
    {
        string Baseurl = "https://localhost:44372/";
        public async Task<IActionResult> GetAllUsers()
        {
            List<User> UserInfo = new List<User>();

            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = await client.GetAsync("api/Users");

                if (Res.IsSuccessStatusCode)
                {
                    var UserResponse = Res.Content.ReadAsStringAsync().Result;
                    UserInfo = JsonConvert.DeserializeObject<List<User>>(UserResponse);

                }
               
                ViewBag.ListofUsers = UserInfo;
                return View(UserInfo);
            }
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
                return RedirectToAction("GetAllUsers");
            }

        public async Task<IActionResult> ViewDetails(int i)
        {
            string Username = HttpContext.Session.GetString("username");
            List<User> UserInfo = new List<User>();

            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = await client.GetAsync("api/Users");

                if (Res.IsSuccessStatusCode)
                {
                    var UserResponse = Res.Content.ReadAsStringAsync().Result;
                    UserInfo = JsonConvert.DeserializeObject<List<User>>(UserResponse);

                }
                int id = (from k in UserInfo
                          where k.UserName == Username
                          select k.UserId).FirstOrDefault();



                User p = new User();
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.GetAsync("https://localhost:44372/api/Users/" + id))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        p = JsonConvert.DeserializeObject<User>(apiResponse);
                    }
                }
                return View(p);
            }
        }
        public async Task<IActionResult> Edit(int i)
            {
                string Username = HttpContext.Session.GetString("username");
                List<User> UserInfo = new List<User>();

            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = await client.GetAsync("api/Users");

                if (Res.IsSuccessStatusCode)
                {
                    var UserResponse = Res.Content.ReadAsStringAsync().Result;
                    UserInfo = JsonConvert.DeserializeObject<List<User>>(UserResponse);

                }
                int id = (from k in UserInfo
                          where k.UserName == Username
                          select k.UserId).FirstOrDefault();



                User p = new User();
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.GetAsync("https://localhost:44372/api/Users/" + id))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        p = JsonConvert.DeserializeObject<User>(apiResponse);
                    }
                }
                return View(p);
            }
            }

            [HttpPost]
            public async Task<IActionResult> Edit(User p)
            {
                User p1 = new User();
                using (var httpClient = new HttpClient())
                {
                    int id = p.UserId;
                    StringContent content1 = new StringContent(JsonConvert.SerializeObject(p), Encoding.UTF8, "application/json");
                    using (var response = await httpClient.PutAsync("https://localhost:44372/api/Users/" + id, content1))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        ViewBag.Result = "Success";
                        p1 = JsonConvert.DeserializeObject<User>(apiResponse);
                    }
                }
                return RedirectToAction("ViewDetails");
            }

            [HttpGet]
            public async Task<ActionResult> Delete(int id)
            {
                TempData["Userid"] = id;
                User e = new User();
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.GetAsync("https://localhost:44372/api/Users/" + id))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        e = JsonConvert.DeserializeObject<User>(apiResponse);
                    }
                }
                return View(e);

            }

            [HttpPost]
            public async Task<ActionResult> Delete(User p)
            {
                int prid = Convert.ToInt32(TempData["Userid"]);
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.DeleteAsync("https://localhost:44/api/Users/" + prid))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                    }
                }

                return RedirectToAction("GetAllUsers");
            }
        }
    }

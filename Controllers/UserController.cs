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
            try
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
            catch (Exception)
            {
                ModelState.AddModelError("", "Invalid Operation");
                return View();
            }

        }

        public async Task<IActionResult> Create()
        {
            try
            {
                return await Task.Run(() => View());
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Invalid Operation");
                return View();
            }
        }
        [HttpPost]
        public async Task<IActionResult> Create(User p)
        {
            try
            {
                User Uobj = new User();
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
                return RedirectToAction("Login", "Login");
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Invalid Operation");
                return View();
            }

        }

        public async Task<IActionResult> AdminCreate()
        {
            try
            {
                return await Task.Run(() => View());
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Invalid Operation");
                return View();
            }

        }
        [HttpPost]
        public async Task<IActionResult> AdminCreate(User p)
        {
            try
            {
                User Uobj = new User();
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
                return RedirectToAction("Login", "Login");
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Invalid Operation");
                return View();
            }

        }

        public async Task<IActionResult> ViewDetails(int i)
        {
            User p = new User();
            try
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




                    using (var httpClient = new HttpClient())
                    {
                        using (var response = await httpClient.GetAsync("https://localhost:44372/api/Users/" + id))
                        {
                            string apiResponse = await response.Content.ReadAsStringAsync();
                            p = JsonConvert.DeserializeObject<User>(apiResponse);
                        }
                    }

                }
                return View(p);
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Invalid Operation");
                return View();
            }
        }

    

        public async Task<IActionResult> Edit(int i)
            {
            try
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
             catch(Exception)
            {
                ModelState.AddModelError("", "Invalid Operation");
                return View();
            }
            }

            [HttpPost]
            public async Task<IActionResult> Edit(User p)
            {
            try
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
            catch(Exception)
            {
                ModelState.AddModelError("", "Invalid Operation");
                return View();
            }
               
            }

            [HttpGet]
            public async Task<ActionResult> Delete(int id)
            {
            try
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
            catch(Exception)
            {
                ModelState.AddModelError("", "Invalid Operation");
                return View();
            }
            }

            [HttpPost]
            public async Task<ActionResult> Delete(User p)
            {
            try
            {
                int prid = Convert.ToInt32(TempData["Userid"]);
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.DeleteAsync("https://localhost:44372/api/Users/" + prid))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                    }
                }

                return RedirectToAction("GetAllUsers");
            }
            catch(Exception)
            {
                ModelState.AddModelError("", "Invalid Operation");
                return View();
            }
                
            }


            public async Task<IActionResult> AdminEdit(int id)
            {
            try
            {

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
            catch(Exception)
            {
                ModelState.AddModelError("", "Invalid Operation");
                return View();
            }
            }

            [HttpPost]
            public async Task<IActionResult> AdminEdit(User p)
            {
            try
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
                return RedirectToAction("GetAllUsers");
            }
            catch(Exception)
            {
                ModelState.AddModelError("", "Invalid Operation");
                return View();
            }
            }
    }
    }

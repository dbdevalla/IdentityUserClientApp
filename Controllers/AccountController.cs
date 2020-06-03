using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using IdentityUserClientApp.Models;
using System.Configuration;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;

namespace IdentityUserClientApp.Controllers
{
    public class AccountController : Controller
    {
        private IConfiguration _configuration;
      

        public AccountController(IConfiguration configuration)
        {
            _configuration = configuration;
           
        }
        // GET: AccountController
        [HttpGet]
        public IActionResult Register()
        {
            
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(UserRegistrationModel user)
        {
            string Idenityurl = _configuration.GetValue<string>("IdendityServer:IdentityUrl");
            string retValue = string.Empty;
            if (!ModelState.IsValid)
            return View();
            using (var client = new HttpClient())
            {
                //client.BaseAddress =new Uri( Idenityurl);
                client.DefaultRequestHeaders.Accept.Clear();
                UserViewModel userViewModel=new UserViewModel { Email=user.Email, UserName=user.UserName,Password=user.Password, UserRole=user.userRole.ToString()};
                 var PostData = JsonConvert.SerializeObject(userViewModel);

                HttpResponseMessage response = await client.PostAsync(Idenityurl,
                    new StringContent(PostData, Encoding.UTF8, "application/json"));
                if (response.IsSuccessStatusCode)
                {
                    retValue =  response.Content.ReadAsStringAsync().Result;
                    ViewBag.SuccessMessage = retValue;
                }


                return View();
                //var result = client.PostAsync(string.Format("{0}/api", Idenityurl, content).Result;

                //var xmlResponse = result.Content.ReadAsStringAsync().Result;

            }


        }
    }
}

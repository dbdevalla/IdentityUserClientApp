using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using IdentityUserClientApp.Models;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http;
using System.Runtime.InteropServices;
using RestSharp;

namespace IdentityUserClientApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginView view)
        {
            try
            {


            string TokenendPoint = _configuration.GetValue<string>("IdendityServer:TokenendPoint");
            string retValue = string.Empty;
            if (!ModelState.IsValid)
                return View();

                #region MyRegion
                var client = new RestClient(TokenendPoint);
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("accept", "application/json");
                request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                request.AddParameter("grant_type", _configuration.GetValue<string>("IdendityServer:grant_type"));
                request.AddParameter("username", view.UserName);
                request.AddParameter("password", view.Password);
                request.AddParameter("scope", _configuration.GetValue<string>("IdendityServer:scope"));
                request.AddParameter("client_id", _configuration.GetValue<string>("IdendityServer:client_id"));
                request.AddParameter("client_secret", _configuration.GetValue<string>("IdendityServer:client_sectret"));
                IRestResponse response = client.Execute(request);

                if (response.IsSuccessful)
                {
                    retValue = response.Content.ToString();
                    var options = new CookieOptions();
                    options.Expires = DateTime.Now.AddMinutes(20);
                    Response.Cookies.Append("TokenValue", retValue, options);
                    ViewBag.SuccessMessage = retValue;
                }

                #endregion
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View("Index");
        }
       
    }
}

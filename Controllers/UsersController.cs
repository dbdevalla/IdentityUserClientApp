using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using IdentityUserClientApp.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace IdentityUserClientApp.Controllers
{
    public class WeatherController : Controller
    {
        private IConfiguration _configuration;

        public WeatherController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // GET: UsersController
        [HttpGet]
        public async Task<ActionResult> GetWeatherData()
        {
            List<WeatherForecast> _weatherForecasts;
            string Idenityurl = _configuration.GetValue<string>("IdendityServer:WeatherController");
            string retValue = string.Empty;
            var token = Request.Cookies["TokenValue"];
            if (!string.IsNullOrEmpty(token))
            {
                var _token = (JObject)JsonConvert.DeserializeObject(token);
                var BearerToken = _token["access_token"].Value<string>();

                using (var client = new System.Net.Http.HttpClient())
                {
                    var request = new HttpRequestMessage();
                    request.Method = HttpMethod.Get;
                    request.Headers.Add("Authorization", "Bearer "+ BearerToken);

                    using (var response = await client.GetAsync(Idenityurl))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        _weatherForecasts = JsonConvert.DeserializeObject<List<WeatherForecast>>(apiResponse);
                    }

                    return View(_weatherForecasts);
                }
            }

          return  RedirectToAction("Index", "Home");
        }
    }
}

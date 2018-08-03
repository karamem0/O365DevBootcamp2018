using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AuthorizationCodeGrant.Controllers
{

    public class HomeController : Controller
    {
        // 自身の情報を取得する Microsoft Graph の URL
        public static readonly string GraphUrl = "https://graph.microsoft.com/v1.0/me";

        private IHttpClientFactory httpClientFactory;

        public HomeController(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        [ActionName("Index")]
        public async Task<IActionResult> IndexAsync()
        {
            if (this.HttpContext.Session.TryGetValue("access_token", out var buffer))
            {
                var accessToken = Encoding.UTF8.GetString(buffer);

                // 自身の情報を取得します
                var httpClient = this.httpClientFactory.CreateClient("default");
                var graphRequestMessage = new HttpRequestMessage(HttpMethod.Get, GraphUrl);
                graphRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                var graphResponseMessage = await httpClient.SendAsync(graphRequestMessage);
                var graphResponseContent = await graphResponseMessage.Content.ReadAsStringAsync();
                var graphResponseJson = (JToken)JsonConvert.DeserializeObject(graphResponseContent);

                this.ViewBag.DisplayName = graphResponseJson["displayName"];
            }
            return this.View();
        }

    }

}

using Microsoft.AspNetCore.Mvc;
using Microsoft.Graph;
using MsalAndGraphSdk.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace MsalAndGraphSdk.Controllers
{

    public class HomeController : Controller
    {

        private IHttpClientFactory httpClientFactory;

        public HomeController(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        [ActionName("Index")]
        public async Task<IActionResult> IndexAsync()
        {
            if (this.HttpContext.Session.Keys.Contains("access_token"))
            {
                // 自身の情報を取得します
                var graphClient = new GraphServiceClient(new AuthenticationProvider(this.HttpContext));
                var graphResponse =  await graphClient.Me.Request().GetAsync();

                this.ViewBag.DisplayName = graphResponse.DisplayName;
            }
            return this.View();
        }

    }

}

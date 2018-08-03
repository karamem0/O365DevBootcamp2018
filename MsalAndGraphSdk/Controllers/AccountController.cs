using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
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

    public class AccountController : Controller
    {

        // テナント名 (***.onmicrosoft.com)
        public static readonly string TenantId = "";
        // アプリケーション ID
        public static readonly string ClientId = "";
        // アプリケーション シークレット
        public static readonly string ClientSecret = "";
        // スコープ
        public static readonly string Scope = "https://graph.microsoft.com/User.Read";
        // リダイレクト URL
        public static readonly string RedirectUrl = "https://localhost:5001/Account/Callback";

        private IHttpClientFactory httpClientFactory;

        public AccountController(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        [ActionName("Authorize")]
        public async Task<IActionResult> AuthorizeAsync()
        {
            // 認証を行うための URL にリダイレクトします
            var clientCredential = new ClientCredential(ClientSecret);
            var clientApplication = new ConfidentialClientApplication(ClientId, RedirectUrl, clientCredential, null, null);
            var requestUrl = await clientApplication.GetAuthorizationRequestUrlAsync(new[] { Scope }, null, null);

            return this.Redirect(requestUrl.ToString());
        }

        [ActionName("Callback")]
        public async Task<IActionResult> CallbackAsync(string code, string error, [FromQuery(Name = "error_description")]string description, string resource, string state)
        {
            // アクセス トークンを取得します
            var clientCredential = new ClientCredential(ClientSecret);
            var clientApplication = new ConfidentialClientApplication(ClientId, RedirectUrl, clientCredential, null, null);
            var authenticationResult = await clientApplication.AcquireTokenByAuthorizationCodeAsync(code, new[] { Scope });

            // アクセス トークンをセッションに格納します
            var accessToken = authenticationResult.AccessToken;
            this.HttpContext.Session.Set("access_token", Encoding.UTF8.GetBytes(accessToken));

            return this.RedirectToAction("Index", "Home");
        }

    }

}

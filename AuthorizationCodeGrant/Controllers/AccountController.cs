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
        // 認証を開始する URL
        public static readonly string AuthorizeUrl = $"https://login.microsoftonline.com/{TenantId}/oauth2/v2.0/authorize";
        // アクセス トークンを取得する URL
        public static readonly string TokenUrl = $"https://login.microsoftonline.com/{TenantId}/oauth2/v2.0/token";

        private IHttpClientFactory httpClientFactory;

        public AccountController(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        public IActionResult Authorize()
        {
            // 認証を行うための URL にリダイレクトします
            return this.Redirect(AuthorizeUrl
                + "?grant_type=authorization_code"
                + "&client_id=" + Uri.EscapeDataString(ClientId)
                + "&scope=" + Uri.EscapeDataString(Scope)
                + "&response_type=code"
                + "&redirect_url=" + Uri.EscapeDataString(RedirectUrl));
        }

        [ActionName("Callback")]
        public async Task<IActionResult> CallbackAsync(string code, string error, [FromQuery(Name = "error_description")]string description, string resource, string state)
        {
            // アクセス トークンを取得します
            var httpClient = this.httpClientFactory.CreateClient("default");
            var tokenRequestContent =
                  "grant_type=authorization_code"
                + "&code=" + Uri.EscapeDataString(code)
                + "&client_id=" + Uri.EscapeDataString(ClientId)
                + "&client_secret=" + Uri.EscapeDataString(ClientSecret)
                + "&scope=" +  Scope;
            var tokenRequestMessage = new HttpRequestMessage(HttpMethod.Post, TokenUrl);
            tokenRequestMessage.Content = new StringContent(tokenRequestContent);
            tokenRequestMessage.Content.Headers.ContentType = new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded");
            var tokenResponseMessage = await httpClient.SendAsync(tokenRequestMessage);
            var tokenResponseContent = await tokenResponseMessage.Content.ReadAsStringAsync();
            var tokenResponseJson = (JToken)JsonConvert.DeserializeObject(tokenResponseContent);

            // アクセス トークンをセッションに格納します
            var accessToken = (string)tokenResponseJson["access_token"];
            this.HttpContext.Session.Set("access_token", Encoding.UTF8.GetBytes(accessToken));

            return this.RedirectToAction("Index", "Home");
        }

    }

}

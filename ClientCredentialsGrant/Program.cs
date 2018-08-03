using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ClientCredentialsGrant
{

    public static class Program
    {

        // テナント名 (***.onmicrosoft.com)
        public static readonly string TenantId = "";
        // アプリケーション ID
        public static readonly string ClientId = "";
        // アプリケーション シークレット
        public static readonly string ClientSecret = "";
        // スコープ
        public static readonly string Scope = "https://graph.microsoft.com/.default";
        // リダイレクト URL
        public static readonly string RedirectUrl = "https://localhost:5001/Account/Callback";
        // アクセス トークンを取得する URL
        public static readonly string TokenUrl = $"https://login.microsoftonline.com/{TenantId}/oauth2/v2.0/token";
        // アプリケーションの同意を行うための URL
        public static readonly string AdminConsentUrl = $"https://login.microsoftonline.com/{TenantId}/adminconsent";
        // 組織内のユーザーの一覧を取得する Microsoft Graph の URL
        public static readonly string GraphUrl = "https://graph.microsoft.com/v1.0/users";

        public static void Main(string[] args)
        {
            ExecuteAsync().Wait();
        }

        private static async Task ExecuteAsync()
        {
            // ブラウザーでアプリケーションの同意を行います
            Console.WriteLine(AdminConsentUrl
                + "?client_id=" + Uri.EscapeDataString(ClientId)
                + "&redirect_url=" + Uri.EscapeUriString(RedirectUrl)
                + "&state=12345");
            Console.ReadLine();

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // アクセス トークンを取得します
            var tokenRequestContent =
                  "grant_type=client_credentials"
                + "&client_id=" + Uri.EscapeDataString(ClientId)
                + "&client_secret=" + Uri.EscapeDataString(ClientSecret)
                + "&scope=" +  Scope;
            var tokenRequestMessage = new HttpRequestMessage(HttpMethod.Post, TokenUrl);
            tokenRequestMessage.Content = new StringContent(tokenRequestContent);
            tokenRequestMessage.Content.Headers.ContentType = new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded");
            var tokenResponseMessage = await httpClient.SendAsync(tokenRequestMessage);
            var tokenResponseContent = await tokenResponseMessage.Content.ReadAsStringAsync();
            var tokenResponseJson = (JToken)JsonConvert.DeserializeObject(tokenResponseContent);

            Console.WriteLine(tokenResponseContent);
            Console.ReadLine();

            var accessToken = (string)tokenResponseJson["access_token"];
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            // 組織内のユーザーの一覧を取得します
            var graphRequestMessage = new HttpRequestMessage(HttpMethod.Get, GraphUrl);   
            var graphResponseMessage = await httpClient.SendAsync(graphRequestMessage);
            var graphResponseContent = await graphResponseMessage.Content.ReadAsStringAsync();

            Console.WriteLine(graphResponseContent);
            Console.ReadLine();
        }

    }

}

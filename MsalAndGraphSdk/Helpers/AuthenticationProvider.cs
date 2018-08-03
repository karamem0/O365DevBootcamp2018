using Microsoft.AspNetCore.Http;
using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace MsalAndGraphSdk.Helpers
{

    public class AuthenticationProvider : IAuthenticationProvider
    {

        private HttpContext httpContext;

        public AuthenticationProvider(HttpContext httpContext)
        {
            this.httpContext = httpContext;
        }

        public Task AuthenticateRequestAsync(HttpRequestMessage request)
        {
            return Task.Run(() =>
            {
                if (this.httpContext.Session.TryGetValue("access_token", out var buffer))
                {
                    var accessToken = Encoding.UTF8.GetString(buffer);
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                }
            });
        }

    }

}

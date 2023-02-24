using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Threading.Tasks;

namespace LocadoraVeiculos.Middlewares
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class FriendListMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string _friendList;

        public FriendListMiddleware(RequestDelegate next, string friendList)
        {
            _next = next;
            _friendList = friendList;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var remoteIP = httpContext.Connection.RemoteIpAddress;

            string[] ip = _friendList.Split(';');
            if (!_friendList.Contains('*'))
            {
                if (!ip.Any(option => option == remoteIP.ToString()))
                {
                    httpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    return;
                }
            }

            await _next(httpContext);
        }
    }
}

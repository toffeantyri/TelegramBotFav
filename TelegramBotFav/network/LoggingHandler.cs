using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBotFav.network
{
    internal class LoggingHandler : DelegatingHandler
    {
        internal LoggingHandler() : base(new HttpClientHandler()) { }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Console.WriteLine($"Send request : {request.Method} {request.RequestUri}");
            Console.WriteLine($"Request Headers : {string.Join(", ", request.Headers)}");

            var responce = await base.SendAsync(request, cancellationToken);

            Console.WriteLine($"Response : {responce.StatusCode}");
            Console.WriteLine($"Response Headers : {string.Join(", ", responce.Headers)}");
            Console.WriteLine($"Response Body : {await responce.Content.ReadAsStringAsync()}");

            return responce;
        }


    }
}

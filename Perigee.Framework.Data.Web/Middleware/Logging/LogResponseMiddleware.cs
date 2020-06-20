namespace Perigee.Web.Middleware.Logging
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;

    // http://www.sulhome.com/blog/10/log-asp-net-core-request-and-response-using-middleware
    public class LogResponseMiddleware
    {
        private readonly ILogger _logger;
        private readonly RequestDelegate _next;
        private Func<object, Exception, string> _defaultFormatter = (state, exception) => state.ToString();

        public LogResponseMiddleware(RequestDelegate next) //, ILogger logger)
        {
            _next = next;
            //_logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            var bodyStream = context.Response.Body;

            var responseBodyStream = new MemoryStream();
            context.Response.Body = responseBodyStream;

            await _next(context).ConfigureAwait(false);

            responseBodyStream.Seek(0, SeekOrigin.Begin);
            var responseBody = new StreamReader(responseBodyStream).ReadToEnd();
            //_logger.Log(LogLevel.Information, 1, $"RESPONSE LOG: {responseBody}", null, _defaultFormatter);
            responseBodyStream.Seek(0, SeekOrigin.Begin);
            await responseBodyStream.CopyToAsync(bodyStream).ConfigureAwait(false);
            //context.Response.Body = bodyStream;
        }
    }
}
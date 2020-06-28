namespace Perigee.Framework.Web.Middleware.Logging
{
    using System;
    using System.IO;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;

    public static class ServiceRequestLoggingMiddlewareExtension
    {
        //public static IApplicationBuilder UseServiceRequestLoggingMiddleware(this IApplicationBuilder app)
        //{
        //    if (app == null)
        //        throw new ArgumentNullException(nameof(app));
        //    return app.UseServiceRequestLoggingMiddleware((Action<ServiceRequestLoggingMiddlewareConfiguration>)(config => { }));
        //}

        public static IApplicationBuilder UseServiceRequestLoggingMiddleware(this IApplicationBuilder app,
            ServiceRequestLoggingMiddlewareConfiguration config)
        {
            if (app == null)
                throw new ArgumentNullException(nameof(app));
            if (config == null)
                throw new ArgumentNullException(nameof(config));

            return app.UseMiddleware<ServiceRequestLoggingMiddleware>((object) config);
        }
    }

    public class ServiceRequestLoggingMiddlewareConfiguration
    {
        public string Thingy { get; set; }
    }

    public class ServiceRequestLoggingMiddleware
    {
        private readonly ServiceRequestLoggingMiddlewareConfiguration _config;
        private readonly RequestDelegate _next;

        public ServiceRequestLoggingMiddleware(RequestDelegate next,
            ServiceRequestLoggingMiddlewareConfiguration config)
        {
            _next = next;
            _config = config;
        }

        public async Task Invoke(HttpContext context, CancellationToken cancellationToken)
        {
            //First, get the incoming request
            var request = await FormatRequest(context.Request, cancellationToken).ConfigureAwait(false);

            //Copy a pointer to the original response body stream
            var originalBodyStream = context.Response.Body;

            //Create a new memory stream...
            await using var responseBody = new MemoryStream();

            //...and use that for the temporary response body
            context.Response.Body = responseBody;

            //Continue down the Middleware pipeline, eventually returning to this class
            await _next(context).ConfigureAwait(false);

            //Format the response from the server
            var response = await FormatResponse(context.Response, cancellationToken).ConfigureAwait(false);

            //TODO: Save log to chosen datastore


            //Copy the contents of the new memory stream (which contains the response) to the original stream, which is then returned to the client.
            await responseBody.CopyToAsync(originalBodyStream, cancellationToken).ConfigureAwait(false);
        }


        private async Task<string> FormatRequest(HttpRequest request, CancellationToken cancellationToken)
        {
            var body = request.Body;

            //This line allows us to set the reader for the request back at the beginning of its stream.
            request.EnableBuffering();

            //We now need to read the request stream.  First, we create a new byte[] with the same length as the request stream...
            var buffer = new byte[Convert.ToInt32(request.ContentLength)];

            //...Then we copy the entire request stream into the new buffer.
            await request.Body.ReadAsync(buffer, 0, buffer.Length, cancellationToken).ConfigureAwait(false);

            //We convert the byte[] into a string using UTF8 encoding...
            var bodyAsText = Encoding.UTF8.GetString(buffer);

            //..and finally, assign the read body back to the request body, which is allowed because of EnableRewind()
            request.Body = body;
            //request.Body.Position = 0;

            return $"{request.Scheme} {request.Host}{request.Path} {request.QueryString} {bodyAsText}";
        }


        //private async Task<string> FormatRequest(HttpRequest request)
        //{
        //    request.EnableRewind();

        //    var buffer = new byte[Convert.ToInt32(request.ContentLength)];

        //    await request.Body.ReadAsync(buffer, 0, buffer.Length).ConfigureAwait(false);

        //    var bodyAsText = Encoding.UTF8.GetString(buffer);

        //    request.Body.Position = 0;

        //    return $"{Environment.NewLine}{Environment.NewLine}{ApiRequest}{Environment.NewLine}{request.Scheme} {request.Host}{request.Path} {request.QueryString} {bodyAsText}{Environment.NewLine}{Environment.NewLine}";
        //}

        private async Task<string> FormatResponse(HttpResponse response, CancellationToken cancellationToken)
        {
            //We need to read the response stream from the beginning...
            response.Body.Seek(0, SeekOrigin.Begin);

            //...and copy it into a string
            var text = await new StreamReader(response.Body).ReadToEndAsync().ConfigureAwait(false);

            //We need to reset the reader for the response so that the client can read it.
            response.Body.Seek(0, SeekOrigin.Begin);

            //Return the string for the response, including the status code (e.g. 200, 404, 401, etc.)
            return $"{response.StatusCode}: {text}";
        }
    }
}
namespace Perigee.Web.Middleware.HttpResponseWrapper
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using Extensions;
    using Microsoft.AspNetCore.Http;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    public class ApiResponseMiddleware
    {
        private readonly RequestDelegate _next;

        public ApiResponseMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (IsSwagger(context))
            {
                await _next(context).ConfigureAwait(false);
            }

            else
            {
                var originalBody = context.Response.Body;
                using (var responseBody = new MemoryStream())
                {
                    context.Response.Body = responseBody;

                    try
                    {
                        await _next.Invoke(context).ConfigureAwait(false);

                        if (new[] {(int) HttpStatusCode.OK, (int) HttpStatusCode.Created}.Contains(context.Response
                            .StatusCode))
                        {
                            var body = await FormatResponse(context.Response).ConfigureAwait(false);
                            await HandleSuccessRequestAsync(context, body, context.Response.StatusCode).ConfigureAwait(false);
                        }
                        else
                        {
                            await HandleNotSuccessRequestAsync(context, context.Response.StatusCode).ConfigureAwait(false);
                        }
                    }
                    catch (Exception ex)
                    {
                        await HandleExceptionAsync(context, ex).ConfigureAwait(false);
                    }
                    finally
                    {
                        responseBody.Seek(0, SeekOrigin.Begin);
                        await responseBody.CopyToAsync(originalBody).ConfigureAwait(false);
                    }
                }
            }
        }

        private async Task<string> FormatResponse(HttpResponse response)
        {
            response.Body.Seek(0, SeekOrigin.Begin);
            var plainBodyText = await new StreamReader(response.Body).ReadToEndAsync().ConfigureAwait(false);
            response.Body.Seek(0, SeekOrigin.Begin);

            return plainBodyText;
        }

        private Task HandleNotSuccessRequestAsync(HttpContext context, int code)
        {
            //context.Response.ContentType = "application/json";
            ApiError apiError = null;
            ApiResponse apiResponse = null;

            if (code == (int) HttpStatusCode.NotFound)
                apiError = new ApiError("The specified URI does not exist. Please verify and try again");
            else if (code == (int) HttpStatusCode.NoContent)
                apiError = new ApiError("The specified URI doesn't not contain any content.");
            else
                apiError = new ApiError("Your request cannot be processed. Please contact support");

            apiResponse = new ApiResponse(code, ResponseMessageEnum.Failure.GetDescription(), null, apiError);
            context.Response.StatusCode = code;

            var json = JsonConvert.SerializeObject(apiResponse);
            return context.Response.WriteAsync(json);
        }

        private Task HandleSuccessRequestAsync(HttpContext context, object body, int code)
        {
            //context.Response.ContentType = "application/json";
            string jsonString, bodyText = string.Empty;
            ApiResponse apiResponse = null;


            if (!body.ToString().IsValidJson())
                bodyText = JsonConvert.SerializeObject(body);
            else
                bodyText = body.ToString();

            dynamic bodyContent = JsonConvert.DeserializeObject<dynamic>(bodyText);
            Type type;

            type = bodyContent?.GetType();

            if (type == typeof(JObject))
            {
                apiResponse = JsonConvert.DeserializeObject<ApiResponse>(bodyText);
                if (apiResponse.StatusCode != code)
                {
                    jsonString = JsonConvert.SerializeObject(apiResponse);
                }
                else if (apiResponse.Result != null)
                {
                    jsonString = JsonConvert.SerializeObject(apiResponse);
                }
                else
                {
                    apiResponse = new ApiResponse(code, ResponseMessageEnum.Success.GetDescription(), bodyContent,
                        null);
                    jsonString = JsonConvert.SerializeObject(apiResponse);
                }
            }
            else
            {
                apiResponse = new ApiResponse(code, ResponseMessageEnum.Success.GetDescription(), bodyContent, null);
                jsonString = JsonConvert.SerializeObject(apiResponse);
            }

            return context.Response.WriteAsync(jsonString);
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            ApiError apiError = null;
            ApiResponse response = null;

            if (exception is ApiException)
            {
                var ex = exception as ApiException;
                apiError = new ApiError(ex.Message);
                apiError.ValidationErrors = ex.Errors;
                apiError.ReferenceErrorCode = ex.ReferenceErrorCode;
                apiError.ReferenceDocumentLink = ex.ReferenceDocumentLink;

                context.Response.StatusCode = ex.StatusCode;
            }
            else if (exception is UnauthorizedAccessException)
            {
                apiError = new ApiError("Unauthorised Access");
                context.Response.StatusCode = (int) HttpStatusCode.Unauthorized;
            }
            else
            {
#if !DEBUG
                var msg = "An unhandled error occurred.";
                string stack = null;
#else
                var msg = exception.GetBaseException().Message;
                var stack = exception.StackTrace;
#endif

                apiError = new ApiError(msg);
                apiError.Details = stack;
                context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
            }

            //context.Response.ContentType = "application/json";
            response = new ApiResponse(context.Response.StatusCode, ResponseMessageEnum.Exception.GetDescription(),
                null, apiError);

            var json = JsonConvert.SerializeObject(response);
            return context.Response.WriteAsync(json);
        }

        private bool IsSwagger(HttpContext context)
        {
            return context.Request.Path.StartsWithSegments("/swagger");
        }
    }
}
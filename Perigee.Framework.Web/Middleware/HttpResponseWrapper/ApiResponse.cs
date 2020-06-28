namespace Perigee.Framework.Web.Middleware.HttpResponseWrapper
{
    using System.Runtime.Serialization;

    [DataContract]
    public class ApiResponse : ApiResponse<object>
    {
        public ApiResponse(int statusCode, string message = "", object result = null, ApiError apiError = null,
            string apiVersion = "1.0.0.0")
            : base(statusCode, message, result, apiError, apiVersion)
        {
        }
    }

    [DataContract]
    public class ApiResponse<TData>
    {
        public ApiResponse(int statusCode, string message = "", object result = null, ApiError apiError = null,
            string apiVersion = "1.0.0.0")
        {
            StatusCode = statusCode;
            Message = message;
            Result = result;
            ResponseException = apiError;
            Version = apiVersion;
        }

        [DataMember] public string Version { get; set; }

        [DataMember] public int StatusCode { get; set; }

        [DataMember] public string Message { get; set; }

        [DataMember(EmitDefaultValue = false)] public ApiError ResponseException { get; set; }

        [DataMember(EmitDefaultValue = false)] public object Result { get; set; }

        [DataMember] public TData Data { get; set; }
    }
}
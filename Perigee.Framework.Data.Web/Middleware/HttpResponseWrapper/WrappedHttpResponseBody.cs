namespace Perigee.Web.Middleware.HttpResponseWrapper
{
    using System.Runtime.Serialization;

    /// <summary>
    ///     See RESTful Best Practices guide at http://www.restapitutorial.com/resources.html
    /// </summary>
    [DataContract]
    public class WrappedHttpResponseBody<TData>
    {
        public const string StatusSuccess = "success";
        public const string StatusError = "error";
        public const string StatusFail = "fail";
        public const string StatusUnknown = "unknown";


        public WrappedHttpResponseBody(int code, TData data)
        {
            if (code >= 200 && code <= 299)
                Status = StatusSuccess;
            else if (code >= 400 && code <= 499)
                Status = StatusError;
            else if (code >= 500 && code <= 599)
                Status = StatusFail;
            else
                Status = StatusUnknown;

            Code = code;
            Data = data;
        }

        [DataMember] public int Code { get; private set; }

        /// <summary>
        ///     Contains success, fail or error where Fail os for values from 500-599 and error is for 400-499
        /// </summary>
        [DataMember]
        public string Status { get; private set; }

        /// <summary>
        ///     Only used for error and fail statuses
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public string Message { get; private set; }

        /// <summary>
        ///     Contains the response body of the call. If status is error or fail, this contains the cause or exception name
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public TData Data { get; private set; }
    }

    /// <summary>
    ///     See RESTful Best Practices guide at http://www.restapitutorial.com/resources.html
    /// </summary>
    [DataContract]
    public class WrappedHttpResponseBody : WrappedHttpResponseBody<object>
    {
        public WrappedHttpResponseBody(int code) : base(code, null)
        {
        }
    }
}
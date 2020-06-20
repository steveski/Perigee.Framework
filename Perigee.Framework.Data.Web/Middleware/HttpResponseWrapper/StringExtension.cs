namespace Perigee.Framework.Data.Web.Middleware.HttpResponseWrapper
{
    using System;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    public static class StringExtension
    {
        public static bool IsValidJson(this string text)
        {
            text = text.Trim();
            if (text.StartsWith("{") && text.EndsWith("}") || //For object
                text.StartsWith("[") && text.EndsWith("]")) //For array
                try
                {
                    var obj = JToken.Parse(text);
                    return true;
                }
                catch (JsonReaderException jex)
                {
                    return false;
                }
                catch (Exception ex)
                {
                    return false;
                }

            return false;
        }
    }
}
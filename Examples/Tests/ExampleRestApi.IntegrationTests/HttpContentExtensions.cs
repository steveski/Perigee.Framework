namespace ExampleRestApi.IntegrationTests
{
    using System;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text.Json;
    using System.Threading.Tasks;

    public static class HttpContentExtensions
    {
        public static async Task<T> ReadAsJsonAsync<T>(this HttpContent content)
        {
            string json = await content.ReadAsStringAsync();
            T value = JsonSerializer.Deserialize<T>(json);
            return value;
        }

        public static HttpContent ToHttpContent<T>(this T data, string mediaType)
        {
            var content = JsonSerializer.Serialize(data);
            var buffer = System.Text.Encoding.UTF8.GetBytes(content);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue(mediaType);

            return byteContent;
        }

    }

}

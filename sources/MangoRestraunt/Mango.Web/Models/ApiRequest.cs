using static Mango.Web.SD;

namespace Mango.Web.Models
{
    public class ApiRequest
    {
        public ApiType Method { get; set; } = ApiType.GET;

        public string? Url { get; set; }

        public string? AccessToken { get; set; }

        public object? Data { get; set; }
    }
}

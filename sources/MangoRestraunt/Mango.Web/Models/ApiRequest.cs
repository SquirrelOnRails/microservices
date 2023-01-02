using static Mango.Web.Models.ApiRequest<T>;
using static Mango.Web.SD;

namespace Mango.Web.Models
{
    public class ApiRequest<T> : ApiRequest
    {
        public T? Data { get; set; }
    }

    public class ApiRequest
    {
        public ApiType Method { get; set; } = ApiType.GET;

        public string Url { get; set; }

        public string? AccessToken { get; set; }
    }
}

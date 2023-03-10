using Mango.Web.Models;

namespace Mango.Web.Services.Interfaces
{
    public interface IBaseService : IDisposable
    {
        ResponseDto _ResponseModel { get; set; }
        Task<T> SendAsync<T>(ApiRequest apiRequest);
    }
}

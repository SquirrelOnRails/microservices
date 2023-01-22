using Mango.Web.Models;
using Mango.Web.Services.Interfaces;

namespace Mango.Web.Services
{
    public class CouponService : BaseService, ICouponService
    {
        private readonly IHttpClientFactory _clientFactory;

        public CouponService(IHttpClientFactory clientFactory) : base(clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<T> GetCouponAsync<T>(string couponCode, string token)
        {
            return await this.SendAsync<T>(new ApiRequest
            {
                Method = SD.ApiType.GET,
                AccessToken = token,
                Url = new Uri(new Uri(SD.CouponAPIBase), $"/api/coupon/{couponCode}").ToString()
            });
        }
    }
}

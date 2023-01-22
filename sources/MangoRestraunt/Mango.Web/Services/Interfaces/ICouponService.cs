namespace Mango.Web.Services.Interfaces
{
    public interface ICouponService
    {
        Task<T> GetCouponAsync<T>(string couponCode, string token);
    }
}

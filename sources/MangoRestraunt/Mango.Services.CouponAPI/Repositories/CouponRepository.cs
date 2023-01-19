using AutoMapper;
using Mango.Services.CouponAPI.DbContexts;
using Mango.Services.CouponAPI.Dto;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.CouponAPI.Repositories
{
    public interface ICouponRepository
    {
        Task<CouponDto> GetCouponByCode(string couponCode);
    }

    public class CouponRepository : ICouponRepository
    {
        private readonly ApplicationDbContext _DbContext;
        private readonly IMapper _mapper;

        public CouponRepository(ApplicationDbContext dbContext, IMapper mapper)
        {
            _DbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<CouponDto> GetCouponByCode(string couponCode)
        {
            var coupon = await _DbContext.Coupons.FirstOrDefaultAsync(c => c.CouponCode == couponCode);
            return _mapper.Map<CouponDto>(coupon);
        }
    }
}

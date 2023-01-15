using AutoMapper;
using Mango.Services.ShoppingCartApi.DbContexts;
using Mango.Services.ShoppingCartApi.Models;
using Mango.Services.ShoppingCartApi.Models.Dto;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.ShoppingCartApi.Repositories
{
    public interface ICartRepository
    {
        Task<CartDto> GetCartByUserId(string userId);

        Task<CartDto> CreateUpdateCart(CartDto cartDto);

        Task<bool> RemoveFromCart(int cartDetailsId);

        Task<bool> ClearCart(int cartHeaderId);
    }

    public class CartRepository : ICartRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public CartRepository(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<bool> ClearCart(int cartHeaderId)
        {
            var dbCartHeader = await _dbContext.CartHeaders.SingleOrDefaultAsync(h => h.CartHeaderId == cartHeaderId);
            if (dbCartHeader != null)
            {
                var detailsToDelete = _dbContext.CartDetails.Where(d => d.CartHeaderId == dbCartHeader.CartHeaderId);
                _dbContext.CartDetails.RemoveRange(detailsToDelete);
                _dbContext.CartHeaders.Remove(dbCartHeader);

                await _dbContext.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<CartDto> CreateUpdateCart(CartDto cartDto)
        {
            var cart = _mapper.Map<Cart>(cartDto);

            // check if incoming product details has a record in ShoppingCart database.
            foreach (var details in cart.CartDetails)
            {
                var dbProduct = await _dbContext.Products.FirstOrDefaultAsync(p => p.ProductId == details.ProductId);

                // update product if it does.
                // otherwise create product.
                if (dbProduct == null)
                    _dbContext.Products.Add(details.Product);
                else
                    dbProduct = details.Product;

                await _dbContext.SaveChangesAsync();
            }

            // check if CartHeader for the given user exists.
            var dbCartHeader = await _dbContext.CartHeaders
                .AsNoTracking()
                .FirstOrDefaultAsync(h => h.UserId == cart.CartHeader.UserId);
            if (dbCartHeader == null)
            {
                // if it doesn't then create one and populate with the given CartDetails.
                _dbContext.CartHeaders.Add(cart.CartHeader);
                await _dbContext.SaveChangesAsync();
            
                // add CartDetails
                foreach (var details in cart.CartDetails)
                {
                    details.CartHeaderId = cart.CartHeader.CartHeaderId;
                    details.Product = null; // need to prevent creation of a product that already exists in db.
                }
                _dbContext.CartDetails.AddRange(cart.CartDetails);
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                // otherwise populate CartHeader with the given CartDetails.
                foreach (var details in cart.CartDetails)
                {
                    details.CartHeaderId = dbCartHeader.CartHeaderId;
                    details.Product = null; // need to prevent creation of a product that already exists in db.

                    // check if details has same product.
                    var dbCartDetails = await _dbContext.CartDetails
                        .AsNoTracking()
                        .FirstOrDefaultAsync(d => d.CartHeaderId == dbCartHeader.CartHeaderId
                                                  && d.ProductId == details.ProductId);
                    if (dbCartDetails == null)
                    {
                        // create given product's details for the cart if there's no record.
                        _dbContext.CartDetails.Add(details);
                    }
                    else
                    {
                        // otherwise update CartDetails's product count.
                        details.Count += dbCartDetails.Count;
                        details.CartDetailsId = dbCartDetails.CartDetailsId;
                        _dbContext.CartDetails.Update(details);
                    }
                    await _dbContext.SaveChangesAsync();
                }
            }

            return _mapper.Map<CartDto>(cart);
        }

        public async Task<CartDto> GetCartByUserId(string userId)
        {
            var dbCartHeader = await _dbContext.CartHeaders.FirstOrDefaultAsync(h => h.UserId == userId);
            if (dbCartHeader != null)
            {
                var dbDetails = await _dbContext.CartDetails
                    .Include(d => d.Product)
                    .Where(d => d.CartHeaderId == dbCartHeader.CartHeaderId)
                    .ToListAsync();

                var cart = new Cart { CartHeader = dbCartHeader, CartDetails = dbDetails };

                return _mapper.Map<CartDto>(cart);
            }
            
            return null;
        }

        public async Task<bool> RemoveFromCart(int cartDetailsId)
        {
            var detailsToRemove = await _dbContext.CartDetails.FirstOrDefaultAsync(d => d.CartDetailsId == cartDetailsId);
            if (detailsToRemove != null)
            {
                var currentItemsCount = await _dbContext.CartDetails
                    .CountAsync(d => d.CartHeaderId == detailsToRemove.CartHeaderId);
                
                _dbContext.CartDetails.Remove(detailsToRemove);

                if (currentItemsCount == 1)
                {
                    var cartHeaderToRemove = await _dbContext.CartHeaders
                        .FirstAsync(h => h.CartHeaderId == detailsToRemove.CartHeaderId);

                    _dbContext.CartHeaders.Remove(cartHeaderToRemove);
                }

                await _dbContext.SaveChangesAsync();
                return true;
            }

            return false;
        }
    }
}

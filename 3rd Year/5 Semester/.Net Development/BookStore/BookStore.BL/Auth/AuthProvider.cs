using System.Threading.Tasks;
using BookStore.BL.Auth.Entities;
using BookStore.DataAccess;
using BookStore.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookStore.BL.Auth
{
    public class AuthProvider : IAuthProvider
    {
        private readonly ApplicationDBContext _context;

        public AuthProvider(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.UserName == request.UserName && u.Password == request.Password);

            if (user != null)
            {
                return new LoginResponse
                {
                    Success = true,
                    Token = "fake-jwt-token-" + user.UserId 
                };
            }

            return new LoginResponse
            {
                Success = false,
                ErrorMessage = "Invalid credentials"
            };
        }
    }
}

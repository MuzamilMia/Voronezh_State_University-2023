using System.Threading.Tasks;
using BookStore.BL.Auth.Entities;

namespace BookStore.BL.Auth
{
    public interface IAuthProvider
    {
        Task<LoginResponse> LoginAsync(LoginRequest request);
    }
}

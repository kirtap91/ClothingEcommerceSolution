using ClothingEcommerceSharedLibrary.DTOs;
using ClothingEcommerceSharedLibrary.Responses;
namespace ClothingEcommerceServer.Repositories
{
    public interface IUserAccount
    {
        Task<ServiceResponse> Register(UserDTO model);
        Task<LoginResponse> Login(LoginDTO model);
        Task<UserSession> GetUserByToken(string token);
        Task<LoginResponse> GetRefreshToken(PostRefreshTokenDTO model);

    }
}



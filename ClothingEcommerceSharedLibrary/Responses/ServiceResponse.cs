
namespace ClothingEcommerceSharedLibrary.Responses
{
    public record class ServiceResponse(bool IsSuccessful, string Message=null!);
    public record class LoginResponse(bool IsSuccessful, string? Message, string Token = null!, string RefreshToken = null!);
}

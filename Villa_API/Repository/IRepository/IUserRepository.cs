using Villa_API.Models.DTO;
using Villa_API.Models;

namespace Villa_API.Repository.IRepository
{
    public interface IUserRepository
    {
        bool IsUniqueUser(string username); // kiem tra xem id da ton tai chua(id duy nhat)
        Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO);
        Task<UserDTO> Register(RegisterationRequestDTO registerationRequestDTO);
    }
}

using Villa_Web.Models.DTO;

namespace Villa_Web.Services.IServices
{
    public interface IUserService
    {
        Task<T> GetAllAsync<T>(string token);
        Task<T> GetAsync<T>(string id, string token);
        Task<T> DeleteAsync<T>(string id, string token);
    }
}

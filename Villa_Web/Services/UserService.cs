using Newtonsoft.Json.Linq;
using Villa.Utility;
using Villa_Web.Models;
using Villa_Web.Models.DTO;
using Villa_Web.Services.IServices;

namespace Villa_Web.Services
{
    public class UserService : BaseService, IUserService
    {
        private readonly IHttpClientFactory _clientFactory;
        private string villaUrl;

        public UserService(IHttpClientFactory clientFactory, IConfiguration configuration) : base(clientFactory)
        {
            _clientFactory = clientFactory;
            villaUrl = configuration.GetValue<string>("ServiceUrls:VillaApi");

        }
        public Task<T> DeleteAsync<T>(string id, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.DELETE,
                Url = villaUrl + "/api/UserManagement/" + id,
                Token = token
            });
        }


        public Task<T> GetAllAsync<T>(string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = villaUrl + "/api/UserManagement",
                Token = token
            });
        }

        public Task<T> GetAsync<T>(string id, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = villaUrl + "/api/UserManagement/Id/" + id,
                Token = token
            });
        }

        
    }
}

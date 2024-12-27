using System.Linq.Expressions;
using Villa_API.Models;

namespace Villa_API.Repository.IRepository
{
    public interface IApplicationUserRepository : IRepository<ApplicationUser>
    {
        Task<ApplicationUser> UpdateAsync(ApplicationUser applicationUser);
    }
}

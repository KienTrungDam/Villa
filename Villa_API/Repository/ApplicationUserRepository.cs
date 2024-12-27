using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;
using Villa_API.Data;
using Villa_API.Models;
using Villa_API.Repository.IRepository;

namespace Villa_API.Repository
{
    public class ApplicationUserRepository : Reposiory<ApplicationUser>, IApplicationUserRepository
    {
        private ApplicationDbContext _db;
        public ApplicationUserRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public async Task<ApplicationUser> UpdateAsync(ApplicationUser applicationUser)
        {;  
            _db.ApplicationUsers.Update(applicationUser);   
            await _db.SaveChangesAsync();
            return applicationUser;
        }
    }
}

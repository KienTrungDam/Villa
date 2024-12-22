using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;
using Villa_API.Data;
using Villa_API.Models;
using Villa_API.Repository.IRepository;

namespace Villa_API.Repository
{
    public class VillaNumberRepository : Reposiory<VillaNumber>, IVillaNumberRepository
    {
        private ApplicationDbContext _db;
        public VillaNumberRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public async Task<VillaNumber> UpdateAsync(VillaNumber entity)
        {
            entity.UpdatedDate = DateTime.Now;  
            _db.VillaNumbers.Update(entity);   
            await _db.SaveChangesAsync();
            return entity;
        }
    }
}

namespace Villa_API.Repository.IRepository
{
    public interface IUnitOfWork
    {
        IVillaRepository Villa { get; }
        IVillaNumberRepository VillaNumber { get; }
        IUserRepository User { get; }
        IApplicationUserRepository ApplicationUser { get; }
    }
}

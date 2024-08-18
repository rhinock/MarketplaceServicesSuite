using Ardalis.SharedKernel;
using Ardalis.Specification.EntityFrameworkCore;

namespace Catalog.Infrastructure.Data;

public class EfRepository<T> : RepositoryBase<T>, IReadRepository<T>, IRepository<T> where T : class, IAggregateRoot
{
    public EfRepository(AppDbContext dbContext) : base(dbContext) { }
}
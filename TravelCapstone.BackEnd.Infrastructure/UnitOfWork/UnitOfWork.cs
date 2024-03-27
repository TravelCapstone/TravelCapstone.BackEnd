using TravelCapstone.BackEnd.Application.IUnitOfWork;
using TravelCapstone.BackEnd.Domain.IData;

namespace TravelCapstone.BackEnd.Infrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDbContext _context;

        public UnitOfWork(IDbContext context)
        {
            _context = context;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
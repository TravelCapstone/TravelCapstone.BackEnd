using NetCore.QK.DbContext;
using TravelCapstone.BackEnd.Application.IRepositories;
using TravelCapstone.BackEnd.Domain.Models;

namespace TravelCapstone.BackEnd.Infrastructure.Repositories;

public class AccountRepository : GenericRepository<Account>, IAccountRepository
{
    public AccountRepository(IDbContext context) : base(context)
    {
    }
}
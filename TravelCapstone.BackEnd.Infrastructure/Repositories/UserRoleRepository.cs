using Microsoft.AspNetCore.Identity;
using NetCore.QK.DbContext;
using TravelCapstone.BackEnd.Application.IRepositories;

namespace TravelCapstone.BackEnd.Infrastructure.Repositories;

public class UserRoleRepository: GenericRepository<IdentityUserRole<string>>,IUserRoleRepository
{
    public UserRoleRepository(IDbContext context) : base(context)
    {
    }
}
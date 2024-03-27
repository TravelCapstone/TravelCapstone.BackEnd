using Microsoft.AspNetCore.Identity;
using NetCore.QK.DbContext;
using TravelCapstone.BackEnd.Application.IRepositories;

namespace TravelCapstone.BackEnd.Infrastructure.Repositories;

public class IdentityRoleRepository: GenericRepository<IdentityRole>,IIdentityRoleRepository
{
    public IdentityRoleRepository(IDbContext context) : base(context)
    {
    }
}
using Microsoft.AspNetCore.Identity;
using NetCore.QK.DbContext;

namespace TravelCapstone.BackEnd.Application.IRepositories;

public interface IUserRoleRepository : IRepository<IdentityUserRole<string>>
{
}
namespace TravelCapstone.BackEnd.Application.IUnitOfWork;

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync();
}
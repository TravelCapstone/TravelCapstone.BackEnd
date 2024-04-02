namespace TravelCapstone.BackEnd.Application;

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync();
}
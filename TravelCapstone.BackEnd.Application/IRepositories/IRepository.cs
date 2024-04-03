using System.Linq.Expressions;
using TravelCapstone.BackEnd.Common.DTO.Response;

namespace TravelCapstone.BackEnd.Application.IRepositories;

public interface IRepository<T> where T : class
{
    Task<PagedResult<T>> GetAllDataByExpression(Expression<Func<T, bool>>? filter, int pageNumber, int pageSize,
        params Expression<Func<T, object>>[]? includes);

    // Task<List<T>> GetAllDataByExpression(Expression<Func<T, bool>> filter,
    //     params Expression<Func<T, object>>[] includes);

    Task<T> GetById(object id);

    Task<T?> GetByExpression(Expression<Func<T?, bool>> filter,
        params Expression<Func<T, object>>[]? includeProperties);

    Task<T> Insert(T entity);

    Task<List<T>> InsertRange(IEnumerable<T> entities);

    Task<List<T>> DeleteRange(IEnumerable<T> entities);

    Task<T> Update(T entity);

    Task<T?> DeleteById(object id);
}
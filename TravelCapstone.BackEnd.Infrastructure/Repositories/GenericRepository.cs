using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TravelCapstone.BackEnd.Application.IRepositories;
using TravelCapstone.BackEnd.Common.DTO;
using TravelCapstone.BackEnd.Domain.IData;

namespace TravelCapstone.BackEnd.Infrastructure.Repositories;

public class GenericRepository<T> : IRepository<T> where T : class
{
    private readonly IDbContext _context;

    public GenericRepository(IDbContext context)
    {
        _context = context;
    }

    public async Task<PagedResult<T>> GetAllDataByExpression(Expression<Func<T, bool>>? filter, int pageNumber,
        int pageSize, params Expression<Func<T, object>>[]? includes)
    {
        IQueryable<T> query = _context.Set<T>();

        if (filter != null) query = query.Where(filter);

        if (includes != null)
            foreach (var include in includes)
                query = query.Include(include);

        var totalItems = await query.CountAsync();
        var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

        query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);

        var result = new PagedResult<T>
        {
            Items = await query.ToListAsync(),
            TotalPages = totalPages
        };

        return result;
    }

    public async Task<T> GetById(object id)
    {
        return (await _context.Set<T>().FindAsync(id))!;
    }

    public async Task<T> Insert(T entity)
    {
        await _context.Set<T>()
            .AddAsync(entity);
        return entity;
    }

    public Task<T> Update(T entity)
    {
        _context.Set<T>().Attach(entity);
        _context.Entry(entity).State = EntityState.Modified;
        return Task.FromResult(entity);
    }

    public async Task<T?> DeleteById(object id)
    {
        var entityToDelete = await _context.Set<T>()
            .FindAsync(id);
        if (entityToDelete != null)
            _context.Set<T>()
                .Remove(entityToDelete);

        return entityToDelete;
    }

    public async Task<T?> GetByExpression(Expression<Func<T?, bool>> filter,
        params Expression<Func<T, object>>[]? includeProperties)
    {
        IQueryable<T> query = _context.Set<T>();

        if (includeProperties != null)
            foreach (var includeProperty in includeProperties)
                query = query.Include(includeProperty!);

        return await query.SingleOrDefaultAsync(filter);
    }

    public Task<List<T>> InsertRange(IEnumerable<T> entities)
    {
        var enumerable = entities.ToList();
        _context.Set<T>()
            .AddRange(enumerable);
        return Task.FromResult(enumerable.ToList());
    }

    public Task<List<T>> DeleteRange(IEnumerable<T> entities)
    {
        var enumerable = entities as T[] ?? entities.ToArray();
        _context.Set<T>()
            .RemoveRange(enumerable);
        return Task.FromResult(enumerable.ToList());
    }
}
using Application;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class WeatherContextRepository<TEntity> : IRepository<TEntity>
    where TEntity : class
{
    private readonly WeatherContext _context;
    private readonly DbSet<TEntity> _dbSet;

    public WeatherContextRepository(WeatherContext context)
    {
        _context = context;
        _dbSet = context.Set<TEntity>();
    }

    public async Task Create(TEntity entity)
    {
        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
    }
}
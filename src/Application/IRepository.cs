namespace Application;

public interface IRepository<TEntity> where TEntity : class
{
    Task Create(TEntity entity);
}
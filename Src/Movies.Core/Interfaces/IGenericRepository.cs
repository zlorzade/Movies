namespace Movies.Core
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        Task AddRangeAsync(IEnumerable<TEntity> entities);
    }
}

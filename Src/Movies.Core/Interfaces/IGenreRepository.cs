

namespace Movies.Core
{
    public interface IGenreRepository : IGenericRepository<Genre>
    {
        Task<IDictionary<int, int>> GetGenresCodeAndId();
        Task<IEnumerable<Genre>> GetAllGenres(int numberOfGenres = 6, int numberOfMovies = 10);
        Task<IEnumerable<Genre>> GetGenresByIds(IEnumerable<int> ids);
        void RemoveRangeByIds(IEnumerable<int> ids);
    }
}

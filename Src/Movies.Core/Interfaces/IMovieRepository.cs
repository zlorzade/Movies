namespace Movies.Core
{
    public interface IMovieRepository : IGenericRepository<Movie>
    {
        Task<IDictionary<int, int>> GetMoviesCodeAndId();
        Task<IEnumerable<Movie>> GetMoviesByIds(IEnumerable<int> ids);
        Task<IEnumerable<Movie>> GetMoviesByGenreId(int genreId, int pageSize, int pageNumber);
        void RemoveRangeByIds(IEnumerable<int> ids);
    }
}

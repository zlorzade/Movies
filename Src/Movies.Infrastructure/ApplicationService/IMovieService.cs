using Movies.Core;

namespace Movies.Infrastructure
{
    public interface IMovieService
    {
        Task SyncMovies(IEnumerable<MovieDto> movieDtos);
        Task<IEnumerable<Movie>> GetMoviesByGenreId(int genreId, int pageSize, int pageNumber);
    }
}

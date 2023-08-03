using Movies.Core;


namespace Movies.Infrastructure
{
    public interface IGenreService
    {
        Task SyncGenres(IEnumerable<GenreDto> genreDtos);
        Task<IEnumerable<Genre>> GetAll();
    }
}

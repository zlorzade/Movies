using Microsoft.EntityFrameworkCore;
using Movies.Core;


namespace Movies.Infrastructure
{
    public class MovieRepository : GenericRepository<Movie>, IMovieRepository
    {
        public MovieRepository(ApplicationContext context) : base(context)
        {

        }
        public async Task<IDictionary<int, int>> GetMoviesCodeAndId()
        {
            return await _context.Movies
                .AsNoTracking()
                .Select(c => new { c.Id, c.Code })
                .ToDictionaryAsync(k => k.Code, v => v.Id);
        }

        public async Task<IEnumerable<Movie>> GetMoviesByIds(IEnumerable<int> ids)
        {
            return await _context.Movies.Where(c => ids.Contains(c.Id)).ToListAsync();

        }

        public async Task<IEnumerable<Movie>> GetMoviesByGenreId(int genreId, int pageSize, int pageNumber)
        {
            pageNumber = pageNumber <= 0 ? 1 : pageNumber;
            return await _context.Movies.AsNoTracking()
                .Where(g => g.GenreId == genreId && g.IsActive)
                .OrderByDescending(c => c.Id)
                .Skip((pageNumber - 1) * pageSize).Take(pageSize)
                .ToListAsync();
        }

        public void RemoveRangeByIds(IEnumerable<int> ids)
        {
            _context.Movies.RemoveRange(_context.Movies.Where(c => ids.Contains(c.Id)));

        }

    }
}

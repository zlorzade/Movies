using Microsoft.EntityFrameworkCore;
using Movies.Core;

namespace Movies.Infrastructure
{
    public class GenreRepository : GenericRepository<Genre>, IGenreRepository
    {
        public GenreRepository(ApplicationContext context) : base(context)
        {
        }

        public async Task<IDictionary<int, int>> GetGenresCodeAndId()
        {
            return await _context.Genres
                .AsNoTracking()
                .Select(c => new { c.Id, c.Code })
                .ToDictionaryAsync(k => k.Code, v => v.Id);
        }

        public async Task<IEnumerable<Genre>> GetGenresByIds(IEnumerable<int> ids)
        {
            return await _context.Genres.Where(c => ids.Contains(c.Id)).ToListAsync();

        }

        public async Task<IEnumerable<Genre>> GetAllGenres(int numberOfGenres = 6, int numberOfMovies = 10)
        {
            return await _context.Genres.AsNoTracking()
                .Include(g => g.Movies.Where(c => c.IsActive).OrderByDescending(c => c.Id).Take(numberOfMovies))
                .OrderBy(c => c.Priority)
                .Where(c => c.Movies.Any(m => m.IsActive) && c.IsActive)
                .Take(numberOfGenres)
                .ToListAsync();
        }

        public void RemoveRangeByIds(IEnumerable<int> ids)
        {
            _context.Genres.RemoveRange(_context.Genres.Where(c => ids.Contains(c.Id)));

        }
    }
}

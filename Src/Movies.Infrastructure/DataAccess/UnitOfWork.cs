using Microsoft.EntityFrameworkCore.Storage;
using Movies.Core;

namespace Movies.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        public UnitOfWork(ApplicationContext context, IMovieRepository movieRepository, IGenreRepository genreRepository)
        {
            _context = context;
            MovieRepository = movieRepository;
            GenreRepository = genreRepository;

        }
        private readonly ApplicationContext _context;
        private IDbContextTransaction _objTran;

        public IMovieRepository MovieRepository { get; }
        public IGenreRepository GenreRepository { get; }


        public void CreateTransaction()
        {
            _objTran = _context.Database.BeginTransaction();
        }
        public async Task CreateTransactionAsync()
        {
            _objTran = await _context.Database.BeginTransactionAsync();
        }
        public void Commit()
        {
            _objTran.Commit();
        }
        public async Task CommitAsync()
        {
            await _objTran.CommitAsync();
        }
        public void Rollback()
        {
            _objTran.Rollback();
            _objTran.Dispose();
        }
        public async Task RollbackAsync()
        {
            await _objTran.RollbackAsync();
            _objTran.Dispose();
        }
        public void Save()
        {
            _context.SaveChanges();
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }

    }
}

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Core
{
    public interface IUnitOfWork : IDisposable
    {
        IGenreRepository GenreRepository { get; }
        IMovieRepository MovieRepository { get; }
        void Commit();
        Task CommitAsync();
        void CreateTransaction();
        Task CreateTransactionAsync();
        void Rollback();
        Task RollbackAsync();
        void Save();
        Task SaveAsync();
    }
}

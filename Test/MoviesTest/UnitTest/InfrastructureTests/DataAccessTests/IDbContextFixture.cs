using Movies.Infrastructure;
using System;

namespace DataAccess.UnitTest;

public interface IDbContextFixture : IAsyncDisposable, IDisposable
{
    ApplicationContext DbContextFake { get; }
}

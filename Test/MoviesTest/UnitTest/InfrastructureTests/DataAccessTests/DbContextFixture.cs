using Microsoft.EntityFrameworkCore;
using Movies.Infrastructure;
using System.Threading.Tasks;

namespace DataAccess.UnitTest;

public sealed class DbContextFixture : IDbContextFixture
{
    private readonly DbContextOptions<ApplicationContext> _options;

    public ApplicationContext DbContextFake { get; }

    public DbContextFixture()
    {
        _options = new DbContextOptionsBuilder<ApplicationContext>()
            .UseSqlite("Data Source=LocalDatabase.db")
            .Options;

        DbContextFake = new ApplicationContext(_options);
        DbContextFake.Database.EnsureCreated();

    }

    public void Dispose()
    {
        DbContextFake.Database.EnsureDeleted();
        DbContextFake.Dispose();
    }
    public async ValueTask DisposeAsync()
    {
        await DbContextFake.Database.EnsureDeletedAsync();
        await DbContextFake.DisposeAsync();
    }
}
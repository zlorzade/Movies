using DataAccess.UnitTest;
using Movies.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Infrastructure.DataAccess
{
    [Collection("Sequential")]
    public sealed class GenreRepositoryTests : IClassFixture<DbContextFixture>
    {
        private readonly DbContextFixture _fixture;

        public GenreRepositoryTests(DbContextFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task AddRange_Genres_Should_Be_Successful()
        {
            var genreRepository = new GenreRepository(_fixture.DbContextFake);

            var genres = AppDbContextData.Genres;

            var exception = await Record.ExceptionAsync(() => genreRepository.AddRangeAsync(genres));
            _fixture.DbContextFake.SaveChanges();

            Assert.Null(exception);
            Assert.True(_fixture.DbContextFake.Genres.Any());
        }

        [Fact]
        public async Task GetAll_Genres_Should_Be_Return_All_Genres_That_Have_Active_Movie_Also_Self_IsActive()
        {
            if (!_fixture.DbContextFake.Genres.Any())
            {
                _fixture.DbContextFake.Genres.AddRange(AppDbContextData.Genres);
                _fixture.DbContextFake.SaveChanges();
            }
            if (!_fixture.DbContextFake.Movies.Any())
            {
                _fixture.DbContextFake.Movies.AddRange(AppDbContextData.Movies);
                _fixture.DbContextFake.SaveChanges();
            }

            var genreRepository = new GenreRepository(_fixture.DbContextFake);

            var genres = await genreRepository.GetAllGenres();

            Assert.True(genres.Any());
        }
    }
}
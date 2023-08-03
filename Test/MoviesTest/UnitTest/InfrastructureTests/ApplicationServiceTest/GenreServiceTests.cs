using DataAccess.UnitTest;
using Moq;
using Movies.Core;
using Movies.Infrastructure;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Infrastructure.ApplicationService
{
    [Collection("Sequential")]
    public sealed class GenreServiceTests : IClassFixture<DbContextFixture>
    {
        private readonly DbContextFixture _fixture;

        public GenreServiceTests(DbContextFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task Sync_Genres_Should_Be_Successful()
        {
            var unitOfWork = new Mock<IUnitOfWork>();
            var genreRepository = new Mock<IGenreRepository>();
            unitOfWork.SetupGet(u => u.GenreRepository).Returns(genreRepository.Object);

            var genreDtos = new List<GenreDto>()
            {
                new()
                {
                    Code = 2000,
                    Name = "ترسناک",
                    Priority = 6,
                    description = "توضیحات",
                    Status = "فعال",
                    LastUpdatedDate = System.DateTime.Now
                },
                new()
                {
                    Code = 2001,
                    Name = "حماسی",
                    Priority = 7,
                    description = "توضیحات",
                    Status = "فعال",
                    LastUpdatedDate = System.DateTime.Now
                }
            };
            var genresFromDb = new List<Genre>()
            {
                new()
                {
                    Code = 2000,
                    Name = "ترسناک",
                    Priority = 2,
                    description = "توضیحات",
                    IsActive = true,
                    LastUpdatedDate = System.DateTime.Now
                }
            };
            genreRepository.Setup(x => x.GetGenresCodeAndId()).ReturnsAsync(AppDbContextData.Genres
                                                                    .Select(c => new { c.Id, c.Code })
                                                                    .ToDictionary(k => k.Code, v => v.Id));
            genreRepository.Setup(x => x.GetGenresByIds(It.IsAny<IEnumerable<int>>())).ReturnsAsync(genresFromDb);
            var genreService = new GenreService(unitOfWork.Object);
            var exception = await Record.ExceptionAsync(() => genreService.SyncGenres(genreDtos));

            Assert.Null(exception);
        }
    }
}
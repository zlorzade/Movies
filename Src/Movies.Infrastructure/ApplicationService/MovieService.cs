using Movies.Core;

namespace Movies.Infrastructure
{
    public class MovieService : IMovieService
    {
        private readonly IUnitOfWork _unitOfWork;

        public MovieService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task SyncMovies(IEnumerable<MovieDto> movieDtos)
        {
            IDictionary<Movie, int> DicOfmoviesAndGenreCodeFromInput = ConvertToDicOfMovieAndGenreCode(movieDtos.ToList());
            IDictionary<int, int> moviesCodeAndIdExistedInDb = await _unitOfWork.MovieRepository.GetMoviesCodeAndId();
            IDictionary<int, int> generesCodeAndIdExistedInDb = await _unitOfWork.GenreRepository.GetGenresCodeAndId();

            IEnumerable<Movie> moviesFromInput = FillGenreId(DicOfmoviesAndGenreCodeFromInput, generesCodeAndIdExistedInDb);
            List<int> mustBeDeleteMoviesId =
                 moviesCodeAndIdExistedInDb
                .ExceptBy(DicOfmoviesAndGenreCodeFromInput.Values, c => c.Key)
                .Select(c => c.Value)
                .ToList();

            List<int> mustBeUpdateMoviesId = moviesCodeAndIdExistedInDb.Values.Except(mustBeDeleteMoviesId).ToList();
            IEnumerable<Movie> mustBeUpdateMovies = await _unitOfWork.MovieRepository.GetMoviesByIds(mustBeUpdateMoviesId);

            List<Movie> mustBeAddMovies = moviesFromInput
                .Where(c => !moviesCodeAndIdExistedInDb.ContainsKey(c.Code))
                .ToList();

            UpdateMovies(moviesFromInput, mustBeUpdateMovies);
            await CommitToDatabase(mustBeAddMovies, mustBeDeleteMoviesId);
        }

        private void UpdateMovies(IEnumerable<Movie> moviesFromExcel, IEnumerable<Movie> moviesFromDb)
        {
            var mustBeUpdateMovies = moviesFromDb.Except(moviesFromExcel).ToList();
            mustBeUpdateMovies.ForEach((d =>
            {
                var item = moviesFromExcel.Single(e => e.Code == d.Code);
                d.Name = item.Name;
                d.description = item.description;
                d.IsActive = item.IsActive;
                d.LastUpdatedDate = item.LastUpdatedDate;
                d.GenreId = item.GenreId;
            }));
        }

        public async Task<IEnumerable<Movie>> GetMoviesByGenreId(int genreId, int pageSize, int pageNumber)
        {
            return await _unitOfWork.MovieRepository.GetMoviesByGenreId(genreId, pageSize, pageNumber);
        }

        private async Task CommitToDatabase(IEnumerable<Movie> mustBeAdd, IEnumerable<int> mustBeDelete)
        {
            try
            {
                await _unitOfWork.CreateTransactionAsync();

                await _unitOfWork.MovieRepository.AddRangeAsync(mustBeAdd);
                _unitOfWork.MovieRepository.RemoveRangeByIds(mustBeDelete);
                await _unitOfWork.SaveAsync();

                await _unitOfWork.CommitAsync();
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackAsync();
                _unitOfWork.Dispose();
                throw;
            }
        }
        private IEnumerable<Movie> FillGenreId(IDictionary<Movie, int> dicOfMovieAndCode, IDictionary<int, int> generesIdsExistedInDb)
        {
            return dicOfMovieAndCode.Select(c => new Movie()
            {
                Id = c.Key.Id,
                GenreId = generesIdsExistedInDb[c.Value],
                Name = c.Key.Name,
                Code = c.Key.Code,
                LastUpdatedDate = c.Key.LastUpdatedDate,
                description = c.Key.description,
                IsActive = c.Key.IsActive,
            });
        }

        private IDictionary<Movie, int> ConvertToDicOfMovieAndGenreCode(List<MovieDto> movieDtos)
        {
            var DicOfMovieAndSelfCode = new Dictionary<Movie, int>();
            movieDtos.ForEach(c => DicOfMovieAndSelfCode.Add(new Movie()
            {
                Code = (int)c.Code,
                Name = c.Name,
                IsActive = c.Status == "فعال",
                description = c.description,

                LastUpdatedDate = c.LastUpdatedDate
            }, (int)c.GenreCode));

            return DicOfMovieAndSelfCode;
        }
    }
}
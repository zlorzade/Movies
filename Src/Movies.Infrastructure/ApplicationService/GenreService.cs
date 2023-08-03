using Movies.Core;

namespace Movies.Infrastructure
{
    public class GenreService : IGenreService
    {
        private readonly IUnitOfWork _unitOfWork;

        public GenreService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Genre>> GetAll()
        {
            return await _unitOfWork.GenreRepository.GetAllGenres();
        }

        public async Task SyncGenres(IEnumerable<GenreDto> genreDtos)
        {

            IEnumerable<Genre> generesFromInput = ConvertToGenres(genreDtos);
            List<int> genresCodeFromInput = generesFromInput.Select(c => c.Code).ToList();

            IDictionary<int, int> dicOfGenresCodeAndIDFromDb = await _unitOfWork.GenreRepository.GetGenresCodeAndId();

            List<Genre> mustBeAddGenres = generesFromInput.Where(c => !dicOfGenresCodeAndIDFromDb.ContainsKey(c.Code)).ToList();
            List<int> mustBeDeleteGenresId =
                dicOfGenresCodeAndIDFromDb
                .ExceptBy(genresCodeFromInput, c => c.Key)
                .Select(c => c.Value)
                .ToList();

            List<int> ExistInDbGenresId = dicOfGenresCodeAndIDFromDb.Values.Except(mustBeDeleteGenresId).ToList();

            IEnumerable<Genre> mustBeUpdateGenres = await _unitOfWork.GenreRepository.GetGenresByIds(ExistInDbGenresId);
            UpdateGenres(generesFromInput, mustBeUpdateGenres);

            await CommitToDatabase(mustBeAddGenres, mustBeDeleteGenresId);
        }

        private void UpdateGenres(IEnumerable<Genre> generesFromInput, IEnumerable<Genre> genresFromDb)
        {
            List<Genre> MustBeUpdateGenres = genresFromDb.Except(generesFromInput).ToList();
            MustBeUpdateGenres.ForEach(d =>
            {
                var item = generesFromInput.Single(e => e.Code == d.Code);
                d.Name = item.Name;
                d.description = item.description;
                d.IsActive = item.IsActive;
                d.Priority = item.Priority;
                d.LastUpdatedDate = item.LastUpdatedDate;
            });
        }

        private async Task CommitToDatabase(IEnumerable<Genre> mustBeAdd, IEnumerable<int> mustBeDelete)
        {
            try
            {
                await _unitOfWork.CreateTransactionAsync();

                await _unitOfWork.GenreRepository.AddRangeAsync(mustBeAdd);
                _unitOfWork.GenreRepository.RemoveRangeByIds(mustBeDelete);
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
        private IEnumerable<Genre> ConvertToGenres(IEnumerable<GenreDto> genreDtos)
        {
            return genreDtos.Select(x => new Genre()
            {
                Code = (int)x.Code,
                Name = x.Name,
                IsActive = x.Status == "فعال",
                description = x.description,
                Priority = (int)x.Priority,
                LastUpdatedDate = x.LastUpdatedDate
            }).ToList();
        }
    }
}

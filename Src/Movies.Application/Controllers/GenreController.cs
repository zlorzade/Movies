using Microsoft.AspNetCore.Mvc;
using Movies.Infrastructure;

namespace Movies.Application
{
    [ApiController]
    [Route("Genre")]
    public class GenreController : Controller
    {
        private readonly IGenreService _genreService;

        public GenreController(IGenreService genreService)
        {
            _genreService = genreService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add(IFormFile file)
        {
            using var stream = file.OpenReadStream();
            var genres = ExcelFileReader.Read<GenreDto>(stream);
            await _genreService.SyncGenres(genres);

            return Ok();
        }



        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _genreService.GetAll();

            return Ok(result);
        }
    }
}

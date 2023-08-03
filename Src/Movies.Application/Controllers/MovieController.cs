using Microsoft.AspNetCore.Mvc;
using Movies.Core;
using Movies.Infrastructure;
using OfficeOpenXml;
using System.Collections.Generic;

namespace Movies.Application
{

    [ApiController]
    [Route("Movie")]
    public class MovieController : Controller
    {
        private readonly IMovieService _movieService;

        public MovieController(IMovieService movieService)
        {
            _movieService = movieService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add(IFormFile file)
        {
            using var stream = file.OpenReadStream();
            var movies = ExcelFileReader.Read<MovieDto>(stream);
            await _movieService.SyncMovies(movies);
            return Ok();
        }

        [HttpGet("GetByGenreId/{genreId}")]
        public async Task<IActionResult> GetByGenreId([FromRoute] int genreId, [FromQuery] int pageSize, [FromQuery] int pageNumber)
        {
            var result = await _movieService.GetMoviesByGenreId(genreId, pageSize, pageNumber);

            return Ok(result);
        }


    }

}

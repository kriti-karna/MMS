using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MMS.Models;

namespace MMS.Controllers
{
    public class MovieController : Controller
    {
        private readonly MoviesContext _context;

        public MovieController(MoviesContext context)
        {
            _context = context;
        }
        public IActionResult Index(string? movieName, int? language)
        {
            var checkLang = _context.Languages.Where(m => m.Status == 1).Count();
            if (checkLang == 0)
            {
                TempData["Error"] = "There is no language data at present. Please enter language to open Movies page.";
                return View("../Home/Dashboard");
            }
            string rawQuery = "SELECT * FROM dbo.movies WHERE status = 1 ";
            if (movieName != null)
            {
                rawQuery += "AND name LIKE '%" + movieName + "%' ";
            }
            if (language != null)
            {
                rawQuery += "AND language = " + language;
            }
            //rawQuery += ";";
            var movies = _context.Movies.FromSqlRaw(rawQuery).OrderBy(m => m.Name).ToList();
            var languages = _context.Languages.Where(m => m.Status == 1).ToList();
            var genres = _context.Genres.ToList();
            ViewData["Movie"] = movies;
            ViewData["Genre"] = genres;
            ViewData["Language"] = languages;
            ViewData["MovieSearch"] = movieName != null ? movieName : "";
            ViewData["LanguageSearch"] = language != null ? language.ToString() : "";
            return View();
        }



        public ActionResult AddMovie(string genre, Movie movie, string type)
        {
            var language = _context.Languages.SingleOrDefault(m => m.Id == movie.Language);
            if (type == "add")
            {
                movie.Status = 1;
                movie.LanguageNavigation = language;
                _context.Add(movie);
                _context.SaveChanges();
                
            }
            else if(type == "edit")
            {
                var existingMovie = _context.Movies.SingleOrDefault(m => m.Id == movie.Id);
                string rawQuery = "DELETE FROM dbo.genre WHERE movie_id = " + movie.Id;
                _context.Database.ExecuteSqlRaw(rawQuery); //To replace the genres if updated
                _context.SaveChanges();
                existingMovie.Name = movie.Name;
                existingMovie.Language = movie.Language;
                existingMovie.LanguageNavigation = language;
                _context.Update(existingMovie);
                _context.SaveChanges();
                movie = existingMovie;
            }
            string[] genres = genre.Split(",");
            foreach (var item in genres)
            {
                Genre gen = new Genre()
                {
                    MovieId = movie.Id,
                    Name = item,
                    Movie = movie
                };
                _context.Add(gen);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        public string DeleteMovie(int id)
        {
            string result;
            try
            {
                var movie = _context.Movies.SingleOrDefault(m => m.Id == id);
                movie.Status = 0;
                _context.Update(movie);
                _context.SaveChanges();
                result = "success";
            }
            catch
            {
                result = "error";
            }
            return result;
        }

        public IActionResult GetMovie(int id)
        {
            var movie = _context.Movies.SingleOrDefault(m => m.Id == id);
            return Json(movie);
        }

        public IActionResult GetGenres(int id)
        {
            var genres = _context.Genres.Where(m => m.MovieId == id).ToList();
            return Json(genres);
        }
    }
}

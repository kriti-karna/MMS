using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MMS.Models;

namespace MMS.Controllers
{
    public class LanguageController : Controller
    {
        private readonly MoviesContext _context;

        public LanguageController(MoviesContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var languages = _context.Languages
                .Where(m => m.Status == 1)
                .OrderBy(m => m.Title)
                .ToList();
            return View(languages);
        }

        public ActionResult AddLanguage(Language language, string type)
        {
            if (type == "add")
            {
                language.Status = 1;
                _context.Add(language);
                _context.SaveChanges();
            }
            else if (type == "edit")
            {
                var existing = _context.Languages.SingleOrDefault(m => m.Id == language.Id);
                existing.Title = language.Title;
                _context.Update(existing);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public string DeleteLanguage(int id)
        {
            string result;
            try
            {
                if (_context.Movies.Where(m => m.Language == id && m.Status == 1).Count() == 0)
                {
                    var language = _context.Languages.SingleOrDefault(m => m.Id == id);
                    language.Status = 0;
                    _context.Update(language);
                    _context.SaveChanges();
                    result = "success";
                }
                else
                {
                    result = "movie";
                }
            }
            catch
            {
                result = "error";
            }
            return result;
        }
    }
}

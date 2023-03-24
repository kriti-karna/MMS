using Microsoft.AspNetCore.Mvc;
using MMS.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Cryptography;
using System.Text;

namespace MMS.Controllers
{
    [Obsolete]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly MoviesContext _context;

        public HomeController(ILogger<HomeController> logger, MoviesContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Dashboard()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(User user)
        {
            var hashP = HashPassword(user.Password);
            var checkUser = _context
                .Users
                .SingleOrDefault(m => m.Username == user.Username && m.Password == hashP);
            if(checkUser == null)
            {
                TempData["Error"] = "Invalid username or password";
                return View("Index");
            }
            HttpContext.Session.SetString("Name", checkUser.Name);
            HttpContext.Session.SetString("Username", checkUser.Username);
            return View("Dashboard");
        }

        public string HashPassword(string Password)
        {
            HashAlgorithm MD5 = new MD5CryptoServiceProvider();
            byte[] bs = Encoding.UTF8.GetBytes(Password);
            bs = MD5.ComputeHash(bs);
            StringBuilder s = new StringBuilder();
            foreach (byte b in bs)
                s.Append(b.ToString("x2").ToLower());
            string hash = s.ToString();
            return hash;
        }


        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
    }
}
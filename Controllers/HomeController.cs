using MySql.Data.MySqlClient;

using Microsoft.AspNetCore.Mvc;
using SafeVault.Data;
using SafeVault.Models;
using SafeVault.Services;

namespace SafeVault.Controllers
{
    public class HomeController : SecureController
    {
        private readonly UserRepository _userRepository;

        public HomeController(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var redirect = RequireLogin();
            if (redirect != null) return redirect;
            
            return View("WebForm");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }


        [HttpPost("/submit")]
        public IActionResult Submit(string username, string email)
        {
            var cleanedUsername = InputSanitizer.Clean(username);
            var cleanedEmail = InputSanitizer.Clean(email);

            if (!InputSanitizer.IsValidUsername(cleanedUsername))
            {
                ModelState.AddModelError("Username", "Invalid username format.");
            }

            if (!InputSanitizer.IsValidEmail(cleanedEmail))
            {
                ModelState.AddModelError("Email", "Invalid email format.");
            }

            if (!ModelState.IsValid)
            {
                // Return the form with validation messages
                ViewData["Error"] = "Invalid input detected.";
                return View("WebForm");
            }

            var user = new User
            {
                Username = cleanedUsername,
                Email = cleanedEmail
            };

            // Save securely with parameterized query
            _userRepository.InsertUserWithPassword(user);

            ViewData["Message"] = "User saved securely.";
            return View("WebForm");
        }

        // [HttpGet("/testdb")]
        // public IActionResult TestDb()
        // {
        //     using var conn = new MySqlConnection(_connectionString);
        //     conn.Open();
        //     return Ok("Connected to MySQL successfully");
        // }

    }
}

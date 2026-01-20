using Microsoft.AspNetCore.Mvc;
using SafeVault.Data;
using SafeVault.Models;
using SafeVault.Services;

namespace SafeVault.Controllers
{
    public class RegistrationController : Controller
    {
        private readonly UserRepository _userRepository;

        public RegistrationController(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(string username, string email, string password)
        {
            // Sanitize input
            username = InputSanitizer.Clean(username);
            email = InputSanitizer.Clean(email);
            password = InputSanitizer.Clean(password);

            if (string.IsNullOrWhiteSpace(username) ||
                string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(password))
            {
                ModelState.AddModelError("", "All fields are required.");
                return View("Index");
            }

            // Generate salt + hash
            var salt = PasswordService.GenerateSalt();
            var hash = PasswordService.HashPassword(password, salt);

            // Build user object
            var user = new User
            {
                Username = username,
                Email = email,
                PasswordSalt = salt,
                PasswordHash = hash,
                Role = "user" // default role
            };

            // Save to DB
            _userRepository.InsertUserWithPassword(user);

            // Redirect to login page or success page
            return RedirectToAction("Index", "Login");
        }
    }
}

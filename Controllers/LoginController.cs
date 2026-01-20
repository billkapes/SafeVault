using Microsoft.AspNetCore.Mvc;
using SafeVault.Data;
using SafeVault.Services;

namespace SafeVault.Controllers
{
    public class LoginController : Controller
    {
        private readonly UserRepository _userRepository;

        public LoginController(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Authenticate(string username, string password)
        {
            // Sanitize input
            username = InputSanitizer.Clean(username);
            password = InputSanitizer.Clean(password);

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                ModelState.AddModelError("", "Username and password are required.");
                return View("Index");
            }

            // Lookup user
            var user = _userRepository.GetUserByUsername(username);
            if (user == null)
            {
                ModelState.AddModelError("", "Invalid username or password.");
                return View("Index");
            }

            // Verify password
            var isValid = PasswordService.VerifyPassword(
                password,
                user.PasswordSalt,
                user.PasswordHash
            );

            if (!isValid)
            {
                ModelState.AddModelError("", "Invalid username or password.");
                return View("Index");
            }

            // Store session data
            HttpContext.Session.SetInt32("UserID", user.UserID);
            HttpContext.Session.SetString("Username", user.Username);
            HttpContext.Session.SetString("Role", user.Role);

            // Redirect to a secure page
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Login");
        }
    }
}

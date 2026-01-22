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
            if (redirect != null)
                return redirect;

            return View("Index");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}

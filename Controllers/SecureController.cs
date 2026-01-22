using Microsoft.AspNetCore.Mvc;

namespace SafeVault.Controllers
{
    public class SecureController : Controller
    {
        protected bool IsLoggedIn => HttpContext.Session.GetInt32("UserID") != null;

        protected string? UserRole => HttpContext.Session.GetString("Role");

        protected IActionResult RequireLogin()
        {
            if (!IsLoggedIn)
            {
                return RedirectToAction("Index", "Login");
            }

            return null!;
        }

        protected IActionResult RequireRole(string role)
        {
            if (!IsLoggedIn)
            {
                return RedirectToAction("Index", "Login");
            }

            if (UserRole != role)
            {
                return RedirectToAction("AccessDenied", "Home");
            }

            return null!;
        }
    }
}

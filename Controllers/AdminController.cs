using Microsoft.AspNetCore.Mvc;

namespace SafeVault.Controllers
{
    public class AdminController : SecureController
    {
        public IActionResult Dashboard()
        {
            var redirect = RequireRole("admin");
            if (redirect != null) return redirect;

            return View();
        }
    }
}

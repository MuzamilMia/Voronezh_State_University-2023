using Microsoft.AspNetCore.Mvc;
using BookStore.BL.Auth;
using BookStore.BL.Auth.Entities;

namespace BookStore.Service.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthProvider _authProvider;

        public AuthController(IAuthProvider authProvider)
        {
            _authProvider = authProvider;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            if (ModelState.IsValid)
            {
                var response = await _authProvider.LoginAsync(request);
                if (response.Success)
                {
                    TempData["Message"] = "Login successful! Token: " + response.Token;
                    return RedirectToAction("Index", "Books");
                }
                ModelState.AddModelError("", response.ErrorMessage ?? "Invalid credentials");
            }
            return View(request);
        }
    }
}

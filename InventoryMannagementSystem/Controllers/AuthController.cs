using InventoryMannagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace InventoryMannagementSystem.Controllers
{
    public class AuthController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult UserRegister()
        {
            return View();
        }

        [HttpPost]
        public IActionResult UserRegister([FromForm] AuthModel user)
        {
            return View(user);
        }

        [HttpGet]
        public IActionResult userLogin()
        {
            return View();
        }

        [HttpPost]
        public IActionResult userLogin([FromForm] AuthModel user)
        {
            if (user != null && ModelState.IsValid)
            {
                HttpContext.Session.SetString("UserName", user.Name);
                HttpContext.Session.SetString("UserEmail", user.Email);
                HttpContext.Session.SetString("UserPassword", user.Password);

                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "Invalid User Details!";
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
            //return RedirectToAction("userLogin");
        }
    }
}

using InventoryMannagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
<<<<<<< HEAD
=======
using InventoryMannagementSystem.Interface;
>>>>>>> acc08df (auth added)

namespace InventoryMannagementSystem.Controllers
{
    public class AuthController : Controller
    {
<<<<<<< HEAD
=======

        private readonly IAuth _register;

        public AuthController(IAuth auth)
        {
            _register = auth;
        }

>>>>>>> acc08df (auth added)
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
<<<<<<< HEAD
        public IActionResult UserRegister([FromForm] AuthModel user)
        {
            return View(user);
=======
        public async Task<IActionResult> UserRegister([FromForm] AuthModel user)
        {
            var res = await _register.InsertRegisterUser(user);    
            return Json(res);
>>>>>>> acc08df (auth added)
        }

        [HttpGet]
        public IActionResult userLogin()
        {
            return View();
        }

        [HttpPost]
<<<<<<< HEAD
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
=======
        public async Task<IActionResult> UserLogin([FromForm] AuthModel user)
        {
            //if (!ModelState.IsValid)
            //{
            //    ViewBag.Error = "Invalid input";
            //    return View();
            //}

            // 1️⃣ Login validate
            var isValidUser = await _register.ValidateUser(user);

            if (!isValidUser.Status)
            {
                ViewBag.Error = "Invalid User Details!";
                return View();
            }

            // 2️⃣ Get user details
            var userDetails = await _register.GetUserDetails(user);

            if (userDetails == null || !userDetails.Any())
            {
                ViewBag.Error = "User details not found!";
                return View();
            }

            var loggedInUser = userDetails.First();

            // 3️⃣ Set session (SAFE data only)
            HttpContext.Session.SetString("UserName", loggedInUser.Name);
            HttpContext.Session.SetString("UserEmail", loggedInUser.Email);

            return RedirectToAction("Index", "Home");
>>>>>>> acc08df (auth added)
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
<<<<<<< HEAD
            //return RedirectToAction("userLogin");
=======
>>>>>>> acc08df (auth added)
        }
    }
}

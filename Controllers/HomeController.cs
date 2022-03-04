using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RateSetter.Models;
using RateSetter.Services.Interfaces;
using RateSetter.ViewModels;

namespace RateSetter.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUserRepository _userRepository;

        public HomeController(ILogger<HomeController> logger, IUserRepository userRepository)
        {
            _logger = logger;
            _userRepository = userRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Registration()
        {
            var model = _userRepository.UserAddressViewModel();
            return View(model);
        }

        [HttpPost]
        public IActionResult Registration(UserAddressViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (_userRepository.CreateUser(model))
                {
                    ViewData["Message"] = "success: Welcome to RateSetter!";
                }
                else
                {
                    ViewData["Message"] = "Error: You must live within 500 meters of existing user or your Referral Code entered by someone!";
                }
            }

            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using Studentenbeheer.Data;
using Studentenbeheer.Models;
using System.Diagnostics;

namespace Studentenbeheer.Controllers
{
    public class HomeController : ApplicationController
    {
        private readonly ILogger<ApplicationController> _logger;

        public HomeController(IdentityContext context, IHttpContextAccessor httpContextAccessor, ILogger<ApplicationController> logger)
            : base(context, httpContextAccessor, logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
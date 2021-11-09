using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using packagesentinel.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;


namespace packagesentinel.Controllers
{

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly APIContext _db;
        private readonly SignInManager<ApplicationUser> _signInMananager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        public HomeController(IUnitOfWork unitOfWork,ILogger<HomeController> logger, APIContext APIContext, SignInManager<ApplicationUser> _signInMananager, UserManager<ApplicationUser> _userManager)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _db = APIContext;
        }

        public IActionResult Index()
        {

            // ensures database is connected 
            _db.Items.Add(new Item() { Name = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") });
            var items = _db.Items.OrderByDescending(p => p.Id).ToList();
            _db.SaveChanges();
            Item item = _db.Items.FirstOrDefault() ;
            if (items.Count >= 2)
                item = items[2];

          
          
            return View(item);
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

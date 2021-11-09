using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using packagesentinel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace packagesentinel.Controllers {
    [Route("api/[controller]")]
    public class NotificationController : BaseController {
        public NotificationController(IUnitOfWork unitOfWork, ILogger<HomeController> logger, APIContext APIContext, SignInManager<ApplicationUser> signInMananager, UserManager<ApplicationUser> userManager) :
               base(unitOfWork, logger, APIContext, signInMananager, userManager) {

        }

        [HttpGet]
        public async Task<IActionResult> GetNotifications() {
            var userName = HttpContext.User.Identity.Name;
            var user = await _userManager.FindByNameAsync(userName);

            var Notification = _unitOfWork.Devices.Map(d => d.Id == user.DeviceId).SelectMany(p => p.Packages).SelectMany(e => e.Notifications).Where(e=> e.createdAt>=DateTime.Now.AddDays(-30) && e.createdAt<= DateTime.Now).ToList();

            return Ok(new { Notifications = Notification });
        }


    }
}

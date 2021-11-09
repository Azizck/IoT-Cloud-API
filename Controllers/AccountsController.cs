using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using packagesentinel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;



namespace packagesentinel.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Accounts : BaseController
    {

        public Accounts(IUnitOfWork unitOfWork, ILogger<HomeController> logger, APIContext APIContext, SignInManager<ApplicationUser> signInMananager, UserManager<ApplicationUser> userManager) :
            base(unitOfWork, logger, APIContext, signInMananager, userManager)

        {

        }

        [Route("setNotificationKey")]
        [HttpPost]
        public async Task<IActionResult> SetNotificationKey(string key)
        {
            if (String.IsNullOrEmpty(key))
                return NotFound("invalid key");
            try
            {
                var userName = HttpContext.User.Identity.Name;
                var user = await _userManager.FindByNameAsync(userName);
                user.NotificationKey = key;
                _unitOfWork.Complete();

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Exception", ex.Message);
                return NotFound(ModelState.Values.Select(e => e.Errors).ToArray());
            }

            return Ok();
        }



        [HttpGet]
        public IEnumerable<Device> Get()
        {
            return _unitOfWork.Devices.GetAll();
        }




    }
}

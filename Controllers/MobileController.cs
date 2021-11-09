using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using packagesentinel.Models;
using packagesentinel.Services;
using packagesentinel.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
namespace packagesentinel.Controllers
{
    /// <summary>
    /// Contains all APIs that are consumed by the Mobile App only 
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public  class MobileController : BaseController
    {
        public MobileController(IUnitOfWork unitOfWork, ILogger<HomeController> logger, APIContext APIContext, SignInManager<ApplicationUser> signInMananager, UserManager<ApplicationUser> userManager) :
            base(unitOfWork, logger, APIContext, signInMananager, userManager)
        {
        }


        [AllowAnonymous]
        [Route("GetVideo")]
        [HttpGet]
        public async Task<IActionResult> GetVideo()
        {

            byte[] result;
            using (FileStream stream = System.IO.File.Open("wwwroot/sa.mp4", FileMode.Open))
            {
                result = new byte[stream.Length];
                await stream.ReadAsync(result, 0, (int)stream.Length);
            }


            return new FileContentResult(result, "video/mp4");
        }

        [Route("GetDeviceInfo")]
        [HttpPost]
        public async Task<IActionResult> GetDeviceInfo()
        {
            var userName = HttpContext.User.Identity.Name;
            var user = await _userManager.FindByNameAsync(userName);
            var device = _unitOfWork.Devices.Map(e => e.Id == user.DeviceId).Include(e => e.Setting).Include(e => e.Packages).Include(e => e.Owners).FirstOrDefault();
            var obj = new { SerialNumber = device.Number, Owners = device.Owners.Select(e => new { UserName = e.UserName, Email = e.Email, FullName = e.FullName }), TotalPackages = device.Packages.Count(), LastSynced = device.SyncedAt.ToString("MM/dd/yyyy HH:mm:ss"), CreatedAt = device.CreatedAt.ToString("MM/dd/yyyy HH:mm:ss"), AlarmStatus = device.Setting.IsAlarmOn ? "On" : "Off" };
            return Ok(obj);
        }
        [Route("ChangePassword")]
        [HttpPost]
        public async Task<IActionResult> ChangePasswordAsync([FromBody] ChangePasswordDTO model)
        {
            try
            {
                var userName = HttpContext.User.Identity.Name;
                var user = await _userManager.FindByNameAsync(userName);
                if (user != null)
                {
                    var checkPassword = await _signInMananager.CheckPasswordSignInAsync(user, model.CurrentPassword, false);
                    if (checkPassword.Succeeded)
                    {
                        user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, model.NewPassword);
                        var updateUser = await _userManager.UpdateAsync(user);
                        if (updateUser.Succeeded)
                        {
                            return Ok();
                        }
                    }
                    else { ModelState.AddModelError("", "Incorrect password"); }
                }
                ModelState.AddModelError("", "User does not exist");
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
            return NotFound(ModelState.Values.Select(e => e.Errors).ToArray());
        }
       
        /// <summary>
        /// Called  by mobile, returns the status of the device
        /// <returns><</cs>DasbhoardDTO</returns>
        [Route("Dashboard")]
        [HttpPost]
        public async Task<IActionResult> Dashboard()
        {
            try
            {
                var model = new DashboardDTO();

                // get the user(device owner)
                var userName = HttpContext.User.Identity.Name;
                var user = await _userManager.FindByNameAsync(userName);


                // get user's device
                var device = _unitOfWork.Devices.Map(e => e.Id == user.DeviceId).Include(e => e.Setting).FirstOrDefault();

                model.IsAlarmOn = device.Setting.IsAlarmOn;

                // DEVICE DISCONNECTED
                if (!device.IsConnected)
                {
                    model.ConnectionStatus = nameof(ConnectionStatus.DISCONNECTED);
                    model.PackageMessage = "Your devices is disconnected";
                    return Ok(model);
                }
                // DEVICE CONNECTED
                else if (device.IsConnected)
                {
                    model.ConnectionStatus = nameof(ConnectionStatus.CONNECTED);
                    // packages that have not been picked up. 
                    //note: a package is not picked up if picked up date is null since datetime is not nullable DateTime.MinValue represents null
                    model.PackagesPlacedCount = _unitOfWork.Devices.Map(e => e.Id == user.DeviceId).SelectMany(e => e.Packages).
                        Where(e => e.PickedUpAt == DateTime.MinValue && !e.PickupAcknowledged).Count();

                    //return number of packages that have been picked up and not acknowledged
                    var pickedUpPacakgesCount = _unitOfWork.Devices.Map(e => e.Id == user.DeviceId).SelectMany(e => e.Packages).
                        Where(e => e.PickedUpAt != DateTime.MinValue && !e.PickupAcknowledged).Count();

                    // PACKAGE CONNECTED, THERE IS A PACKAGE OR MORE 
                    if (model.PackagesPlacedCount > 0)
                    {
                        model.Status = nameof(DeviceStatus.PACKAGE_WAITING);
                        model.PackageMessage = String.Format("You have {0} package waiting for pickup", model.PackagesPlacedCount);
                    }
                    else if (pickedUpPacakgesCount > 0)
                    {
                        model.Status = nameof(DeviceStatus.PACKAGE_PICKED_UP);
                        model.PackageMessage = String.Format("Your pacakge was picked up");
                    }
                    else if (model.PackagesPlacedCount == 0)
                    {
                        // Device CONNECTED, THERE ARE NO PACKAGES      
                        model.Status = nameof(DeviceStatus.NO_PACKAGE);
                        model.PackageMessage = "You have no packages";
                    }
                }
                return Ok(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Exception", ex.Message);
                return NotFound(ModelState.Values.Select(e => e.Errors).ToArray());
            }
        }



        [Route("AcknowledgePackage")]
        [HttpPost]
        public async Task<IActionResult> AcknowledgePackage()
        {
            try
            {
                var userName = HttpContext.User.Identity.Name;

                var user = await _userManager.FindByNameAsync(userName);
                var device = _unitOfWork.Devices.Map(e => e.Id == user.DeviceId).Include(e => e.Setting).FirstOrDefault();
                var packages = _unitOfWork.Devices.Map(e => e.Id == user.DeviceId).SelectMany(e => e.Packages).Where(p => !p.PickupAcknowledged).ToList();
                //acknowledge all packages
                packages.ForEach(p => p.PickupAcknowledged = true);
                _unitOfWork.Complete();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Exception", ex.Message);
                return NotFound(ModelState.Values.Select(e => e.Errors).ToArray());
            }
            return Ok();
        }

        [Route("GetDevice")]
        [HttpPost]
        public async Task<IActionResult> GetDevice()
        {
            var userName = HttpContext.User.Identity.Name;
            var user = await _userManager.FindByNameAsync(userName);
            var device = _unitOfWork.Devices.Map(e => e.Id == user.DeviceId).Include(e => e.Setting).FirstOrDefault();
            var model = new DeviceDTO() { IsAlarmOn = device.Setting.IsAlarmOn, IsSleep = device.Setting.IsSleep };
            return Ok(model);
        }
        [Route("UpdateDevice")]
        [HttpPost]
        public async Task<IActionResult> UpdateDevice(DeviceDTO deviceDto)
        {
            if (!ModelState.IsValid)
                return NotFound(ModelState.Values.Select(e => e.Errors).ToArray());
            var userName = HttpContext.User.Identity.Name;
            var user = await _userManager.FindByNameAsync(userName);
            var device = _unitOfWork.Devices.Map(e => e.Id == user.DeviceId).Include(e => e.Setting).FirstOrDefault();
            device.Setting.IsSleep = deviceDto.IsSleep;
            device.Setting.IsAlarmOn = deviceDto.IsAlarmOn;
            _unitOfWork.Complete();
            return Ok();
        }

        [Route("ToggleAlarm")]
        [HttpPost]
        public async Task<IActionResult> ToggleAlarm(int status = 0)
        {
            var userName = HttpContext.User.Identity.Name;
            var user = await _userManager.FindByNameAsync(userName);
            var device = _unitOfWork.Devices.Map(e => e.Id == user.DeviceId).Include(e => e.Setting).FirstOrDefault();
            device.Setting.IsAlarmOn = status == 0 ? false : true;
            _unitOfWork.Complete();
            return Ok();
        }

        [Route("GetUserInfo")]
        [HttpGet]
        public async Task<IActionResult> GetUserInfo() {
            var userName = HttpContext.User.Identity.Name;
            var user = await _userManager.FindByNameAsync(userName);
            var userDto = new { FullName = user.FullName, Name = user.UserName, Email = user.Email };
            return Ok(userDto);
        }



    }
}

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
    [Route("api/[Controller]")]
    public class DeviceController : BaseController

    {

        public DeviceController(IUnitOfWork unitOfWork, ILogger<HomeController> logger, APIContext APIContext, SignInManager<ApplicationUser> signInMananager, UserManager<ApplicationUser> userManager) :
          base(unitOfWork, logger, APIContext, signInMananager, userManager)
        {
        }

        /// <summary>
        /// Contains all APIs that are consumed by the PI only
        /// </summary>
        [Route("PackagePickedUp")]
        [HttpPost]
        public async Task<IActionResult> PackageRemoved()
        {
            var model = new DashboardDTO();
            try
            {
                var serialNumber = HttpContext.User.Identity.Name;
                var device = _unitOfWork.Devices.Map(e => e.Number == serialNumber).Include(e => e.Packages).Include(e => e.Setting).FirstOrDefault();
                var users = _unitOfWork.Devices.Map(e => e.Number == serialNumber).SelectMany(e => e.Owners).ToList();
                var packages = _unitOfWork.Devices.Map(e => e.Number == serialNumber).SelectMany(e => e.Packages).Where(e => e.PickedUpAt == DateTime.MinValue && !e.PickupAcknowledged).Include(e=>e.Notifications).ToList();
                packages.ForEach(e => e.PickedUpAt = DateTime.Now);

                var notification = new Notification() {Title="Package Picked Up",IsDismissed= false,createdAt=DateTime.Now };

                //packages.ForEach(e => e.PickedUpBy = userName);

                foreach (var user in users)
                {
                    if (!String.IsNullOrEmpty(user.NotificationKey))
                    {
                        String notificationTitle = "";
                        if (device.Setting.IsAlarmOn)
                        {
                            notificationTitle = $"Alert!! {user.UserName} your package has been picked up by an unauthorized person !";
                            notification.Data = notificationTitle;
                            notification.Type = "DANGER";
                        }
                        else
                        {
                            notificationTitle = $"Hey, {user.UserName} your package has been picked up by an authorized person";
                            notification.Data = notificationTitle;
                            notification.Type = "SUCCESS";
                        }
                        await new NotificationServices().NotifyUserAsync(user.NotificationKey, "Package Picked Up", notificationTitle);
                    }
                }

                foreach(var package in packages) {
                    notification.PackageId = package.Id;
                    package.Notifications.Add(notification);
                }
                _unitOfWork.Complete();

                var triggerAlarm = device != null ? device.Setting.IsAlarmOn : false;
                return Ok(new { trriggerAlarm = triggerAlarm });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Exception", ex.Message);
                return NotFound(ModelState.Values.Select(e => e.Errors).ToArray());
            }
            return Ok(false);
        }

        /// <summary>
        ///  Change the status of device to connected
        /// <returns><</cs></returns>
        [Route("Sync")]
        [HttpPost]
        public IActionResult Sync()
        {
            try
            {
                var serialNumber = HttpContext.User.Identity.Name;
                var device = _unitOfWork.Devices.Map(e => e.Number == serialNumber).FirstOrDefault();
                if (device == null)
                    throw new Exception("No Device found with provided Serial Number");
                device.SyncedAt = DateTime.Now;
                _unitOfWork.Complete();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Exception", ex.Message);
                return NotFound(ModelState.Values.Select(e => e.Errors).ToArray());
            }
            return Ok();
        }

        [Route("TurnOffAlarm")]
        [HttpPost]
        public IActionResult TurnOffAlarm()
        {
            var seriaNumber = HttpContext.User.Identity.Name;
            var device = _unitOfWork.Devices.Map(e => e.Number == seriaNumber).Include(e => e.Setting).FirstOrDefault();
            if (device.Setting.IsAlarmOn)
            {
                return Ok(false);
            }

            return Ok(true);
        }

        [Route("PackagePlaced")]
        [HttpPost]
        public async Task<IActionResult> PackagePlaced()
        {
            try
            {
                var serialNumber = HttpContext.User.Identity.Name;
                var device = _unitOfWork.Devices.Map(e => e.Number == serialNumber).Include(e => e.Packages).Include(e => e.Setting).FirstOrDefault();
                var users = _unitOfWork.Devices.Map(e => e.Number == serialNumber).SelectMany(e => e.Owners).ToList();
                var package = new Package() { DeviceId = device.Id, PlacedAt = DateTime.Now, Notifications = new List<Notification>() };
                var notification = new Notification() { Type = "WARNING", Title = "Package Placed", IsDismissed = false, Data = "Alert, a package has been placed and the video recording has been saved!", PackageId = package.Id,createdAt=DateTime.Now};
                package.Notifications.Add(notification);
                device.Packages.Add(package);
                _unitOfWork.Complete();
                foreach (var user in users)
                {

                    if (!String.IsNullOrEmpty(user.NotificationKey))
                        await new NotificationServices().NotifyUserAsync(user.NotificationKey, "Package Placed", $"Alert!! {user.UserName} , a package has been placed!");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Exception", ex.Message);
                return NotFound(ModelState.Values.Select(e => e.Errors).ToArray());
            }
            return Ok();
        }

        [Route("isAllPackagesAcknowledged")]
        [HttpPost]
        public async Task<IActionResult> isAllPackagesAcknowledged()
        {
            bool allAcknowledged = true;
            try
            {
                var serialNumber = HttpContext.User.Identity.Name;
                var device = _unitOfWork.Devices.Map(e => e.Number == serialNumber).Include(e => e.Packages).Include(e => e.Setting).FirstOrDefault();
                var packages = _unitOfWork.Devices.Map(e => e.Number == device.Number).SelectMany(e => e.Packages);
                var unAcknowledgedPackages = packages.Where(p => !p.PickupAcknowledged).Count();
                if (unAcknowledgedPackages > 0)
                {
                    allAcknowledged = false;
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Exception", ex.Message);
                return NotFound(ModelState.Values.Select(e => e.Errors).ToArray());
            }
            return Ok(new { allAcknowledged = allAcknowledged });
        }



        [Route("UploadFile")]
        [HttpPost]
        public async Task<IActionResult> UploadVideo([FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
                return Content("file not selected");

            var path = Path.Combine(
                        Directory.GetCurrentDirectory(), "wwwroot",
                        file.FileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }



            return Ok(file.Name);
        }











    }
}

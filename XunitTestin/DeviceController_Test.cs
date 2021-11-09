using System;
using Xunit;
using packagesentinel;
using packagesentinel.Controllers;
using Moq;
using packagesentinel.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using packagesentinel.ViewModels;

namespace XunitTestin
{
    public class DeviceController_Test
    {

        private APIContext db = null;
        private MobileController mobileController = null;
        private DeviceController deviceController = null;
        //constructor
        public DeviceController_Test()
        {
            // get the fake repository that is populated with dummy data
            db = GetDB();

            // get the device controller 
            controller = GetDevicesController();
        }
        //[the name of the tested method]
        //_[expected input / tested state] _[expected behavior]

        [Fact]
        public void TurnOffAlarm_alarmStateOn_false()
        {
            db.Device.Include(e => e.Setting).FirstOrDefault().Setting.IsAlarmOn = false;
            db.SaveChanges();

            var result = controller.TurnOffAlarm() as OkObjectResult;

            Assert.Equal(true, result.Value);
        }


        [Fact]
        public void TurnOffAlarm_alarmStateOff_true()
        {
            db.Device.Include(e => e.Setting).FirstOrDefault().Setting.IsAlarmOn = true;
            db.SaveChanges();

            var result = controller.TurnOffAlarm() as OkObjectResult;

            Assert.Equal(false, result.Value);
        }





        [Fact]
        public async Task Dashboard_oldLastSyncedAt_disconnected()
        {
            db.Device.FirstOrDefault().SyncedAt = DateTime.Now.AddSeconds(-17);
            db.SaveChanges();
            var result = await controller.Dashboard() as OkObjectResult;
            var model = result.Value as DashboardDTO;

            Assert.Equal(nameof(ConnectionStatus.DISCONNECTED), model.ConnectionStatus);

        }


        [Fact]
        public async Task Dashboard_nowLastSynced_connectedStatus()
        {
            db.Device.FirstOrDefault().SyncedAt = DateTime.Now;
            db.SaveChanges();
            var result = await controller.Dashboard() as OkObjectResult;
            var model = result.Value as DashboardDTO;

            Assert.Equal(nameof(ConnectionStatus.CONNECTED), model.ConnectionStatus);

        }

        [Fact]
        public async Task Dashboard_packagedPlaced_connectedPackageWaiting()
        {
            db.Device.FirstOrDefault().SyncedAt = DateTime.Now;
            db.SaveChanges();

            var result = await controller.Dashboard() as OkObjectResult;
            var model = result.Value as DashboardDTO;

            Assert.True(nameof(ConnectionStatus.CONNECTED) == model.ConnectionStatus && model.Status == nameof(DeviceStatus.PACKAGE_WAITING));

        }


        [Fact]
        public async Task Dashboard_packagePickedUpNotAcknowledged_connectedPackagePickedUp()
        {
            db.Device.FirstOrDefault().SyncedAt = DateTime.Now;
            db.Device.SelectMany(e => e.Packages).ToList().ForEach(e => e.PickedUpAt = DateTime.Now);
            db.SaveChanges();

            var result = await controller.Dashboard() as OkObjectResult;
            var model = result.Value as DashboardDTO;

            Assert.True(nameof(ConnectionStatus.CONNECTED) == model.ConnectionStatus && model.Status == nameof(DeviceStatus.PACKAGE_PICKED_UP));

        }


        [Fact]
        public async Task Dashboard_noPackage_connectedNoPackage()
        {
            db.Device.FirstOrDefault().SyncedAt = DateTime.Now;
            db.Device.SelectMany(e => e.Packages).ToList().ForEach(e => { e.PickedUpAt = DateTime.Now; e.PickupAcknowledged = true; });
            db.SaveChanges();

            var result = await controller.Dashboard() as OkObjectResult;
            var model = result.Value as DashboardDTO;

            Assert.True(nameof(ConnectionStatus.CONNECTED) == model.ConnectionStatus && model.Status == nameof(DeviceStatus.NO_PACKAGE));

        }
        [Fact]
        public void AcknowledgePackage_allTrue_ok()
        {
            db.Device.FirstOrDefault().SyncedAt = DateTime.Now;
            db.Device.SelectMany(e => e.Packages).ToList().ForEach(e => { e.PickedUpAt = DateTime.Now; e.PickupAcknowledged = true; });
            db.SaveChanges();

            var result = controller.AcknowledgePackage().Result;
            Assert.IsType<OkResult>(result);

        }


        [Fact]
        public void Sync_updates_syncedAt()
        {

            // sync api is called by raspberry pi every 15 seconds; this way we know the pi is up and running.
            var result = controller.Sync();

            // get the device
            var device = db.Device.FirstOrDefault();

            // current Date  -  date on which sync was called 
            var timeDifference = (DateTime.Now - device.SyncedAt).TotalSeconds;

            // was it  synced few seconds(3s) ago ? 
            Assert.True(timeDifference < 3);

        }







        [Fact]
        public void PackagePlaced_addNewPacakge_PackgeCountIncreasedByOne()
        {
            //arrange

            //setup
            var packageCountBeforePlacing = db.Device.SelectMany(e => e.Packages).Count();
            var result = controller.PackagePlaced();
            var packageCountAfterPlacing = db.Device.SelectMany(e => e.Packages).Count();

            // act
            Assert.True(packageCountBeforePlacing == packageCountAfterPlacing - 1);

        }

        [Fact]
        public void PackageRemoved_updatePickUpDateOfPackage_PackageDecreaseByOne()
        {


            //setup
            var packageCountBeforePickingup = db.Device.SelectMany(e => e.Packages).Where(e => e.PickedUpAt == DateTime.MinValue).Count();
            var result = controller.PackageRemoved();
            var packageCountAfterPickingup = db.Device.SelectMany(e => e.Packages).Where(e => e.PickedUpAt == DateTime.MinValue).Count();

            // act
            Assert.True(packageCountBeforePickingup == packageCountAfterPickingup + 1);

        }









        private APIContext GetDB()
        {
            var options = new DbContextOptionsBuilder<APIContext>()
           .UseInMemoryDatabase(databaseName: "mydb")
           .Options;
            var context = new APIContext(options);

            var hasher = new PasswordHasher<ApplicationUser>();
            context.ApplicationUser.Add(new ApplicationUser() { Id = new Guid().ToString(), DeviceId = 1, UserName = "admin", Email = "admin", NormalizedEmail = "admin", NormalizedUserName = "admin", SecurityStamp = new Guid().ToString(), PasswordHash = hasher.HashPassword(null, "admin"), });
            context.Device.Add(new Device() { Id = 1, Number = "admin", Note = "created by seeding", Version = "v1.9", CreatedAt = DateTime.Now });
            context.Setting.Add(new Setting() { DeviceId = 1, Id = 1 });
            context.Package.Add(new Package() { Id = 1, DeviceId = 1, PlacedAt = DateTime.Now });

            context.SaveChanges();



            return context;
        }
        private MobileController GetDevicesController()
        {


            // create fake user 
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] {
                                        new Claim(ClaimTypes.Name, "admin"),
                                        new Claim(ClaimTypes.Role, "admin")
                                   }, "TestAuthentication"));


            var userStoreMock = new Mock<IUserRoleStore<ApplicationUser>>();
            var userStore = new Mock<IUserStore<ApplicationUser>>();

            // create user manager
            var us = new Mock<UserManager<ApplicationUser>>(userStoreMock.Object, null, null, null, null, null, null, null, null);

            // to hash the password
            var hasher = new PasswordHasher<ApplicationUser>();
            // set up fake returns 
            us.Setup(x => x.FindByNameAsync(It.IsAny<string>()))
           .Returns(Task.FromResult(new ApplicationUser() { Id = new Guid().ToString(), DeviceId = 1, UserName = "admin", Email = "admin", NormalizedEmail = "admin", NormalizedUserName = "admin", SecurityStamp = new Guid().ToString(), PasswordHash = hasher.HashPassword(null, "admin") }));

            // update Device controller context
            var controller = new MobileController(new UnitOfWork(db), null, null, null, us.Object)
            {
                ControllerContext = new ControllerContext()
            };
            controller.ControllerContext.HttpContext = new DefaultHttpContext { User = user };



            return controller;
        }






    }
}

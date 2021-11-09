
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using packagesentinel.Models;
using packagesentinel.ViewModels;

namespace packagesentinel.Controllers
{
    public class AuthenticateController : BaseController
    {


        private readonly IMapper _mapper;
        #region Property  
        /// <summary>  
        /// Property Declaration  
        /// </summary>  
        /// <param name="data"></param>  
        /// <returns></returns>  
      
        #endregion
        
        public AuthenticateController(IUnitOfWork unitOfWork, ILogger<HomeController> logger, 
            APIContext APIContext, SignInManager<ApplicationUser> signInMananager, 
            UserManager<ApplicationUser> userManager,IMapper mapper, IConfiguration config) :
         base(unitOfWork, logger, APIContext, signInMananager, userManager,config)

        {
            _mapper = mapper;
        }

        #region GenerateJWT  
        /// <summary>  
        /// Generate Json Web Token Method  
        /// </summary>  
        /// <param name="userInfo"></param>  
        /// <returns></returns>  
        private string GenerateJSONWebToken(User userInfo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Issuer"],
              claims:  new List<System.Security.Claims.Claim>
                {
                    new Claim(ClaimTypes.Name,userInfo.UserName),
                    new Claim(ClaimTypes.Role,"ADMIN")
                },
              null,
              expires: DateTime.Now.AddMinutes(120),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        #endregion

        #region AuthenticateUser  
        /// <summary>  
        /// Hardcoded the User authentication  
        /// </summary>  
        /// <param name="login"></param>  
        /// <returns></returns>  
        private async Task<ApplicationUser> AuthenticateUser(LoginModel login)
        {
            var user = await _userManager.FindByEmailAsync(login.UserName);
            if (user != null) {
                var result = await _signInMananager.PasswordSignInAsync(user.UserName, login.Password, true, false);

                if (result.Succeeded) {
                    return user;
                }
            }

            return null;
        }
        #endregion

        #region Login Validation  
        /// <summary>  
        /// Login Authenticaton using JWT Token Authentication  
        /// </summary>  
        /// <param name="data"></param>  
        /// <returns></returns>  
        [AllowAnonymous]
        [HttpPost(nameof(Login))]
        public async Task<IActionResult> Login([FromBody] LoginModel data)
        {
            IActionResult response = Unauthorized();
            var applicationUser = await AuthenticateUser(data);
            var returnUser = _mapper.Map<User>(applicationUser);

            if (returnUser != null)
            {
                var tokenString = GenerateJSONWebToken(returnUser);
                response = Ok(new { Token = tokenString, Message = "Success", User = returnUser });
            }
            return response;
        }


        [AllowAnonymous]
        [HttpPost]
        [Route("AuthenticateDevice")]
        public IActionResult AuthenticateDevice(string serialNumber)
        {
            IActionResult response = Unauthorized();

            var device = _unitOfWork.Devices.Find(e => e.Number == serialNumber).FirstOrDefault();
            if (device != null)
            {
                var tokenString = GenerateJSONWebToken(new User() { UserName = device.Number });
                response = Ok(new { Token = tokenString, Message = "Success" });
            }
            return response;
        }








        #endregion


        [AllowAnonymous]
        [HttpPost(nameof(Register))]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel data)
        {
            ApplicationUser applicationuser = null;
            User returnUser = null;

            if (ModelState.IsValid)
            {

                try {
                    var applicationUser = new ApplicationUser() { Email = data.Email, UserName = data.Email, FullName = data.Name };
                    var registerResult = await _userManager.CreateAsync(applicationUser, data.Password);
                    applicationuser = await _userManager.FindByEmailAsync(applicationUser.Email);
                    returnUser = _mapper.Map<User>(applicationUser);

                    if (registerResult.Succeeded)
                    {


                        var userDevice = _unitOfWork.Devices.Find(e => e.Number == data.SerialNumber).FirstOrDefault();
                        if (userDevice == null)
                        {
                            var newDevice = new Device() { Note = $"Device created at {DateTime.Now}", Number = data.SerialNumber , Setting = new Setting() {IsAlarmOn = true } };
                            newDevice.Owners.Add(applicationUser);
                            _unitOfWork.Devices.Add(newDevice);
                            _unitOfWork.Complete();
                        }
                        else
                        {
                            userDevice.Owners.Add(applicationUser);
                            _unitOfWork.Complete();
                        }


                    }
                    else
                    {
                        return NotFound(registerResult.Errors);
                    }

                }
                catch (Exception ex)
                {
                    return NotFound(ex.Message);

                }


                return Ok(new { User = returnUser });
            }
            else
            {
               return  NotFound(ModelState.Values.Select(e => e.Errors).ToArray());

            }
            return NotFound();
        }








        #region Get  
        /// <summary>  
        /// Authorize the Method  
        /// </summary>  
        /// <returns></returns>  
        [HttpGet(nameof(Get))]
        public async Task<IEnumerable<string>> Get()
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            return new string[] { accessToken };
        }


        #endregion

    }

    #region JsonProperties  
    /// <summary>  
    /// Json Properties  
    /// </summary>  
    public class LoginModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
    #endregion
}
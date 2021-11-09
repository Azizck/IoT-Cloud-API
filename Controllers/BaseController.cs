

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using packagesentinel.Models;
using System.Net;
using System.Threading.Tasks;

namespace packagesentinel.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
    public class BaseController : ControllerBase
    {
        protected ILogger<HomeController> _logger { get; }
        protected  APIContext _db { get; }
        protected  SignInManager<ApplicationUser> _signInMananager { get; }
        protected  UserManager<ApplicationUser> _userManager { get; }
        protected IUnitOfWork _unitOfWork { get; }
        protected IConfiguration _config { get; }
        public BaseController(IUnitOfWork unitOfWork, ILogger<HomeController> logger, APIContext APIContext, SignInManager<ApplicationUser> signInMananager, UserManager<ApplicationUser> userManager, IConfiguration config = null)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _db = APIContext;
            _signInMananager = signInMananager;
            _userManager = userManager;
            _config = config;
        }
       
    }
}


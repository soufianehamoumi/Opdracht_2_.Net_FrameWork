using Microsoft.AspNetCore.Mvc;
using Studentenbeheer.Areas.Identity.Data;
using Studentenbeheer.Data;
using Studentenbeheer.Services;

namespace Studentenbeheer.Controllers
{
    public class ApplicationController : Controller
    {
        protected readonly StudentenbeheerUser _user;
        protected readonly IdentityContext _context;
        protected readonly IHttpContextAccessor _httpContextAccessor;
        protected readonly ILogger<ApplicationController> _logger;

        protected ApplicationController(IdentityContext context,
                                        IHttpContextAccessor httpContextAccessor,
                                        ILogger<ApplicationController> logger)
        {
            _context = context;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            //_user = SessionUser.GetUser(httpContextAccessor.HttpContext.User.Identity.Name);
            //_user = SessionUser.GetUser(httpContextAccessor.HttpContext.User.Identity.Name);
            _user = SessionUser.GetUser(httpContextAccessor.HttpContext);
        }
        
    }
}


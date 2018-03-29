using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AspNetCore.Data;
using AspNetCore.Models.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AspNetCore.Controllers
{
    [Authorize(Policy = "ApiUser")]
    [Produces("application/json")]
    public class TestController : Controller
    {
        private readonly ApplicationDbContext _appDbContext;
        private readonly ClaimsPrincipal _caller;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;

        public TestController(UserManager<AppUser> userManager, IMapper mapper, ApplicationDbContext appDbContext,
            IHttpContextAccessor httpContextAccessor)
        {
            _caller = httpContextAccessor.HttpContext.User;
            _userManager = userManager;
            _mapper = mapper;
            _appDbContext = appDbContext;
        }

        [HttpGet("api/accessToken")]
        public async Task<IActionResult> CheckToken()
        {
            var userId = _caller.Claims.Single(c => c.Type == "id");
            var user = await _appDbContext.Customers.Include(c => c.Identity)
                .SingleAsync(c => c.Identity.Id == userId.Value);
            return Ok($"Token is valid, your Role - {user.Identity.Role}");
        }

        [HttpGet("drop")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> DropBase()
        {
            var userId = _caller.Claims.Single(c => c.Type == "id");
            var admin = await _appDbContext.Customers.Include(c => c.Identity)
                .SingleAsync(c => c.Identity.Id == userId.Value);
            if (admin.Identity.Role != Roles.Admin)
                return Unauthorized();
            var customers = _appDbContext.Customers;
            foreach (var customer in customers)
                _appDbContext.Customers.Remove(customer);
            var users = _appDbContext.Users;
            foreach (var user in users)
                _appDbContext.Users.Remove(user);
            _appDbContext.SaveChanges();
            return Ok("База дропнута");
        }
    }
}
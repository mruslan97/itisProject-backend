using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AspNetCore.Data;
using AspNetCore.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AspNetCore.Controllers
{
    [Authorize(Policy = "ApiUser")]
    public class PrivateController : Controller
    {
        private readonly ApplicationDbContext _appDbContext;
        private readonly ClaimsPrincipal _caller;

        public PrivateController(UserManager<AppUser> userManager, ApplicationDbContext appDbContext,
            IHttpContextAccessor httpContextAccessor)
        {
            _caller = httpContextAccessor.HttpContext.User;
            _appDbContext = appDbContext;
        }

        // GET api/dashboard/home
        [HttpGet("api/auth/currentUser")]
        public async Task<IActionResult> Home()
        {
            // retrieve the user info
            //HttpContext.User
            var userId = _caller.Claims.Single(c => c.Type == "id");

            var customer = await _appDbContext.Customers.Include(c => c.Identity)
                .SingleAsync(c => c.Identity.Id == userId.Value);
            var userRole = Enum.GetName(typeof(Roles), customer.Identity.Role);
            return new OkObjectResult(new
            {
                userRole,
                customer.Identity.FirstName,
                customer.Identity.LastName,
                customer.Balance,
                customer.CurrentTariff,
                customer.PassportSeries
            });
        }

        [HttpGet("api/changeTariff")]
        public async Task<IActionResult> ChangeTariff(string tariff)
        {
            var userId = _caller.Claims.Single(c => c.Type == "id");
            var customer = await _appDbContext.Customers.Include(c => c.Identity)
                .SingleAsync(c => c.Identity.Id == userId.Value);
            customer.CurrentTariff = tariff;
            _appDbContext.SaveChanges();
            return Ok();
        }

        [HttpGet("api/upBalance")]
        public async Task<IActionResult> UpBalance(int sum)
        {
            var userId = _caller.Claims.Single(c => c.Type == "id");
            var customer = await _appDbContext.Customers.Include(c => c.Identity)
                .SingleAsync(c => c.Identity.Id == userId.Value);
            customer.Balance += sum;
            _appDbContext.SaveChanges();
            return Ok();
        }

        [HttpGet("api/admin")]
        public async Task<IActionResult> Admin()
        {
            // retrieve the user info
            //HttpContext.User
            var userId = _caller.Claims.Single(c => c.Type == "id");
            var customer = await _appDbContext.Customers.Include(c => c.Identity)
                .SingleAsync(c => c.Identity.Id == userId.Value);
            if (customer.Identity.Role != Roles.Admin)
                return Unauthorized();
            var customers = _appDbContext.Customers;
            return new OkObjectResult(customers);
        }
    }
}
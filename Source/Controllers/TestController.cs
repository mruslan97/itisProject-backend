using System.Threading.Tasks;
using AspNetCore.Data;
using AspNetCore.Models.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCore.Controllers
{
    [Produces("application/json")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class TestController : Controller
    {
        private readonly ApplicationDbContext _appDbContext;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;

        public TestController(UserManager<AppUser> userManager, IMapper mapper, ApplicationDbContext appDbContext)
        {
            _userManager = userManager;
            _mapper = mapper;
            _appDbContext = appDbContext;
        }

        [HttpGet("drop")]
        public async Task<IActionResult> DropBase()
        {
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
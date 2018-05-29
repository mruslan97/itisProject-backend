using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCore.Auth;
using AspNetCore.Data;
using AspNetCore.Models;
using AspNetCore.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace AspNetCore.Controllers
{
    public class InfoController : Controller
    {
        private readonly ApplicationDbContext _appDbContext;

        public InfoController(ApplicationDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        [HttpGet("api/supports")]
        public async Task<IActionResult> GetSupports()
        {
            return new OkObjectResult(new
            {
                supportUsers = _appDbContext.Customers.Where(c => c.Identity.Role == Roles.Support).Select(e => e.Identity.Email).ToList()
            });
        }
        [HttpGet("api/tariffs")]
        public async Task<IActionResult> GetTariffs()
        {
            
            return new OkObjectResult(new
            {
                _appDbContext.Tariffs
            });
        }

        [HttpGet("api/republics")]
        public async Task<IActionResult> GetRepublics()
        {
            return new OkObjectResult(_appDbContext.Republics);
        }

        [HttpGet("api/republicStats")]
        public async Task<IActionResult> GetRepublicStats()
        {
           return new OkObjectResult( _appDbContext.Republics.Include(r => r.Customers).Select(r => new { id = r.Id, republicName = r.Name, count = r.Customers.Count}).ToList());
        }
    }
}

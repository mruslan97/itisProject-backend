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
        [HttpGet("api/tariffs")]
        public async Task<IActionResult> GetTariffs()
        {
            // retrieve the user info
            //HttpContext.User

            return new OkObjectResult(new
            {
                _appDbContext.Tariffs
            });
        }

        [HttpGet("api/republics")]
        public async Task<IActionResult> GetRepublics()
        {
            // retrieve the user info
            //HttpContext.User
            
            return new OkObjectResult(_appDbContext.Republics);
        }

        [HttpGet("api/republicStats")]
        public async Task<IActionResult> GetRepublicStats()
        {
            // retrieve the user info
            //HttpContext.User

            return new OkObjectResult( _appDbContext.Republics.Include(r => r.Customers).Select(r => new { id = r.Id, republicName = r.Name, count = r.Customers.Count}).ToList());
        }
    }
}

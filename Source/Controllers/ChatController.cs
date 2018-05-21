using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AspNetCore.Data;
using AspNetCore.Models.Entities;
using AspNetCore.ViewModels;
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
    public class ChatController : Controller
    {
        private readonly ApplicationDbContext _appDbContext;
        private readonly ClaimsPrincipal _caller;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;

        public ChatController(UserManager<AppUser> userManager, IMapper mapper, ApplicationDbContext appDbContext,
            IHttpContextAccessor httpContextAccessor)
        {
            _caller = httpContextAccessor.HttpContext.User;
            _userManager = userManager;
            _mapper = mapper;
            _appDbContext = appDbContext;
        }

        [HttpGet("api/getMessages")]
        public async Task<IActionResult> RecieveMessages()
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var userId = _caller.Claims.Single(c => c.Type == "id");
            var user = await _appDbContext.Customers.Include(c => c.Identity)
                .SingleAsync(c => c.Identity.Id == userId.Value);
            var messages = _appDbContext.Messages.Where(m => m.IsReaded == false && m.To.Email == user.Identity.Email).Include(m => m.From).ToList();
            _appDbContext.Messages.Where(m => m.IsReaded == false && m.To.Email == user.Identity.Email).ToList()
                .ForEach(m => m.IsReaded = true);
            _appDbContext.SaveChanges();
            return new OkObjectResult(messages.Select(m => new { From = m.From.Email, m.Text }));
        }

        [HttpPost("api/sendMessage")]
        public async Task<IActionResult> SendMessage([FromBody] MessageViewModel inputMessage)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var userId = _caller.Claims.Single(c => c.Type == "id");
            var fromUser = await _appDbContext.Customers.Include(c => c.Identity)
                .SingleAsync(c => c.Identity.Id == userId.Value);
            var toUser = _appDbContext.Customers.Include(c => c.Identity)
                .SingleOrDefault(u => u.Identity.Email.ToLower() == inputMessage.ToUserName)
                ?.Identity;
            var message = new Message
            {
                From = fromUser.Identity,
                IsReaded = false,
                Text = inputMessage.Text,
                To = toUser
            };
            _appDbContext.Add(message);
            _appDbContext.SaveChanges();
            return Ok("Message sended");
        }

    }
}
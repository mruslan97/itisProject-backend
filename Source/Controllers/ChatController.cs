using System;
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

        [HttpGet("api/getChatHistory")]
        public async Task<IActionResult> GetChatHistory(int id)
        {
            //var dialogues = _appDbContext.Dialogues.Where(d => d.Id == id);
            var messages = _appDbContext.Messages
                .Where(m => m.Dialogue.Id == id);
            var result = messages.Select(m => new
            {
                m.Text,
                Time = m.DateTime,
                Role = Enum.GetName(typeof(Roles), m.FromUser.Role)
            }).AsNoTracking().ToList();

            return new OkObjectResult(result);
        }

        [HttpGet("api/getMessages")]
        public async Task<IActionResult> RecieveMessages()
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var userId = _caller.Claims.Single(c => c.Type == "id");
            var user = await _appDbContext.Customers.Include(c => c.Identity)
                .SingleAsync(c => c.Identity.Id == userId.Value);
            var dialogues = _appDbContext.Dialogues.Where(d => d.To.Email == user.Identity.Email);
            var messages = _appDbContext.Messages
                .Where(m => m.IsReaded == false && dialogues.Contains(m.Dialogue));
            var result = messages.Select(m => new {DialogueId = m.Dialogue.Id, m.Text, Time = m.DateTime})
                .AsNoTracking().ToList();
            _appDbContext.Messages.Where(m => dialogues.Contains(m.Dialogue)).ToList()
                .ForEach(m => m.IsReaded = true);
            _appDbContext.SaveChanges();
            return new OkObjectResult(result);
        }

        [HttpPost("api/sendMessage")]
        public async Task<IActionResult> SendMessage([FromBody] MessageViewModel inputMessage)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var userId = _caller.Claims.Single(c => c.Type == "id");
            var fromUser = await _appDbContext.Customers.Include(c => c.Identity)
                .SingleAsync(c => c.Identity.Id == userId.Value);
            var dialogue = _appDbContext.Dialogues?.SingleOrDefault(d => d.Id == inputMessage.DialogueId);
            var message = new Message
            {
                FromUser = fromUser.Identity,
                IsReaded = false,
                Text = inputMessage.Text,
                Dialogue = dialogue,
                DateTime = DateTime.Now
            };
            _appDbContext.Add(message);
            _appDbContext.SaveChanges();
            return Ok("Message sended");
        }

        [HttpPost("api/createDialog")]
        public async Task<IActionResult> CreateDialogue([FromBody] DialogueViewModel inputDialog)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var userId = _caller.Claims.Single(c => c.Type == "id");
            var fromUser = await _appDbContext.Customers.Include(c => c.Identity)
                .SingleAsync(c => c.Identity.Id == userId.Value);
            var toUser = _appDbContext.Customers.Include(c => c.Identity)
                .SingleOrDefault(c => c.Identity.Email == inputDialog.ToUserEmail);
            var dialogue = new Dialogue
            {
                From = fromUser.Identity,
                To = toUser.Identity,
                Subject = inputDialog.Subject
            };
            _appDbContext.Add(dialogue);
            _appDbContext.SaveChanges();
            return Ok(_appDbContext.Dialogues.Last().Id);
        }

        [HttpGet("api/getDialogs")]
        public async Task<IActionResult> GetDialogues()
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var userId = _caller.Claims.Single(c => c.Type == "id");
            var user = await _appDbContext.Customers.Include(c => c.Identity)
                .SingleAsync(c => c.Identity.Id == userId.Value);
            var dialogues = _appDbContext.Dialogues.Where(d => d.To.Email == user.Identity.Email
                                                               || d.From.Email == user.Identity.Email);
            return new OkObjectResult(dialogues.Select(d => new {DialogueId = d.Id, d.Subject}));
        }
    }
}
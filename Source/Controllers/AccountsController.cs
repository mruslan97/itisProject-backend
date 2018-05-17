using System;
using System.Linq;
using System.Threading.Tasks;
using AspNetCore.Data;
using AspNetCore.Helpers;
using AspNetCore.Models.Entities;
using AspNetCore.Services;
using AspNetCore.ViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCore.Controllers
{
    [Route("api/registration")]
    public class AccountsController : Controller
    {
        private readonly ApplicationDbContext _appDbContext;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;

        public AccountsController(UserManager<AppUser> userManager, IMapper mapper, ApplicationDbContext appDbContext)
        {
            _userManager = userManager;
            _mapper = mapper;
            _appDbContext = appDbContext;
        }

        // registration
        // POST api/accounts
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] RegistrationViewModel model)
        {
            var emailService = new EmailService();
            var random = new Random();
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var userIdentity = _mapper.Map<AppUser>(model);

            var result = await _userManager.CreateAsync(userIdentity, model.Password);

            if (!result.Succeeded) return new BadRequestObjectResult(Errors.AddErrorsToModelState(result, ModelState));
            var balance = random.Next(0, 1000);
            await _appDbContext.Customers.AddAsync(new Customer
            {
                IdentityId = userIdentity.Id,
                Balance = balance,
                Tariff = _appDbContext.Tariffs.SingleOrDefault(t => t.Id == 1),
                Republic = _appDbContext.Republics.SingleOrDefault(r => r.Id == model.RepublicId),
                PassportSeries = model.PassportSeries
                //IsAdmin = false
            });
            await _appDbContext.SaveChangesAsync();

            await emailService.SendEmailAsync(model.Email, "Регистрация завершена",
                $"{model.FirstName}, благодарим Вас за регистрацию на нашем портале! Ваш баланс: {balance}, тариф Smart");
            return new OkObjectResult("Account created");
        }
    }
}
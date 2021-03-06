﻿using System.Security.Claims;
using System.Threading.Tasks;
using AspNetCore.Auth;
using AspNetCore.Data;
using AspNetCore.Helpers;
using AspNetCore.Models;
using AspNetCore.Models.Entities;
using AspNetCore.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace AspNetCore.Controllers
{
    [Route("api")]
    public class AuthController : Controller
    {
        private readonly IJwtFactory _jwtFactory;
        private readonly JwtIssuerOptions _jwtOptions;
        private readonly UserManager<AppUser> _userManager;
        private readonly ApplicationDbContext _appDbContext;

        public AuthController(UserManager<AppUser> userManager, IJwtFactory jwtFactory,
            IOptions<JwtIssuerOptions> jwtOptions, ApplicationDbContext appDbContext)
        {
            _userManager = userManager;
            _jwtFactory = jwtFactory;
            _jwtOptions = jwtOptions.Value;
            _appDbContext = appDbContext;
        }

        // POST api/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Post([FromBody] CredentialsViewModel credentials)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var identity = await GetClaimsIdentity(credentials.UserName, credentials.Password);
            if (identity == null)
                return BadRequest(Errors.AddErrorToModelState("login_failure", "Invalid username or password.",
                    ModelState));

            var jwt = await Tokens.GenerateJwt(identity, _jwtFactory, credentials.UserName, _jwtOptions,
                new JsonSerializerSettings {Formatting = Formatting.Indented});
            return new OkObjectResult(jwt);
        }

        private async Task<ClaimsIdentity> GetClaimsIdentity(string userName, string password)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
                return await Task.FromResult<ClaimsIdentity>(null);

            // get the user to verifty
            var userToVerify = await _userManager.FindByNameAsync(userName);

            if (userToVerify == null) return await Task.FromResult<ClaimsIdentity>(null);

            // check the credentials
            if (await _userManager.CheckPasswordAsync(userToVerify, password))
                return await Task.FromResult(_jwtFactory.GenerateClaimsIdentity(userName, userToVerify.Id));

            // Credentials are invalid, or account doesn't exist
            return await Task.FromResult<ClaimsIdentity>(null);
        }
    }
}
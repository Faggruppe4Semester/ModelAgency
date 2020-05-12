using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ModelsApi.Data;
using ModelsApi.Models.DTOs;
using ModelsApi.Models.Entities;
using ModelsApi.Utilities;
using System;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ModelsApi.Services;
using static BCrypt.Net.BCrypt;

namespace ModelsApi.Controllers
{
    /// <summary>
    /// Use this API to login and change password.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AccountController : ControllerBase
    {
        private readonly AccountService _accountService;

        public AccountController(ApplicationDbContext context,
            IOptions<AppSettings> appSettings)
        {
            _accountService = new AccountService(context, appSettings.Value);
        }

        /// <summary>
        /// You must login before you can use any other api call.
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        [HttpPost("login"), AllowAnonymous]
        public async Task<ActionResult<Token>> Login([FromBody]Login login)
        {
            try
            {
                return await _accountService.Login(login).ConfigureAwait(false);
            }
            catch (ArgumentNullException e)
            {
                return BadRequest(e);
            }
            catch (ArgumentException e)
            {
                return BadRequest(e);
            }
        }

        /// <summary>
        /// Use to change the password.
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        [HttpPut("Password")]
        public async Task<ActionResult<Token>> ChangePassword([FromBody]Login login)
        {
            try
            {
                return await _accountService.ChangePassword(login).ConfigureAwait(false);
            }
            catch (ArgumentNullException e)
            {
                return BadRequest(e);
            }
            catch (ArgumentException e)
            {
                return BadRequest(e);
            }
        }
    }
}
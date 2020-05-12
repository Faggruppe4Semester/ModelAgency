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
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ModelsApi.Services.Interfaces;
using static BCrypt.Net.BCrypt;

namespace ModelsApi.Services
{
    public class AccountService : IAccountService
    {
        private readonly ApplicationDbContext _context;
        private readonly AppSettings _appSettings;

        public AccountService(ApplicationDbContext context, AppSettings appSettings)
        {
            _context = context;
            _appSettings = appSettings;
        }

        public async Task<Token> Login(Login login)
        {
            if (login == null) throw new ArgumentNullException(nameof(login));

            login.Email = login.Email.ToLowerInvariant();
            var account = await _context.Accounts.Where(u => u.Email == login.Email)
                .FirstOrDefaultAsync().ConfigureAwait(false);
            if (account == null) throw new ArgumentException($"Account was not found using email: {login.Email}");

            var validPwd = Verify(login.Password, account.PwHash);
            if (!validPwd) throw new ArgumentException($"{login.Password} is not a valid password");

            long modelId = -1;
            if (!account.IsManager)
            {
                var model = await _context.Models.Where(m => m.EfAccountId == account.EfAccountId)
                    .FirstOrDefaultAsync().ConfigureAwait(false);
                if (model != null)
                    modelId = model.EfModelId;
            }

            var jwt = GenerateToken(account.Email, modelId, account.IsManager);
            var token = new Token() { JWT = jwt };
            return token;
        }

        public async Task<Token> ChangePassword(Login login)
        {
            if (login == null) throw new ArgumentNullException(nameof(login));

            login.Email = login.Email.ToLowerInvariant();

            var account = await _context.Accounts.Where(u => u.Email == login.Email)
                .FirstOrDefaultAsync().ConfigureAwait(false);
            if (account == null) throw new ArgumentException($"Account with email: {account.Email} wasn't found.");

            var validPwd = Verify(login.OldPassword, account.PwHash);
            if (!validPwd) throw new ArgumentException($"Old password: {login.OldPassword} wasn't valid.");
            account.PwHash = HashPassword(login.Password, _appSettings.BcryptWorkfactor);
            await _context.SaveChangesAsync().ConfigureAwait(false);

            return new Token{JWT = GenerateToken(account.Email, account.EfAccountId, account.IsManager)};
        }

        public string GenerateToken(string email, long modelId, bool isManager)
        {
            Claim roleClaim;
            roleClaim = isManager ? new Claim(ClaimTypes.Role, "Manager") : new Claim(ClaimTypes.Role, "Model");

            var claims = new Claim[]
            {
                new Claim(ClaimTypes.Email, email),
                roleClaim,
                new Claim("ModelId", modelId.ToString()),
                new Claim(JwtRegisteredClaimNames.Nbf, new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds().ToString()),
                new Claim(JwtRegisteredClaimNames.Exp, new DateTimeOffset(DateTime.Now.AddDays(1)).ToUnixTimeSeconds().ToString()),
            };

            var key = Encoding.ASCII.GetBytes(_appSettings.SecretKey);
            var token = new JwtSecurityToken(
                new JwtHeader(new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)),
                new JwtPayload(claims));

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
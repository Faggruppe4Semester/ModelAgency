using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ModelsApi.Models.DTOs;

namespace ModelsApi.Services.Interfaces
{
    public interface IAccountService
    {
        public Task<Token> Login(Login login);
        public Task<Token> ChangePassword(Login login);
        public string GenerateToken(string email, long modelId, bool isManager);
    }
}
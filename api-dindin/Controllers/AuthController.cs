using api_dindin.Context;
using api_dindin.Helper;
using api_dindin.Models;
using api_dindin.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api_dindin.Controllers
{
    [ApiController]
    [Route("api/login")]
    public class AuthController : Controller
    {
        private readonly DbConnectionContext _dbConnectionContext;

        public AuthController(DbConnectionContext context)
        {
            _dbConnectionContext = context;
        }

        [HttpPost]
        public async Task<IActionResult> Auth(LoginUser user)
        {
            var searchUser = await _dbConnectionContext.Users.FirstOrDefaultAsync(u => u.email == user.email);
            if (user == null)
            {
                return BadRequest("Usuário inválido.");
            }

            if (user.email.ToLower() == searchUser.email && user.password.GenerateHash() == searchUser.password)
            {
                var token = TokenService.GenerateToken(searchUser);
                return Ok(new
                {
                    token,
                    searchUser.id,
                    searchUser.name,
                    searchUser.email
                });
            }

            return BadRequest("Usuário e/ou senha inválido(s).");
        }
    }

    public class LoginUser
    {
        public string email { get; set; }
        public string password { get; set; }
    }
}

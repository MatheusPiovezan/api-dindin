using api_dindin.Context;
using api_dindin.Models;
using api_dindin.Services;
using Microsoft.AspNetCore.Mvc;

namespace api_dindin.Controllers
{
    [ApiController]
    [Route("api/login")]
    public class AuthController : Controller
    {
        private readonly DbConnectionContext _context;

        public AuthController(DbConnectionContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult Auth(LoginUser user)
        {
            var searchUser = _context.Users.FirstOrDefault(u => u.email == user.email);
            if (user == null)
            {
                return BadRequest("Usuário inválido.");
            }

            if (user.email == searchUser.email && user.password == searchUser.password)
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

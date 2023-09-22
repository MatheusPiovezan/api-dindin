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
        public IActionResult Auth(string email, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.email == email);
            if (user == null)
            {
                return BadRequest("Usuário inválido.");
            }

            if (email == user.email && password == user.password)
            {
                var token = TokenService.GenerateToken(user);
                return Ok(token);
            }

            return BadRequest("Usuário e/ou senha inválido(s).");
        }
    }
}

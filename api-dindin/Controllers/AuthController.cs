using api_dindin.Models;
using api_dindin.Services;
using Microsoft.AspNetCore.Mvc;

namespace api_dindin.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : Controller
    {
        [HttpPost]
        public IActionResult Auth(string email, string password)
        {
            if (email == "matheus@test.com" && password == "123")
            {
                var token = TokenService.GenerateToken(new User("Matheus", "matheus@test.com", "123"));
                return Ok(token);
            }

            return BadRequest("Email or Password invalid.");
        }
    }
}

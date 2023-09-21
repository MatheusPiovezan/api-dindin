using api_dindin.Context;
using api_dindin.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api_dindin.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        private readonly DbConnectionContext _context;

        public UserController(DbConnectionContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult Post(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();

            return Ok();
        }

        [Authorize]
        [HttpGet]
        public IActionResult Get()
        {
            var users = _context.Users.ToList();
            if (users is null)
            {
                return NotFound("Não a usuários.");
            }

            return Ok(users);
        }

        [Authorize]
        [HttpGet("{id:int}")]
        public IActionResult Get(int id)
        {
            var user = _context.Users.FirstOrDefault(u => u.id == id);
            if (user == null)
            {
                return NotFound("Usuário não encontrado.");
            }

            return Ok(user);
        }

        [HttpPut("{id:int}")]
        public IActionResult Put(int id, User user)
        {
            if (id != user.id)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;
            _context.SaveChanges();

            return Ok(user);
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            var user = _context.Users.FirstOrDefault(u => u.id == id);
            //var user = _context.Users.Find(id);
            if (user == null)
            {
                return NotFound("Não encontrado.");
            }

            _context.Users.Remove(user);
            _context.SaveChanges();

            return Ok(user);
        }
    }
}

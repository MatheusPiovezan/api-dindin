using api_dindin.Context;
using api_dindin.Models;
using api_dindin.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace api_dindin.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        private readonly DbConnectionContext _dbConnectionContext;
        private readonly CurrentUser _currentUser;

        public UserController(DbConnectionContext dbConnectionContext, CurrentUser currentUser)
        {
            _dbConnectionContext = dbConnectionContext;
            _currentUser = currentUser;
        }

        [HttpPost]
        public IActionResult Post(User user)
        {
            var validateEmail = _dbConnectionContext.Users.FirstOrDefault(e => e.email == user.email);
            if (validateEmail != null)
            {
                return BadRequest("Já existe usuário cadastrado com o e-mail informado.");
            }

            var expReg = @"^[^@\s]+@[^@\s]+\.(com|net|org|gov)$";
            if (!Regex.IsMatch(user.email, expReg, RegexOptions.IgnoreCase))
            {
                return BadRequest("Email inválido");
            }

            user.SetPasswordHash();

            _dbConnectionContext.Users.Add(user);
            _dbConnectionContext.SaveChanges();


            return Ok(new
            {
                user.id,
                user.name,
                user.email,
            });
        }

        [Authorize]
        [HttpGet]
        public IActionResult Get()
        {
            var id = _currentUser.Id;
            var user = _dbConnectionContext.Users.FirstOrDefault(u => u.id == id);
            if (user == null)
            {
                return NotFound("Usuário não encontrado.");
            }

            return Ok(new
            {
                user.id,
                user.name,
                user.email,
            });
        }

        [Authorize]
        [HttpPut]
        public IActionResult Put(User user)
        {
            var userId = _currentUser.Id;

            var currentUser = _dbConnectionContext.Users.Any(u => u.id == userId);
            if (!currentUser)
            {
                return NotFound("Usuário não encontrado.");
            }

            var validateEmail = _dbConnectionContext.Users.Any(e => e.email == user.email);
            if (validateEmail && _currentUser.Email != user.email)
            {
                return BadRequest("Já existe usuário cadastrado com o e-mail informado.");
            }

            var expReg = @"^[^@\s]+@[^@\s]+\.(com|net|org|gov)$";
            if (!Regex.IsMatch(user.email, expReg, RegexOptions.IgnoreCase))
            {
                return BadRequest("Email inválido");
            }

            user.SetPasswordHash();

            user.id = userId;

            _dbConnectionContext.Entry(user).State = EntityState.Modified;
            _dbConnectionContext.SaveChanges();

            return Ok();
        }
    }
}

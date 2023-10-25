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
        public async Task<IActionResult> Post(User user)
        {
            try
            {
                var validateEmail = await _dbConnectionContext.Users.FirstOrDefaultAsync(e => e.email == user.email);
                if (validateEmail != null)
                {
                    return BadRequest("Já existe usuário cadastrado com o e-mail informado.");
                }

                var expReg = @"^[^@\s]+@[^@\s]+\.(com|net|org|gov)$";
                if (!Regex.IsMatch(user.email, expReg, RegexOptions.IgnoreCase))
                {
                    return BadRequest("Email inválido.");
                }

                user.SetPasswordHash();

                _dbConnectionContext.Users.Add(user);
                await _dbConnectionContext.SaveChangesAsync();


                return Ok(new
                {
                    user.id,
                    user.name,
                    user.email,
                });
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var id = _currentUser.Id;
                var user = await _dbConnectionContext.Users.FirstOrDefaultAsync(u => u.id == id);
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
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> Put(User user)
        {
            try
            {
                var userId = _currentUser.Id;

                var currentUser = await _dbConnectionContext.Users.AnyAsync(u => u.id == userId);
                if (!currentUser)
                {
                    return NotFound("Usuário não encontrado.");
                }

                var validateEmail = await _dbConnectionContext.Users.AnyAsync(e => e.email == user.email);
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
                await _dbConnectionContext.SaveChangesAsync();

                return Ok();
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
        }
    }
}

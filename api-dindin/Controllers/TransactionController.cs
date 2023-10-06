using api_dindin.Context;
using api_dindin.Models;
using api_dindin.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api_dindin.Controllers
{
    [Route("api/transaction")]
    [ApiController]
    public class TransactionController : Controller
    {
        private readonly DbConnectionContext _dbConnectionContext;
        private readonly CurrentUser _currentUser;

        public TransactionController(DbConnectionContext dbConnectionContext, CurrentUser currentUser)
        {
            _dbConnectionContext = dbConnectionContext;
            _currentUser = currentUser;
        }

        [Authorize]
        [HttpGet]
        public IActionResult Get()
        {
            var userId = _currentUser.Id;
            var transactions = _dbConnectionContext.Transactions.Join(_dbConnectionContext.Categories, t => t.category_id, c => c.id, (t, c) => new
            {
                id = t.id,
                type = t.type,
                description = t.description,
                value = t.value,
                date = t.date,
                user_id = t.user_id,
                category_id = t.category_id,
                category_name = c.description,
            }).Where(t => t.user_id == userId);

            return Ok(transactions);
        }

        [Authorize]
        [HttpGet("{id:int}")]
        public IActionResult Get(int id)
        {
            var userId = _currentUser.Id;
            var transaction = _dbConnectionContext.Transactions.Join(_dbConnectionContext.Categories, t => t.category_id, c => c.id, (t, c) => new
            {
                id = t.id,
                type = t.type,
                description = t.description,
                value = t.value,
                date = t.date,
                user_id = t.user_id,
                category_id = t.category_id,
                category_name = c.description,
            }).Where(t => t.id == id && t.user_id == userId);
            if (!transaction.Any())
            {
                return BadRequest("Transação não encontrada.");
            }

            return Ok(transaction);
        }

        [Authorize]
        [HttpPost]
        public IActionResult Post(Transaction transaction)
        {
            if (transaction.id != 0 || transaction.user_id != 0)
            {
                return BadRequest();
            }
            if (transaction.category_id < 1 || transaction.category_id > 17)
            {
                return BadRequest("Categoria não encontrada!");
            }
            if (transaction.type != "entry" && transaction.type != "exit")
            {
                return BadRequest("Tipo de transação incorreta!");
            }

            transaction.user_id = _currentUser.Id;
            _dbConnectionContext.Transactions.Add(transaction);
            _dbConnectionContext.SaveChanges();

            var searchTransaction = _dbConnectionContext.Transactions.Join(_dbConnectionContext.Categories, t => t.category_id, c => c.id, (t, c) => new
            {
                id = t.id,
                type = t.type,
                description = t.description,
                value = t.value,
                date = t.date,
                user_id = t.user_id,
                category_id = t.category_id,
                category_name = c.description,
            }).FirstOrDefault(t => t.id == transaction.id);

            return Ok(searchTransaction);
        }

        [Authorize]
        [HttpPut("{id:int}")]
        public IActionResult Put(int id, Transaction transaction)
        {
            var userId = _currentUser.Id;

            if (transaction.id != 0 || transaction.user_id != 0)
            {
                return BadRequest();
            }
            if (transaction.category_id < 1 || transaction.category_id > 17)
            {
                return BadRequest("Categoria não encontrada!");
            }
            if (transaction.type != "entry" && transaction.type != "exit")
            {
                return BadRequest("Tipo de transação incorreta!");
            }

            var searchTransaction = _dbConnectionContext.Transactions.Any(t => t.id == id && t.user_id == userId);
            if (!searchTransaction)
            {
                return BadRequest("O usuário autenticado não possui acesso a essa transação.");
            }

            transaction.user_id = userId;
            transaction.id = id;
            _dbConnectionContext.Entry(transaction).State = EntityState.Modified;
            _dbConnectionContext.SaveChanges();

            return Ok();
        }

        [Authorize]
        [HttpDelete("{id:int}")]

        public IActionResult Delete(int id)
        {
            var userId = _currentUser.Id;

            var searchTransaction = _dbConnectionContext.Transactions.FirstOrDefault(t => t.id == id && t.user_id == userId);

            if (searchTransaction is null)
            {
                return NotFound("Transação não encontrada.");
            }

            _dbConnectionContext.Remove(searchTransaction);
            _dbConnectionContext.SaveChanges();

            return Ok();
        }
    }
}

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
        public async Task<IActionResult> Get([FromQuery] string[]? filter)
        {
            try
            {
                var userId = _currentUser.Id;
                var transactions = await _dbConnectionContext.Transactions.Join(_dbConnectionContext.Categories, t => t.category_id, c => c.id, (t, c) => new
                {
                    id = t.id,
                    type = t.type,
                    description = t.description,
                    value = t.value,
                    date = t.date,
                    user_id = t.user_id,
                    category_id = t.category_id,
                    category_name = c.description,
                }).Where(t => t.user_id == userId).ToListAsync();

                if (filter.Any())
                {
                    List<Object> filteredTransactions = new List<Object>();

                    foreach (var f in filter)
                    {
                        foreach (var transaction in transactions)
                        {
                            if (transaction.category_name.ToLower() == f.ToLower())
                            {
                                filteredTransactions.Add(transaction);
                            }
                        }
                    }

                    return Ok(filteredTransactions);
                }
                else
                {
                    return Ok(transactions);
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var userId = _currentUser.Id;
                var transaction = await _dbConnectionContext.Transactions.Join(_dbConnectionContext.Categories, t => t.category_id, c => c.id, (t, c) => new
                {
                    id = t.id,
                    type = t.type,
                    description = t.description,
                    value = t.value,
                    date = t.date,
                    user_id = t.user_id,
                    category_id = t.category_id,
                    category_name = c.description,
                }).Where(t => t.id == id && t.user_id == userId).ToListAsync();

                if (!transaction.Any())
                {
                    return BadRequest("Transação não encontrada.");
                }

                return Ok(transaction);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Post(Transaction transaction)
        {
            try
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
                await _dbConnectionContext.SaveChangesAsync();

                var searchTransaction = await _dbConnectionContext.Transactions.Join(_dbConnectionContext.Categories, t => t.category_id, c => c.id, (t, c) => new
                {
                    id = t.id,
                    type = t.type,
                    description = t.description,
                    value = t.value,
                    date = t.date,
                    user_id = t.user_id,
                    category_id = t.category_id,
                    category_name = c.description,
                }).FirstOrDefaultAsync(t => t.id == transaction.id);

                return Ok(searchTransaction);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, Transaction transaction)
        {
            try
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

                var searchTransaction = await _dbConnectionContext.Transactions.AnyAsync(t => t.id == id && t.user_id == userId);
                if (!searchTransaction)
                {
                    return BadRequest("O usuário autenticado não possui acesso a essa transação.");
                }

                transaction.user_id = userId;
                transaction.id = id;

                _dbConnectionContext.Entry(transaction).State = EntityState.Modified;
                await _dbConnectionContext.SaveChangesAsync();

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var userId = _currentUser.Id;
                var searchTransaction = await _dbConnectionContext.Transactions.FirstOrDefaultAsync(t => t.id == id && t.user_id == userId);

                if (searchTransaction is null)
                {
                    return NotFound("Transação não encontrada.");
                }

                _dbConnectionContext.Remove(searchTransaction);
                await _dbConnectionContext.SaveChangesAsync();

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("extract")]
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Get2()
        {
            try
            {
                var userId = _currentUser.Id;
                var extractTransactionEntry = await _dbConnectionContext.Transactions.Where(t => t.type == "entry" && t.user_id == userId).SumAsync(v => v.value);
                var extractTransactionExit = await _dbConnectionContext.Transactions.Where(t => t.type == "exit" && t.user_id == userId).SumAsync(v => v.value);

                var extract =
                    new
                    {
                        entry = extractTransactionEntry,
                        exit = extractTransactionExit
                    };

                return Ok(extract);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            };
        }
    }
}

using api_dindin.Context;
using api_dindin.Models;
using api_dindin.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
    }
}

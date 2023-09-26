using api_dindin.Context;
using api_dindin.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api_dindin.Controllers
{
    [Route("api/category")]
    public class CategoryController : Controller
    {
        private readonly DbConnectionContext _dbConnectionContext;
        private readonly CurrentUser _currentUser;
        public CategoryController(DbConnectionContext dbConnectionContext, CurrentUser currentUser)
        {
            _dbConnectionContext = dbConnectionContext;
            _currentUser = currentUser;
        }

        [Authorize]
        [HttpGet]
        public IActionResult Get()
        {
            var categories = _dbConnectionContext.Categories.ToList();

            return Ok(categories);
        }
    }
}

using api_dindin.Models;
using api_dindin.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace api_dindin.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        [HttpPost]
        public IActionResult Add(UserViewModel userViewModel)
        {
            var user = new User(userViewModel.Name, userViewModel.Email, userViewModel.Password);
            _userRepository.Add(user);

            return Ok();
        }

        [HttpGet]
        public IActionResult Get()
        {
            var user = _userRepository.Get();

            return Ok(user);
        }
    }
}

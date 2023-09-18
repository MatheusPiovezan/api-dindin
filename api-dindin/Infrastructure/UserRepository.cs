using api_dindin.Models;

namespace api_dindin.Infrastructure
{
    public class UserRepository : IUserRepository
    {
        private readonly ConnectionContext _context = new ConnectionContext();

        public void Add(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public List<User> Get()
        {
            return _context.Users.ToList();
        }
    }
}

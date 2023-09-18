namespace api_dindin.Models
{
    public interface IUserRepository
    {
        void Add(User user);

        List<User> Get();
    }
}

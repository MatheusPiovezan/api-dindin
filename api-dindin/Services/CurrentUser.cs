namespace api_dindin.Services
{
    public class CurrentUser
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public CurrentUser(IHttpContextAccessor httpContextAccessor)
        {
            var httpContext = httpContextAccessor.HttpContext;
            var claims = httpContext.User.Claims;

            if (claims.Any(x => x.Type == "userid"))
            {
                Id = Convert.ToInt32(claims.First(x => x.Type == "userid").Value);
            }
            if (claims.Any(x => x.Type == "name"))
            {
                Name = claims.First(x => x.Type == "name").Value;
            }
            if (claims.Any(x => x.Type == "uemail"))
            {
                Email = claims.First(x => x.Type == "useremail").Value;
            }
        }
    }
}

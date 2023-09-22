using api_dindin.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace api_dindin.Services
{
    public class TokenService
    {
        public static object GenerateToken(User user)
        {
            var key = Encoding.ASCII.GetBytes(Key.Secret); //Converte a Key para um array de Bytes
            var tokenConfig = new SecurityTokenDescriptor //Conteudo do Token
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("userid", user.id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Name, user.name),
                    new Claim("useremail", user.email),
                }),
                Expires = DateTime.UtcNow.AddHours(1), //Tempo de expiração do Token
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature) //Realiza assinatura do token
            };

            var tokenHandler = new JwtSecurityTokenHandler(); //Cria um instância do JwtSecurityTokenHandler
            var token = tokenHandler.CreateToken(tokenConfig); //Gera um Token
            var tokenString = tokenHandler.WriteToken(token); //Gera uma string do Token

            return new
            {
                token = tokenString
            };
        }
    }
}

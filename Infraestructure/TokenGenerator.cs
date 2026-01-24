using Microsoft.IdentityModel.Tokens;
using System.Text;
using PlanificadorDeHorarios.Api.Common;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using PlanificadorDeHorarios.Api.Domain;
using Microsoft.IdentityModel.JsonWebTokens;

namespace PlanificadorDeHorarios.Api.Infraestructure
{
    public class TokenGenerator : ITokenGenerator
    {
        private JwtOptions _config;
        public TokenGenerator(IOptions<JwtOptions> options)
        {
            _config = options.Value;
        }

        public string GenerarToken(Usuario usuario)
        {
            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.Secreto));
            SigningCredentials sign = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.Email, usuario.Email)
                }),
                SigningCredentials = sign
            };

            JsonWebTokenHandler tokenHandler = new JsonWebTokenHandler();
            return tokenHandler.CreateToken(tokenDescriptor);
        }
    }
}
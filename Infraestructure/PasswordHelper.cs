using Microsoft.AspNetCore.Identity;
using PlanificadorDeHorarios.Api.Domain;
using PlanificadorDeHorarios.Api.Ports;

namespace PlanificadorDeHorarios.Api.Infraestructure
{
    public class PasswordHelper : IPasswordHelper
    {
        private readonly PasswordHasher<object> _hasher = new();
        public string HashearPassword(string password)
        {
            return _hasher.HashPassword(null, password);
        }

        public bool VerificarPassword(string password, string hashedPassword)
        {
            var result = _hasher.VerifyHashedPassword(null, hashedPassword, password);
            return result == PasswordVerificationResult.Success;
        }
    }
}

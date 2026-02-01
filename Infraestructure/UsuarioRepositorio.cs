using Npgsql;
using PlanificadorDeHorarios.Api.Domain;
using PlanificadorDeHorarios.Api.Ports;
using System.Data;

namespace PlanificadorDeHorarios.Api.Infraestructure
{
    public class UsuarioRepositorio : IUsuarioRepositorio
    {
        private readonly string _connectionString;

        public UsuarioRepositorio(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Default");
        }

        public async Task AgregarUsuarioAsync(Usuario usuario)
        {
            await using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            string sql = "INSERT INTO users(nombre, email, password) VALUES(@nombre, @email, @password)";

            await using var cmd = new NpgsqlCommand(sql,connection);
            cmd.Parameters.AddWithValue("nombre", usuario.Nombre);
            cmd.Parameters.AddWithValue("email", usuario.Email);
            cmd.Parameters.AddWithValue("password", usuario.HashPassword);

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<Usuario?> BuscarPorEmailAsync(string email)
        {
            await using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            string sql = "SELECT id, nombre, email, password FROM users WHERE email = @email";

            await using var cmd = new NpgsqlCommand(sql, connection);

            cmd.Parameters.AddWithValue("email", email);
            await using var reader =  await cmd.ExecuteReaderAsync(CommandBehavior.SingleRow);

            Usuario? usuario = null;

            if (await reader.ReadAsync())
            {
                usuario =  new Usuario()
                {
                    Id = reader.GetInt32(0),
                    Nombre = reader.GetString(1),
                    Email = reader.GetString(2),
                    HashPassword = reader.GetString(3)
                };
            }
            return usuario;
        }

        public async Task<List<Usuario>> ObtenerTodosAsync()
        {
            await using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            string sql = "SELECT id, nombre, email, password FROM users";

            await using var cmd = new NpgsqlCommand(sql, connection);
            await using var reader = await cmd.ExecuteReaderAsync();

            List<Usuario> usuarios = [];

            while(await reader.ReadAsync()) 
            {
                usuarios.Add(new()
                {
                    Id = reader.GetInt32(0),
                    Nombre = reader.GetString(1),
                    Email = reader.GetString(2),
                    HashPassword = reader.GetString(3)
                });
            }
            return usuarios;
        }

        public async Task<bool> VerificarSiExistePorEmailAsync(string email)
        {
            await using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            string sql = "SELECT EXISTS(SELECT 1 FROM users WHERE email = @email)";

            await using var cmd = new NpgsqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("email", email);
            bool existe = (bool) (await cmd.ExecuteScalarAsync() ?? false);

            return existe;
        }
    }
}

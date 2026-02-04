using Npgsql;
using PlanificadorDeHorarios.Api.Domain;
using PlanificadorDeHorarios.Api.Ports;
using System.Text.Json;

namespace PlanificadorDeHorarios.Api.Infraestructure
{
    public class HorarioRepositorio : IHorarioRepositorio
    {
        private readonly string _connectionString;
        public HorarioRepositorio(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Default");
        }

        public async Task GuardarHorario(int idUsuario, Horario horario)
        {
            await using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            string sql = "INSERT INTO horarios(idUsuario, data) VALUES(@idusuario, @data::json)";

            await using var cmd = new NpgsqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("idusuario", idUsuario);
            cmd.Parameters.AddWithValue("data", JsonSerializer.Serialize(horario));

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<List<Horario>> ObtenerHorariosGuardados(int idUsuario)
        {
            var horarios = new List<Horario>();

            await using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            string sql = "SELECT data FROM horarios WHERE idUsuario = @idUsuario";

            await using var cmd = new NpgsqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("isUsuario", idUsuario);

            await using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                string jsonString = reader.GetString(0);
                Console.WriteLine(jsonString);
                horarios.Add(JsonSerializer.Deserialize<Horario>(jsonString));
            }

            return horarios;
        }
    }
}

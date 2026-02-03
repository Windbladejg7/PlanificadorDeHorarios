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

        public async Task GuardarHorariosGenerados(int idUsuario, List<Horario> horarios)
        {
            await using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            string sql = "INSERT INTO horarios(idUsuario, data) VALUES(@idusuario, @data::json)";

            await using var cmd = new NpgsqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("idusuario", idUsuario);
            cmd.Parameters.AddWithValue("data", JsonSerializer.Serialize(horarios));

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<List<List<Horario>>> ObtenerHorariosGuardados(int idUsuario)
        {
            var horarios = new List<List<Horario>>();

            await using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            string sql = "SELECT data FROM horarios WHERE idUsuario = @idUsuario";

            await using var cmd = new NpgsqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("isUsuario", idUsuario);

            await using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                string jsonString = reader.GetString(0);
                horarios.Add(JsonSerializer.Deserialize<List<Horario>>(jsonString));
            }

            return horarios;
        }
    }
}

using HydroponicsServer.Models;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace DAL.PostgresSQL
{
    public class TemperatureRepository : ITemperatureRepository
    {
        private readonly AppSettings _settings;

        private readonly NpgsqlConnection? _connection;

        public TemperatureRepository(IConfiguration appSettings)
        {
            _settings = appSettings.Get<AppSettings>();

            _connection = new NpgsqlConnection(_settings.PostgresqlConnectionString);
            _connection.Open();
        }

        public string? GetVersion()
        {
            using var cmd = new NpgsqlCommand("SELECT version()", _connection);

            return cmd.ExecuteScalar().ToString();
        }

        public async Task Add(string table, TemperatureRecording obj)
        {
            try
            {
                string commandText = $"INSERT INTO \"{table}\" (created, lastupdated, deleted, time, temperature, zone) VALUES (@createdOn, @lastUpdated, @deleted, @time, @temperature, @zone)";

                using (var cmd = new NpgsqlCommand(commandText, _connection))
                {
                    cmd.Parameters.AddWithValue("createdOn", obj.CreatedOn);
                    cmd.Parameters.AddWithValue("lastUpdated", obj.LastUpdated);
                    cmd.Parameters.AddWithValue("deleted", obj.DeletedOn);
                    cmd.Parameters.AddWithValue("time", obj.Time);
                    cmd.Parameters.AddWithValue("temperature", obj.Temperature);
                    cmd.Parameters.AddWithValue("zone", obj.Zone);

                    var result = await cmd.ExecuteNonQueryAsync();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<ZoneTemperature> GetTemperatureForZone(string zone)
        {
            try
            {
                string commandText = $"SELECT * FROM \"{Table.TEMPERATURE_RECORDINGS_AIR}\" WHERE zone='{zone}'";

                using var cmd = new NpgsqlCommand(commandText, _connection);

                List<TemperatureRecordingResponse> results = new();

                await using (NpgsqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        results.Add(ReadTemperatureRecording(reader));
                    }
                }

                return new ZoneTemperature(zone, results);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private static TemperatureRecordingResponse ReadTemperatureRecording(NpgsqlDataReader reader)
        {
            var created = (DateTime)reader["created"];
            var lastUpdated = (DateTime)reader["lastupdated"];
            var deleted = reader["deleted"] as DateTime?;
            var time = (long)reader["time"];
            var temperature = (float)reader["temperature"];

            return new TemperatureRecordingResponse(time, temperature);
        }
    }
}
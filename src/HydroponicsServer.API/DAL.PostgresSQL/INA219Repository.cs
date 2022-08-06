using HydroponicsServer.Models;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace DAL.PostgresSQL
{
    public class INA219Repository : IINA219Repository
    {
        private readonly AppSettings _settings;

        private readonly NpgsqlConnection? _connection;

        public INA219Repository(IConfiguration appSettings)
        {
            _settings = appSettings.Get<AppSettings>();

            _connection = new NpgsqlConnection(_settings.PostgresqlConnectionString);
            _connection.Open();
        }
        public async Task Add(string table, INA219Recording obj)
        {
            try
            {
                string commandText = $"INSERT INTO \"{table}\" (created, lastupdated, deleted, time, voltageIn, voltageOut, shuntVoltage, shuntCurrent, powerCalc, powerRegister) VALUES (@createdOn, @lastUpdated, @deleted, @time, @voltageIn, @voltageOut, @shuntVoltage, @shuntCurrent, @powerCalc, @powerRegister)";

                using (var cmd = new NpgsqlCommand(commandText, _connection))
                {
                    cmd.Parameters.AddWithValue("createdOn", obj.CreatedOn);
                    cmd.Parameters.AddWithValue("lastUpdated", obj.LastUpdated);
                    cmd.Parameters.AddWithValue("deleted", obj.DeletedOn);
                    cmd.Parameters.AddWithValue("time", obj.Time);
                    cmd.Parameters.AddWithValue("voltageIn", obj.VoltageIn);
                    cmd.Parameters.AddWithValue("voltageOut", obj.VoltageOut);
                    cmd.Parameters.AddWithValue("shuntVoltage", obj.ShuntVoltage);
                    cmd.Parameters.AddWithValue("shuntCurrent", obj.ShuntCurrent);
                    cmd.Parameters.AddWithValue("powerCalc", obj.PowerCalc);
                    cmd.Parameters.AddWithValue("powerRegister", obj.PowerRegister);

                    var result = await cmd.ExecuteNonQueryAsync();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
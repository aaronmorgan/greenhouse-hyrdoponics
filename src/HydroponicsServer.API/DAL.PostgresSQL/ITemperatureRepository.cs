using HydroponicsServer.Models;

namespace DAL.PostgresSQL
{
    public interface ITemperatureRepository
    {
        /// <summary>
        /// Adds a new temperature recording to the table provided.
        /// </summary>
        Task Add(string table, TemperatureRecording obj);

        /// <summary>
        /// 
        /// </summary>
        Task<ZoneTemperature> GetTemperatureForZone(string zone);

        /// <summary>
        /// 
        /// </summary>
        string? GetVersion();
    }
}
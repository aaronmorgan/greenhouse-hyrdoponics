using HydroponicsServer.Models;

namespace DAL.PostgresSQL
{
    public interface IINA219Repository
    {
        /// <summary>
        /// Adds a new INA219 current recording to the table provided.
        /// </summary>
        Task Add(string table, INA219Recording obj);
    }
}
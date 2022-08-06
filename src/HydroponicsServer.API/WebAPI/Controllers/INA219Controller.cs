using DAL.PostgresSQL;
using HydroponicsServer.Models;
using HydroponicsServer.WebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace HydroponicsServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class INA219Controller : ControllerBase
    {
        private readonly IINA219Repository _db;

        private static readonly DateTime UnixEpoch = new(1970, 1, 1);
        public static long GetTime(DateTime dateTime) => (long)(dateTime.ToUniversalTime() - UnixEpoch).TotalMilliseconds;

        public INA219Controller(IINA219Repository databaseAgent)
        {
            _db = databaseAgent;
        }

        [HttpPost]
        public async Task<ActionResult<TemperatureRecording>> AddTemperatureRecording([FromBody] INA219RequestDto requestObj)
        {
            var time = GetTime(DateTime.Parse(requestObj.Time));

            await _db.Add(
                Table.INA219_RECORDINGS,
                new INA219Recording(
                    time,
                    requestObj.VoltageIn,
                    requestObj.VoltageOut,
                    requestObj.ShuntVoltage,
                    requestObj.ShuntCurrent,
                    requestObj.PowerCalc,
                    requestObj.PowerRegister));

            return Ok();
        }
    }
}
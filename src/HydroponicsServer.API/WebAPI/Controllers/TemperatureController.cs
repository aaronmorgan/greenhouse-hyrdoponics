using DAL.PostgresSQL;
using HydroponicsServer.Models;
using HydroponicsServer.WebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Prometheus;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HydroponicsServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TemperatureController : ControllerBase
    {
        private readonly ILogger<TemperatureController> _logger;
        private readonly ITemperatureRepository _db;

        private static readonly DateTime UnixEpoch = new(1970, 1, 1);
        public static long GetTime(DateTime dateTime) => (long)(dateTime.ToUniversalTime() - UnixEpoch).TotalMilliseconds;

        private static readonly Gauge AirTemperature = Metrics.CreateGauge("air_temperature", "Current air temperature");
        private static readonly Gauge WaterTemperature = Metrics.CreateGauge("water_temperature", "Current temperature of the water reservoir");

        public TemperatureController(ILogger<TemperatureController> logger, ITemperatureRepository databaseAgent)
        {
            _logger = logger;
            _db = databaseAgent;
        }

        [HttpGet]
        public async Task<ActionResult<List<double[]>>> Get() => Ok(await _db.GetTemperatureForZone("Aarons Study"));

        [HttpPost]
        public async Task<ActionResult<TemperatureRecording>> AddTemperatureRecording([FromBody] TemperatureRequestDto requestObj)
        {
            _logger.LogInformation("{functionName}, Request received='{request}'", nameof(AddTemperatureRecording), JsonConvert.SerializeObject(requestObj));

            var time = GetTime(DateTime.Parse(requestObj.Time));

            string tableName;

            switch (requestObj.Type.ToUpper())
            {
                case "AIR":
                    {
                        AirTemperature.Set(requestObj.Temperature);
                        tableName = Table.TEMPERATURE_RECORDINGS_AIR;
                        break;
                    }
                case "WATER":
                    {
                        WaterTemperature.Set(requestObj.Temperature);
                        tableName = Table.TEMPERATURE_RECORDINGS_WATER;
                        break;
                    }

                default:
                    return BadRequest($"Unknown type '{requestObj.Type}'");
            }

            await _db.Add(tableName, new TemperatureRecording(time, requestObj.Temperature, requestObj.Zone));

            return Ok();
        }
    }
}
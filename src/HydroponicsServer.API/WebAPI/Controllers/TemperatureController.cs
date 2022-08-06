﻿using DAL.PostgresSQL;
using HydroponicsServer.Models;
using HydroponicsServer.WebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HydroponicsServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TemperatureController : ControllerBase
    {
        private readonly ITemperatureRepository _db;

        private static readonly DateTime UnixEpoch = new(1970, 1, 1);
        public static long GetTime(DateTime dateTime) => (long)(dateTime.ToUniversalTime() - UnixEpoch).TotalMilliseconds;

        public TemperatureController(ITemperatureRepository databaseAgent)
        {
            _db = databaseAgent;
        }

        [HttpGet]
        public async Task<ActionResult<List<double[]>>> Get() => Ok(await _db.GetTemperatureForZone("Aarons Study"));

        [HttpPost]
        public async Task<ActionResult<TemperatureRecording>> AddTemperatureRecording([FromBody] TemperatureRequestDto requestObj)
        {
            var time = GetTime(DateTime.Parse(requestObj.Time));

            string tableName;

            switch (requestObj.Type.ToUpper())
            {
                case "AIR":
                    {
                        tableName = Table.TEMPERATURE_RECORDINGS_AIR;
                        break;
                    }
                case "WATER":
                    {
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
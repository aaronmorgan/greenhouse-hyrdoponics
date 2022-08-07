﻿using DAL.PostgresSQL;
using HydroponicsServer.Models;
using HydroponicsServer.WebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Prometheus;
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

        private static readonly Gauge VoltageIn = Metrics.CreateGauge("voltage_in", "");
        private static readonly Gauge VoltageOut = Metrics.CreateGauge("voltage_out", "");
        private static readonly Gauge ShuntVoltage = Metrics.CreateGauge("shunt_voltage", "");
        private static readonly Gauge ShuntCurrent = Metrics.CreateGauge("shunt_current", "");
        private static readonly Gauge PowerCalc = Metrics.CreateGauge("power_calc", "");
        private static readonly Gauge PowerRegister = Metrics.CreateGauge("power_register", "");

        public INA219Controller(IINA219Repository databaseAgent)
        {
            _db = databaseAgent;
        }

        [HttpPost]
        public async Task<ActionResult<TemperatureRecording>> AddTemperatureRecording([FromBody] INA219RequestDto requestObj)
        {
            var time = GetTime(DateTime.Parse(requestObj.Time));

            VoltageIn.Set(requestObj.VoltageIn);
            VoltageOut.Set(requestObj.VoltageOut);
            ShuntVoltage.Set(requestObj.ShuntVoltage);
            ShuntCurrent.Set(requestObj.ShuntCurrent);
            PowerCalc.Set(requestObj.PowerCalc);
            PowerRegister.Set(requestObj.PowerRegister);

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
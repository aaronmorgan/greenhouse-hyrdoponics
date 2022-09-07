using HydroponicsServer.Models;
using HydroponicsServer.WebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Prometheus;
using System;

namespace HydroponicsServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class INA219Controller : ControllerBase
    {
        private readonly ILogger<INA219Controller> _logger;

        private static readonly DateTime UnixEpoch = new(1970, 1, 1);
        public static long GetTime(DateTime dateTime) => (long)(dateTime.ToUniversalTime() - UnixEpoch).TotalMilliseconds;

        private static readonly Gauge VoltageIn = Metrics.CreateGauge("voltage_in", "");
        private static readonly Gauge VoltageOut = Metrics.CreateGauge("voltage_out", "");
        private static readonly Gauge ShuntVoltage = Metrics.CreateGauge("shunt_voltage", "");
        private static readonly Gauge ShuntCurrent = Metrics.CreateGauge("shunt_current", "");
        private static readonly Gauge PowerCalc = Metrics.CreateGauge("power_calc", "");
        private static readonly Gauge PowerRegister = Metrics.CreateGauge("power_register", "");

        private static readonly Gauge SolarPanelCharging = Metrics.CreateGauge("hy_solar_panel_charging", "");
        private static readonly Gauge WaterPumpOn = Metrics.CreateGauge("hy_water_pump_on", "");

        public INA219Controller(ILogger<INA219Controller> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public ActionResult<TemperatureRecording> AddTemperatureRecording([FromBody] INA219RequestDto requestObj)
        {
            _logger.LogInformation("{functionName}, Request received='{request}'", nameof(AddTemperatureRecording), JsonConvert.SerializeObject(requestObj));

            var time = GetTime(DateTime.Parse(requestObj.Time));

            VoltageIn.Set(requestObj.VoltageIn);
            VoltageOut.Set(requestObj.VoltageOut);
            ShuntVoltage.Set(requestObj.ShuntVoltage);
            ShuntCurrent.Set(requestObj.ShuntCurrent);
            PowerCalc.Set(requestObj.PowerCalc);
            PowerRegister.Set(requestObj.PowerRegister);

            SolarPanelCharging.Set(Convert.ToDouble(requestObj.SolarPanelCharging));
            WaterPumpOn.Set(Convert.ToDouble(requestObj.WaterPumpOn));

            return Ok();
        }
    }
}
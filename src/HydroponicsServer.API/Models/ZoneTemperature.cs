using System.Collections.Generic;

namespace HydroponicsServer.Models
{
    public class ZoneTemperature
    {
        public string Zone { get; }
        public int RecordCount { get; }

        public List<TemperatureRecordingResponse> Temperatures { get; }

        public ZoneTemperature(string zone, List<TemperatureRecordingResponse> temperatures)
        {
            Zone = zone;
            RecordCount = temperatures.Count;
            Temperatures = temperatures;
        }
    }
}

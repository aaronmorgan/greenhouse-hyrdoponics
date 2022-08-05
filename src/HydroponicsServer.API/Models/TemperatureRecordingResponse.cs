namespace HydroponicsServer.Models
{
    public class TemperatureRecordingResponse
    {
        public long Time { get; set; }
        public float Temperature { get; set; }

        public TemperatureRecordingResponse(long time, float temperature)
        {
            Time = time;
            Temperature = temperature;
        }
    }
}

namespace HydroponicsServer.Models
{
    public class TemperatureRecording : BaseEntity
    {
        public long Time { get; set; }
        public float Temperature { get; set; }
        public string Zone { get; set; }

        public TemperatureRecording(long time, float temperature, string zone)
        {
            Time = time;
            Temperature = temperature;
            Zone = zone;
        }
    }
}

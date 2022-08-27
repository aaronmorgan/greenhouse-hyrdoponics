namespace HydroponicsServer.WebAPI.Models
{
    public class INA219RequestDto
    {
        public string Time;
        public float VoltageOut;
        public float VoltageIn;
        public float ShuntVoltage;
        public float ShuntCurrent;
        public float PowerCalc;
        public float PowerRegister;

        public bool SolarPanelCharging;
    }
}

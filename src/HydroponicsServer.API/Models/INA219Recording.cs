namespace HydroponicsServer.Models
{
    public class INA219Recording : BaseEntity
    {
        public long Time { get; set; }

        public float VoltageOut { get; set; }
        public float VoltageIn { get; set; }
        public float ShuntVoltage { get; set; }
        public float ShuntCurrent { get; set; }
        public float PowerCalc { get; set; }
        public float PowerRegister { get; set; }

        public INA219Recording(long time, float voltageIn, float voltageOut, float shuntVoltage, float shuntCurrent, float powerCalc, float powerRegister)
        {
            Time = time;
            VoltageIn = voltageIn;
            VoltageOut = voltageOut;
            ShuntCurrent = shuntCurrent;
            ShuntVoltage = shuntVoltage;
            PowerCalc = powerCalc;
            PowerRegister = powerRegister;
        }
    }
}

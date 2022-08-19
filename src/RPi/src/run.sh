export SENSOR_POLLING_INTERVAL_SECONDS="600"
export HOST_ADDRESS="http://192.168.1.74:8083"

echo -e "Starting Initialisation..."

echo "  • Disabling onboard Bluetooth"
sudo hciconfig hci0 down

echo -e "\nInitialisation complete.\n"
python greenhouse-hydroponics.py

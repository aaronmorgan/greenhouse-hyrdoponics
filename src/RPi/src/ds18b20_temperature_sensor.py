# See https://www.circuitbasics.com/raspberry-pi-ds18b20-temperature-sensor-tutorial/ for Linux setup instructions.

# 1. At the command prompt, enter sudo nano /boot/config.txt, then add this to the bottom of the file:
#       $ dtoverlay=w1-gpio
# 2. Exit Nano, and reboot the Pi with sudo reboot
# 3. Log in to the Pi again, and at the command prompt enter:
#       $ sudo modprobe w1-gpio
# 4. Then enter:
#       $ sudo modprobe w1-therm
# 5. Change directories to the /sys/bus/w1/devices directory by entering cd /sys/bus/w1/devices
# 6. Now enter ls to list the devices, e.g. 28-000006637696 w1_bus_master1 is displayed.
# 7. Now enter cd 28-XXXXXXXXXXXX (change the X’s to your own address)
# 8. Enter cat w1_slave which will show the raw temperature reading output by the sensor, e.g. t=28625 means a temperature of 28.625 degrees Celsius.

import os
import glob
import RPi.GPIO as GPIO


os.system('modprobe w1-gpio')
os.system('modprobe w1-therm')

GPIO.setmode(GPIO.BCM)

# Water temperature sensor
base_dir = '/sys/bus/w1/devices/'

def __read_temp_raw(device_file):
    f = open(device_file, 'r')
    lines = f.readlines()
    f.close()
    return lines
 
def read_temp(device_folder):
    device_folder = glob.glob(base_dir + device_folder)[0]
    device_file = device_folder + '/w1_slave'

    lines = __read_temp_raw(device_file)

    while lines[0].strip()[-3:] != 'YES':
        time.sleep(0.2)
        lines = __read_temp_raw(device_file)

    equals_pos = lines[1].find('t=')

    if equals_pos != -1:
        temp_string = lines[1][equals_pos+2:]
        temp_c = float(temp_string) / 1000.0
        temp_f = temp_c * 9.0 / 5.0 + 32.0
        return temp_c#, temp_f

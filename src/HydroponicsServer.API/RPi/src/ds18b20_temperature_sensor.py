# https://www.amazon.com/dp/B087JQ6MCP?psc=1&ref=ppx_yo2ov_dt_b_product_details

import os
import glob
import RPi.GPIO as GPIO

# See https://www.circuitbasics.com/raspberry-pi-ds18b20-temperature-sensor-tutorial/ for Linux setup instructions.
os.system('modprobe w1-gpio')
os.system('modprobe w1-therm')

GPIO.setmode(GPIO.BCM)

# Water temperature sensor
base_dir = '/sys/bus/w1/devices/'
device_folder = glob.glob(base_dir + '28-012062f1a501')[0]
device_file = device_folder + '/w1_slave'

def read_temp_raw():
    f = open(device_file, 'r')
    lines = f.readlines()
    f.close()
    return lines
 
def read_temp():
    lines = read_temp_raw()
    while lines[0].strip()[-3:] != 'YES':
        time.sleep(0.2)
        lines = read_temp_raw()
    equals_pos = lines[1].find('t=')
    if equals_pos != -1:
        temp_string = lines[1][equals_pos+2:]
        temp_c = float(temp_string) / 1000.0
        temp_f = temp_c * 9.0 / 5.0 + 32.0
        return temp_c#, temp_f

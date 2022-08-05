#!/usr/bin/env python

import os
import sys
import json
import requests
import datetime

import time
import grovepi

import RPi.GPIO as GPIO

from grove_rgb_lcd import *

def myconverter(o):
    if isinstance(o, datetime.datetime):
        return o.__str__()

sleep_period = float(os.getenv('TEMP_SLEEP_SECONDS'))
host_url = os.getenv('AIR_TEMP_HOST_URL')

print("TEMP_SLEEP_SECONDS: " + str(sleep_period))
print("AIR_TEMP_HOST_URL: " + str(host_url))
print("")

if sleep_period is None:
    sleep_period = 120
    print ("Error: TEMP_SLEEP_SECONDS is not set, using default value of " + str(sleep_period) + " seconds")

if host_url is None:
    print ("Error: AIR_TEMP_HOST_URL is not set, using defaults")
    sys.exit()

LED_ON = 1
LED_OFF = 0

# Temperature sensor.
airTempSensorPort = 14
grovepi.pinMode(airTempSensorPort, "INPUT")

# Debugging LED
led = 11

GPIO.setmode(GPIO.BCM)
GPIO.setup(led, GPIO.OUT, initial=GPIO.LOW)

data = {
    'temperature': 0.0,
    'type': 'air',
    'zone': 'Greenhouse'
}

headers = {'content-type': 'application/json', 'Accept': 'application/json'}

while True:
    try:
        temp_sensor = grovepi.temp(airTempSensorPort, '1.2')
        print(str(temp_sensor))

        setText("Temp: " + str(round(temp_sensor, 2)))
        setRGB(0,128,64)

        data['time'] = datetime.datetime.now()
        data['temperature'] = temp_sensor

        GPIO.output(led, GPIO.HIGH) # Turn the LED on while transmitting.

        response = requests.post(host_url,
                          data=json.dumps(data, default=myconverter), headers=headers)

        if response.status_code != 200:
          print(response.status_code, response.reason)

        # grovepi.digitalWrite(led, LED_OFF)

    except ValueError:
        print ("Math Error");
    except KeyboardInterrupt:
        # grovepi.digitalWrite(led, LED_OFF)
        break
    except IOError:
        print("Error")
    finally:
        GPIO.output(led, GPIO.LOW)
        time.sleep(sleep_period)

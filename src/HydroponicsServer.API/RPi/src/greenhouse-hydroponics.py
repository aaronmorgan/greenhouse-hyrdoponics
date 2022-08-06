#!/usr/bin/env python

import os
import sys
import datetime

import time
import grovepi

import RPi.GPIO as GPIO

from grove_rgb_lcd import *

import rest_client
import ds18b20_temperature_sensor

sleep_period = float(os.getenv('TEMP_SLEEP_SECONDS'))

if sleep_period is None:
    sleep_period = 120
    print ("Error: TEMP_SLEEP_SECONDS is not set, using default value of " + str(sleep_period) + " seconds")

# Temperature sensor.
airTempSensorPort = 14
grovepi.pinMode(airTempSensorPort, "INPUT")

# Debugging LED
led = 11

GPIO.setmode(GPIO.BCM)
GPIO.setup(led, GPIO.OUT, initial=GPIO.LOW)


while True:
    try:
        air_temp_sensor = grovepi.temp(airTempSensorPort, '1.2')
        print(str(air_temp_sensor))

        water_temp = ds18b20_temperature_sensor.read_temp()
        print(water_temp)

        setText("Temp: " + str(round(air_temp_sensor, 2)))
        setRGB(0,128,64)

        GPIO.output(led, GPIO.HIGH) # Turn the LED on while transmitting.

        rest_client.post({
            'time': datetime.datetime.now(),
            'temperature': air_temp_sensor,
            'type': 'air',
            'zone': 'Greenhouse'
        })

        rest_client.post({
            'time': datetime.datetime.now(),
            'temperature': water_temp,
            'type': 'water',
            'zone': 'Greenhouse'
        })

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

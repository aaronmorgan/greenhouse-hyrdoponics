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
import ina219_sensor

host_address = os.getenv('HOST_ADDRESS')

if host_address is None:
    print ("Error: HOST_ADDRESS is not set, using defaults")
    sys.exit()

polling_period_seconds = float(os.getenv('SENSOR_POLLING_INTERVAL_SECONDS'))

if polling_period_seconds is None:
    polling_period_seconds = 120
    print ("Error: SENSOR_POLLING_INTERVAL_SECONDS is not set, using default value of " + str(polling_period_seconds) + " seconds")

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
        print('Room Temperature: {}'.format(str(air_temp_sensor)))

        water_temp = ds18b20_temperature_sensor.read_temp()
        print('Water Temperature: {}'.format(water_temp))

        setText("Temp: " + str(round(air_temp_sensor, 2)))
        setRGB(0,128,64)

        GPIO.output(led, GPIO.HIGH) # Turn the LED on while transmitting.

        rest_client.post(
            '{}/api/temperature/'.format(host_address),
            {
                'time': datetime.datetime.now(),
                'temperature': air_temp_sensor,
                'type': 'air',
                'zone': 'Greenhouse'
            })

        rest_client.post(
            '{}/api/temperature/'.format(host_address),
            {            
                'time': datetime.datetime.now(),
                'temperature': water_temp,
                'type': 'water',
                'zone': 'Greenhouse'
            })

        print()
        ina219_data = ina219_sensor.get_state()

        if ina219_data is not None:
            ina219_data['time'] = datetime.datetime.now()

            rest_client.post(
                '{}/api/ina219/'.format(host_address),
                ina219_data)

    except ValueError:
        print ("Math Error");
    except KeyboardInterrupt:
        # grovepi.digitalWrite(led, LED_OFF)
        break
    except IOError:
        print("Error")
    finally:
        GPIO.output(led, GPIO.LOW)
        time.sleep(polling_period_seconds)
        print('--- END ---')

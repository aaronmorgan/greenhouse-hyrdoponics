#!/usr/bin/env python

import os
import sys
import datetime
import glob
import time
import logging

import rest_client
import ds18b20_temperature_sensor
import ina219_sensor

logging.basicConfig(level=logging.DEBUG)
logging.info("{}: Service started".format(datetime.datetime.today()))

host_address = os.getenv('HOST_ADDRESS')

if host_address is None:
    logging.info("Error: HOST_ADDRESS is not set, exiting")
    sys.exit()

polling_period_seconds = float(os.getenv('SENSOR_POLLING_INTERVAL_SECONDS'))

if polling_period_seconds is None:
    polling_period_seconds = 600
    logging.info("Error: SENSOR_POLLING_INTERVAL_SECONDS is not set, using default value of " + str(polling_period_seconds) + " seconds")

logging.info("Polling set to {} seconds".format(polling_period_seconds))

water_temp_sensor_device_folder = '28-01212e312bdd'
air_temp_sensor_device_folder = '28-012062f1a501'

while True:
    try:
        logging.info('Gathering data...')

        air_temp = ds18b20_temperature_sensor.read_temp(air_temp_sensor_device_folder)
        logging.info('  Air Temperature: {}'.format(air_temp))

        water_temp = ds18b20_temperature_sensor.read_temp(water_temp_sensor_device_folder)
        logging.info('  Water Temperature: {}'.format(water_temp))

        rest_client.post(
            '{}/api/temperature/'.format(host_address),
            {
                'time': datetime.datetime.now(),
                'temperature': air_temp,
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

        ina219_data = ina219_sensor.get_state()

        if ina219_data is not None:
            ina219_data['time'] = datetime.datetime.now()

            rest_client.post(
                '{}/api/ina219/'.format(host_address),
                ina219_data)

    except ValueError:
        logging.info("Math Error");
    except KeyboardInterrupt:
        break
    except IOError:
        logging.info("Error")
    finally:
        logging.info("Execution complete, sleeping for {} seconds".format(polling_period_seconds))
        time.sleep(polling_period_seconds)

logging.info("Service terminate")
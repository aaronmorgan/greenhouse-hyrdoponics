#!/usr/bin/env python

import os
import sys
import datetime
import glob
import time
import logging

_logger = logging.getLogger(__name__)

import RPi.GPIO as GPIO

GPIO.setmode(GPIO.BCM)

relay_pin = 27
relay_state = GPIO.LOW

GPIO.setup(relay_pin, GPIO.OUT) # set a port/pin as an output   

def set_state_on():
    relay_state = GPIO.HIGH
    GPIO.output(relay_pin, relay_state)
    _logger.info('Relay state: {}'.format(relay_state))

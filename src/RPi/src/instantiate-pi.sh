#!/bin/bash

sudo apt-get update
sudo apt upgrade -y

sudo apt install -y git

# Required for the INA219 sensor and the 'board' import.
# https://pypi.org/project/pi-ina219/
sudo apt install -y python3
sudo apt install -y python3-pip
sudo pip3 install Adafruit-Blinka
#sudo pip3 install pi-ina219
sudo pip3 install adafruit-circuitpython-ina219

# Enable the I2C port (required for INA219 sensor).
sudo raspi-config nonint do_i2c 0

# Tidy up.
sudo apt-get clean

sudo modprobe w1-gpio
sudo modprobe w1-therm

cd /sys/bus/w1/devices

# By default the 'one wire' (W1) protocol is disabled on the RPi. Enable it with (default pin is BCM4):
#dtoverlay=w1-gpio 

# To activate the protocol an one or more custom pins (default is BCM4) use:
#dtoverlay=w1-gpio,gpiopin=4
#dtoverlay=w1-gpio,gpiopin=24

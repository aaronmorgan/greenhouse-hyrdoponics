# SPDX-FileCopyrightText: 2021 ladyada for Adafruit Industries
# SPDX-License-Identifier: MIT

"""Sample code and test for adafruit_ina219"""

# https://github.com/adafruit/Adafruit_CircuitPython_INA219

import time
import board
from adafruit_ina219 import ADCResolution, BusVoltageRange, INA219

i2c_bus = board.I2C()

ina219 = INA219(i2c_bus)

# display some of the advanced field (just to test)
print("Config register:")
print("  bus_voltage_range:    0x%1X" % ina219.bus_voltage_range)
print("  gain:                 0x%1X" % ina219.gain)
print("  bus_adc_resolution:   0x%1X" % ina219.bus_adc_resolution)
print("  shunt_adc_resolution: 0x%1X" % ina219.shunt_adc_resolution)
print("  mode:                 0x%1X" % ina219.mode)
print("")

# optional : change configuration to use 32 samples averaging for both bus voltage and shunt voltage
ina219.bus_adc_resolution = ADCResolution.ADCRES_12BIT_32S
ina219.shunt_adc_resolution = ADCResolution.ADCRES_12BIT_32S
# optional : change voltage range to 16V
ina219.bus_voltage_range = BusVoltageRange.RANGE_16V

# measure and display loop
def get_state():
    bus_voltage = ina219.bus_voltage  # voltage on V- (load side)
    shunt_voltage = ina219.shunt_voltage  # voltage between V+ and V- across the shunt
    current = ina219.current  # current in mA
    power = ina219.power  # power in watts

    voltageIn = bus_voltage + shunt_voltage
    shunt_current = current / 1000
    power_calc = bus_voltage * (current / 1000)

    # INA219 measure bus voltage on the load side. So PSU voltage = bus_voltage + shunt_voltage
    print("  Voltage (VIN+) : {:6.3f}   V".format(voltageIn))
    print("  Voltage (VIN-) : {:6.3f}   V".format(bus_voltage))
    print("  Shunt Voltage  : {:8.5f} V".format(shunt_voltage))
    print("  Shunt Current  : {:7.4f}  A".format(shunt_current))
    print("  Power Calc.    : {:8.5f} W".format(power_calc))
    print("  Power Register : {:6.3f}   W".format(power))
    print("")

    # Check internal calculations haven't overflowed (doesn't detect ADC overflows)
    if ina219.overflow:
        print("Internal Math Overflow Detected!")
        print("")
        return

    return {
        'voltageIn': voltageIn,
        'voltageOut': bus_voltage,
        'shuntVoltage': shunt_voltage,
        'shuntCurrent': shunt_current,
        'powerCalc': power_calc,
        'powerRegister': power
    }

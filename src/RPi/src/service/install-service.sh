#!/bin/bash

sudo cp gh-hydro.service /etc/systemd/system/gh-hydro.service
sudo chmod +x ../greenhouse-hydroponics.sh

# Configure the service to start on boot.
systemctl enable gh-hydro

# Start the service now, run these manually:
# systemctl stop gh-hydro; systemctl start gh-hydro; systemctl daemon-reload

# List running services
# systemctl 

# Get logs for a specific service
# journalctl -u service-name.service
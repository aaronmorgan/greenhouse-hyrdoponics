import os
import json
import requests
import datetime
import logging

_logger = logging.getLogger(__name__)

def dateTimeConverter(o):
    if isinstance(o, datetime.datetime):
        return o.__str__()

headers = {
    'content-type': 'application/json',
    'Accept': 'application/json'
}


def post(route, data):
    try:
        response = requests.post(route, data=json.dumps(data, default=dateTimeConverter), headers=headers)

        if response.status_code != 200:
            print(response.status_code, response.reason)

        return response
    except IOError:
        print("Error")

import os
import json
import requests
import datetime

host_url = os.getenv('TEMP_HOST_URL')

if host_url is None:
    print ("Error: AIR_TEMP_HOST_URL is not set, using defaults")
    sys.exit()

def dateTimeConverter(o):
    if isinstance(o, datetime.datetime):
        return o.__str__()

headers = {
    'content-type': 'application/json',
    'Accept': 'application/json'
}


def post(data):
    try:
        response = requests.post(host_url, data=json.dumps(data, default=dateTimeConverter), headers=headers)

        if response.status_code != 200:
            print(response.status_code, response.reason)

        return response
    except IOError:
        print("Error")

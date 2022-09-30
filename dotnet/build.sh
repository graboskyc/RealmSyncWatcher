#!/bin/bash

echo
echo "+======================"
echo "| START: RealmSyncTimerApp"
echo "+======================"
echo

source .env
echo "Using args ${REALMAPPID} and ${APIKEY}"

docker build -t graboskyc/rsta:latest .
docker stop rsta
docker rm rsta
docker run -t -i -d--name rsta -e "REALMAPPID=${REALMAPPID}" -e "APIKEY=${APIKEY}" --restart unless-stopped graboskyc/rsta:latest

echo
echo "+======================"
echo "| END: RealmSyncTimerApp"
echo "+======================"
echo
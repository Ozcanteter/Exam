#!/bin/bash

if [[ ! -d certs ]]
then
    mkdir certs
    cd certs/
    if [[ ! -f localhost.pfx ]]
    then
        dotnet dev-certs https -v -ep localhost.pfx -p 80086731-2876-4b83-860a-cdca3d722d2f -t
    fi
    cd ../
fi

docker-compose up -d

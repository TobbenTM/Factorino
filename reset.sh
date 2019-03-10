#!/bin/bash

docker-compose -f "infrastructure\kafka\docker-compose.yml" down
docker-compose -f "infrastructure\kafka\docker-compose.yml" up -d --force-recreate

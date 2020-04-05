#!/bin/bash

docker-compose -f "docker-compose.yml" down -v
docker-compose -f "docker-compose.yml" up -d --force-recreate

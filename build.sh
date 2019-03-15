#!/bin/bash

# Build factory pod
docker image build -f src/FNO.FactoryPod/Dockerfile -t factorino/factory-pod  .

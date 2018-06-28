#!/bin/bash

# Build all docker images
for project in src/*; do
  if [ -d "${project}" ]; then
      pushd "${project}"
      
      echo "Evaluating ${project}..."

      if [ -f Dockerfile ]; then
        echo "Building docker image..."
      else
        echo "No Dockerfile found, skipping!"
      fi

      popd
    fi
done

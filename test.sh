#!/bin/bash

# We're trying with -W first, for Windows compat
coverageDir="$(pwd -W || pwd)/coverage"
echo "Creating coverage reports in $coverageDir.."

# Create necessary folders, or clean if already present
if [ -d $coverageDir ]; then
  echo "Existing coverage folder found, cleaning before tests.."
  rm -rf $coverageDir
fi

# Creating output folder..
mkdir -p $coverageDir

# ..and initial coverage aggregate file
echo {} > $coverageDir/coverage.json

# We'll be testing all test/*.Tests projects
for project in test/*; do
  if [[ $project != *".Tests" ]]; then
    continue
  fi
  projectPath="$project/$(basename $project).csproj"
  echo "Testing $projectPath.."

  # We're using coverlet to get coverage from the unit tests
  dotnet test $projectPath \
    -p:CollectCoverage=true \
    -p:MergeWith=\"$coverageDir/coverage.json\" \
    -p:CoverletOutputFormat=\"json,opencover\" \
    -p:CoverletOutput=$coverageDir/ \
    -p:Exclude="[xunit*]*"

done

# This is the docker file for CI builds of Factorino

FROM microsoft/dotnet:2.1-sdk AS build-env
WORKDIR /app

# We need a token for this build
ARG sonarcloud_token

# The SonarQube scanner need java..
RUN apt-get update && apt-get -yq install openjdk-8-jre

# ..and node for analysing CSS files..
RUN apt-get update -yq \
    && apt-get install curl gnupg -yq \
    && curl -sL https://deb.nodesource.com/setup_8.x | bash \
    && apt-get install nodejs -yq

# Install tools
RUN dotnet tool install --global dotnet-sonarscanner
ENV PATH="${PATH}:/root/.dotnet/tools"

# Copy everything
COPY . ./

# Init SonarCloud analysis
RUN dotnet sonarscanner begin \
      /k:"TobbenTM_Factorino" \
      /o:"tobbentm-github" \
      /d:sonar.host.url="https://sonarcloud.io" \
      /d:sonar.login="$sonarcloud_token" \
      /d:sonar.language="cs" \
      /d:sonar.exclusions="**/bin/**/*,**/obj/**/*,**/vendor/**/*,**/dist/**/*,**/node_modules/**/*" \
      /d:sonar.coverage.exclusions="test/**" \
      /d:sonar.sourceEncoding="UTF-8" \
      /d:sonar.cs.opencover.reportsPath="/app/coverage/coverage.opencover.xml"

# Build and test
RUN dotnet restore
RUN dotnet build
RUN bash test.sh

# Finish analysis
RUN dotnet sonarscanner end /d:sonar.login="$sonarcloud_token"

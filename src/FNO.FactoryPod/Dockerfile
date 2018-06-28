FROM microsoft/dotnet:2.1-sdk AS build-env

WORKDIR /opt

# Install xz tools
RUN apt-get update && apt-get install xz-utils

# Download and extract factorio into /opt
RUN curl -SL https://www.factorio.com/get-download/0.16.51/headless/linux64 | tar -xJ

# Changing to application workdir
WORKDIR /app

# Copy solution into build container and build/test/publish
COPY . ./
RUN dotnet restore src/FNO.FactoryPod/FNO.FactoryPod.csproj
RUN dotnet publish src/FNO.FactoryPod -c Release -o /app/out

# Build runtime image
FROM microsoft/dotnet:2.1-runtime
WORKDIR /app
RUN mkdir -p /opt/factorio/
COPY --from=build-env /app/out .
COPY --from=build-env /opt/factorio /opt/factorio

# Install mods
#RUN mkdir -p /opt/factorio/mods/factorino_0.1.0/
#COPY --from=build-env /app/mods/factorino /opt/factorio/mods/factorino_0.1.0/

VOLUME /opt/factorino/saves /opt/factorino/seeds
EXPOSE 34197/udp
ENTRYPOINT ["dotnet", "FNO.FactoryPod.dll"]

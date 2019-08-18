FROM microsoft/dotnet:2.2-sdk AS build-env
WORKDIR /app

# Copy solution into build container and build/test/publish
COPY . ./
RUN dotnet restore src/FNO.Broker/FNO.Broker.csproj
RUN dotnet publish src/FNO.Broker -c Release -o /app/out

# Build runtime image
FROM microsoft/dotnet:2.2-runtime
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "FNO.Broker.dll"]

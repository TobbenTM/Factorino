FROM microsoft/dotnet:2.1-sdk AS build-env
WORKDIR /app

# Copy solution into build container and build/test/publish
COPY . ./
RUN dotnet restore src/FNO.WebApp/FNO.WebApp.csproj
RUN dotnet publish src/FNO.WebApp -c Release -o /app/out

# Build runtime image
FROM microsoft/dotnet:2.1-aspnetcore-runtime
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "FNO.WebApp.dll"]

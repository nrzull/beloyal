FROM mcr.microsoft.com/dotnet/sdk:9.0-alpine AS build
WORKDIR /src

COPY backend/api/Beloyal.API.csproj /src/
RUN dotnet restore

COPY backend/api /src
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0-alpine AS final
WORKDIR /app
RUN apk add --no-cache tzdata

COPY --from=build /app/publish .
EXPOSE 8080
ENTRYPOINT ["dotnet", "Beloyal.API.dll"]

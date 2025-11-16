FROM mcr.microsoft.com/dotnet/sdk:9.0-alpine AS build
WORKDIR /src

COPY backend/api /src
RUN dotnet restore

COPY .config /src/
RUN dotnet tool restore
RUN dotnet ef migrations script --idempotent -o /src/migrations.sql

FROM alpine/psql:17.4 AS final
WORKDIR /app

COPY --from=build /src/migrations.sql /app/

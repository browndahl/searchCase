﻿FROM mcr.microsoft.com/dotnet/sdk:7.0-bullseye-slim AS build
WORKDIR /app

COPY . .
RUN dotnet publish /app/SearchAPI/SearchAPI.csproj -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:7.0-alpine AS runtime
EXPOSE 5262

ENV ASPNETCORE_URLS=http://+:5262

WORKDIR /app

COPY --from=build /app/publish ./

ENTRYPOINT ["dotnet", "/app/SearchAPI.dll"]
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /app

COPY QuickTaskAPI.csproj ./
RUN dotnet restore

COPY . ./

RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out ./
ENTRYPOINT ["dotnet", "QuickTaskAPI.dll"]
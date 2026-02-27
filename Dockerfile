FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-stage
WORKDIR /src

COPY . ./
RUN dotnet restore
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /src
COPY --from=build-stage /src/out .
ENTRYPOINT ["dotnet", "WelwiseGames.NotificationService.Host.dll"]
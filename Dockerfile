FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY *.csproj .
RUN dotnet restore
COPY . .
RUN dotnet publish -c Release -o /publish

FROM base as final
WORKDIR /app
COPY --from=build /publish/* .

EXPOSE 80
ENTRYPOINT ["dotnet", "your-workout-guide-backend.dll"]
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS base

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app
EXPOSE 80
EXPOSE 443

COPY PocketGymTrainer/PocketGymTrainer.csproj .
RUN dotnet restore

COPY . .
RUN dotnet publish -c Release -o publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT [ "dotnet", "PocketGymTrainer.dll" ]
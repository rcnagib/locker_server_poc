# Use the official .NET SDK image to build the app
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Use the SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY worker.csproj worker/
RUN dotnet restore "worker/worker.csproj"
COPY . worker/
WORKDIR /src/worker
RUN dotnet build "worker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "worker.csproj" -c Release -o /app/publish

# Copy the build output to the base image and set the entry point
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "worker.dll"]
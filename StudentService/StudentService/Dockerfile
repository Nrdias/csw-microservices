# Use the official .NET 10 runtime image as the base image
FROM mcr.microsoft.com/dotnet/aspnet:10.0-preview AS base
WORKDIR /app
EXPOSE 80

# Use the SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build
WORKDIR /src
COPY ["StudentService.csproj", "."]
RUN dotnet restore "./StudentService.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "StudentService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "StudentService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "StudentService.dll"]
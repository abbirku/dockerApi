FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build

WORKDIR /src
COPY ["Docker.Core/*.csproj", "Docker.Core/"]
COPY ["Docker.Infrastructure/*.csproj", "Docker.Infrastructure/"]
COPY ["Docker.WebApi/*.csproj", "Docker.WebApi/"]

RUN dotnet restore "Docker.WebApi/Docker.WebApi.csproj"
COPY . .
WORKDIR "/src/Docker.WebApi"

RUN dotnet build "Docker.WebApi.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Docker.WebApi.csproj" -c Release -o /app

FROM build AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Docker.WebApi.dll"]
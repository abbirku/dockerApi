FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build

WORKDIR /src
COPY ["DevTrack.Core/*.csproj", "DevTrack.Core/"]
COPY ["DevTrack.Infrastructure/*.csproj", "DevTrack.Infrastructure/"]
COPY ["DevTrack.WebApi/*.csproj", "DevTrack.WebApi/"]

RUN dotnet restore "DevTrack.WebApi/DevTrack.WebApi.csproj"
COPY . .
WORKDIR "/src/DevTrack.WebApi"

RUN dotnet build "DevTrack.WebApi.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "DevTrack.WebApi.csproj" -c Release -o /app

FROM build AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "DevTrack.WebApi.dll"]
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["DockerAPI/*.csproj", "DockerAPI/"]
RUN dotnet restore "DockerAPI/DockerAPI.csproj"
COPY . .
WORKDIR "/src/DockerAPI"
RUN dotnet build "DockerAPI.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "DockerAPI.csproj" -c Release -o /app

FROM build AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "DockerAPI.dll"]
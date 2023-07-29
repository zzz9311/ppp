FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/CommonBLL/CommonBLL.csproj", "CommonBLL/"]
RUN dotnet restore "src/CommonBLL/CommonBLL.csproj"
COPY . .
WORKDIR "/src/CommonBLL"
RUN dotnet build "CommonBLL.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CommonBLL.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CommonBLL.dll"]

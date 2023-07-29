FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/CommonBLL.Abstarctions/CommonBLL.Abstarctions.csproj", "CommonBLL.Abstarctions/"]
RUN dotnet restore "src/CommonBLL.Abstarctions/CommonBLL.Abstarctions.csproj"
COPY . .
WORKDIR "/src/CommonBLL.Abstarctions"
RUN dotnet build "CommonBLL.Abstarctions.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CommonBLL.Abstarctions.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CommonBLL.Abstarctions.dll"]

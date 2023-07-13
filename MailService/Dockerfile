FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["MailService/MailService.csproj", "MailService/"]
RUN dotnet restore "MailService/MailService.csproj"
COPY . .
WORKDIR "/src/MailService"
RUN dotnet build "MailService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MailService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MailService.dll"]

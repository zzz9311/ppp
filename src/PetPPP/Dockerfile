FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["PetPPP/PetPPP.csproj", "PetPPP/"]
RUN dotnet restore "PetPPP/PetPPP.csproj"
COPY . .
WORKDIR "/src/PetPPP"
RUN dotnet build "PetPPP.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PetPPP.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PetPPP.dll"]

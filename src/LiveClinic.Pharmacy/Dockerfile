FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["src/LiveClinic.Inventory/LiveClinic.Inventory.csproj", "LiveClinic.Inventory/"]
RUN dotnet restore "src/LiveClinic.Inventory/LiveClinic.Inventory.csproj"
COPY . .
WORKDIR "/src/LiveClinic.Inventory"
RUN dotnet build "LiveClinic.Inventory.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "LiveClinic.Inventory.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LiveClinic.Inventory.dll"]

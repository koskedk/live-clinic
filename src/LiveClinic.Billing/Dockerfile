FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["src/LiveClinic.Billing/LiveClinic.Billing.csproj", "LiveClinic.Billing/"]
RUN dotnet restore "src/LiveClinic.Billing/LiveClinic.Billing.csproj"
COPY . .
WORKDIR "/src/LiveClinic.Billing"
RUN dotnet build "LiveClinic.Billing.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "LiveClinic.Billing.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LiveClinic.Billing.dll"]

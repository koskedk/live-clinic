FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["src/LiveClinic.Ordering/LiveClinic.Ordering.csproj", "LiveClinic.Ordering/"]
RUN dotnet restore "src/LiveClinic.Ordering/LiveClinic.Ordering.csproj"
COPY . .
WORKDIR "/src/LiveClinic.Ordering"
RUN dotnet build "LiveClinic.Ordering.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "LiveClinic.Ordering.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LiveClinic.Ordering.dll"]

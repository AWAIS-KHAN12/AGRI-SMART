FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["AgriSmart.sln", "./"]
COPY ["AgriSmart.Web/AgriSmart.Web.csproj", "AgriSmart.Web/"]
RUN dotnet restore
COPY . .
WORKDIR "/src/AgriSmart.Web"
RUN dotnet publish "AgriSmart.Web.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_HTTP_PORTS=8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "AgriSmart.Web.dll"]

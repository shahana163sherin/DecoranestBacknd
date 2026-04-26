FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["DecoranestBacknd.csproj", "./"]
RUN dotnet restore "./DecoranestBacknd.csproj"

COPY . .
RUN dotnet publish "DecoranestBacknd.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

USER app

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "DecoranestBacknd.dll"]

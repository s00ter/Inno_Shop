FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS base
RUN apk add --no-cache icu-libs
RUN apk add --no-cache krb5-libs
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Inno_Shop.Api/Inno_Shop.AccountApi.csproj", "Inno_Shop.Api/"]
COPY ["Inno_Shop.BusinessLogic/Inno_Shop.BusinessLogic.csproj", "Inno_Shop.BusinessLogic/"]
COPY ["Inno_Shop.DataAccess/Inno_Shop.DataAccess.csproj", "Inno_Shop.DataAccess/"]
RUN dotnet restore "Inno_Shop.Api/Inno_Shop.AccountApi.csproj"
COPY . .
WORKDIR "/src/Inno_Shop.Api"
RUN dotnet build "Inno_Shop.AccountApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Inno_Shop.AccountApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final

ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=0

WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Inno_Shop.AccountApi.dll"]

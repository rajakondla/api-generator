# build stage
FROM mcr.microsoft.com/dotnet/core/sdk:2.2 as build-env
#FROM mcr.microsoft.com/dotnet/core/aspnet:2.2 AS base
WORKDIR /app

COPY api.csproj .
Run dotnet restore

COPY . .
Run dotnet publish -o /publish

# runtime stage
FROM mcr.microsoft.com/dotnet/core/aspnet:2.2 
WORKDIR /publish
COPY --from=build-env /publish .
ENTRYPOINT ["dotnet","/publish/api.dll"]

#FROM mcr.microsoft.com/dotnet/core/aspnet:2.2 AS base
#WORKDIR /app
#EXPOSE 5051
#FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build
#WORKDIR /src
#........
#
#RUN dotnet restore "Api.csproj"
#WORKDIR "/src/Api"
#COPY . .
#WORKDIR "/src/Api"
#RUN dotnet build "Api.csproj" -c Release -o /app
#
#FROM build AS publish
#RUN dotnet publish "Api.csproj" -c Release -o /app
#
#FROM base AS final
#WORKDIR /app
#COPY --from=publish /app .
#
#ENV ASPNETCORE_ENVIRONMENT DevStaging
#ENV ASPNETCORE_URLS=http://+:5051
#
#ENTRYPOINT ["dotnet", "Api.dll"]
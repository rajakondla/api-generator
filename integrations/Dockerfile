FROM mcr.microsoft.com/dotnet/core/sdk:2.2 as build-env
WORKDIR /integration
COPY integrations.csproj .
Run dotnet restore
COPY . .
ENTRYPOINT ["dotnet","test"]

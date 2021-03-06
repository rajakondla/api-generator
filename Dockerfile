# Build stage
FROM microsoft/dotnet:2.2-sdk AS build-env

WORKDIR /generator

#restore
COPY api/api.csproj ./api/
RUN dotnet restore api/api.csproj
COPY tests/tests.csproj ./tests/
RUN dotnet restore tests/tests.csproj

#copy src
COPY . .

#tests
ENV TEAMCITY_PROJECT_NAME=fake
RUN dotnet test tests/tests.csproj

#integration test
#RUN dotnet test integrations/integrations.csproj

#publish
RUN dotnet publish api/api.csproj -o /publish

#Runtime stage
FROM microsoft/dotnet:2.2-aspnetcore-runtime
COPY --from=build-env /publish /publish
WORKDIR /publish
ENTRYPOINT ["dotnet","api.dll"]
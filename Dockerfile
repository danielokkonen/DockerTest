FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /DockerTest
COPY . ./
RUN dotnet restore && dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /DockerTest
COPY --from=build-env /DockerTest/out .
ENTRYPOINT ["dotnet", "DockerTest.dll"]

EXPOSE 8080
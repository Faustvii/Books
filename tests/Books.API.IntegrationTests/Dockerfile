LABEL maintainer="Faust"

FROM microsoft/dotnet:2.0-runtime AS base
WORKDIR /app

FROM microsoft/dotnet:2.0-sdk AS build
WORKDIR /src
COPY Books.sln ./
COPY ../tests/Books.API.IntegrationTests/Books.API.IntegrationTests.csproj ../tests/Books.API.IntegrationTests/
COPY Books.API/Books.API.csproj Books.API/
COPY Books.CommandHandlers/Books.CommandHandlers.csproj Books.CommandHandlers/
COPY Books.EF/Books.EF.csproj Books.EF/
COPY Books.Models/Books.Models.csproj Books.Models/
RUN dotnet restore -nowarn:msb3202,nu1503
COPY . .
WORKDIR /src/../tests/Books.API.IntegrationTests
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Books.API.IntegrationTests.dll"]

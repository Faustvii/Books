FROM microsoft/aspnetcore:2.0 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:2.0 AS build
WORKDIR /src
COPY Books.API/Books.API.csproj Books.API/
COPY Books.CommandHandlers/Books.CommandHandlers.csproj Books.CommandHandlers/
COPY Books.EF/Books.EF.csproj Books.EF/
COPY Books.Models/Books.Models.csproj Books.Models/
RUN dotnet restore Books.API/Books.API.csproj
COPY . .
WORKDIR /src/Books.API
RUN dotnet build Books.API.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish Books.API.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Books.API.dll"]

FROM microsoft/aspnetcore:2.0 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:2.0 AS build
RUN mkdir -p /books
WORKDIR /books
#Restore packages
COPY ./src/*.sln /src/
COPY src/*/*.csproj ./
RUN for file in *.csproj; do mkdir -p /src/${file%.*}/ && mv $file /src/${file%.*}/; done

COPY ./tests/*/*.csproj ./
RUN for file in *.csproj; do mkdir -p /tests/${file%.*}/ && mv $file /tests/${file%.*}/; done

WORKDIR /src
RUN dotnet restore

# build api
COPY ./src/ .
# RUN ls -alR

WORKDIR /src/Books.API
RUN dotnet publish --no-restore Books.API.csproj -c Release -o /app

# tests - build the project directory structure
WORKDIR /books
COPY ./src/ ./src/
COPY ./tests/ ./tests/
RUN dotnet test ./tests/Books.API.IntegrationTests/Books.API.IntegrationTests.csproj

FROM base AS final
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "Books.API.dll"]

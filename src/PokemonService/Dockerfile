# Baseimage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build 

# Set working directory as app 
WORKDIR /app 

# Expose port 80 in the container 
EXPOSE 80

# ----------------------------------------------------------------------------------------------------
# CACHED LAYER for all subsequent docker files

# Copy all .csproj files and restore distinct layers. Use of same Copy command for every dockerfile 
# in the project to take advantage of docker caching

# OBS -> All of these are not transfered to the targeted service, because we switch image later on to runtime image

COPY PokemonStore.sln PokemonStore.sln
COPY src/PokemonService/PokemonService.csproj src/PokemonService/PokemonService.csproj
COPY src/SearchService/SearchService.csproj src/SearchService/SearchService.csproj
COPY src/IdentityService/IdentityService.csproj src/IdentityService/IdentityService.csproj
COPY src/GatewayService/GatewayService.csproj src/GatewayService/GatewayService.csproj
COPY src/CartService/CartService.csproj src/CartService/CartService.csproj
COPY src/Contracts/Contracts.csproj src/Contracts/Contracts.csproj

RUN dotnet restore PokemonStore.sln

# ----------------------------------------------------------------------------------------------------

# Copy over (service/app) files
COPY src/PokemonService src/PokemonService
COPY src/Contracts src/Contracts

# Generate published version of the PokemonService
WORKDIR /app/src/PokemonService
RUN dotnet publish -c Release -o /app/src/out

# Build runtime image, smaller image than sdk which is not needed for final image 
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

# Change Working directory in runtime image
WORKDIR /app

# Copy published version of Pokemon Service located in build in /app/src/out into our runtime image and directory /app
COPY --from=build /app/src/out .

ENTRYPOINT [ "dotnet", "PokemonService.dll" ]
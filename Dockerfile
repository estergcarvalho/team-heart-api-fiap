# Imagem base
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Imagem para build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia o csproj
COPY TeamHeartFiap.csproj .

# Restaura dependências
RUN dotnet restore "TeamHeartFiap.csproj"

# Copia todo o conteúdo da API
COPY . .

# Compila
RUN dotnet publish -c Release -o /app/publish

# Imagem final
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "TeamHeartFiap.dll"]

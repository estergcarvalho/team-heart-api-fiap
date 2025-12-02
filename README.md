# team-heart-api-fiap

# TeamHeartFiap - API de Treinamentos

API em .NET 8 para gerenciamento de treinamentos, com Oracle, testes automatizados e Swagger.

---

## Estrutura

TeamHeartFiap/
├─ Controllers/
├─ Domain/
├─ Infrastructure/
├─ ViewModels/
├─ TeamHeartFiap.csproj
└─ TeamHeartFiap.Tests/

## Pré-requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Docker + Docker Compose
- SQL Developer (Oracle)

---

## Rodando localmente

```bash
# Restaurar pacotes e compilar
dotnet restore
dotnet build

# Rodar a API
dotnet run --project TeamHeartFiap

# Rodar os testes
dotnet test TeamHeartFiap.Tests

## Swagger:
http://localhost:5194/swagger/index.html

## Banco Oracle:
Usuário: RM560895
Senha: Fiap25
Data Source: oracle:1521/XE

## Rodar via Docker Compose:
docker compose up --build
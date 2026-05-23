## Sobre o projeto

Esta **API**, desenvolvida com **.NET 8**, segue princípios de **Domain-Driven Design (DDD)** para oferecer uma solução organizada e escalável de gestão de faturamentos para barbearias.

Além do CRUD de faturamentos, o projeto agora possui **autenticação completa com JWT**, incluindo cadastro de usuários, login, gerenciamento de perfil e proteção de rotas por autorização.

A arquitetura é dividida em camadas independentes:

- **API**
- **Application**
- **Communication**
- **Domain**
- **Infrastructure**
- **Exception**

Também utiliza:

- **Entity Framework Core** + **Pomelo MySQL** para persistência
- **FluentValidation** para validações
- **AutoMapper** para mapeamento
- **Swagger** para documentação interativa
- **ClosedXML** e **PDFsharp/MigraDoc** para relatórios

## Features

- Arquitetura em camadas com DDD.
- Autenticação com JWT (`Bearer Token`).
- Cadastro de usuário com retorno de token.
- Login com retorno de token.
- Gestão de perfil do usuário autenticado:
- `GET /api/User`
- `PUT /api/User`
- `PUT /api/User/change-password`
- `DELETE /api/User`
- CRUD de faturamentos protegido por autenticação.
- **Faturamentos vinculados ao usuário logado** (isolamento por usuário).
- Paginação na listagem de faturamentos.
- Relatórios semanais em PDF e Excel.
- Autorização por role para relatórios (`administrator`).
- Tratamento global de exceções.
- Testes automatizados unitários, de validação e integração/Web API.

## Estrutura da solução

```text
BarberBoss
 ┣ src
 ┃ ┣ BarberBoss.Api
 ┃ ┣ BarberBoss.Application
 ┃ ┣ BarberBoss.Communication
 ┃ ┣ BarberBoss.Domain
 ┃ ┣ BarberBoss.Exception
 ┃ ┗ BarberBoss.Infrastructure
 ┗ tests
   ┣ CommonTestUtilities
   ┣ UseCases.Tests
   ┣ Validators.Tests
   ┗ WebApi.Test
```

## Construído com

![badge-dot-net]
![badge-csharp]
![badge-mysql]
![badge-swagger]
![badge-entityframework]
![badge-xunit]
![badge-visual-studio]

## Endpoints principais

### Autenticação e usuário

- `POST /api/User` - cadastro de usuário (retorna `name` e `token`).
- `POST /api/Login` - login (retorna `name` e `token`).
- `GET /api/User` - consulta perfil do usuário autenticado.
- `PUT /api/User` - atualiza perfil do usuário autenticado.
- `PUT /api/User/change-password` - altera senha.
- `DELETE /api/User` - remove conta do usuário autenticado.

### Faturamentos (rota protegida)

Todas as rotas de faturamento exigem token JWT.

- `POST /api/Billings` - cadastra faturamento para o usuário logado.
- `GET /api/Billings?page=1&pageSize=10` - lista faturamentos do usuário logado com paginação.
- `GET /api/Billings/{id}` - busca faturamento do usuário logado por ID.
- `PUT /api/Billings/{id}` - atualiza faturamento do usuário logado.
- `DELETE /api/Billings/{id}` - remove faturamento do usuário logado.

### Relatórios (rota protegida + role)

As rotas de relatório exigem usuário autenticado com role `administrator`.

- `GET /api/Report/pdf?date=2026-03-16` - gera relatório semanal em PDF.
- `GET /api/Report/excel` com header `date` - gera relatório semanal em Excel.

## Regras de negócio observadas

### Usuários e autenticação

- Nome obrigatório no cadastro.
- E-mail obrigatório e em formato válido.
- E-mail único por usuário.
- Senha obrigatória com regra de complexidade:
- mínimo de 8 caracteres
- ao menos 1 letra maiúscula
- ao menos 1 letra minúscula
- ao menos 1 número
- ao menos 1 caractere especial entre `! ? * .`
- Token JWT com claims de nome, identificador do usuário e role.

### Faturamentos

- Data obrigatória.
- Nome do barbeiro obrigatório (2 a 80 caracteres).
- Nome do cliente obrigatório (2 a 120 caracteres).
- Nome do serviço obrigatório (2 a 120 caracteres).
- Valor maior ou igual a zero.
- Quando status for `Cancelado`, valor deve ser `0`.
- Forma de pagamento deve ser válida no enum.
- Status deve ser válido no enum.
- Observações com máximo de 500 caracteres.
- Operações de faturamento respeitam o usuário autenticado (não acessa dados de outro usuário).

## Getting Started

### Requisitos

- **.NET SDK 8.0**
- **MySQL Server**
- **Visual Studio 2022+** ou **Visual Studio Code**

### Instalação

1. Clone o repositório:

```sh
git clone https://github.com/matheusdamacena593/barberboss-api.git
```

2. Acesse a pasta do projeto:

```sh
cd barberboss-api
```

3. Configure `ConnectionStrings` e `Settings:Jwt` em `src/BarberBoss.Api/appsettings.Development.json`:

```json
{
  "ConnectionStrings": {
    "Connection": "Server=localhost;Database=barberboss_db;Uid=root;Pwd=123456;"
  },
  "Settings": {
    "Jwt": {
      "SigningKey": "SUA_CHAVE_FORTE_AQUI",
      "ExpiresMinutes": 1000
    }
  }
}
```

4. Restaure os pacotes:

```sh
dotnet restore
```

5. Execute a aplicação:

```sh
dotnet run --project src/BarberBoss.Api
```

6. Abra o Swagger e autentique com `Bearer {token}`.

## Testes

Para executar todos os testes:

```sh
dotnet test
```

A solução possui testes para:

- Use cases.
- Validadores.
- Endpoints da Web API (integração).

## Exemplo de payload

### Cadastro de usuário

```json
{
  "name": "Matheus Damacena",
  "email": "matheus@email.com",
  "password": "Senha@123"
}
```

### Login

```json
{
  "email": "matheus@email.com",
  "password": "Senha@123"
}
```

### Faturamento

```json
{
  "date": "2026-03-16T14:30:00",
  "barberName": "Matheus Damacena",
  "clientName": "João da Silva",
  "serviceName": "Corte + Barba",
  "amount": 70.00,
  "paymentMethod": 3,
  "status": 0,
  "notes": "Cliente preferiu atendimento rápido e pagou via Pix."
}
```

## Autor

Desenvolvido por **Matheus Damacena**

[LinkedIn][linkedin]

mateusdamacena593@gmail.com

<!-- Links -->
[dot-net-sdk]: https://dotnet.microsoft.com/en-us/download/dotnet/8.0
[linkedin]: https://www.linkedin.com/in/matheus-damacena-carvalho-19bb74255/

<!-- Images -->
[hero-image]: images/heroimage.png

<!-- Badges -->
[badge-dot-net]: https://img.shields.io/badge/.NET-512BD4?logo=dotnet&logoColor=fff&style=for-the-badge
[badge-csharp]: https://img.shields.io/badge/C%23-239120?logo=csharp&logoColor=fff&style=for-the-badge
[badge-mysql]: https://img.shields.io/badge/MySQL-4479A1?logo=mysql&logoColor=fff&style=for-the-badge
[badge-swagger]: https://img.shields.io/badge/Swagger-85EA2D?logo=swagger&logoColor=000&style=for-the-badge
[badge-entityframework]: https://img.shields.io/badge/Entity%20Framework%20Core-512BD4?logo=nuget&logoColor=fff&style=for-the-badge
[badge-xunit]: https://img.shields.io/badge/xUnit-5C2D91?logo=dotnet&logoColor=fff&style=for-the-badge
[badge-visual-studio]: https://img.shields.io/badge/Visual%20Studio-5C2D91?logo=visualstudio&logoColor=fff&style=for-the-badge

## Sobre o projeto

Esta **API**, desenvolvida com **.NET 8**, foi estruturada com base nos princípios de **Domain-Driven Design (DDD)** para oferecer uma solução organizada e escalável no gerenciamento de **faturamentos de uma barbearia**. O objetivo principal do projeto é permitir o cadastro, consulta, atualização e remoção de faturamentos, registrando informações como barbeiro, cliente, serviço prestado, data e hora, valor, forma de pagamento, status e observações, com persistência em banco de dados **MySQL**.

A aplicação segue o padrão **REST**, utilizando métodos **HTTP** convencionais para disponibilizar seus recursos de forma clara e padronizada. Além disso, conta com **Swagger** para documentação e testes interativos dos endpoints, facilitando o consumo da API durante o desenvolvimento e a integração com clientes externos.

A solução foi dividida em camadas independentes, separando responsabilidades entre **API**, **Application**, **Communication**, **Domain**, **Infrastructure** e **Exception**, o que torna o projeto mais limpo, testável e de fácil manutenção. Na camada de aplicação, o **AutoMapper** é utilizado para o mapeamento entre entidades e objetos de requisição/resposta. O **FluentValidation** centraliza as regras de validação de entrada para garantir a consistência dos dados. O **Entity Framework Core** com **Pomelo para MySQL** é responsável pelo acesso aos dados. Para a exportação de relatórios, o projeto utiliza **ClosedXML** para geração de arquivos **Excel** e **PDFsharp/MigraDoc** para a construção de relatórios em **PDF**.

Atualmente, a API também oferece a **geração de relatórios semanais em PDF e Excel**, permitindo visualizar o faturamento do período de forma prática e organizada.

### Features

- **Arquitetura em camadas com DDD**: separação clara entre domínio, regras de negócio, contratos, infraestrutura e API.

- **CRUD de faturamentos**: cadastro, listagem paginada, consulta por identificador, atualização e remoção de registros.

- **Validação de dados com FluentValidation**: regras para campos obrigatórios, tamanhos mínimos e máximos, enums válidos e consistência de valor/status.

- **Paginação na listagem**: retorno estruturado com página atual, tamanho da página, total de itens e total de páginas.

- **Geração de relatórios semanais**: exportação dos faturamentos do período em **PDF** e **Excel**.

- **Documentação com Swagger**: interface interativa para explorar e testar os endpoints da API.

- **Tratamento global de exceções**: filtro centralizado para padronização das respostas de erro da aplicação.

- **Testes automatizados**: projeto de testes com **xUnit**, **FluentAssertions**, **Shouldly** e utilitários de geração de dados com **Bogus**.

### Estrutura da solução

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
   ┗ Validators.Tests
```

### Construído com

![badge-dot-net]
![badge-csharp]
![badge-mysql]
![badge-swagger]
![badge-entityframework]
![badge-xunit]
![badge-visual-studio]

## Endpoints principais

### Faturamentos

- `POST /api/Billings` — cadastra um novo faturamento
- `GET /api/Billings?page=1&pageSize=10` — lista faturamentos com paginação
- `GET /api/Billings/{id}` — busca um faturamento por identificador
- `PUT /api/Billings/{id}` — atualiza um faturamento existente
- `DELETE /api/Billings/{id}` — remove um faturamento

### Relatórios

- `GET /api/Report/pdf?date=2026-03-16` — gera relatório semanal em PDF a partir de uma data de referência
- `GET /api/Report/excel` com header `date` — gera relatório semanal em Excel

## Regras de negócio observadas

Algumas validações já implementadas na API:

- **Data** obrigatória
- **Nome do barbeiro** obrigatório, com mínimo de 2 e máximo de 80 caracteres
- **Nome do cliente** obrigatório, com mínimo de 2 e máximo de 120 caracteres
- **Nome do serviço** obrigatório, com mínimo de 2 e máximo de 120 caracteres
- **Valor** deve ser maior ou igual a zero
- Quando o status for **Cancelado**, o valor deve ser **0**
- **Forma de pagamento** deve ser um valor válido do enum
- **Status** deve ser um valor válido do enum
- **Observações** podem ter no máximo 500 caracteres

## Getting Started

Para obter uma cópia local funcionando, siga estes passos simples.

### Requisitos

* **.NET SDK 8.0** instalado
* **MySQL Server**
* **Visual Studio 2022+** ou **Visual Studio Code**

### Instalação

1. Clone o repositório:
    ```sh
    git clone https://github.com/matheusdamacena593/barberboss-api.git
    ```

2. Acesse a pasta do projeto:
    ```sh
    cd barberboss-api
    ```

3. Configure a string de conexão no arquivo `src/BarberBoss.Api/appsettings.Development.json`:
    ```json
    {
      "ConnectionStrings": {
        "Connection": "Server=localhost;Database=barberboss_db;Uid=root;Pwd=123456;"
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

6. Acesse o Swagger no navegador para testar os endpoints.

## Testes

Para executar os testes automatizados do projeto:

```sh
dotnet test
```

Atualmente, a solução possui projeto de testes voltado para validação das regras de entrada dos faturamentos, além de utilitários compartilhados para geração de dados de teste.

## Exemplo de payload

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

## 👨‍💻 Autor

Desenvolvido por **Matheus Damacena**

[LinkedIn][linkedin]

mateusdamacena593@gmail.com

Sinta-se à vontade para entrar em contato para oportunidades, colaborações ou networking na área de desenvolvimento **.NET**, arquitetura de software e construção de APIs escaláveis.

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

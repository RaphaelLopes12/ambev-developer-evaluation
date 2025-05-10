# Ambev Developer Evaluation Project

Este projeto é uma implementação de uma API para gerenciamento de vendas, seguindo os requisitos do desafio de avaliação da Ambev.

## 🔍 Visão Geral

O sistema implementa uma API completa para registro e gerenciamento de vendas, utilizando padrões modernos de desenvolvimento como DDD (Domain-Driven Design), CQRS (Command Query Responsibility Segregation) via MediatR, e o padrão Repository. A solução foi construída com atenção especial à separação de responsabilidades, testabilidade e manutenibilidade.

### Requisitos do Negócio Implementados

- ✅ Registro completo de vendas incluindo:
  - Número de venda
  - Data da venda
  - Cliente
  - Valor total da venda
  - Filial onde a venda foi realizada
  - Produtos
  - Quantidades
  - Preços unitários
  - Descontos
  - Valor total por item
  - Estado (Cancelada/Não Cancelada)

- ✅ Cálculo automático de descontos baseado em regras de negócio:
  - 10% de desconto para compras com 4 ou mais itens idênticos
  - 20% de desconto para compras entre 10 e 20 itens idênticos
  - Limite máximo de 20 itens idênticos por produto
  - Sem desconto para compras com menos de 4 itens

- ✅ Publicação de eventos para integrações:
  - SaleCreated
  - SaleModified
  - SaleCancelled
  - ItemCancelled
  Assim como demais eventos dos outros fluxos.

## 🏗️ Arquitetura

O sistema foi construído seguindo uma arquitetura em camadas, organizada com base nos princípios de Clean Architecture e DDD:

```
Ambev.DeveloperEvaluation/
├── src/
│   ├── Ambev.DeveloperEvaluation.WebApi/       # API e Controllers
│   ├── Ambev.DeveloperEvaluation.Application/  # Casos de uso e lógica de aplicação
│   ├── Ambev.DeveloperEvaluation.Domain/       # Entidades, regras de negócio e interfaces
│   ├── Ambev.DeveloperEvaluation.Common/       # Componentes compartilhados
│   ├── Ambev.DeveloperEvaluation.ORM/          # Implementação do acesso a dados
│   ├── Ambev.DeveloperEvaluation.IoC/          # Configuração de Injeção de Dependências
└── tests/
    ├── Ambev.DeveloperEvaluation.Unit/         # Testes unitários
    ├── Ambev.DeveloperEvaluation.Integration/  # Testes de integração
    └── Ambev.DeveloperEvaluation.Functional/   # Testes funcionais
```

### Persistência de Dados

O sistema utiliza duas tecnologias de banco de dados para atender a diferentes necessidades:

- **PostgreSQL**: Armazena as entidades principais do sistema
  - Entidades: Sale, SaleItem, Branch, Product, User
  - Vantagens: Forte consistência, integridade referencial, transações ACID
  - Usado para: Dados transacionais e entidades com relacionamentos complexos

- **MongoDB**: Armazena informações de clientes (Customer)
  - Vantagens: Flexibilidade de esquema, melhor desempenho para consultas específicas
  - Utilização: Implementação do padrão External Identities, permitindo referenciar entidades de outros domínios com denormalização de descrições

Esta abordagem híbrida demonstra a implementação do padrão External Identities mencionado nos requisitos, onde referenciamos entidades de outros domínios mantendo uma cópia denormalizada de suas descrições.

## 🛠️ Tecnologias Utilizadas

### Backend
- **.NET 8.0** e **C#** - Plataforma e linguagem de desenvolvimento
- **ASP.NET Core** - Framework web
- **Entity Framework Core** - ORM para PostgreSQL
- **MongoDB Driver** - Cliente oficial para MongoDB

### Frameworks e Bibliotecas
- **MediatR** - Implementação do padrão Mediator para CQRS
- **AutoMapper** - Mapeamento entre objetos
- **FluentValidation** - Validação de comandos e entidades
- **Serilog** - Logging estruturado
- **Swagger/OpenAPI** - Documentação da API

### Bancos de Dados
- **PostgreSQL** - Banco de dados relacional
- **MongoDB** - Banco de dados NoSQL
- **Redis** - Cache distribuído (opcional)

### Testes
- **xUnit** - Framework de testes
- **NSubstitute** - Biblioteca de mocking
- **FluentAssertions** - Assertions expressivas
- **Bogus** - Geração de dados de teste

### Infraestrutura
- **Docker** e **Docker Compose** - Containerização
- **JWT** - Autenticação e autorização

## 🚀 Como Executar o Projeto

### Pré-requisitos
- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Docker](https://www.docker.com/products/docker-desktop/)
- [Docker Compose](https://docs.docker.com/compose/install/)

### Configuração e Execução usando Docker

1. Clone o repositório:
   ```bash
   git clone https://github.com/seu-usuario/ambev-developer-evaluation.git
   cd ambev-developer-evaluation
   ```

2. Execute o Docker Compose para iniciar todos os serviços:
   ```bash
   docker-compose up -d
   ```

   Isso iniciará:
   - API ASP.NET Core (porta 8080/8081)
   - PostgreSQL (porta 5432)
   - MongoDB (porta 27017)
   - Redis (porta 6379)

3. Acesse a API:
   - Swagger UI: http://localhost:8080/swagger
   - A API estará disponível em: http://localhost:8080/api

### Configuração e Execução Manual (sem Docker)

1. Configure as strings de conexão:
   
   Abra o arquivo `src/Ambev.DeveloperEvaluation.WebApi/appsettings.json` e atualize as strings de conexão:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Host=localhost;Port=5432;Database=ambev_store;Username=ambev_user;Password=YourStrongPassword123;TrustServerCertificate=True"
     },
     "MongoDbSettings": {
       "ConnectionString": "mongodb://ambev_user:YourStrongPassword123@localhost:27017/ambev_store_customers?authSource=admin",
       "DatabaseName": "ambev_store_customers"
     },
     "Redis": {
       "ConnectionString": "localhost:6379,password=YourStrongPassword123"
     }
   }
   ```

2. Restaure os pacotes e compile o projeto:
   ```bash
   dotnet restore
   dotnet build
   ```

3. Execute as migrações do banco de dados PostgreSQL:
   ```bash
   cd src/Ambev.DeveloperEvaluation.ORM
   dotnet ef database update
   ```

4. Execute a aplicação:
   ```bash
   cd ../Ambev.DeveloperEvaluation.WebApi
   dotnet run
   ```

5. Acesse a API:
   - Swagger UI: http://localhost:5000/swagger
   - A API estará disponível em: http://localhost:5000/api

## 🧪 Testes

O projeto inclui três tipos de testes:

### Testes Unitários
Testes isolados para domínio e lógica de aplicação. Estes testes não acessam bancos de dados ou serviços externos.

```bash
dotnet test tests/Ambev.DeveloperEvaluation.Unit
```

### Testes de Integração
Testes que verificam a integração entre componentes, como repositórios e bancos de dados.

```bash
dotnet test tests/Ambev.DeveloperEvaluation.Integration
```

### Testes Funcionais
Testes de ponta a ponta que verificam o comportamento da API.

```bash
dotnet test tests/Ambev.DeveloperEvaluation.Functional
```

## 📚 API Endpoints

A API implementa os seguintes endpoints principais:

### Autenticação
- `POST /api/auth` - Autentica um usuário e retorna um token JWT

### Usuários
- `POST /api/users` - Cria um novo usuário
- `GET /api/users/{id}` - Obtém um usuário específico
- `DELETE /api/users/{id}` - Remove um usuário

### Vendas
- `POST /api/sales` - Cria uma nova venda
- `GET /api/sales` - Lista todas as vendas com paginação (requer permissão de gerente)
- `GET /api/sales/{id}` - Obtém uma venda específica
- `PUT /api/sales/{id}` - Atualiza uma venda existente
- `PATCH /api/sales/{id}/cancel` - Cancela uma venda (requer permissão de gerente)
- `PATCH /api/sales/{saleId}/items/{productId}/cancel` - Cancela um item de venda (requer permissão de gerente)
- `DELETE /api/sales/{id}` - Remove uma venda (requer permissão de administrador)

### Produtos
- `POST /api/products` - Adiciona um novo produto (requer permissão de gerente)
- `GET /api/products` - Lista todos os produtos com paginação e ordenação
- `GET /api/products/{id}` - Obtém um produto específico
- `GET /api/products/categories` - Lista todas as categorias de produtos
- `GET /api/products/category/{category}` - Lista produtos por categoria
- `PUT /api/products/{id}` - Atualiza um produto existente (requer permissão de gerente)
- `PUT /api/products/{id}/stock` - Atualiza o estoque de um produto
- `POST /api/products/{id}/rating` - Adiciona uma avaliação a um produto
- `DELETE /api/products/{id}` - Remove um produto (requer permissão de gerente)

### Filiais
- `POST /api/branches` - Adiciona uma nova filial (requer permissão de gerente)
- `GET /api/branches` - Lista todas as filiais
- `GET /api/branches/{id}` - Obtém uma filial específica
- `GET /api/branches/active` - Lista todas as filiais ativas
- `PUT /api/branches/{id}` - Atualiza uma filial existente (requer permissão de gerente)
- `PATCH /api/branches/{id}/activate` - Ativa uma filial (requer permissão de gerente)
- `PATCH /api/branches/{id}/deactivate` - Desativa uma filial (requer permissão de gerente)
- `DELETE /api/branches/{id}` - Remove uma filial (requer permissão de gerente)

## 🔒 Autenticação e Autorização

O sistema utiliza autenticação baseada em JWT (JSON Web Tokens) e implementa as seguintes políticas de autorização:

- **RequireAdminRole**: Acesso restrito a administradores
- **RequireManagerRole**: Acesso para gerentes e administradores
- **RequireCustomerRole**: Acesso para clientes, gerentes e administradores

Alguns endpoints são marcados com `[AllowAnonymous]` para permitir acesso público, como consulta de produtos e filiais.

## 📝 Regras de Negócio Implementadas

### Descontos
- Compras com 4 ou mais itens idênticos recebem 10% de desconto
- Compras entre 10 e 20 itens idênticos recebem 20% de desconto
- Não é possível vender mais de 20 itens idênticos em uma única venda
- Compras com menos de 4 itens não recebem desconto

### Eventos
Os seguintes eventos são publicados para integração:
- `SaleCreated` - Quando uma nova venda é criada
- `SaleModified` - Quando uma venda é modificada
- `SaleCancelled` - Quando uma venda é cancelada
- `ItemCancelled` - Quando um item de venda é cancelado

## 🧱 Estrutura do Projeto

### Domain
Contém as entidades de domínio, interfaces de repositório e regras de negócio:
- `Entities` - Classes de entidade (Sale, SaleItem, Product, Branch, Customer, User)
- `Repositories` - Interfaces de repositório
- `Exceptions` - Exceções de domínio personalizadas
- `Events` - Definições de eventos de domínio
- `Enums` - Enumerações de domínio

### Application
Implementa os casos de uso da aplicação usando o padrão CQRS:
- `Sales` - Comandos e consultas relacionados a vendas
- `Products` - Comandos e consultas relacionados a produtos
- `Branches` - Comandos e consultas relacionados a filiais
- `Customers` - Comandos e consultas relacionados a clientes
- `Users` - Comandos e consultas relacionados a usuários
- `Auth` - Comandos para autenticação

### WebApi
Expõe a API REST e configura a aplicação:
- `Controllers` - Controladores da API
- `Middleware` - Middlewares personalizados
- `Filters` - Filtros de ação e exceção
- `Features` - Organização dos recursos por funcionalidade

### ORM
Implementa o acesso a dados para PostgreSQL usando Entity Framework Core:
- `Repositories` - Implementações de repositório
- `Configurations` - Configurações de mapeamento de entidades
- `Migrations` - Migrações de banco de dados

### MongoDB
Implementa o acesso a dados para MongoDB:
- `Repositories` - Implementações de repositório para entidades NoSQL
- `Configurations` - Configurações de mapeamento de documentos

## 📋 Considerações de Design

### Padrão External Identities
Implementamos o padrão External Identities conforme solicitado nos requisitos, usando MongoDB para armazenar clientes e PostgreSQL para entidades principais. Isso demonstra como referenciar entidades de outros domínios com denormalização das descrições.

### CQRS com MediatR
Utilizamos o padrão CQRS para separar as operações de leitura e escrita, o que melhora a manutenibilidade e testabilidade do código. O MediatR facilita a implementação desse padrão e permite a adição de comportamentos transversais como validação e logging.

### Validação com FluentValidation
Todas as entradas são validadas usando FluentValidation antes de serem processadas pelos handlers, garantindo consistência dos dados. Os validadores são implementados como classes separadas por request, facilitando a manutenção e testabilidade.

### Respostas Padronizadas
A API retorna respostas padronizadas usando classes como `ApiResponse` e `ApiResponseWithData<T>`, garantindo consistência na interface com o cliente.

### Testes Abrangentes
Implementamos três níveis de testes (unitários, integração e funcionais) para garantir a qualidade do código e o comportamento correto da aplicação.

## 🤝 Contribuição

Para contribuir com o projeto, siga estas etapas:

1. Faça um fork do repositório
2. Crie uma branch para sua feature: `git checkout -b feature/nova-feature`
3. Commit suas mudanças: `git commit -m 'feat: Adiciona nova feature'`
4. Push para a branch: `git push origin feature/nova-feature`
5. Abra um Pull Request

## 📄 Licença

Este projeto está licenciado sob a Licença MIT - veja o arquivo [LICENSE](LICENSE) para detalhes.

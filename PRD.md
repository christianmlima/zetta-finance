# PRD — Zetta Finance

**Versão:** 1.0  
**Data:** 2026-04-11  
**Status:** Em planejamento

---

## 1. Visão Geral

Sistema de gestão financeira pessoal/empresarial com foco em controle de receitas, despesas, categorias, contas e relatórios. Arquitetura moderna e escalável, pronta para evolução.

---

## 2. Funcionalidades

### 2.1 MVP (Fase 1)

#### Autenticação e Usuários
- Registro e login com JWT (access token + refresh token)
- Perfil do usuário (nome, e-mail, avatar)
- Troca de senha

#### Contas Financeiras
- CRUD de contas (corrente, poupança, carteira, investimento)
- Saldo atual calculado a partir das transações
- Suporte a múltiplas contas por usuário

#### Transações
- CRUD de transações (receita / despesa / transferência)
- Campos: valor, data, descrição, conta, categoria, tags
- Filtros: período, tipo, categoria, conta, valor
- Paginação e ordenação

#### Categorias
- CRUD de categorias com ícone e cor
- Hierarquia: categoria pai / subcategoria
- Categorias padrão + categorias personalizadas por usuário

#### Dashboard
- Saldo total consolidado
- Resumo do mês atual (receitas, despesas, saldo)
- Gráfico de fluxo mensal (receitas x despesas)
- Top 5 categorias de despesa

### 2.2 Fase 2

#### Orçamentos
- Definição de limite mensal por categoria
- Alertas ao atingir percentual do limite (ex: 80%)
- Histórico de orçamentos

#### Metas Financeiras
- Criação de metas com valor alvo e prazo
- Progresso em tempo real
- Vinculação de transações a metas

#### Relatórios
- Extrato detalhado por período
- Análise de gastos por categoria (pizza / barras)
- Evolução patrimonial (linha do tempo)
- Exportação CSV/PDF (fase futura)

#### Notificações
- Alertas de orçamento excedido
- Lembrete de contas a pagar
- Resumo semanal por e-mail (ou in-app)

### 2.3 Fase 3 (Futuro)
- Importação de extratos OFX/CSV
- Conciliação automática
- Multi-tenancy (equipes/empresas)
- Módulo de investimentos
- Integração com Open Finance (Brasil)

---

## 3. Arquitetura

### 3.1 Visão Macro

```
┌─────────────────────────────────────────────────────────┐
│                        Frontend                          │
│            React + TypeScript + Vite + Zustand           │
└─────────────────────────┬───────────────────────────────┘
                          │ HTTP / REST
┌─────────────────────────▼───────────────────────────────┐
│                      API Gateway                         │
│               .NET 10 Minimal API + JWT                  │
├──────────────────────────────────────────────────────────┤
│  Web Layer (Controllers/Endpoints, Middlewares, DTOs)    │
├──────────────────────────────────────────────────────────┤
│  Application Layer (CQRS — Commands, Queries, Handlers)  │
├──────────────────────────────────────────────────────────┤
│  Domain Layer (Entities, Value Objects, Domain Events,   │
│               Aggregates, Interfaces, Specs)             │
├──────────────────────────────────────────────────────────┤
│  Infrastructure Layer (EF Core, Redis, Repositories,     │
│                        MassTransit/RabbitMQ, Email)      │
└──────────────────────────────────────────────────────────┘
         │                          │
   ┌─────▼──────┐           ┌───────▼──────┐
   │ PostgreSQL │           │    Redis      │
   └────────────┘           └──────────────┘
                                    │
                          ┌─────────▼────────┐
                          │    RabbitMQ       │
                          │  (MassTransit)    │
                          └──────────────────┘
```

### 3.2 Estrutura de Pastas — Backend

```
src/
├── Zetta.Api/                    # Web Layer
│   ├── Endpoints/                # Minimal API endpoints por feature
│   ├── Middlewares/              # Error handling, logging, auth
│   ├── Extensions/               # DI registration, pipeline config
│   └── Program.cs
│
├── Zetta.Application/            # Application Layer
│   ├── Features/
│   │   ├── Transactions/
│   │   │   ├── Commands/         # CreateTransaction, UpdateTransaction...
│   │   │   ├── Queries/          # GetTransactions, GetTransactionById...
│   │   │   └── Validators/       # FluentValidation
│   │   ├── Accounts/
│   │   ├── Categories/
│   │   ├── Budgets/
│   │   └── Auth/
│   ├── Common/
│   │   ├── Behaviors/            # Validation, Logging, Caching (pipeline)
│   │   └── Abstractions/
│   └── Mappings/                 # Mapster profiles
│
├── Zetta.Domain/                 # Domain Layer
│   ├── Aggregates/
│   │   ├── Transactions/         # Transaction aggregate root
│   │   ├── Accounts/
│   │   ├── Budgets/
│   │   └── Users/
│   ├── Entities/
│   ├── ValueObjects/             # Money, DateRange, Color...
│   ├── Enums/
│   ├── Events/                   # Domain events
│   ├── Interfaces/               # Repository contracts
│   ├── Exceptions/               # Domain exceptions
│   └── Specifications/           # Spec pattern para queries
│
├── Zetta.Infrastructure/         # Infrastructure Layer
│   ├── Persistence/
│   │   ├── AppDbContext.cs
│   │   ├── Configurations/       # EF Core Fluent API
│   │   ├── Repositories/
│   │   └── Migrations/
│   ├── Caching/                  # Redis (IDistributedCache)
│   ├── Messaging/                # MassTransit consumers e publishers
│   ├── Email/                    # SMTP / SendGrid
│   └── Identity/                 # JWT, password hashing
│
└── Zetta.SharedKernel/           # Tipos compartilhados
    ├── Result.cs                 # Result<T> pattern
    ├── Error.cs
    ├── BaseEntity.cs
    ├── AggregateRoot.cs
    └── DomainEvent.cs
```

### 3.3 Estrutura de Pastas — Frontend

```
frontend/
├── src/
│   ├── app/
│   │   ├── routes/               # React Router v7 routes
│   │   ├── providers/            # QueryClient, Theme, Auth
│   │   └── App.tsx
│   ├── features/                 # Feature slices
│   │   ├── auth/
│   │   ├── dashboard/
│   │   ├── transactions/
│   │   ├── accounts/
│   │   ├── categories/
│   │   └── budgets/
│   ├── shared/
│   │   ├── components/           # UI components reutilizáveis
│   │   ├── hooks/
│   │   ├── api/                  # axios client + interceptors
│   │   └── utils/
│   └── store/                    # Zustand stores globais
├── public/
├── index.html
├── vite.config.ts
├── tailwind.config.ts
└── tsconfig.json
```

---

## 4. Stack Tecnológica

### 4.1 Backend

| Tecnologia | Versão | Uso |
|---|---|---|
| .NET | 10 | Runtime / SDK |
| ASP.NET Core Minimal API | 10 | Camada Web |
| Entity Framework Core | 9+ | ORM |
| PostgreSQL | 16 | Banco de dados principal |
| Redis | 7 | Cache distribuído |
| RabbitMQ | 3.13 | Message broker |
| MassTransit | 8+ | Abstração de mensageria |
| MediatR | 12 | CQRS / Mediator pattern |
| FluentValidation | 11 | Validação de commands/queries |
| Mapster | 7 | Mapeamento de objetos |
| BCrypt.Net | — | Hash de senhas |
| Serilog | — | Structured logging |
| OpenTelemetry | — | Observabilidade |
| Bogus | — | Seed de dados fake |
| xUnit + NSubstitute + FluentAssertions | — | Testes |

### 4.2 Frontend

| Tecnologia | Versão | Uso |
|---|---|---|
| React | 19 | UI Framework |
| TypeScript | 5+ | Tipagem estática |
| Vite | 6 | Build tool |
| Zustand | 5 | Estado global |
| TanStack Query | 5 | Server state / cache |
| TanStack Router | 1 | Roteamento tipado |
| Tailwind CSS | 4 | Estilização |
| shadcn/ui | — | Componentes base |
| Recharts | — | Gráficos |
| React Hook Form + Zod | — | Formulários e validação |
| Axios | — | HTTP client |
| Day.js | — | Manipulação de datas |

### 4.3 Infraestrutura

| Tecnologia | Uso |
|---|---|
| Docker + Docker Compose | Containerização de todos os serviços |
| Nginx | Reverse proxy para o frontend |
| GitHub Actions | CI/CD (futuro) |

---

## 5. Padrões e Decisões Técnicas

### 5.1 CQRS com MediatR
- **Commands** alteram estado (Create, Update, Delete)
- **Queries** apenas leem dados, podem usar projeções diretas sem passar pelos agregados
- **Pipeline Behaviors**: ValidationBehavior → LoggingBehavior → CachingBehavior (queries)

### 5.2 Result Pattern
- Sem exceções para fluxo de controle no domínio
- `Result<T>` retorna sucesso ou erro com código + mensagem
- Endpoints mapeiam `Result` para HTTP status codes

### 5.3 Domain Events
- Eventos publicados pelos agregados após mutações (ex: `TransactionCreatedEvent`)
- Processados via `INotification` do MediatR (in-process) ou MassTransit (out-of-process)
- Exemplos: atualizar saldo da conta, verificar orçamento excedido

### 5.4 Repository + Unit of Work
- Repositórios por agregado (não por entidade)
- `IUnitOfWork` exposto na camada de Application
- EF Core como implementação, abstraído para permitir troca

### 5.5 Caching com Redis
- Cache de queries custosas (ex: dashboard, relatórios)
- Invalidação por evento de domínio
- TTL configurável por query

### 5.6 Mensageria com MassTransit
- Consumers para processamento assíncrono (ex: enviar e-mail de alerta)
- Outbox pattern para garantir entrega de mensagens
- Retry policy configurada

### 5.7 Soft Delete
- Entidades não são deletadas fisicamente
- Campo `DeletedAt` + filtro global no EF Core

### 5.8 Multi-usuário
- Todas as entidades possuem `UserId`
- Filtro global no DbContext por usuário autenticado
- Validação de ownership em commands

---

## 6. Modelo de Domínio

### Principais Agregados

```
User
├── Id, Name, Email, PasswordHash
└── Accounts[]

Account
├── Id, UserId, Name, Type (Checking/Savings/Wallet/Investment)
├── InitialBalance, Color, Icon
└── Balance (calculado)

Transaction
├── Id, UserId, AccountId, CategoryId
├── Type (Income/Expense/Transfer)
├── Amount (ValueObject: Money)
├── Date, Description, Tags[]
└── TransferTargetAccountId? (para transferências)

Category
├── Id, UserId?, Name, Icon, Color
├── Type (Income/Expense)
└── ParentCategoryId?

Budget
├── Id, UserId, CategoryId
├── Month, LimitAmount
└── SpentAmount (calculado)
```

### Value Objects
- `Money` — valor + moeda (BRL por padrão)
- `DateRange` — início + fim com validações
- `Email` — validação de formato
- `Color` — hex color com validação

---

## 7. API — Endpoints Principais

```
POST   /auth/register
POST   /auth/login
POST   /auth/refresh
POST   /auth/logout

GET    /accounts
POST   /accounts
GET    /accounts/{id}
PUT    /accounts/{id}
DELETE /accounts/{id}

GET    /transactions?page&pageSize&from&to&type&categoryId&accountId
POST   /transactions
GET    /transactions/{id}
DELETE /transactions/{id}

GET    /categories
POST   /categories
PUT    /categories/{id}
DELETE /categories/{id}

GET    /budgets?month
POST   /budgets
PUT    /budgets/{id}
DELETE /budgets/{id}

GET    /dashboard/summary?month
GET    /dashboard/chart/monthly?year
GET    /dashboard/chart/categories?month
```

---

## 8. Docker Compose — Serviços

```yaml
services:
  api:          # .NET 10 API
  frontend:     # React + Nginx
  postgres:     # PostgreSQL 16
  redis:        # Redis 7
  rabbitmq:     # RabbitMQ 3.13 + Management UI
```

---

## 9. Fases de Desenvolvimento

### Fase 1 — Fundação (Backend)
- [ ] Setup do projeto e estrutura de pastas
- [ ] SharedKernel (Result, BaseEntity, DomainEvent)
- [ ] Domínio: User, Account, Transaction, Category
- [ ] Infrastructure: EF Core, PostgreSQL, migrations seed
- [ ] Application: Auth commands/queries
- [ ] Application: Accounts CRUD
- [ ] Application: Categories CRUD
- [ ] Application: Transactions (criar, cancelar, consultar com filtros)
- [ ] Web: endpoints Minimal API + JWT middleware
- [ ] Docker Compose básico

### Fase 2 — Fundação (Frontend)
- [ ] Setup Vite + React + TypeScript + Tailwind + shadcn/ui
- [ ] Auth flow (login, registro, refresh token)
- [ ] Layout base + navegação
- [ ] Tela de Accounts
- [ ] Tela de Transactions (listagem + filtros + CRUD)
- [ ] Tela de Categories

### Fase 3 — Features Avançadas
- [ ] Dashboard com gráficos
- [ ] Budgets (backend + frontend)
- [ ] Redis cache no backend
- [ ] Domain Events + MassTransit consumers
- [ ] Alertas de orçamento

### Fase 4 — Qualidade e Observabilidade
- [ ] Testes unitários (Domain + Application)
- [ ] Testes de integração (API)
- [ ] Serilog + OpenTelemetry
- [ ] Metas financeiras

---

## 10. Convenções de Código

- **Commits:** Conventional Commits (`feat:`, `fix:`, `chore:`, `refactor:`)
- **Branches:** `feature/`, `fix/`, `chore/`
- **Nomenclatura C#:** PascalCase para tipos, camelCase para variáveis, `_camelCase` para campos privados
- **Nomenclatura TS:** camelCase para variáveis/funções, PascalCase para tipos/componentes
- **Endpoints:** kebab-case nas rotas
- **Erros de domínio:** código string (`"ACCOUNT_NOT_FOUND"`) + mensagem legível

---

*Documento vivo — atualizar conforme o projeto evolui.*

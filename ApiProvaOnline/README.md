# Prova-On API 📝

API RESTful para Sistema de Provas Online, construída com **ASP.NET Core 8** e **Entity Framework Core**.

---

## 🏗️ Arquitetura

O projeto segue uma arquitetura limpa (Clean Architecture) com separação em camadas:

```
ApiProvaOnline/
├── Application/               # Camada de Aplicação
│   ├── DTOs/                   # Data Transfer Objects
│   │   ├── Requests/           # DTOs de entrada (LoginRequest, etc.)
│   │   └── Responses/          # DTOs de saída (ApiResponse<T>)
│   ├── Interfaces/             # Contratos (IRepository, IUnitOfWork, etc.)
│   ├── Mappings/               # Perfis do AutoMapper
│   ├── Services/               # Lógica de negócio
│   └── Validators/             # FluentValidation validators
├── Controllers/                # Endpoints da API
├── Infrastructure/             # Camada de Infraestrutura
│   ├── Configuration/          # Fluent API (EntityTypeConfiguration)
│   ├── Data/                   # DbContext, UnitOfWork, Migrations
│   ├── Middleware/              # ExceptionHandlingMiddleware
│   ├── Repositories/           # Implementação de repositórios
│   └── Security/               # TokenService (JWT)
├── Models/                     # Entidades de domínio
└── Program.cs                  # Configuração e DI
```

---

## 🛠️ Tecnologias

| Tecnologia                | Versão   | Finalidade                     |
|---------------------------|----------|--------------------------------|
| ASP.NET Core              | 8.0      | Framework principal            |
| Entity Framework Core     | 8.0.x    | ORM                            |
| Pomelo.EntityFrameworkCore | 8.0.2   | Provider MySQL                 |
| BCrypt.Net-Next           | 4.2.0    | Hash de senhas                 |
| JWT Bearer                | 8.0.11   | Autenticação via tokens        |
| Serilog                   | 10.x     | Logging estruturado            |
| FluentValidation          | 11.x     | Validação de modelos           |
| AutoMapper                | 16.x     | Mapeamento Entity ↔ DTO       |
| Swagger / Swashbuckle     | —        | Documentação de API            |
| xUnit + Moq               | —        | Testes unitários               |

---

## 📦 Padrões Implementados

### Repository Pattern + Unit of Work
- `IRepository<T>` com operações CRUD genéricas
- `BaseRepository<T>` como implementação base
- Repositórios específicos herdam do `BaseRepository`
- `IUnitOfWork` centraliza o `SaveChangesAsync` / `CommitAsync()`

### Fluent API Configuration
- Cada entidade possui seu próprio `IEntityTypeConfiguration<T>`
- Localização: `Infrastructure/Configuration/`
- `DbContext.OnModelCreating` usa `ApplyConfigurationsFromAssembly`

### Base Entity (Auditoria)
- `BaseEntity` com campos `CreatedAt`, `UpdatedAt`, `IsDeleted`
- Preenchimento automático via override de `SaveChangesAsync`

### Segurança
- **BCrypt**: Hash de senhas no cadastro e atualização
- **JWT**: Tokens com claims (Name, Email, Role, Id), 2h de validade
- **Pipeline**: `UseAuthentication()` → `UseAuthorization()`

### Error Handling
- `ExceptionHandlingMiddleware` global captura exceções não tratadas
- Respostas padronizadas via `ApiResponse<T>` (Success, Message, Data, Errors)
- Serilog para logs em console e arquivo (`Logs/log-*.txt`)

---

## 🚀 Como Executar

### Pré-requisitos
- .NET 8 SDK
- MySQL 8.0+

### Configuração

1. Clone o repositório
2. Configure a connection string em `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=ProvaOn;Uid=root;Pwd=sua_senha;"
  }
}
```

3. Aplique as migrations:
```bash
cd ApiProvaOnline/ApiProvaOnline
dotnet ef database update
```

4. Execute a API:
```bash
dotnet run
```

5. Acesse o Swagger: `https://localhost:5001/swagger`

---

## 🧪 Testes

O projeto de testes utiliza **xUnit**, **Moq** e **FluentAssertions**:

```bash
cd ApiProvaOnline/ApiProvaOnline.Tests
dotnet test
```

### Cobertura de testes:
- `ProfessorServiceTests` — CRUD, hash BCrypt, login
- `ProvaServiceTests` — CRUD, ordenação, validações

---

## 📊 Diagrama ER

```
┌──────────────┐     ┌──────────────────┐     ┌──────────────────┐
│  Professor   │     │      Prova       │     │      Aluno       │
├──────────────┤     ├──────────────────┤     ├──────────────────┤
│ Id (PK)      │──┐  │ Id (PK)          │  ┌──│ Id (PK)          │
│ Nome         │  └─>│ IdProfessor (FK) │  │  │ Nome             │
│ Email        │     │ Titulo           │  │  │ Email            │
│ Senha        │     │ Codigo           │  │  │ Senha            │
│ DataHoraReg  │     │ DataHoraInicio   │  │  │ IdProva (FK)     │──┐
└──────────────┘     │ DataHoraFim      │  │  │ DataHoraRegistro │  │
                     │ DataHoraRegistro │  │  └──────────────────┘  │
                     └──────────────────┘  │                        │
                            │              │                        │
                            ▼              │                        │
               ┌─────────────────────┐     │                        │
               │  PerguntaObjetiva   │     │                        │
               ├─────────────────────┤     │                        │
               │ Id (PK)             │     │                        │
               │ IdProva (FK)        │─────┘                        │
               │ Titulo              │                              │
               │ Descricao           │                              │
               │ OpcaoA ~ OpcaoE     │                              │
               │ OpcaoCorreta        │                              │
               │ Pontuacao           │                              │
               └─────────────────────┘                              │
                            │                                       │
                            ▼                                       │
              ┌───────────────────────────┐                         │
              │  AlunoRespostaObjetiva    │                         │
              ├───────────────────────────┤                         │
              │ Id (PK)                   │                         │
              │ IdAluno (FK)              │◄────────────────────────┘
              │ IdPerguntaObjetiva (FK)   │
              │ Resposta                  │
              │ PontuacaoFinal            │
              └───────────────────────────┘

┌──────────────────┐     ┌──────────────────────┐
│  Pergunta        │     │  AlunoResposta       │
├──────────────────┤     ├──────────────────────┤
│ Id (PK)          │     │ Id (PK)              │
│ IdProva (FK)     │     │ IdAluno (FK)         │
│ Titulo           │     │ IdPergunta (FK)      │
│ Descricao        │     │ Resposta             │
│ Pontuacao        │     └──────────────────────┘
└──────────────────┘

┌──────────────────┐
│ Administrativo   │
├──────────────────┤
│ Id (PK)          │
│ Nome             │
│ Email            │
│ Senha            │
│ DataHoraRegistro │
└──────────────────┘
```

---

## 📌 Endpoints Principais

| Método | Rota                           | Descrição                       |
|--------|--------------------------------|---------------------------------|
| POST   | `/api/Professor`               | Cadastrar professor             |
| POST   | `/api/Professor/login`         | Login do professor              |
| GET    | `/api/Prova`                   | Listar provas                   |
| POST   | `/api/Prova`                   | Cadastrar prova                 |
| GET    | `/api/Prova/{id}`              | Buscar prova por ID             |
| POST   | `/api/Aluno`                   | Cadastrar aluno                 |
| GET    | `/api/Aluno/prova/{provaId}`   | Listar alunos de uma prova      |
| POST   | `/api/PerguntaObjetiva`        | Criar pergunta objetiva         |
| POST   | `/api/AlunoRespostaObjetiva`   | Registrar resposta do aluno     |
| POST   | `/api/Correcao/{provaId}`      | Corrigir prova automaticamente  |

> Consulte o **Swagger UI** para documentação completa e interativa.

---

## 📄 Licença

Este projeto é de uso acadêmico.

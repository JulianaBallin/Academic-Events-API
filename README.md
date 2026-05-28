<p align="center">
  <img src="docs/diagrams/logo.svg" alt="Academic Events API" width="500">
</p>

<p align="center">
  API REST para <strong>gerenciamento de eventos acadêmicos</strong>, com inscrições, comentários e reações,<br>
  construída com ASP.NET Core, PostgreSQL, Entity Framework Core e autenticação JWT.<br>
  <em>Trabalho Final | UEA · Módulo Desenvolvimento em C#</em>
</p>

---

<h2 align="center">Tecnologias Utilizadas</h2>

<p align="center">
  <img alt=".NET" src="https://img.shields.io/badge/.NET-8.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white">
  <img alt="C#" src="https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=csharp&logoColor=white">
  <img alt="PostgreSQL" src="https://img.shields.io/badge/PostgreSQL-4169E1?style=for-the-badge&logo=postgresql&logoColor=white">
  <img alt="EF Core" src="https://img.shields.io/badge/EF_Core-512BD4?style=for-the-badge">
  <img alt="JWT" src="https://img.shields.io/badge/JWT-000000?style=for-the-badge&logo=jsonwebtokens&logoColor=white">
  <img alt="Swagger" src="https://img.shields.io/badge/Swagger-85EA2D?style=for-the-badge&logo=swagger&logoColor=black">
  <img alt="Docker" src="https://img.shields.io/badge/Docker-2496ED?style=for-the-badge&logo=docker&logoColor=white">
</p>

---

<h2 align="center">Descrição do Projeto</h2>

A **Academic Events API** é uma API REST para gerenciar eventos acadêmicos: palestras, workshops, seminários e afins. Usuários se cadastram, criam eventos, se inscrevem, comentam e reagem ao conteúdo, de forma parecida com uma rede social voltada ao ambiente universitário.

O projeto segue **arquitetura em camadas** separando a solution em cinco projetos independentes: `API`, `Application`, `Domain`, `Infrastructure` e `Exceptions`. A autenticação é feita via **JWT Bearer Token** e o banco de dados é **PostgreSQL** acessado pelo **Entity Framework Core**.

---

<h2 align="center">Entidades do Domínio</h2>

| Entidade | Descrição |
|----------|-----------|
| `User` | Usuário da plataforma. Pode organizar eventos, se inscrever, comentar e reagir. |
| `Event` | Evento acadêmico com título, descrição, data e local. Tem um organizador. |
| `Registration` | Inscrição de um usuário em um evento. Impede duplicatas. |
| `Comment` | Comentário feito por um usuário em um evento. |
| `Reaction` | Reação de um usuário a um evento (Curtir, Adorei, Interessante, Vou Participar). |

---

<h2 align="center">Arquitetura em Camadas</h2>

```
Cliente / Swagger / Postman
         |
AcademicEvents.API
         |
AcademicEvents.Application
         |
AcademicEvents.Domain
         |
AcademicEvents.Infrastructure
         |
      PostgreSQL
```

| Projeto | Responsabilidade |
|---------|-----------------|
| `AcademicEvents.API` | Controllers, configuração JWT, Swagger, middlewares e `Program.cs`. |
| `AcademicEvents.Application` | DTOs, services, interfaces dos services, validações e casos de uso. |
| `AcademicEvents.Domain` | Entidades, enums e regras do domínio. Sem dependência de framework ou banco. |
| `AcademicEvents.Infrastructure` | `DbContext`, repositories, migrations e configuração do EF Core. |
| `AcademicEvents.Exceptions` | Exceções customizadas e padronização das respostas de erro. |

---

<h2 align="center">Estrutura do Projeto</h2>

```text
AcademicEvents/
├── AcademicEvents.sln
├── docker-compose.yml
├── .gitignore
├── AcademicEvents.API/
│   ├── Controllers/
│   │   ├── AuthController.cs
│   │   ├── EventsController.cs
│   │   ├── CommentsController.cs
│   │   ├── ReactionsController.cs
│   │   ├── RegistrationsController.cs
│   │   └── UsersController.cs
│   ├── appsettings.json
│   └── Program.cs
├── AcademicEvents.Application/
│   ├── DTOs/
│   │   ├── Auth/
│   │   │   ├── LoginRequest.cs
│   │   │   ├── RegisterRequest.cs
│   │   │   └── AuthResponse.cs
│   │   ├── Event/
│   │   │   ├── CreateEventRequest.cs
│   │   │   ├── UpdateEventRequest.cs
│   │   │   └── EventResponse.cs
│   │   ├── Comment/
│   │   │   ├── CreateCommentRequest.cs
│   │   │   └── CommentResponse.cs
│   │   ├── Reaction/
│   │   │   ├── CreateReactionRequest.cs
│   │   │   └── ReactionResponse.cs
│   │   └── Registration/
│   │       ├── CreateRegistrationRequest.cs
│   │       └── RegistrationResponse.cs
│   ├── Interfaces/
│   │   ├── IAuthService.cs
│   │   ├── IEventService.cs
│   │   ├── ICommentService.cs
│   │   ├── IReactionService.cs
│   │   └── IRegistrationService.cs
│   ├── Services/
│   │   ├── AuthService.cs
│   │   ├── EventService.cs
│   │   ├── CommentService.cs
│   │   ├── ReactionService.cs
│   │   └── RegistrationService.cs
│   └── ApplicationDependencyInjectionExtension.cs
├── AcademicEvents.Domain/
│   ├── Entities/
│   │   ├── User.cs
│   │   ├── Event.cs
│   │   ├── Comment.cs
│   │   ├── Reaction.cs
│   │   └── Registration.cs
│   └── Enums/
│       ├── StatusEvento.cs
│       ├── StatusInscricao.cs
│       └── TipoReacao.cs
├── AcademicEvents.Infrastructure/
│   ├── Data/
│   │   └── AcademicEventsDbContext.cs
│   ├── Repositories/
│   │   ├── IAcademicEventsRepository.cs
│   │   └── AcademicEventsRepository.cs
│   ├── Migrations/
│   └── InfrastructureDependencyInjectionExtension.cs
├── AcademicEvents.Exceptions/
│   ├── NotFoundException.cs
│   ├── DuplicateEmailException.cs
│   ├── UnauthorizedException.cs
│   └── InvalidCredentialsException.cs
└── docs/
    └── diagrams/
        ├── logo.svg
        ├── c4_nivel1_contexto.puml
        ├── c4_nivel2_container.puml
        ├── c4_nivel3_componente.puml
        └── c4_nivel4_codigo.puml
```

---

<h2 align="center">Pré-requisitos</h2>

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [Docker](https://www.docker.com/) (para subir o PostgreSQL)
- [dotnet-ef CLI](https://learn.microsoft.com/ef/core/cli/dotnet) (para rodar migrations)

Instalar o dotnet-ef globalmente:

```bash
dotnet tool install --global dotnet-ef
```

---

<h2 align="center">Como Executar</h2>

**1. Clonar o repositório**

```bash
git clone https://github.com/JulianaBallin/Academic-Events-API.git
cd Academic-Events-API
```

**2. Subir o PostgreSQL com Docker**

```bash
docker compose up -d
```

**3. Configurar a connection string e o JWT**

Abra `AcademicEvents.API/appsettings.json` e ajuste as chaves conforme o seu ambiente:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=academic_events_db;Username=academic_user;Password=academic_password"
  },
  "Jwt": {
    "Key": "sua-chave-secreta-aqui-minimo-32-caracteres",
    "Issuer": "AcademicEventsAPI",
    "Audience": "AcademicEventsClientes",
    "ExpiresInHours": 8
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

**4. Rodar as migrations**

```bash
dotnet ef database update --project AcademicEvents.Infrastructure --startup-project AcademicEvents.API
```

**5. Iniciar a API**

```bash
cd AcademicEvents.API
dotnet run
```

Acesse o Swagger em: `http://localhost:5000/swagger`

---

<h2 align="center">Endpoints Principais</h2>

**Autenticação (públicos)**

| Método | Rota | Descrição |
|--------|------|-----------|
| `POST` | `/api/auth/register` | Cadastra um novo usuário na plataforma |
| `POST` | `/api/auth/login` | Autentica e retorna o token JWT |

**Usuário autenticado (protegido)**

| Método | Rota | Descrição |
|--------|------|-----------|
| `GET` | `/api/me` | Retorna os dados do usuário logado |

**Eventos**

| Método | Rota | Proteção | Descrição |
|--------|------|----------|-----------|
| `GET` | `/api/events` | Público | Lista todos os eventos publicados |
| `GET` | `/api/events/{id}` | Público | Busca um evento por ID |
| `GET` | `/api/events?status=Publicado` | Público | Filtra eventos por status |
| `GET` | `/api/events?organizadorId={id}` | Público | Filtra eventos por organizador |
| `POST` | `/api/events` | Protegido | Cria um novo evento |
| `PUT` | `/api/events/{id}` | Protegido | Atualiza um evento (só o organizador) |
| `DELETE` | `/api/events/{id}` | Protegido | Remove um evento (só o organizador) |

**Inscrições (protegidas)**

| Método | Rota | Descrição |
|--------|------|-----------|
| `POST` | `/api/registrations` | Inscreve o usuário autenticado em um evento |
| `GET` | `/api/registrations/me` | Lista as inscrições do usuário autenticado |
| `DELETE` | `/api/registrations/{id}` | Cancela uma inscrição |

**Comentários (protegidos)**

| Método | Rota | Descrição |
|--------|------|-----------|
| `POST` | `/api/comments` | Adiciona comentário em um evento |
| `GET` | `/api/comments?eventoId={id}` | Lista comentários de um evento |
| `DELETE` | `/api/comments/{id}` | Remove comentário (só o autor) |

**Reações (protegidas)**

| Método | Rota | Descrição |
|--------|------|-----------|
| `POST` | `/api/reactions` | Adiciona reação em um evento |
| `GET` | `/api/reactions?eventoId={id}` | Lista reações de um evento |

---

<h2 align="center">Autenticação JWT</h2>

Depois de fazer login, copie o token retornado e clique em **Authorize** no Swagger. Digite:

```
Bearer eyJhbGci...
```

Rotas marcadas com cadeado exigem esse token. Senhas são armazenadas com hash BCrypt e nunca em texto puro.

---

<h2 align="center">Diagramas C4</h2>

Os diagramas estão em `docs/diagrams/` no formato PlantUML (`.puml`).

Para visualizar: [PlantUML Online](https://www.plantuml.com/plantuml/uml/) ou plugin PlantUML no VS Code.

| Arquivo | Nível | Descrição |
|---------|-------|-----------|
| `c4_nivel1_contexto.puml` | Nível 1 | Visão geral: usuários, sistema e banco |
| `c4_nivel2_container.puml` | Nível 2 | Projetos da solution e responsabilidades |
| `c4_nivel3_componente.puml` | Nível 3 | Componentes internos da API e Application |
| `c4_nivel4_codigo.puml` | Nível 4 | Classes do domínio e relacionamentos |

---

<h2 align="center">Padrão de Documentação do Código</h2>

Todo arquivo C# deve ter um comentário XML no topo da classe principal, em português:

```csharp
/// <summary>
/// Service responsável pelo cadastro e login de usuários.
/// Gera o token JWT após validar as credenciais.
/// </summary>
public class AuthService : IAuthService
{
    // verifica se o email já existe antes de criar o usuário
    public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
    {
        if (await _repository.GetUserByEmailAsync(request.Email) is not null)
            throw new DuplicateEmailException("Esse email já está cadastrado.");
        ...
    }
}
```

---

<h2 align="center">Padrão de Commits</h2>

Commits convencionais em português, sem indicação de IA:

```
feat(Event): adiciona entidade Event com enum StatusEvento
feat(AuthService): implementa registro e login com JWT
fix(RegistrationService): corrige validacao de inscricao duplicada
docs(readme): atualiza secao de endpoints
```

Sempre usar a branch `develop` para enviar as alterações.

---

<h2 align="center">Decisões Arquiteturais</h2>

| Decisão | Motivo |
|---------|--------|
| Solution com 5 projetos separados | Requisito do trabalho e separação clara de responsabilidades |
| JWT Bearer Token | Padrão REST sem estado no servidor, amplamente adotado |
| BCrypt para senhas | Hash seguro, impossível reverter para o texto original |
| DTOs em vez de entidades nos endpoints | Evita expor detalhes internos e facilita evoluir a API |
| Extension methods para DI | Padrão do professor: `AddInfrastructure()`, `AddApplication()` |
| `EnsureCreated` ou migrations | `EnsureCreated` no desenvolvimento, migrations para produção |

---

<h2 align="center">Equipe</h2>

<p align="center">

| Nome | Responsabilidade |
|------|-----------------|
| Thailsson Clementino de Andrade | Solution, estrutura inicial, .gitignore e organização do repositório |
| Stevão Whinter Marques de Andrade | Domain: entidades, enums e interfaces de repository |
| Marcio Franklin de Oliveira Lima | Infrastructure: DbContext, EF Core, migrations e repositories |
| Allef Oliveira Ramos | API: controllers CRUD, Swagger e Program.cs base |
| Juliana Ballin Lima | Application layer: DTOs, services, JWT, exceções e testes |

</p>

---

<h3 align="center">UEA · Módulo Desenvolvimento em C# · Trabalho Final · Grupo 6</h3>

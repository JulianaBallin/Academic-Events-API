<p align="center">
  <img src="docs/diagrams/logo.svg" alt="Academic Events API" width="500">
</p>

<p align="center">
  API REST para <strong>gerenciamento de eventos acadêmicos</strong>, com inscrições, comentários e reações,<br>
  construída com ASP.NET Core, PostgreSQL, Entity Framework Core e autenticação JWT.<br>
  <em>Trabalho Final | Módulo Desenvolvimento em C#</em>
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
| `AcademicEvents.Infrastructure` | `DbContext`, repositories e configuração do EF Core. |
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
│   │   ├── UserRepository.cs
│   │   ├── EventRepository.cs
│   │   ├── RegistrationRepository.cs
│   │   ├── CommentRepository.cs
│   │   └── ReactionRepository.cs
│   └── InfrastructureDependencyInjectionExtension.cs
├── AcademicEvents.Exceptions/
│   ├── NotFoundException.cs
│   ├── DuplicateEmailException.cs
│   ├── UnauthorizedException.cs
│   ├── InscricaoDuplicadaException.cs
│   └── InvalidCredentialsException.cs
├── AcademicEvents.Tests/
│   ├── AuthServiceTests.cs
│   ├── EventServiceTests.cs
│   ├── RegistrationServiceTests.cs
│   ├── CommentServiceTests.cs
│   └── ReactionServiceTests.cs
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
- [dotnet-ef CLI](https://learn.microsoft.com/ef/core/cli/dotnet) (opcional, para criar migrations)

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
    "Key": "sua-chave-secreta-aqui-mínimo-32-caracteres",
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

**4. Iniciar a API**

```bash
cd AcademicEvents.API
dotnet run
```

Na inicialização, o `Program.cs` chama `EnsureCreated()` para criar as tabelas no PostgreSQL quando elas ainda não existem.

Acesse o Swagger em: `http://localhost:5136/swagger`

**5. Rodar os testes automatizados**

Na raiz do projeto:

```bash
dotnet test AcademicEvents.sln
```

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
| `GET` | `/api/events` | Público | Lista todos os eventos |
| `GET` | `/api/events/{id}` | Público | Busca um evento por ID |
| `GET` | `/api/events?status=Publicado` | Público | Filtra eventos por status |
| `GET` | `/api/events?organizadorId={id}` | Público | Filtra eventos por organizador |
| `GET` | `/api/events?status=Publicado&organizadorId={id}` | Público | Combina os filtros por status e organizador |
| `GET` | `/api/events/meus` | Protegido | Lista eventos do organizador autenticado |
| `POST` | `/api/events` | Protegido | Cria um novo evento |
| `PUT` | `/api/events/{id}` | Protegido | Atualiza um evento (só o organizador) |
| `DELETE` | `/api/events/{id}` | Protegido | Remove um evento (só o organizador) |

**Inscrições (protegidas)**

| Método | Rota | Descrição |
|--------|------|-----------|
| `POST` | `/api/registrations` | Inscreve o usuário autenticado em um evento |
| `GET` | `/api/registrations/me` | Lista as inscrições do usuário autenticado |
| `DELETE` | `/api/registrations/{id}` | Cancela uma inscrição |

**Comentários**

| Método | Rota | Proteção | Descrição |
|--------|------|----------|-----------|
| `GET` | `/api/comments?eventoId={id}` | Público | Lista comentários de um evento |
| `POST` | `/api/comments` | Protegido | Adiciona comentário em um evento |
| `DELETE` | `/api/comments/{id}` | Protegido | Remove comentário (só o autor) |

**Reações**

| Método | Rota | Proteção | Descrição |
|--------|------|----------|-----------|
| `GET` | `/api/reactions?eventoId={id}` | Público | Lista reações de um evento |
| `POST` | `/api/reactions` | Protegido | Adiciona reação em um evento |
| `DELETE` | `/api/reactions/{id}` | Protegido | Remove reação (só o autor) |

---

<h2 align="center">Autenticação JWT</h2>

Depois de fazer login, copie o token retornado e clique em **Authorize** no Swagger. Digite:

```
eyJhbGci...
```

No Swagger, cole apenas o token, sem a palavra `Bearer`. Em clientes HTTP como o arquivo `endpoints.http`, o header recomendado é `Authorization: Bearer {token}`. A API também aceita o token puro no header `Authorization` para facilitar a demonstração pelo Swagger. Rotas marcadas com cadeado exigem esse token. Senhas são armazenadas com hash BCrypt e nunca em texto puro.

Os enums de entrada podem ser enviados como texto no JSON, por exemplo `"Publicado"` para status do evento e `"VouParticipar"` para tipo de reação.

---

<h2 align="center">Respostas de Erro</h2>

A API possui um middleware de tratamento de exceções para padronizar erros conhecidos de regra de negócio. Assim, respostas como email duplicado, credenciais inválidas, recurso inexistente e falta de permissão seguem o mesmo formato:

```json
{
  "mensagem": "Evento não encontrado.",
  "statusCode": 404,
  "caminho": "/api/events/99",
  "dataHoraUtc": "2026-06-01T20:30:00Z"
}
```

Principais códigos usados:

| Código | Quando acontece |
|--------|-----------------|
| `400` | Dados inválidos, email duplicado, inscrição duplicada ou reação duplicada |
| `401` | Login inválido ou ausência de token em rota protegida |
| `403` | Usuário autenticado tentando alterar recurso de outra pessoa |
| `404` | Evento, comentário, inscrição ou reação não encontrada |

---

<h2 align="center">Diagramas C4</h2>

Os diagramas estão em `docs/diagrams/` no formato PlantUML (`.puml`).

Para visualizar: [PlantUML Online](https://www.plantuml.com/plantuml/uml/) ou plugin PlantUML no VS Code.

Para validar localmente:

```bash
plantuml -checkonly docs/diagrams/c4_nivel1_contexto.puml docs/diagrams/c4_nivel2_container.puml docs/diagrams/c4_nivel3_componente.puml docs/diagrams/c4_nivel4_codigo.puml
```

| Arquivo | Nível | Descrição |
|---------|-------|-----------|
| `c4_nivel1_contexto.puml` | Nível 1 | Visão geral: usuários, sistema e banco |
| `c4_nivel2_container.puml` | Nível 2 | Projetos da solution e responsabilidades |
| `c4_nivel3_componente.puml` | Nível 3 | Componentes internos da API e Application |
| `c4_nivel4_codigo.puml` | Nível 4 | Classes do domínio e relacionamentos |

---

<h2 align="center">Testes Automatizados</h2>

O projeto inclui `AcademicEvents.Tests` com xUnit e Moq para validar regras dos services sem depender do PostgreSQL.

```bash
dotnet test AcademicEvents.sln
```

Cobertura atual:

- `AuthService`: email duplicado, normalização de email e credenciais inválidas
- `EventService`: datas inválidas, textos aparados, filtros inválidos e permissão do organizador
- `RegistrationService`: evento inexistente, id inválido e inscrição duplicada
- `CommentService`: evento inexistente, conteúdo em branco, texto aparado e remoção por outro usuário
- `ReactionService`: evento inexistente, id inválido e reação duplicada

O repositório também possui workflow de CI em `.github/workflows/ci.yml`, rodando restore, build e testes automaticamente.

---

<h2 align="center">Padrão de Documentação do Código</h2>

Todo arquivo C# deve ter um comentário XML no topo da classe principal, em português. O Swagger lê os comentários XML do projeto `AcademicEvents.API`, então os resumos dos controllers aparecem na documentação interativa.

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
        if (await _repository.GetByEmailAsync(request.Email) is not null)
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
fix(RegistrationService): corrige validação de inscrição duplicada
docs(readme): atualiza seção de endpoints
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

| Nome | GitHub | Responsabilidade |
|------|--------|-----------------|
| Juliana Ballin Lima | [JulianaBallin](https://github.com/JulianaBallin) | Desenvolvimento, documentação, testes, relatório técnico e revisão da apresentação |
| Allef Oliveira Ramos | [allef-oliveira](https://github.com/allef-oliveira) | Testes, estudo de stacks e apoio técnico ao Grupo 6 |

</p>

---

<h3 align="center">Módulo Desenvolvimento em C# · Trabalho Final · Grupo 6</h3>

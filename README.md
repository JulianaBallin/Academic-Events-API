<p align="center">
  <img src="docs/diagrams/logo.svg" alt="Academic Events API" width="500">
</p>

<p align="center">
  REST API for <strong>academic event management</strong>, with registrations, comments and reactions,<br>
  built with ASP.NET Core, PostgreSQL, Entity Framework Core and JWT authentication.<br>
  <em>Final Project | C# Development Module</em>
</p>

---

<h2 align="center">Technologies Used</h2>

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

<h2 align="center">Project Description</h2>

The **Academic Events API** is a REST API for managing academic events: lectures, workshops, seminars and more. Users register, create events, enroll, comment and react to content, similar to a social network focused on the university environment.

The project follows a **layered architecture** splitting the solution into five independent projects: `API`, `Application`, `Domain`, `Infrastructure` and `Exceptions`. Authentication is done via **JWT Bearer Token** and the database is **PostgreSQL** accessed through **Entity Framework Core**.

---

<h2 align="center">Domain Entities</h2>

| Entity | Description |
|--------|-------------|
| `User` | Platform user. Can organize events, register, comment and react. |
| `Event` | Academic event with title, description, dates and location. Has an organizer. |
| `Registration` | User registration for an event. Prevents duplicates. |
| `Comment` | Comment made by a user on an event. |
| `Reaction` | User reaction to an event (Like, Loved, Interesting, WillParticipate). |
| `Activity` | Activities that are part of an event schedule. |

---

<h2 align="center">Layered Architecture</h2>

```
Client / Swagger / Postman
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

| Project | Responsibility |
|---------|---------------|
| `AcademicEvents.API` | Controllers, JWT configuration, Swagger, middlewares and `Program.cs`. |
| `AcademicEvents.Application` | DTOs, services, service interfaces, validations and use cases. |
| `AcademicEvents.Domain` | Entities, enums and domain rules. No framework or database dependencies. |
| `AcademicEvents.Infrastructure` | `DbContext`, repositories and EF Core configuration. |
| `AcademicEvents.Exceptions` | Custom exceptions and error response standardization. |

---

<h2 align="center">Project Structure</h2>

```text
├── AcademicEvents.API
│   ├── AcademicEvents.API.csproj
│   ├── AcademicEvents.API.http
│   ├── appsettings.json
│   ├── Controllers
│   │   ├── ActivityController.cs
│   │   ├── AuthController.cs
│   │   ├── CommentsController.cs
│   │   ├── EventsController.cs
│   │   ├── ReactionsController.cs
│   │   ├── RegistrationsController.cs
│   │   └── UsersController.cs
│   ├── Middlewares
│   │   └── ExceptionHandlingMiddleware.cs
│   ├── Program.cs
│   └── Properties
│       └── launchSettings.json
├── AcademicEvents.Application
│   ├── AcademicEvents.Application.csproj
│   ├── ApplicationDependencyInjectionExtension.cs
│   ├── DTOs
│   │   ├── Activity
│   │   │   ├── ActivityResponse.cs
│   │   │   ├── CreateActivityRequest.cs
│   │   │   └── UpdateActivityRequest.cs
│   │   ├── Auth
│   │   │   ├── AuthResponse.cs
│   │   │   ├── LoginRequest.cs
│   │   │   └── RegisterRequest.cs
│   │   ├── Comment
│   │   │   ├── CommentResponse.cs
│   │   │   └── CreateCommentRequest.cs
│   │   ├── Event
│   │   │   ├── CreateEventRequest.cs
│   │   │   ├── EventResponse.cs
│   │   │   └── UpdateEventRequest.cs
│   │   ├── Reaction
│   │   │   ├── CreateReactionRequest.cs
│   │   │   └── ReactionResponse.cs
│   │   └── Registration
│   │       ├── CreateRegistrationRequest.cs
│   │       └── RegistrationResponse.cs
│   ├── Interfaces
│   │   ├── IActivityService.cs
│   │   ├── IAuthService.cs
│   │   ├── ICommentService.cs
│   │   ├── IEventService.cs
│   │   ├── IReactionService.cs
│   │   └── IRegistrationService.cs
│   └── Services
│       ├── ActivityService.cs
│       ├── AuthService.cs
│       ├── CommentService.cs
│       ├── EventService.cs
│       ├── ReactionService.cs
│       └── RegistrationService.cs
├── AcademicEvents.Domain
│   ├── AcademicEvents.Domain.csproj
│   ├── Entities
│   │   ├── Activity.cs
│   │   ├── Comment.cs
│   │   ├── Event.cs
│   │   ├── Reaction.cs
│   │   ├── Registration.cs
│   │   └── User.cs
│   ├── Enums
│   │   ├── ActivityType.cs
│   │   ├── EventStatus.cs
│   │   ├── ReactionType.cs
│   │   └── RegistrationStatus.cs
│   └── Interfaces
│       ├── IActivityRepository.cs
│       ├── ICommentRepository.cs
│       ├── IEventRepository.cs
│       ├── IReactionRepository.cs
│       ├── IRegistrationRepository.cs
│       └── IUserRepository.cs
├── AcademicEvents.Exceptions
│   ├── AcademicEvents.Exceptions.csproj
│   ├── DuplicateEmailException.cs
│   ├── DuplicateRegistrationException.cs
│   ├── ErrorResponse.cs
│   ├── InvalidCredentialsException.cs
│   ├── NotFoundException.cs
│   └── UnauthorizedException.cs
├── AcademicEvents.Infrastructure
│   ├── AcademicEvents.Infrastructure.csproj
│   ├── Data
│   │   └── AcademicEventsDbContext.cs
│   ├── InfrastructureDependencyInjectionExtension.cs
│   └── Repositories
│       ├── ActivityRepository.cs
│       ├── CommentRepository.cs
│       ├── EventRepository.cs
│       ├── ReactionRepository.cs
│       ├── RegistrationRepository.cs
│       └── UserRepository.cs
├── AcademicEvents.sln
├── AcademicEvents.Tests
│   ├── AcademicEvents.Tests.csproj
│   ├── AuthServiceTests.cs
│   ├── CommentServiceTests.cs
│   ├── EventServiceTests.cs
│   ├── ReactionServiceTests.cs
│   └── RegistrationServiceTests.cs
├── docker-compose.yml
├── docs
│   ├── apresentacao
│   │   └── Curso C#_Grupo06_Academic Events API.pdf
│   ├── demonstracao
│   │   └── roteiro_payloads_demonstracao.md
│   ├── diagrams
│   │   ├── c4_nivel1_contexto.puml
│   │   ├── c4_nivel2_container.puml
│   │   ├── c4_nivel3_componente.puml
│   │   ├── c4_nivel4_codigo.puml
│   │   └── logo.svg
│   └── relatorios
│       ├── relatorio_academic_events_grupo6.pdf
│       └── relatorio_academic_events_grupo6.tex
├── endpoints.http
└── README.md
```

---

<h2 align="center">Prerequisites</h2>

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [Docker](https://www.docker.com/) (to run PostgreSQL)
- [dotnet-ef CLI](https://learn.microsoft.com/ef/core/cli/dotnet) (optional, for creating migrations)

Install dotnet-ef globally:

```bash
dotnet tool install --global dotnet-ef
```

---

<h2 align="center">How to Run</h2>

**1. Clone the repository**

```bash
git clone https://github.com/JulianaBallin/Academic-Events-API.git
cd Academic-Events-API
```

**2. Start PostgreSQL with Docker**

```bash
docker compose up -d
```

**3. Configure the connection string and JWT**

Open `AcademicEvents.API/appsettings.json` and adjust the keys for your environment:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=academic_events_db;Username=academic_user;Password=academic_password"
  },
  "Jwt": {
    "Key": "your-secret-key-here-minimum-32-characters",
    "Issuer": "AcademicEventsAPI",
    "Audience": "AcademicEventsClients",
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

**4. Start the API**

```bash
cd AcademicEvents.API
dotnet run
```

On startup, `Program.cs` calls `EnsureCreated()` to create the PostgreSQL tables when they do not yet exist.

Access Swagger at: `http://localhost:5136/swagger`

**5. Run automated tests**

From the project root:

```bash
dotnet test AcademicEvents.sln
```

---

<h2 align="center">Main Endpoints</h2>

**Authentication (public)**

| Method | Route | Description |
|--------|-------|-------------|
| `POST` | `/api/auth/register` | Registers a new user |
| `POST` | `/api/auth/login` | Authenticates and returns the JWT token |

**Authenticated user (protected)**

| Method | Route | Description |
|--------|-------|-------------|
| `GET` | `/api/me` | Returns the logged-in user data |

**Events**

| Method | Route | Protection | Description |
|--------|-------|------------|-------------|
| `GET` | `/api/events` | Public | Lists all events |
| `GET` | `/api/events/{id}` | Public | Fetches an event by ID |
| `GET` | `/api/events?status=Published` | Public | Filters events by status |
| `GET` | `/api/events?organizerId={id}` | Public | Filters events by organizer |
| `GET` | `/api/events?status=Published&organizerId={id}` | Public | Combines status and organizer filters |
| `GET` | `/api/events/meus` | Protected | Lists events of the authenticated organizer |
| `POST` | `/api/events` | Protected | Creates a new event |
| `PUT` | `/api/events/{id}` | Protected | Updates an event (organizer only) |
| `DELETE` | `/api/events/{id}` | Protected | Removes an event (organizer only) |

**Registrations (protected)**

| Method | Route | Description |
|--------|-------|-------------|
| `POST` | `/api/registrations` | Registers the authenticated user for an event |
| `GET` | `/api/registrations/me` | Lists registrations of the authenticated user |
| `DELETE` | `/api/registrations/{id}` | Cancels a registration |

**Comments**

| Method | Route | Protection | Description |
|--------|-------|------------|-------------|
| `GET` | `/api/comments?eventId={id}` | Public | Lists comments for an event |
| `POST` | `/api/comments` | Protected | Adds a comment on an event |
| `DELETE` | `/api/comments/{id}` | Protected | Removes a comment (author only) |

**Reactions**

| Method | Route | Protection | Description |
|--------|-------|------------|-------------|
| `GET` | `/api/reactions?eventId={id}` | Public | Lists reactions for an event |
| `POST` | `/api/reactions` | Protected | Adds a reaction to an event |
| `DELETE` | `/api/reactions/{id}` | Protected | Removes a reaction (author only) |

**Activities**

| Method | Route | Protection | Description |
|--------|-------|------------|-------------|
| `GET` | `/api/activity/{id}` | Public | Returns an activity by id |
| `GET` | `/api/activity/event/{eventId}` | Public | Returns all activities for an event |
| `POST` | `/api/activity` | Protected | Creates a new activity (organizer only) |
| `PUT` | `/api/activity/{id}` | Protected | Updates an activity (organizer only) |
| `DELETE` | `/api/activity/{id}` | Protected | Removes an activity (organizer only) |

---

<h2 align="center">JWT Authentication</h2>

After logging in, copy the returned token and click **Authorize** in Swagger. Enter:

```
eyJhbGci...
```

In Swagger, paste only the token without the word `Bearer`. In HTTP clients such as the `endpoints.http` file, the recommended header is `Authorization: Bearer {token}`. The API also accepts the bare token in the `Authorization` header to simplify Swagger demos. Routes marked with a padlock require this token. Passwords are stored with BCrypt hash and never in plain text.

Enum values can be sent as text in the JSON body, for example `"Published"` for event status and `"WillParticipate"` for reaction type. Available values:

| Enum | Values |
|------|--------|
| `EventStatus` | `Draft`, `Published`, `Canceled`, `Finished` |
| `ReactionType` | `Like`, `Loved`, `Interesting`, `WillParticipate` |
| `ActivityType` | `Opening`, `Lecture`, `Workshop`, `RoundTable`, `CoffeeBreak`, `Closing`, `TechnicalSession`, `Other` |
| `RegistrationStatus` | `Pending`, `Confirmed`, `Canceled` |

---

<h2 align="center">Error Responses</h2>

The API has an exception handling middleware to standardize known business rule errors. Responses such as duplicate email, invalid credentials, resource not found and missing permission all follow the same format:

```json
{
  "Message": "Evento nao encontrado.",
  "StatusCode": 404,
  "Path": "/api/events/99",
  "UtcTime": "2026-06-01T20:30:00Z"
}
```

Main status codes used:

| Code | When it happens |
|------|----------------|
| `400` | Invalid data, duplicate email, duplicate registration or duplicate reaction |
| `401` | Invalid login or missing token on a protected route |
| `403` | Authenticated user trying to modify another person's resource |
| `404` | Event, comment, registration, reaction or activity not found |

---

<h2 align="center">C4 Diagrams</h2>

Diagrams are in `docs/diagrams/` in PlantUML format (`.puml`).

To view: [PlantUML Online](https://www.plantuml.com/plantuml/uml/) or PlantUML plugin for VS Code.

To validate locally:

```bash
plantuml -checkonly docs/diagrams/c4_nivel1_contexto.puml docs/diagrams/c4_nivel2_container.puml docs/diagrams/c4_nivel3_componente.puml docs/diagrams/c4_nivel4_codigo.puml
```

| File | Level | Description |
|------|-------|-------------|
| `c4_nivel1_contexto.puml` | Level 1 | Overview: users, system and database |
| `c4_nivel2_container.puml` | Level 2 | Solution projects and responsibilities |
| `c4_nivel3_componente.puml` | Level 3 | Internal components of API and Application |
| `c4_nivel4_codigo.puml` | Level 4 | Domain classes and relationships |

---

<h2 align="center">Automated Tests</h2>

The project includes `AcademicEvents.Tests` with xUnit and Moq to validate service rules without depending on PostgreSQL.

```bash
dotnet test AcademicEvents.sln
```

Current coverage:

- `AuthService`: duplicate email, email normalization and invalid credentials
- `EventService`: invalid dates, trimmed texts, invalid filters and organizer permission
- `RegistrationService`: non-existent event, invalid id and duplicate registration
- `CommentService`: non-existent event, blank content, trimmed text and removal by another user
- `ReactionService`: non-existent event, invalid id and duplicate reaction

---

<h2 align="center">Code Documentation Standard</h2>

Every C# file should have an XML comment at the top of the main class. Swagger reads the XML comments from the `AcademicEvents.API` project, so controller summaries appear in the interactive documentation.

```csharp
/// <summary>
/// Service responsible for user registration and login.
/// Generates the JWT token after validating credentials.
/// </summary>
public class AuthService : IAuthService
{
    public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
    {
        if (await _repository.GetByEmailAsync(request.Email) is not null)
            throw new DuplicateEmailException("Esse email ja esta cadastrado.");
        ...
    }
}
```

---

<h2 align="center">Commit Pattern</h2>

Conventional commits in English, without AI authorship indication:

```
feat(Event): add Event entity with EventStatus enum
feat(AuthService): implement register and login with JWT
fix(RegistrationService): fix duplicate registration validation
docs(readme): update endpoints section
```

Always use the `develop` branch to push changes.

---

<h2 align="center">Architectural Decisions</h2>

| Decision | Reason |
|----------|--------|
| Solution with 5 separate projects | Project requirement and clear separation of responsibilities |
| JWT Bearer Token | Stateless REST standard, widely adopted |
| BCrypt for passwords | Secure hash, impossible to reverse to the original text |
| DTOs instead of entities in endpoints | Avoids exposing internal details and makes the API easier to evolve |
| Extension methods for DI | Professor pattern: `AddInfrastructure()`, `AddApplication()` |
| `EnsureCreated` or migrations | `EnsureCreated` in development, migrations for production |

---

<h2 align="center">Team</h2>

<p align="center">

| Name | GitHub | Responsibility |
|------|--------|---------------|
| Juliana Ballin Lima | [JulianaBallin](https://github.com/JulianaBallin) | Development, documentation, tests, technical report and presentation review |
| Allef Oliveira Ramos | [allef-oliveira](https://github.com/allef-oliveira) | Tests, stack research and technical support for Group 6 |
| Thailsson Clementino de Andrade | [clementino1971](https://github.com/clementino1971) | Activity entity implementation and documentation review |

</p>

---

<h3 align="center">C# Development Module · Final Project · Group 6</h3>

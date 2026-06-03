# feat(project): finalizes Academic Events API

## Summary

PR from `develop` to `main` with the final delivery of the Academic Events API, the final project for the C# Development Module.

The API implements a REST platform for managing academic events with JWT authentication, PostgreSQL, Entity Framework Core, layered architecture, Swagger, technical documentation, automated tests and demonstration materials.

## Main deliveries

### Architecture in 5 projects

- `AcademicEvents.API`: controllers, JWT configuration, Swagger, error middleware and `Program.cs`
- `AcademicEvents.Application`: DTOs, service interfaces, services and business rules
- `AcademicEvents.Domain`: entities, enums and repository interfaces with no external dependencies
- `AcademicEvents.Infrastructure`: `DbContext`, EF Core, Npgsql and concrete repositories
- `AcademicEvents.Exceptions`: custom exceptions and error response model

### Domain entities

| Entity | Role |
| --- | --- |
| `User` | Registered user, event organizer and participant |
| `Event` | Academic event with title, description, dates, location and status |
| `Registration` | User registration for an event, with duplicate blocking |
| `Comment` | Comment made by a user on an event |
| `Reaction` | User reaction to an event, one reaction per user |
| `Activity` | Activity belonging to an event schedule |

### Main endpoints

**Authentication**

- `POST /api/auth/register`: registers user and returns JWT
- `POST /api/auth/login`: authenticates user and returns JWT
- `GET /api/me`: returns authenticated user data

**Events**

- `GET /api/events`: lists events
- `GET /api/events?status=X`: filters events by status
- `GET /api/events?organizerId=X`: filters events by organizer
- `GET /api/events/{id}`: fetches event by id
- `GET /api/events/meus`: lists events of the authenticated user
- `POST /api/events`: creates event (authenticated)
- `PUT /api/events/{id}`: updates event, organizer only
- `DELETE /api/events/{id}`: removes event, organizer only

**Registrations, comments and reactions**

- `POST /api/registrations`: registers authenticated user in an event
- `GET /api/registrations/me`: lists registrations of the authenticated user
- `DELETE /api/registrations/{id}`: cancels registration
- `GET /api/comments?eventId=X`: lists comments for an event
- `POST /api/comments`: adds comment
- `DELETE /api/comments/{id}`: removes comment, author only
- `GET /api/reactions?eventId=X`: lists reactions for an event
- `POST /api/reactions`: adds reaction
- `DELETE /api/reactions/{id}`: removes reaction, author only

**Activities**

- `GET /api/activity/{id}`: fetches activity by id
- `POST /api/activity`: creates activity for an event (organizer only)
- `GET /api/activity/event/{eventId}`: lists all activities for an event
- `PUT /api/activity/{id}`: updates activity (organizer only)
- `DELETE /api/activity/{id}`: removes activity (organizer only)

## Implemented business rules

- Unique email validated in `AuthService` and protected by unique index in the database
- Email normalized before saving and searching
- Passwords stored with BCrypt hash, never in plain text
- `EndedAt` must be after `StartAt` for both events and activities
- Main texts are trimmed before saving
- Only the organizer can edit or remove an event
- Only the organizer can create, edit or remove activities of an event
- Activity dates must be within the event dates
- Duplicate registration blocked in the service and by unique index in the database
- Duplicate reaction blocked in the service and by unique index in the database
- Only the author can remove a comment or reaction
- Invalid ids return error 400
- Non-existent resources return error 404
- Missing permission returns error 403

## Recent improvements on develop branch

- Standardized error responses with `ExceptionHandlingMiddleware`
- Created `ErrorResponse` with `message`, `statusCode`, `path` and `timestampUtc`
- Configured standardized response for DTO validation errors
- Removed repeated `try/catch` from controllers
- Enabled XML comment reading in Swagger
- Documented error formats in README
- Strengthened defensive validations in services
- Expanded automated test coverage from 15 to 21 tests
- Added `.github/workflows/` to `.gitignore`
- Added `Activity` entity with full CRUD and `ActivityType` enum
- Translated all code identifiers, comments and enums to English
- Resolved merge conflict between develop and main (English property names + Activity)
- Fixed bugs in `ActivityService` (properties renamed in main: `OrganizerId`, `StartAt`, `EndedAt`)
- Added Thailsson Clementino de Andrade as team member in documentation
- Updated presentation PDF

## Infrastructure and persistence

- PostgreSQL 15 via Docker Compose
- Entity Framework Core with Npgsql provider
- `EnsureCreated()` on startup for automatic table creation in local environment
- Fluent API mappings in `AcademicEventsDbContext`
- Unique index on `User.Email`
- Unique index on `Registration(UserId, EventId)`
- Unique index on `Reaction(UserId, EventId)`
- `DeleteBehavior.Restrict` between `Event` and organizer `User`
- `DeleteBehavior.Cascade` on dependent relationships
- `DbSet<Activity>` with cascade delete from `Event`

## Documentation and materials

- `README.md`: execution instructions, architecture, endpoints, authentication and error responses
- `workflow.md`: development flow and manual tests
- `endpoints.http`: HTTP request collection
- `docs/diagrams/`: C4 diagrams in PlantUML
- `docs/relatorios/relatorio_academic_events_grupo6.pdf`: technical report
- `docs/demonstracao/roteiro_payloads_demonstracao.md`: payloads for demonstration
- `docs/apresentacao/Curso C#_Grupo06_Academic Events API.pdf`: final presentation PDF

## How to test locally

Start the database:

```bash
docker compose up -d
```

Run the API:

```bash
cd AcademicEvents.API
dotnet run --urls http://localhost:5000
```

Open Swagger:

```text
http://localhost:5000/swagger
```

If port 5000 is busy:

```bash
dotnet run --urls http://localhost:5002
```

Alternative Swagger:

```text
http://localhost:5002/swagger
```

## Automated tests

Commands used locally:

```bash
dotnet build AcademicEvents.sln
dotnet test AcademicEvents.sln
```

Most recent local result:

- Build with no errors
- Build with no warnings
- 21 automated tests passing
- `git diff --check` with no issues

## PR statistics

- 12 commits in `develop` ahead of `main`
- 6 domain entities
- 4 enums (`EventStatus`, `RegistrationStatus`, `ReactionType`, `ActivityType`)
- 6 services
- 6 repositories
- 24 REST endpoints
- 5 projects in the solution
- 21 automated tests

## Team

| Name | GitHub |
| --- | --- |
| Juliana Ballin Lima | [JulianaBallin](https://github.com/JulianaBallin) |
| Allef Oliveira Ramos | [allef-oliveira](https://github.com/allef-oliveira) |
| Thailsson Clementino de Andrade | [clementino1971](https://github.com/clementino1971) |

## Checklist

- [x] REST API functional
- [x] Layered architecture with 5 projects
- [x] PostgreSQL configured via Docker Compose
- [x] Entity Framework Core with Npgsql
- [x] JWT authentication
- [x] BCrypt password hashing
- [x] Input and output DTOs
- [x] Services with business rules
- [x] Repositories in Infrastructure
- [x] Swagger with Bearer authentication
- [x] Standardized error responses
- [x] Automated tests
- [x] Updated README
- [x] Technical report in PDF
- [x] Demonstration material
- [x] Activity entity with full CRUD
- [x] All identifiers standardized to English

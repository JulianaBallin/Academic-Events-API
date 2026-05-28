# Workflow - Academic Events API

Ola pessoal, nosso projeto e a **Academic Events API**, uma API REST em .NET para gerenciar eventos academicos. Aqui esta a divisao de tarefas para os 5 integrantes.

O professor deixou o repositorio `minisocial-project` como referencia. Sigam o mesmo estilo de codigo que ele usa la: extension methods para DI, repository pattern, service com regras de negocio, DTOs separados e `EnsureCreated` no Program.cs. A diferenca do nosso projeto e que vamos ter **JWT** e **5 projetos separados** em vez de um so.

> **Branch de trabalho: sempre usar `develop`. Nunca subir direto na `main`.**

---

## Padroes obrigatorios

- Commits convencionais em portugues, sem mencionar IA: `feat(NomeArquivo): descricao`
- Comentarios XML no topo das classes (ver exemplos abaixo)
- Comentarios inline em portugues, naturais, so onde o codigo nao fala por si mesmo
- Nunca usar travessao (--) em comentarios ou docs, usar hifen (-) mesmo
- Todas as propriedades das entidades iniciadas com letra maiuscula (padrao C#)

### Exemplo de documentacao XML (obrigatorio em todo arquivo)

```csharp
/// <summary>
/// Service que cuida do cadastro e login de usuarios.
/// Gera o token JWT depois de validar email e senha.
/// </summary>
public class AuthService : IAuthService
{
    // pega o secret do appsettings pra assinar o token
    private readonly string _jwtKey;
}
```

---

## Pessoa 1 - Juliana Ballin Lima

**Tarefa: Estrutura inicial da solution + Application layer completa + autenticacao JWT + excecoes + testes**

Essa e a parte mais complexa e inicial do projeto. A Juliana ficou responsavel por criar a base que todos os outros vao usar, alem de toda a camada Application.

### 1.1 - Estrutura da solution (ja feita - commit na develop)

A solution `AcademicEvents` ja foi criada com os 5 projetos:
- `AcademicEvents.API` - controllers, JWT, Swagger, Program.cs
- `AcademicEvents.Application` - DTOs, interfaces, services
- `AcademicEvents.Domain` - entidades, enums, interfaces de repository
- `AcademicEvents.Infrastructure` - DbContext, EF Core, repositories
- `AcademicEvents.Exceptions` - excecoes customizadas

Referencias ja configuradas entre os projetos. Pacotes instalados:
- Infrastructure: `Npgsql.EntityFrameworkCore.PostgreSQL 8.0.11`, `Microsoft.EntityFrameworkCore.Design 8.0.11`
- Application: `BCrypt.Net-Next`, `System.IdentityModel.Tokens.Jwt 7.1.2`
- API: `Swashbuckle.AspNetCore 6.6.2`, `Microsoft.AspNetCore.Authentication.JwtBearer 8.0.15`

O `docker-compose.yml` com PostgreSQL tambem ja esta na raiz.

### 1.2 - Excecoes customizadas (ja feitas - AcademicEvents.Exceptions/)

Cinco excecoes criadas: `NotFoundException` (404), `DuplicateEmailException` (400), `InvalidCredentialsException` (401), `UnauthorizedException` (403), `InscricaoDuplicadaException` (400).

### 1.3 - DTOs (ja feitos - AcademicEvents.Application/DTOs/)

Um par Request/Response para cada entidade: Auth, Event, Comment, Reaction, Registration.

### 1.4 - Interfaces dos services (ja feitas - AcademicEvents.Application/Interfaces/)

`IAuthService`, `IEventService`, `ICommentService`, `IReactionService`, `IRegistrationService`.

### 1.5 - Services (ja feitos - AcademicEvents.Application/Services/)

- `AuthService`: hash com BCrypt, valida credenciais e gera token JWT
- `EventService`: CRUD de eventos com validacao de organizador
- `CommentService`: criacao e remocao de comentarios
- `ReactionService`: uma reacao por usuario por evento
- `RegistrationService`: inscricao com verificacao de duplicata antes de chegar no banco

### 1.6 - ApplicationDependencyInjectionExtension (ja feito)

Extension method `AddApplication()` que registra todos os services no DI.

### 1.7 - Configuracao JWT no Program.cs (ja feito - AcademicEvents.API/Program.cs)

JWT Bearer configurado com validacao de issuer, audience, lifetime e chave. Swagger configurado com botao Authorize para rotas protegidas.

### 1.8 - AuthController e UsersController (ja feitos)

`POST /api/auth/register`, `POST /api/auth/login`, `GET /api/me`.

### 1.9 - Coleção de testes (ja feita - endpoints.http)

Arquivo `endpoints.http` na raiz com roteiro completo de testes para todos os endpoints.

---

## Pessoa 2 - Thailsson Clementino de Andrade

**Tarefa: Domain - entidades completas, enums e interfaces de repository**

O Domain ja tem a estrutura base criada. Sua tarefa e revisar e completar as entidades se necessario, garantindo que as propriedades e navegacoes estao corretas conforme o que o professor usa no `minisocial-project`.

Entidades para verificar/completar em `AcademicEvents.Domain/Entities/`:
- `User.cs`, `Event.cs`, `Registration.cs`, `Comment.cs`, `Reaction.cs`

Interfaces de repository em `AcademicEvents.Domain/Interfaces/`:
- `IUserRepository`, `IEventRepository`, `IRegistrationRepository`, `ICommentRepository`, `IReactionRepository`

Enums em `AcademicEvents.Domain/Enums/`:
- `StatusEvento`, `StatusInscricao`, `TipoReacao`

Garanta que cada arquivo tem o comentario XML no topo da classe principal em portugues.

---

## Pessoa 3 - Stevao Whinter Marques de Andrade

**Tarefa: Infrastructure - DbContext, EF Core, migrations e repositories**

O `AcademicEventsDbContext` e os repositories ja tem a estrutura base em `AcademicEvents.Infrastructure/`. Sua tarefa e revisar os mapeamentos do DbContext e garantir que todos os repositories estao implementando corretamente as interfaces do Domain.

Verifique em `AcademicEvents.Infrastructure/Data/AcademicEventsDbContext.cs` se os relacionamentos e indices unicos estao corretos.

Depois que o banco estiver de pe via Docker, rode as migrations:

```bash
dotnet ef migrations add CriacaoInicial --project AcademicEvents.Infrastructure --startup-project AcademicEvents.API
dotnet ef database update --project AcademicEvents.Infrastructure --startup-project AcademicEvents.API
```

Garanta que cada arquivo tem o comentario XML no topo da classe principal em portugues.

---

## Pessoa 4 - Marcio Franklin de Oliveira Lima

**Tarefa: API - controllers CRUD completos**

Os controllers de `Auth`, `Users`, `Events`, `Comments`, `Reactions` e `Registrations` ja tem a estrutura base em `AcademicEvents.API/Controllers/`. Sua tarefa e revisar se todos os endpoints estao corretos, se o tratamento de excecoes esta adequado e se o retorno HTTP (200, 201, 400, 401, 403, 404) esta certo para cada caso.

O controller so recebe o request, chama o service e devolve a resposta. Regra de negocio fica no service, nunca aqui.

Garanta que cada arquivo tem o comentario XML no topo da classe principal em portugues.

---

## Pessoa 5 - Allef Oliveira Ramos

**Tarefa: Testes manuais, Swagger e documentacao final**

Com a API rodando, sua tarefa e:

1. Subir o banco com `docker compose up -d`
2. Rodar a API com `dotnet run` dentro de `AcademicEvents.API/`
3. Testar todos os endpoints usando o arquivo `endpoints.http` na raiz ou o Swagger em `http://localhost:5000/swagger`
4. Registrar quais endpoints funcionam, quais tem problema e abrir issues no repositorio
5. Verificar se o Swagger esta documentando corretamente todas as rotas

Tambem e responsavel por garantir que o README esta completo e atualizado para a apresentacao.

---

## Como rodar o projeto

```bash
# 1. subir o banco
docker compose up -d

# 2. rodar as migrations (se ainda nao foram rodadas)
dotnet ef database update --project AcademicEvents.Infrastructure --startup-project AcademicEvents.API

# 3. iniciar a API
cd AcademicEvents.API
dotnet run
```

Acesse o Swagger em: `http://localhost:5000/swagger`

---

## Orientacoes gerais

- Todos precisam entender a arquitetura completa para a apresentacao final
- Seguir os padroes do professor no `minisocial-project` como referencia de estrutura e estilo
- Qualquer duvida sobre JWT, ver a documentacao do `Microsoft.AspNetCore.Authentication.JwtBearer`
- O Swagger deve funcionar completamente com o botao Authorize para rotas protegidas

Boa sorte pessoal!

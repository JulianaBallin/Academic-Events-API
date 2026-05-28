# Workflow - Academic Events API

Olá pessoal, nosso projeto é a **Academic Events API**, uma API REST em .NET para gerenciar eventos acadêmicos. Aqui está a divisão de tarefas para os 5 integrantes.

O professor deixou o repositório `minisocial-project` como referência. Sigam o mesmo estilo de código que ele usa lá: extension methods para DI, repository pattern, service com regras de negócio, DTOs separados e `EnsureCreated` no Program.cs. A diferença do nosso projeto é que vamos ter **JWT** e **5 projetos separados** em vez de um só.

> **Branch de trabalho: sempre usar `develop`. Nunca subir direto na `main`.**

---

## Padrões obrigatórios

- Commits convencionais em português, sem mencionar IA: `feat(NomeArquivo): descrição`
- Comentários XML no topo das classes (ver exemplos abaixo)
- Comentários inline em português, naturais, só onde o código não fala por si mesmo
- Nunca usar travessão (--) em comentários ou docs, usar hífen (-) mesmo
- Todas as propriedades das entidades iniciadas com letra maiúscula (padrão C#)

### Exemplo de documentação XML (obrigatório em todo arquivo)

```csharp
/// <summary>
/// Service que cuida do cadastro e login de usuários.
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

**Tarefa: Estrutura inicial da solution + Application layer completa + autenticação JWT + exceções + testes**

Essa é a parte mais complexa e inicial do projeto. A Juliana ficou responsável por criar a base que todos os outros vão usar, além de toda a camada Application.

### 1.1 - Estrutura da solution (já feita - commit na develop)

A solution `AcademicEvents` já foi criada com os 5 projetos:
- `AcademicEvents.API` - controllers, JWT, Swagger, Program.cs
- `AcademicEvents.Application` - DTOs, interfaces, services
- `AcademicEvents.Domain` - entidades, enums, interfaces de repository
- `AcademicEvents.Infrastructure` - DbContext, EF Core, repositories
- `AcademicEvents.Exceptions` - exceções customizadas

Referências já configuradas entre os projetos. Pacotes instalados:
- Infrastructure: `Npgsql.EntityFrameworkCore.PostgreSQL 8.0.11`, `Microsoft.EntityFrameworkCore.Design 8.0.11`
- Application: `BCrypt.Net-Next`, `System.IdentityModel.Tokens.Jwt 7.1.2`
- API: `Swashbuckle.AspNetCore 6.6.2`, `Microsoft.AspNetCore.Authentication.JwtBearer 8.0.15`

O `docker-compose.yml` com PostgreSQL também já está na raiz.

### 1.2 - Exceções customizadas (já feitas - AcademicEvents.Exceptions/)

Cinco exceções criadas: `NotFoundException` (404), `DuplicateEmailException` (400), `InvalidCredentialsException` (401), `UnauthorizedException` (403), `InscricaoDuplicadaException` (400).

### 1.3 - DTOs (já feitos - AcademicEvents.Application/DTOs/)

Um par Request/Response para cada entidade: Auth, Event, Comment, Reaction, Registration.

### 1.4 - Interfaces dos services (já feitas - AcademicEvents.Application/Interfaces/)

`IAuthService`, `IEventService`, `ICommentService`, `IReactionService`, `IRegistrationService`.

### 1.5 - Services (já feitos - AcademicEvents.Application/Services/)

- `AuthService`: hash com BCrypt, valida credenciais e gera token JWT
- `EventService`: CRUD de eventos com validação de organizador
- `CommentService`: criação e remoção de comentários
- `ReactionService`: uma reação por usuário por evento
- `RegistrationService`: inscrição com verificação de duplicata antes de chegar no banco

### 1.6 - ApplicationDependencyInjectionExtension (já feito)

Extension method `AddApplication()` que registra todos os services no DI.

### 1.7 - Configuração JWT no Program.cs (já feito - AcademicEvents.API/Program.cs)

JWT Bearer configurado com validação de issuer, audience, lifetime e chave. Swagger configurado com botão Authorize para rotas protegidas.

### 1.8 - AuthController e UsersController (já feitos)

`POST /api/auth/register`, `POST /api/auth/login`, `GET /api/me`.

### 1.9 - Coleção de testes (já feita - endpoints.http)

Arquivo `endpoints.http` na raiz com roteiro completo de testes para todos os endpoints.

---

## Pessoa 2 - Thailsson Clementino de Andrade

**Tarefa: Domain - entidades completas, enums e interfaces de repository**

O Domain já tem a estrutura base criada. Sua tarefa é revisar e completar as entidades se necessário, garantindo que as propriedades e navegações estão corretas conforme o que o professor usa no `minisocial-project`.

Entidades para verificar/completar em `AcademicEvents.Domain/Entities/`:
- `User.cs`, `Event.cs`, `Registration.cs`, `Comment.cs`, `Reaction.cs`

Interfaces de repository em `AcademicEvents.Domain/Interfaces/`:
- `IUserRepository`, `IEventRepository`, `IRegistrationRepository`, `ICommentRepository`, `IReactionRepository`

Enums em `AcademicEvents.Domain/Enums/`:
- `StatusEvento`, `StatusInscricao`, `TipoReacao`

Garanta que cada arquivo tem o comentário XML no topo da classe principal em português.

---

## Pessoa 3 - Stevão Whinter Marques de Andrade

**Tarefa: Infrastructure - DbContext, EF Core, migrations e repositories**

O `AcademicEventsDbContext` e os repositories já têm a estrutura base em `AcademicEvents.Infrastructure/`. Sua tarefa é revisar os mapeamentos do DbContext e garantir que todos os repositories estão implementando corretamente as interfaces do Domain.

Verifique em `AcademicEvents.Infrastructure/Data/AcademicEventsDbContext.cs` se os relacionamentos e índices únicos estão corretos.

Depois que o banco estiver de pe via Docker, rode as migrations:

```bash
dotnet ef migrations add CriacaoInicial --project AcademicEvents.Infrastructure --startup-project AcademicEvents.API
dotnet ef database update --project AcademicEvents.Infrastructure --startup-project AcademicEvents.API
```

Garanta que cada arquivo tem o comentário XML no topo da classe principal em português.

---

## Pessoa 4 - Márcio Franklin de Oliveira Lima

**Tarefa: API - controllers CRUD completos**

Os controllers de `Auth`, `Users`, `Events`, `Comments`, `Reactions` e `Registrations` já têm a estrutura base em `AcademicEvents.API/Controllers/`. Sua tarefa é revisar se todos os endpoints estão corretos, se o tratamento de exceções está adequado e se o retorno HTTP (200, 201, 400, 401, 403, 404) está certo para cada caso.

O controller só recebe o request, chama o service e devolve a resposta. Regra de negócio fica no service, nunca aqui.

Garanta que cada arquivo tem o comentário XML no topo da classe principal em português.

---

## Pessoa 5 - Allef Oliveira Ramos

**Tarefa: Testes manuais, Swagger e documentação final**

Com a API rodando, sua tarefa e:

1. Subir o banco com `docker compose up -d`
2. Rodar a API com `dotnet run` dentro de `AcademicEvents.API/`
3. Testar todos os endpoints usando o arquivo `endpoints.http` na raiz ou o Swagger em `http://localhost:5000/swagger`
4. Registrar quais endpoints funcionam, quais têm problema e abrir issues no repositório
5. Verificar se o Swagger está documentando corretamente todas as rotas

Também é responsável por garantir que o README está completo e atualizado para a apresentação.

---

## Como rodar o projeto

```bash
# 1. subir o banco
docker compose up -d

# 2. rodar as migrations (se ainda não foram rodadas)
dotnet ef database update --project AcademicEvents.Infrastructure --startup-project AcademicEvents.API

# 3. iniciar a API
cd AcademicEvents.API
dotnet run
```

Acesse o Swagger em: `http://localhost:5000/swagger`

---

## Orientações gerais

- Todos precisam entender a arquitetura completa para a apresentação final
- Seguir os padrões do professor no `minisocial-project` como referência de estrutura e estilo
- Qualquer dúvida sobre JWT, ver a documentação do `Microsoft.AspNetCore.Authentication.JwtBearer`
- O Swagger deve funcionar completamente com o botão Authorize para rotas protegidas

Boa sorte pessoal!

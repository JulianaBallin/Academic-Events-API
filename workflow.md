# Workflow - Academic Events API

Ola pessoal, nosso projeto é a **Academic Events API**, uma API REST em .NET para gerenciar eventos academicos. Aqui esta a divisao de tarefas para os 5 integrantes.

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
- `EventService`: CRUD de eventos com validacao de organizador, filtro por status e por organizador
- `CommentService`: criacao e remocao de comentarios
- `ReactionService`: uma reacao por usuario por evento
- `RegistrationService`: inscricao com verificacao de duplicata antes de chegar no banco

### 1.6 - ApplicationDependencyInjectionExtension (ja feito)

Extension method `AddApplication()` que registra todos os services no DI.

### 1.7 - Configuracao JWT no Program.cs (ja feito - AcademicEvents.API/Program.cs)

JWT Bearer configurado com validacao de issuer, audience, lifetime e chave. Swagger configurado com botao Authorize para rotas protegidas.

### 1.8 - AuthController e UsersController (ja feitos)

`POST /api/auth/register`, `POST /api/auth/login`, `GET /api/me`.

### 1.9 - Colecao de testes (ja feita - endpoints.http)

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

Observacao importante: para retornar 403 com mensagem no body, usar `StatusCode(403, ex.Message)` em vez de `Forbid(ex.Message)` - o metodo `Forbid()` nao aceita texto de mensagem.

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

## Estrategia de testes manuais

### Roteiro basico (usar no Swagger ou no endpoints.http)

**Passo 1 - Autenticacao**
```
POST /api/auth/register  - cadastra um usuario de teste
POST /api/auth/login     - faz login, copia o token JWT retornado
```

**Passo 2 - Eventos (publico)**
```
GET /api/events                    - lista todos os eventos
GET /api/events?status=Publicado   - filtra por status
GET /api/events/{id}               - busca evento especifico
```

**Passo 3 - Eventos (protegido - colocar Bearer Token no Swagger)**
```
POST /api/events                   - cria novo evento (anote o id retornado)
GET  /api/events/meus              - lista seus proprios eventos
PUT  /api/events/{id}              - edita o evento criado
DELETE /api/events/{id}            - remove o evento
```

**Passo 4 - Inscricoes**
```
POST /api/registrations            - inscreve no evento (body: {"eventoId": 1})
GET  /api/registrations/me         - lista minhas inscricoes
POST /api/registrations            - tenta de novo no mesmo evento (deve retornar 400)
DELETE /api/registrations/{id}     - cancela inscricao
```

**Passo 5 - Comentarios**
```
GET  /api/comments?eventoId=1      - lista comentarios (publico)
POST /api/comments                 - adiciona comentario (body: {"eventoId": 1, "conteudo": "..."})
DELETE /api/comments/{id}          - remove comentario proprio
DELETE /api/comments/{id}          - tenta remover comentario de outro usuario (deve retornar 403)
```

**Passo 6 - Reacoes**
```
GET  /api/reactions?eventoId=1     - lista reacoes (publico)
POST /api/reactions                - adiciona reacao (body: {"eventoId": 1, "tipo": "Curtir"})
POST /api/reactions                - tenta reagir de novo no mesmo evento (deve retornar 400)
DELETE /api/reactions/{id}         - remove reacao
```

### Casos de erro que devem ser testados

| Cenario                                 | Endpoint              | Esperado |
|-----------------------------------------|-----------------------|----------|
| Login com senha errada                  | POST /api/auth/login  | 401      |
| Email duplicado no cadastro             | POST /api/auth/register | 400    |
| Criar evento sem token                  | POST /api/events      | 401      |
| Editar evento de outro usuario          | PUT /api/events/{id}  | 403      |
| Buscar evento que nao existe            | GET /api/events/9999  | 404      |
| Inscricao duplicada no mesmo evento     | POST /api/registrations | 400    |
| Reagir duas vezes no mesmo evento       | POST /api/reactions   | 400      |
| Remover comentario de outro usuario     | DELETE /api/comments/{id} | 403  |

### Melhorias possiveis para o futuro

- Adicionar paginacao nos endpoints de lista (GET /api/events)
- Endpoint para confirmar inscricao manualmente (organizador muda status de Pendente para Confirmada)
- Endpoint para listar inscricoes de um evento (organizador ve quem se inscreveu)
- Validacao de data: impedir criacao de evento com data no passado
- Rate limiting para prevenir abuso dos endpoints publicos
- Testes de integracao com banco real (xUnit + testcontainers)

---

## Orientacoes gerais

- Todos precisam entender a arquitetura completa para a apresentacao final
- Seguir os padroes do professor no `minisocial-project` como referencia de estrutura e estilo
- Qualquer duvida sobre JWT, ver a documentacao do `Microsoft.AspNetCore.Authentication.JwtBearer`
- O Swagger deve funcionar completamente com o botao Authorize para rotas protegidas

Boa sorte pessoal!

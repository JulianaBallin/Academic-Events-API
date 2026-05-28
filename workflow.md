# Workflow - Academic Events API

Olá pessoal, nosso projeto é a **Academic Events API**, uma API REST em .NET para gerenciar eventos acadêmicos. Aqui está a divisão de tarefas para os 5 integrantes, levando em conta o nível de complexidade de cada parte.

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

    public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
    {
        // verifica se o email já está em uso antes de criar o usuário
        if (await _repository.GetUserByEmailAsync(request.Email) is not null)
            throw new DuplicateEmailException("Esse email já está cadastrado.");

        // hash da senha antes de salvar, nunca salvar senha pura
        string senhaHash = BCrypt.Net.BCrypt.HashPassword(request.Senha);
        ...
    }
}
```

---

## Pessoa 1 - Thailsson Clementino de Andrade

**Tarefa: Base do projeto, solution e organização**

Criar a estrutura inicial da solution com os 5 projetos exigidos pelo trabalho.

Passos:

```bash
# criar a solution
dotnet new sln -n AcademicEvents

# criar os projetos
dotnet new webapi -n AcademicEvents.API
dotnet new classlib -n AcademicEvents.Application
dotnet new classlib -n AcademicEvents.Domain
dotnet new classlib -n AcademicEvents.Infrastructure
dotnet new classlib -n AcademicEvents.Exceptions

# adicionar os projetos na solution
dotnet sln add AcademicEvents.API/AcademicEvents.API.csproj
dotnet sln add AcademicEvents.Application/AcademicEvents.Application.csproj
dotnet sln add AcademicEvents.Domain/AcademicEvents.Domain.csproj
dotnet sln add AcademicEvents.Infrastructure/AcademicEvents.Infrastructure.csproj
dotnet sln add AcademicEvents.Exceptions/AcademicEvents.Exceptions.csproj
```

Referências entre projetos (quem depende de quem):

```bash
# API depende de Application e Infrastructure
dotnet add AcademicEvents.API reference AcademicEvents.Application
dotnet add AcademicEvents.API reference AcademicEvents.Infrastructure
dotnet add AcademicEvents.API reference AcademicEvents.Exceptions

# Application depende de Domain e Exceptions
dotnet add AcademicEvents.Application reference AcademicEvents.Domain
dotnet add AcademicEvents.Application reference AcademicEvents.Exceptions

# Infrastructure depende de Domain
dotnet add AcademicEvents.Infrastructure reference AcademicEvents.Domain
```

Pacotes na Infrastructure:

```bash
cd AcademicEvents.Infrastructure
dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Npgsql
```

Pacotes na API:

```bash
cd AcademicEvents.API
dotnet add package Swashbuckle.AspNetCore
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer
dotnet add package Microsoft.EntityFrameworkCore.Design
```

Pacote para hash de senha (na Application):

```bash
cd AcademicEvents.Application
dotnet add package BCrypt.Net-Next
```

Criar o `.gitignore` na raiz (igual ao do professor, ignorar `bin/`, `obj/`, `.vs/`, `.idea/`, `.env`).

Criar o `docker-compose.yml` na raiz da solution:

```yaml
services:
  academic-events-postgres:
    image: postgres:15
    container_name: academic-events-postgres
    restart: unless-stopped
    environment:
      POSTGRES_DB: academic_events_db
      POSTGRES_USER: academic_user
      POSTGRES_PASSWORD: academic_password
    ports:
      - "5432:5432"
    volumes:
      - academic_events_postgres_data:/var/lib/postgresql/data
    networks:
      - academic-events-network

volumes:
  academic_events_postgres_data:

networks:
  academic-events-network:
    driver: bridge
```

Entregas: solution compilando, projetos criados, `.gitignore`, `docker-compose.yml`, `appsettings.json` com connection string e chave JWT configuradas.

---

## Pessoa 2 - Stevão Whinter Marques de Andrade

**Tarefa: Domain - entidades, enums e interfaces de repository**

Criar tudo dentro do projeto `AcademicEvents.Domain`. Nenhuma dependência de EF Core ou banco de dados aqui.

**Entidades** (`AcademicEvents.Domain/Entities/`):

`User.cs`:
```csharp
/// <summary>
/// Representa um usuário cadastrado na plataforma.
/// Pode organizar eventos, se inscrever, comentar e reagir.
/// </summary>
public class User
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    // senha nunca fica aqui em texto puro, sempre o hash
    public string SenhaHash { get; set; } = string.Empty;
    public DateTime CriadoEm { get; set; } = DateTime.UtcNow;

    public List<Event> EventosOrganizados { get; set; } = new();
    public List<Registration> Inscricoes { get; set; } = new();
    public List<Comment> Comentarios { get; set; } = new();
    public List<Reaction> Reacoes { get; set; } = new();
}
```

`Event.cs`:
```csharp
/// <summary>
/// Evento acadêmico criado por um usuário organizador.
/// Pode ser publicado, cancelado ou concluído.
/// </summary>
public class Event
{
    public int Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public DateTime DataInicio { get; set; }
    public DateTime DataFim { get; set; }
    public string Local { get; set; } = string.Empty;
    public StatusEvento Status { get; set; } = StatusEvento.Rascunho;
    public int OrganizadorId { get; set; }
    public DateTime CriadoEm { get; set; } = DateTime.UtcNow;

    public User? Organizador { get; set; }
    public List<Registration> Inscricoes { get; set; } = new();
    public List<Comment> Comentarios { get; set; } = new();
    public List<Reaction> Reacoes { get; set; } = new();
}
```

`Comment.cs`, `Reaction.cs` e `Registration.cs`: mesma estrutura, com FK para User e Event, navegação de volta para as entidades relacionadas.

**Enums** (`AcademicEvents.Domain/Enums/`):

```csharp
public enum StatusEvento { Rascunho, Publicado, Cancelado, Concluido }
public enum StatusInscricao { Pendente, Confirmada, Cancelada }
public enum TipoReacao { Curtir, Adorei, Interessante, VouParticipar }
```

**Interfaces de repository** (`AcademicEvents.Domain/Interfaces/`):

Cria uma interface para cada entidade, por exemplo `IEventRepository.cs`:
```csharp
/// <summary>
/// Contrato do repository de eventos.
/// A implementação fica na Infrastructure.
/// </summary>
public interface IEventRepository
{
    Task<Event> CreateAsync(Event evento);
    Task<Event?> GetByIdAsync(int id);
    Task<List<Event>> GetAllAsync();
    Task<List<Event>> GetByStatusAsync(StatusEvento status);
    Task<List<Event>> GetByOrganizadorAsync(int organizadorId);
    Task<Event?> UpdateAsync(Event evento);
    Task DeleteAsync(int id);
}
```

Cria também `IUserRepository`, `ICommentRepository`, `IReactionRepository` e `IRegistrationRepository` com métodos equivalentes.

---

## Pessoa 3 - Marcio Franklin de Oliveira Lima

**Tarefa: Infrastructure - DbContext, EF Core, migrations e repositories**

Tudo dentro de `AcademicEvents.Infrastructure`. Segue o mesmo padrão do professor no `MiniSocialDbContext`.

**DbContext** (`AcademicEvents.Infrastructure/Data/AcademicEventsDbContext.cs`):

```csharp
/// <summary>
/// DbContext principal do projeto.
/// Mapeia as entidades do domínio para o PostgreSQL via EF Core.
/// </summary>
public class AcademicEventsDbContext : DbContext
{
    public AcademicEventsDbContext(DbContextOptions<AcademicEventsDbContext> options)
        : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Event> Events => Set<Event>();
    public DbSet<Registration> Registrations => Set<Registration>();
    public DbSet<Comment> Comments => Set<Comment>();
    public DbSet<Reaction> Reactions => Set<Reaction>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // email único por usuário
        modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();

        // um usuário não pode se inscrever duas vezes no mesmo evento
        modelBuilder.Entity<Registration>()
            .HasIndex(r => new { r.UsuarioId, r.EventoId })
            .IsUnique();

        // relacionamentos de Event com User (organizador)
        modelBuilder.Entity<Event>()
            .HasOne(e => e.Organizador)
            .WithMany(u => u.EventosOrganizados)
            .HasForeignKey(e => e.OrganizadorId);

        // ... demais relacionamentos seguem o mesmo padrão
    }
}
```

**Extension method de DI** (igual ao do professor):

```csharp
/// <summary>
/// Extension que registra tudo da Infrastructure no DI do ASP.NET.
/// Chamado no Program.cs com builder.Services.AddInfrastructure(config).
/// </summary>
public static class InfrastructureDependencyInjectionExtension
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        string connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string nao encontrada.");

        services.AddDbContext<AcademicEventsDbContext>(options =>
            options.UseNpgsql(connectionString));

        // registra cada repository como Scoped
        services.AddScoped<IEventRepository, EventRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        // ... restante dos repositories
    }
}
```

**Repositories** (`AcademicEvents.Infrastructure/Repositories/`):

Implementa cada interface do Domain. Exemplo `EventRepository.cs`:
```csharp
/// <summary>
/// Repository de eventos. Usa o DbContext para acessar o PostgreSQL.
/// </summary>
public class EventRepository : IEventRepository
{
    private readonly AcademicEventsDbContext _context;

    public EventRepository(AcademicEventsDbContext context)
    {
        _context = context;
    }

    public async Task<List<Event>> GetByStatusAsync(StatusEvento status)
    {
        // filtra pelo status e já inclui o organizador pra não precisar de outra query
        return await _context.Events
            .Include(e => e.Organizador)
            .Where(e => e.Status == status)
            .OrderByDescending(e => e.DataInicio)
            .ToListAsync();
    }
    // ... demais métodos
}
```

Criar migrations depois que o domínio estiver pronto:

```bash
dotnet ef migrations add CriacaoInicial --project AcademicEvents.Infrastructure --startup-project AcademicEvents.API
dotnet ef database update --project AcademicEvents.Infrastructure --startup-project AcademicEvents.API
```

---

## Pessoa 4 - Allef Oliveira Ramos

**Tarefa: API - controllers CRUD, Swagger e Program.cs base**

Tudo dentro de `AcademicEvents.API`. O controller só recebe o request, chama o service e devolve a resposta. Regra de negócio fica no service, nunca aqui.

**EventsController.cs** (exemplo de CRUD):

```csharp
/// <summary>
/// Controller de eventos acadêmicos.
/// Rotas GET são públicas. POST, PUT e DELETE exigem autenticação.
/// </summary>
[ApiController]
[Route("api/events")]
public class EventsController : ControllerBase
{
    private readonly IEventService _service;

    public EventsController(IEventService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? status)
    {
        return Ok(await _service.GetAllAsync(status));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        EventResponse? response = await _service.GetByIdAsync(id);
        if (response is null) return NotFound();
        return Ok(response);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create(CreateEventRequest request)
    {
        try
        {
            // pega o id do usuário logado do token JWT
            int usuarioId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            EventResponse response = await _service.CreateAsync(request, usuarioId);
            return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id:int}")]
    [Authorize]
    public async Task<IActionResult> Update(int id, UpdateEventRequest request) { ... }

    [HttpDelete("{id:int}")]
    [Authorize]
    public async Task<IActionResult> Delete(int id) { ... }
}
```

Seguir o mesmo padrão para `CommentsController`, `ReactionsController` e `RegistrationsController`.

**Program.cs** - estrutura base (a Juliana vai completar com JWT e DI dos services):

```csharp
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Infrastructure registra o DbContext e os repositories
builder.Services.AddInfrastructure(builder.Configuration);

// Application registra os services (Juliana implementa esse método)
builder.Services.AddApplication();

var app = builder.Build();

// cria o banco se não existir (útil no desenvolvimento)
using (IServiceScope scope = app.Services.CreateScope())
{
    AcademicEventsDbContext context = scope.ServiceProvider
        .GetRequiredService<AcademicEventsDbContext>();
    context.Database.EnsureCreated();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
```

---

## Pessoa 5 - Juliana Ballin Lima

**Tarefa: Application layer completa + autenticação JWT + exceções + testes**

Essa é a parte mais complexa do projeto. Envolve: toda a camada Application (DTOs, interfaces e services), configuração do JWT no `Program.cs`, camada de exceções customizadas e a coleção de testes.

### 5.1 - Exceções customizadas (`AcademicEvents.Exceptions/`)

```csharp
/// <summary>
/// Lançada quando um recurso solicitado não existe no banco.
/// O controller deve capturar e retornar 404.
/// </summary>
public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message) { }
}

// outras: DuplicateEmailException (400), UnauthorizedException (403),
// InvalidCredentialsException (401), InscricaoDuplicadaException (400)
```

### 5.2 - DTOs (`AcademicEvents.Application/DTOs/`)

Um par Request/Response para cada entidade. Exemplo de auth:

```csharp
/// <summary>
/// Dados recebidos no endpoint de login.
/// </summary>
public class LoginRequest
{
    public string Email { get; set; } = string.Empty;
    public string Senha { get; set; } = string.Empty;
}

/// <summary>
/// Resposta dos endpoints de autenticação.
/// Contém o token JWT e dados básicos do usuário.
/// </summary>
public class AuthResponse
{
    public string Token { get; set; } = string.Empty;
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime ExpiraEm { get; set; }
}
```

### 5.3 - Interfaces dos services (`AcademicEvents.Application/Interfaces/`)

```csharp
/// <summary>
/// Contrato do service de autenticação.
/// </summary>
public interface IAuthService
{
    Task<AuthResponse> RegisterAsync(RegisterRequest request);
    Task<AuthResponse> LoginAsync(LoginRequest request);
}

// IEventService, ICommentService, IReactionService, IRegistrationService
// seguem o mesmo padrão
```

### 5.4 - AuthService (`AcademicEvents.Application/Services/AuthService.cs`)

Service mais complexo: faz hash com BCrypt, valida credenciais e gera o token JWT.

```csharp
/// <summary>
/// Implementação do service de autenticação.
/// Usa BCrypt para hash de senha e gera JWT Bearer Token.
/// </summary>
public class AuthService : IAuthService
{
    private readonly IUserRepository _repository;
    private readonly IConfiguration _configuration;

    public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
    {
        // não pode ter dois usuários com o mesmo email
        if (await _repository.GetByEmailAsync(request.Email) is not null)
            throw new DuplicateEmailException("Esse email já está em uso.");

        // BCrypt cuida do salt automaticamente, não precisa passar nada extra
        string senhaHash = BCrypt.Net.BCrypt.HashPassword(request.Senha);

        User usuario = new User
        {
            Nome = request.Nome,
            Email = request.Email,
            SenhaHash = senhaHash
        };

        User criado = await _repository.CreateAsync(usuario);
        return GerarTokenResponse(criado);
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        User? usuario = await _repository.GetByEmailAsync(request.Email);

        // retorna o mesmo erro independente se email ou senha estão errados (segurança)
        if (usuario is null || !BCrypt.Net.BCrypt.Verify(request.Senha, usuario.SenhaHash))
            throw new InvalidCredentialsException("Email ou senha inválidos.");

        return GerarTokenResponse(usuario);
    }

    private AuthResponse GerarTokenResponse(User usuario)
    {
        // pega as configs do appsettings pra assinar o token
        string key = _configuration["Jwt:Key"]!;
        string issuer = _configuration["Jwt:Issuer"]!;
        string audience = _configuration["Jwt:Audience"]!;
        int expiresIn = int.Parse(_configuration["Jwt:ExpiresInHours"] ?? "8");

        SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        SigningCredentials credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        Claim[] claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
            new Claim(ClaimTypes.Email, usuario.Email),
            new Claim(ClaimTypes.Name, usuario.Nome)
        };

        DateTime expira = DateTime.UtcNow.AddHours(expiresIn);

        JwtSecurityToken token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: expira,
            signingCredentials: credentials
        );

        return new AuthResponse
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            Nome = usuario.Nome,
            Email = usuario.Email,
            ExpiraEm = expira
        };
    }
}
```

### 5.5 - AuthController (`AcademicEvents.API/Controllers/AuthController.cs`)

```csharp
/// <summary>
/// Controller de autenticação. Todos os endpoints aqui são públicos.
/// </summary>
[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _service;

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        try
        {
            return Ok(await _service.RegisterAsync(request));
        }
        catch (DuplicateEmailException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        try
        {
            return Ok(await _service.LoginAsync(request));
        }
        catch (InvalidCredentialsException ex)
        {
            return Unauthorized(ex.Message);
        }
    }
}
```

### 5.6 - GET /api/me (`AcademicEvents.API/Controllers/UsersController.cs`)

```csharp
/// <summary>
/// Controller de dados do usuário autenticado.
/// </summary>
[ApiController]
[Route("api")]
public class UsersController : ControllerBase
{
    [HttpGet("me")]
    [Authorize]
    public IActionResult Me()
    {
        // as claims do JWT ficam disponíveis no User do ControllerBase
        string id = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        string email = User.FindFirst(ClaimTypes.Email)!.Value;
        string nome = User.FindFirst(ClaimTypes.Name)!.Value;

        return Ok(new { Id = id, Email = email, Nome = nome });
    }
}
```

### 5.7 - Configuração JWT no `Program.cs`

Adicionar no Program.cs depois do `AddInfrastructure`:

```csharp
// configura JWT Bearer Token
string jwtKey = builder.Configuration["Jwt:Key"]
    ?? throw new InvalidOperationException("Jwt:Key nao configurada.");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

// configura o Swagger para aceitar o token Bearer no botão Authorize
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Digite: Bearer {seu token JWT}"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            Array.Empty<string>()
        }
    });
});
```

### 5.8 - Extension method da Application

```csharp
/// <summary>
/// Registra os services da camada Application no DI do ASP.NET.
/// Chamar no Program.cs com builder.Services.AddApplication().
/// </summary>
public static class ApplicationDependencyInjectionExtension
{
    public static void AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IEventService, EventService>();
        services.AddScoped<ICommentService, CommentService>();
        services.AddScoped<IReactionService, ReactionService>();
        services.AddScoped<IRegistrationService, RegistrationService>();
    }
}
```

### 5.9 - Services restantes

`EventService`, `CommentService`, `ReactionService` e `RegistrationService` seguem o padrão do professor em `SocialService.cs`. Cada service recebe o repository via construtor, valida o request e mapeia para o DTO de resposta.

Ponto de atenção no `RegistrationService`: verificar se o usuário já está inscrito antes de criar uma nova inscrição (índice único no banco já impede, mas melhor lançar uma exceção com mensagem clara).

### 5.10 - Coleção de testes (`endpoints.http`)

Criar na raiz do projeto um arquivo `endpoints.http` com o roteiro completo de testes:

```http
### 1. Cadastrar usuário
POST http://localhost:5000/api/auth/register
Content-Type: application/json

{
  "nome": "Maria Silva",
  "email": "maria@teste.com",
  "senha": "Senha123!"
}

### 2. Fazer login e copiar o token retornado
POST http://localhost:5000/api/auth/login
Content-Type: application/json

{
  "email": "maria@teste.com",
  "senha": "Senha123!"
}

### 3. Ver dados do usuário logado (substituir TOKEN pelo valor retornado no login)
GET http://localhost:5000/api/me
Authorization: Bearer TOKEN

### 4. Criar evento (substituir TOKEN)
POST http://localhost:5000/api/events
Content-Type: application/json
Authorization: Bearer TOKEN

{
  "titulo": "Workshop de C# na UEA",
  "descricao": "Workshop prático sobre ASP.NET Core",
  "dataInicio": "2026-06-10T09:00:00",
  "dataFim": "2026-06-10T18:00:00",
  "local": "Bloco A - Sala 201"
}

### 5. Listar todos os eventos
GET http://localhost:5000/api/events

### 6. Filtrar por status
GET http://localhost:5000/api/events?status=Publicado

### 7. Se inscrever em um evento (substituir TOKEN e id)
POST http://localhost:5000/api/registrations
Content-Type: application/json
Authorization: Bearer TOKEN

{
  "eventoId": 1
}

### 8. Ver minhas inscrições
GET http://localhost:5000/api/registrations/me
Authorization: Bearer TOKEN

### 9. Comentar em um evento
POST http://localhost:5000/api/comments
Content-Type: application/json
Authorization: Bearer TOKEN

{
  "eventoId": 1,
  "conteudo": "Ótimo evento, vai aprender muito!"
}

### 10. Reagir a um evento
POST http://localhost:5000/api/reactions
Content-Type: application/json
Authorization: Bearer TOKEN

{
  "eventoId": 1,
  "tipo": "VouParticipar"
}
```

---

## Orientações gerais

- Todos precisam entender a arquitetura completa para a apresentação final
- Seguir os padrões do professor no `minisocial-project` como referência de estrutura e estilo
- Qualquer dúvida sobre JWT, ver a documentação do `Microsoft.AspNetCore.Authentication.JwtBearer`
- O Swagger deve funcionar completamente com o botão Authorize para rotas protegidas

Boa sorte pessoal!

# Roteiro de Demonstração - Payloads para Popular o Banco

Este roteiro é uma cola para usar no Swagger durante a demonstração prática da API.

URL base esperada:

```text
http://localhost:5000/swagger
```

Se a porta `5000` estiver ocupada, rode a API em outra porta, por exemplo `5002`, e troque a URL para:

```text
http://localhost:5002/swagger
```

---

## Antes de Começar

Subir o banco:

```bash
docker compose up -d
docker compose ps
```

Rodar a API:

```bash
cd AcademicEvents.API
dotnet run --urls http://localhost:5000
```

Se precisar usar a porta `5002`:

```bash
cd AcademicEvents.API
dotnet run --urls http://localhost:5002
```

---

## Variáveis para Anotar Durante a Demonstração

Preencha estes valores conforme as respostas do Swagger:

```text
TOKEN_MARIA      =
TOKEN_JOAO       =
EVENT_ID         =
ACTIVITY_ID      =
COMMENT_ID       =
REACTION_ID      =
REGISTRATION_ID  =
```

Se o banco já tiver sido usado antes e retornar erro de e-mail duplicado, troque os e-mails ou limpe o volume com:

```bash
docker compose down -v
docker compose up -d
```

---

## PARTE 1 - USUÁRIOS

### 1. Cadastrar a Maria, usuária principal

Endpoint:

```text
POST /api/auth/register
```

Payload:

```json
{
  "Name": "Maria Silva",
  "Email": "maria@test.com",
  "Password": "Password123!"
}
```

O que anotar:

```text
TOKEN_MARIA = valor do campo "Token"
```

Resposta esperada:

```json
{
  "Token": "eyJhbGci...",
  "Name": "Maria Silva",
  "Email": "maria@test.com",
  "ExpiresIn": "2026-06-04T..."
}
```

---

### 2. Cadastrar o João, segundo usuário para testar restrição de permissão

Endpoint:

```text
POST /api/auth/register
```

Payload:

```json
{
  "Name": "João Souza",
  "Email": "joao@test.com",
  "Password": "Password123!"
}
```

O que anotar:

```text
TOKEN_JOAO = valor do campo "Token"
```

Resposta esperada:

```json
{
  "Token": "eyJhbGci...",
  "Name": "João Souza",
  "Email": "joao@test.com",
  "ExpiresIn": "2026-06-04T..."
}
```

---

### 3. Fazer login com a Maria

Repita o login para obter um token novo, caso o cadastro tenha sido feito antes:

Endpoint:

```text
POST /api/auth/login
```

Payload:

```json
{
  "Email": "maria@test.com",
  "Password": "Password123!"
}
```

Resposta esperada:

```json
{
  "Token": "eyJhbGci...",
  "Name": "Maria Silva",
  "Email": "maria@test.com",
  "ExpiresIn": "2026-06-04T..."
}
```

---

### 4. Autorizar no Swagger com o token da Maria

Clique em `Authorize` e cole apenas o token, sem a palavra `Bearer`:

```text
TOKEN_MARIA
```

Em clientes HTTP externos, o formato é `Authorization: Bearer TOKEN`.

---

### 5. Conferir o usuário autenticado

Endpoint:

```text
GET /api/me
```

Não precisa de payload.

Resposta esperada:

```json
{
  "Id": "1",
  "Email": "maria@test.com",
  "Name": "Maria Silva"
}
```

---

## PARTE 2 - EVENTOS

### 6. Criar evento

Endpoint:

```text
POST /api/events
```

Payload:

```json
{
  "Title": "Workshop de C# em Manaus",
  "Description": "Workshop prático sobre ASP.NET Core, Entity Framework Core e construção de APIs REST.",
  "StartDate": "2026-06-10T09:00:00Z",
  "EndDate": "2026-06-10T18:00:00Z",
  "Location": "Bloco A - Sala 201"
}
```

O que anotar:

```text
EVENT_ID = valor do campo "Id"
```

Resposta esperada:

```json
{
  "Id": 1,
  "Title": "Workshop de C# em Manaus",
  "Status": "Draft",
  "OrganizerId": 1,
  "OrganizerName": "Maria Silva"
}
```

---

### 7. Publicar o evento

Este passo é importante para o filtro `?status=Published` encontrar o evento.

Endpoint:

```text
PUT /api/events/{EVENT_ID}
```

Payload:

```json
{
  "Title": "Workshop de C# em Manaus - Atualizado",
  "Description": "Workshop prático sobre ASP.NET Core, Entity Framework Core, autenticação JWT e construção de APIs REST.",
  "StartDate": "2026-06-10T09:00:00Z",
  "EndDate": "2026-06-10T18:00:00Z",
  "Location": "Bloco B - Auditório",
  "EventStatus": "Published"
}
```

Resposta esperada:

```json
{
  "Id": 1,
  "Title": "Workshop de C# em Manaus - Atualizado",
  "Status": "Published",
  "OrganizerId": 1,
  "OrganizerName": "Maria Silva"
}
```

---

### 8. Listar todos os eventos

Endpoint:

```text
GET /api/events
```

Não precisa de payload nem de token.

Resposta esperada:

```json
[
  {
    "Id": 1,
    "Title": "Workshop de C# em Manaus - Atualizado",
    "Status": "Published",
    "OrganizerId": 1,
    "OrganizerName": "Maria Silva"
  }
]
```

---

### 9. Filtrar eventos por status

Endpoint:

```text
GET /api/events?status=Published
```

Não precisa de payload.

Resposta esperada:

```json
[
  {
    "Id": 1,
    "Title": "Workshop de C# em Manaus - Atualizado",
    "Status": "Published",
    "OrganizerName": "Maria Silva"
  }
]
```

---

### 10. Listar meus eventos

Endpoint:

```text
GET /api/events/meus
```

Precisa estar autorizado com `TOKEN_MARIA`.

Resposta esperada:

```json
[
  {
    "Id": 1,
    "Title": "Workshop de C# em Manaus - Atualizado",
    "Status": "Published",
    "OrganizerId": 1,
    "OrganizerName": "Maria Silva"
  }
]
```

---

## PARTE 3 - ATIVIDADES

### 11. Criar primeira atividade, Cerimônia de Abertura

Endpoint:

```text
POST /api/activity
```

Payload:

```json
{
  "EventId": 1,
  "Title": "Cerimônia de Abertura",
  "Description": "Abertura oficial do Workshop de C#.",
  "Type": "Opening",
  "StartAt": "2026-06-10T09:00:00Z",
  "EndedAt": "2026-06-10T09:30:00Z",
  "Location": "Auditório Principal"
}
```

O que anotar:

```text
ACTIVITY_ID = valor do campo "Id"
```

Resposta esperada:

```json
{
  "Id": 1,
  "EventId": 1,
  "Title": "Cerimônia de Abertura",
  "Type": "Opening",
  "StartAt": "2026-06-10T09:00:00Z",
  "EndedAt": "2026-06-10T09:30:00Z",
  "Location": "Auditório Principal"
}
```

---

### 12. Criar segunda atividade, Workshop Técnico

Endpoint:

```text
POST /api/activity
```

Payload:

```json
{
  "EventId": 1,
  "Title": "Workshop ASP.NET Core",
  "Description": "Introdução prática ao ASP.NET Core.",
  "Type": "Workshop",
  "StartAt": "2026-06-10T10:00:00Z",
  "EndedAt": "2026-06-10T12:00:00Z",
  "Location": "Laboratório 01"
}
```

Resposta esperada:

```json
{
  "Id": 2,
  "EventId": 1,
  "Title": "Workshop ASP.NET Core",
  "Type": "Workshop",
  "StartAt": "2026-06-10T10:00:00Z",
  "EndedAt": "2026-06-10T12:00:00Z",
  "Location": "Laboratório 01"
}
```

---

### 13. Listar atividades do evento

Endpoint:

```text
GET /api/activity/event/{EVENT_ID}
```

Exemplo:

```text
GET /api/activity/event/1
```

Resposta esperada:

```json
[
  {
    "Id": 1,
    "EventId": 1,
    "Title": "Cerimônia de Abertura",
    "Type": "Opening",
    "StartAt": "2026-06-10T09:00:00Z",
    "EndedAt": "2026-06-10T09:30:00Z",
    "Location": "Auditório Principal"
  },
  {
    "Id": 2,
    "EventId": 1,
    "Title": "Workshop ASP.NET Core",
    "Type": "Workshop",
    "StartAt": "2026-06-10T10:00:00Z",
    "EndedAt": "2026-06-10T12:00:00Z",
    "Location": "Laboratório 01"
  }
]
```

---

## PARTE 4 - INSCRIÇÕES

### 14. Inscrever a Maria no evento

Precisa estar autorizado com `TOKEN_MARIA`.

Endpoint:

```text
POST /api/registrations
```

Payload:

```json
{
  "EventId": 1
}
```

Se o evento criado tiver outro id, troque `1` pelo valor de `EVENT_ID`.

O que anotar:

```text
REGISTRATION_ID = valor do campo "Id"
```

Resposta esperada:

```json
{
  "Id": 1,
  "EventId": 1,
  "EventTitle": "Workshop de C# em Manaus - Atualizado",
  "UserId": 1,
  "UserName": "Maria Silva",
  "Status": "Pending",
  "CreatedAt": "2026-06-03T..."
}
```

---

### 15. Tentar inscrição duplicada

Endpoint:

```text
POST /api/registrations
```

Payload:

```json
{
  "EventId": 1
}
```

Resposta esperada, erro 400:

```text
Você já está inscrito neste evento.
```

---

## PARTE 5 - COMENTÁRIOS

### 16. Adicionar comentário como Maria

Precisa estar autorizado com `TOKEN_MARIA`.

Endpoint:

```text
POST /api/comments
```

Payload:

```json
{
  "EventId": 1,
  "Content": "Ótimo evento, vai ajudar muito na prática com APIs em C#."
}
```

O que anotar:

```text
COMMENT_ID = valor do campo "Id"
```

Resposta esperada:

```json
{
  "Id": 1,
  "EventId": 1,
  "UserId": 1,
  "UserName": "Maria Silva",
  "Content": "Ótimo evento, vai ajudar muito na prática com APIs em C#.",
  "CreatedAt": "2026-06-03T..."
}
```

---

### 17. Trocar para o token do João

Clique em `Authorize` e substitua o token da Maria pelo token do João:

```text
TOKEN_JOAO
```

---

### 18. João tenta deletar o comentário da Maria, erro 403

Precisa estar autorizado com `TOKEN_JOAO`.

Endpoint:

```text
DELETE /api/comments/{COMMENT_ID}
```

Não precisa de payload.

Resposta esperada, erro 403:

```text
Apenas o autor pode remover este comentário.
```

---

### 19. Voltar para o token da Maria e listar comentários

Clique em `Authorize` e volte a usar `TOKEN_MARIA`, depois acesse o endpoint público:

Endpoint:

```text
GET /api/comments?eventId=1
```

Não precisa de token.

Resposta esperada:

```json
[
  {
    "Id": 1,
    "EventId": 1,
    "UserName": "Maria Silva",
    "Content": "Ótimo evento, vai ajudar muito na prática com APIs em C#."
  }
]
```

---

## PARTE 6 - REAÇÕES

### 20. Adicionar reação como Maria

Precisa estar autorizado com `TOKEN_MARIA`.

Endpoint:

```text
POST /api/reactions
```

Payload:

```json
{
  "EventId": 1,
  "Type": "WillParticipate"
}
```

O que anotar:

```text
REACTION_ID = valor do campo "Id"
```

Resposta esperada:

```json
{
  "Id": 1,
  "EventId": 1,
  "UserId": 1,
  "UserName": "Maria Silva",
  "Type": "WillParticipate",
  "CreatedAt": "2026-06-03T..."
}
```

---

### 21. Listar reações do evento

Endpoint:

```text
GET /api/reactions?eventId=1
```

Não precisa de token.

Resposta esperada:

```json
[
  {
    "Id": 1,
    "EventId": 1,
    "UserName": "Maria Silva",
    "Type": "WillParticipate"
  }
]
```

---

## Ordem Resumida para a Apresentação

```text
--- USUÁRIOS ---
1.  POST /api/auth/register  (Maria)
2.  POST /api/auth/register  (João)
3.  POST /api/auth/login     (Maria)
4.  Authorize com TOKEN_MARIA
5.  GET  /api/me

--- EVENTOS ---
6.  POST /api/events
7.  PUT  /api/events/{EVENT_ID}  (publicar)
8.  GET  /api/events
9.  GET  /api/events?status=Published
10. GET  /api/events/meus

--- ATIVIDADES ---
11. POST /api/activity  (Cerimônia de Abertura)
12. POST /api/activity  (Workshop ASP.NET Core)
13. GET  /api/activity/event/{EVENT_ID}

--- INSCRIÇÕES ---
14. POST /api/registrations
15. POST /api/registrations  (erro 400, duplicada)

--- COMENTÁRIOS ---
16. POST /api/comments
17. Authorize com TOKEN_JOAO
18. DELETE /api/comments/{COMMENT_ID}  (erro 403)
19. GET  /api/comments?eventId=1

--- REAÇÕES ---
20. POST /api/reactions
21. GET  /api/reactions?eventId=1
```

---

## Problemas Comuns na Hora da Aula

### Porta 5432 ocupada

Se o PostgreSQL local estiver usando a porta:

```bash
sudo systemctl stop postgresql
docker compose up -d
```

Depois da aula:

```bash
sudo systemctl start postgresql
```

### Porta 5000 ocupada

Rode a API na porta `5002`:

```bash
cd AcademicEvents.API
dotnet run --urls http://localhost:5002
```

Use:

```text
http://localhost:5002/swagger
```

### Token não funciona no Swagger

Pare a API com `Ctrl+C`, rode novamente e gere um token novo.

No Swagger:

```text
Cole apenas o token, sem Bearer.
```

Em clientes HTTP externos:

```text
Authorization: Bearer TOKEN
```

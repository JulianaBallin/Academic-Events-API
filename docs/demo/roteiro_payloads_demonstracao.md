# Roteiro de Demonstração - Payloads para Popular o Banco

Este roteiro de demonstração prática da API.

URL base esperada:

```text
http://localhost:5000/swagger
```

---

## Antes de Começar

Subir o banco:

```bash
docker compose up -d
docker compose ps
```

Se o banco já tiver sido usado antes e retornar erro limpe o volume com:

```bash
docker compose down -v
docker compose up -d
```

Rodar a API:

```bash
cd AcademicEvents.API
dotnet run --urls http://localhost:5000
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
  "name": "Maria Silva",
  "email": "maria@test.com",
  "password": "Password123!"
}
```

```text
TOKEN_MARIA = valor do token
```

Resposta esperada:

```json
{
  "token": "eyJhbGci...",
  "name": "Maria Silva",
  "email": "maria@test.com",
  "expiresIn": "2026-06-04T..."
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
  "name": "João Souza",
  "email": "joao@test.com",
  "password": "Password123!"
}
```

O que anotar:

```text
TOKEN_JOAO = valor do token
```

Resposta esperada:

```json
{
  "token": "eyJhbGci...",
  "name": "João Souza",
  "email": "joao@test.com",
  "expiresIn": "2026-06-04T..."
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
  "email": "maria@test.com",
  "password": "Password123!"
}
```

Resposta esperada:

```json
{
  "token": "eyJhbGci...",
  "name": "Maria Silva",
  "email": "maria@test.com",
  "expiresIn": "2026-06-04T..."
}
```

---

### 4. Autorizar no Swagger com o token da Maria

Em `Authorize` colar o token:

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
  "id": "1",
  "email": "maria@test.com",
  "name": "Maria Silva"
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
  "title": "Workshop de C# em Manaus",
  "description": "Workshop prático sobre ASP.NET Core, Entity Framework Core e construção de APIs REST.",
  "startDate": "2026-06-10T09:00:00Z",
  "endDate": "2026-06-10T18:00:00Z",
  "location": "Bloco A - Sala 201"
}
```

```text
EVENT_ID = valor do id
```

Resposta esperada:

```json
{
  "id": 1,
  "title": "Workshop de C# em Manaus",
  "status": "Draft",
  "organizerId": 1,
  "organizerName": "Maria Silva"
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
  "title": "Workshop de C# em Manaus - Atualizado",
  "description": "Workshop prático sobre ASP.NET Core, Entity Framework Core, autenticação JWT e construção de APIs REST.",
  "startDate": "2026-06-10T09:00:00Z",
  "endDate": "2026-06-10T18:00:00Z",
  "location": "Bloco B - Auditório",
  "eventStatus": "Published"
}
```

Resposta esperada:

```json
{
  "id": 1,
  "title": "Workshop de C# em Manaus - Atualizado",
  "status": "Published",
  "organizerId": 1,
  "organizerName": "Maria Silva"
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
    "id": 1,
    "title": "Workshop de C# em Manaus - Atualizado",
    "status": "Published",
    "organizerId": 1,
    "organizerName": "Maria Silva"
  }
]
```

---

### 9. Filtrar eventos por status

No Swagger, use o mesmo endpoint `GET /api/events` e preencha o campo `status` com o valor `Published`.

Endpoint:

```text
GET /api/events?status=Published
```

Não precisa de payload.

Resposta esperada:

```json
[
  {
    "id": 1,
    "title": "Workshop de C# em Manaus - Atualizado",
    "status": "Published",
    "organizerName": "Maria Silva"
  }
]
```

---

### 10. Listar meus eventos

Endpoint:

```text
GET /api/events/mine
```

Precisa estar autorizado com `TOKEN_MARIA`.

Resposta esperada:

```json
[
  {
    "id": 1,
    "title": "Workshop de C# em Manaus - Atualizado",
    "status": "Published",
    "organizerId": 1,
    "organizerName": "Maria Silva"
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
  "eventId": 1,
  "title": "Cerimônia de Abertura",
  "description": "Abertura oficial do Workshop de C#.",
  "type": "Opening",
  "startAt": "2026-06-10T09:00:00Z",
  "endedAt": "2026-06-10T09:30:00Z",
  "location": "Auditório Principal"
}
```

O que anotar:

```text
ACTIVITY_ID = valor do campo "id"
```

Resposta esperada:

```json
{
  "id": 1,
  "eventId": 1,
  "title": "Cerimônia de Abertura",
  "type": "Opening",
  "startAt": "2026-06-10T09:00:00Z",
  "endedAt": "2026-06-10T09:30:00Z",
  "location": "Auditório Principal"
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
  "eventId": 1,
  "title": "Workshop ASP.NET Core",
  "description": "Introdução prática ao ASP.NET Core.",
  "type": "Workshop",
  "startAt": "2026-06-10T10:00:00Z",
  "endedAt": "2026-06-10T12:00:00Z",
  "location": "Laboratório 01"
}
```

Resposta esperada:

```json
{
  "id": 2,
  "eventId": 1,
  "title": "Workshop ASP.NET Core",
  "type": "Workshop",
  "startAt": "2026-06-10T10:00:00Z",
  "endedAt": "2026-06-10T12:00:00Z",
  "location": "Laboratório 01"
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
    "id": 1,
    "eventId": 1,
    "title": "Cerimônia de Abertura",
    "type": "Opening",
    "startAt": "2026-06-10T09:00:00Z",
    "endedAt": "2026-06-10T09:30:00Z",
    "location": "Auditório Principal"
  },
  {
    "id": 2,
    "eventId": 1,
    "title": "Workshop ASP.NET Core",
    "type": "Workshop",
    "startAt": "2026-06-10T10:00:00Z",
    "endedAt": "2026-06-10T12:00:00Z",
    "location": "Laboratório 01"
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
  "eventId": 1
}
```

Se o evento criado tiver outro id, troque `1` pelo valor de `EVENT_ID`.

O que anotar:

```text
REGISTRATION_ID = valor do campo "id"
```

Resposta esperada:

```json
{
  "id": 1,
  "eventId": 1,
  "eventTitle": "Workshop de C# em Manaus - Atualizado",
  "userId": 1,
  "userName": "Maria Silva",
  "status": "Pending",
  "createdAt": "2026-06-03T..."
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
  "eventId": 1
}
```

Resposta esperada, erro 400:

```text
The user is already registered for this event.
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
  "eventId": 1,
  "content": "Ótimo evento, vai ajudar muito na prática com APIs em C#."
}
```

O que anotar:

```text
COMMENT_ID = valor do campo "id"
```

Resposta esperada:

```json
{
  "id": 1,
  "eventId": 1,
  "userId": 1,
  "userName": "Maria Silva",
  "content": "Ótimo evento, vai ajudar muito na prática com APIs em C#.",
  "createdAt": "2026-06-03T..."
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
    "id": 1,
    "eventId": 1,
    "userName": "Maria Silva",
    "content": "Ótimo evento, vai ajudar muito na prática com APIs em C#."
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
  "eventId": 1,
  "type": "WillParticipate"
}
```

O que anotar:

```text
REACTION_ID = valor do campo "id"
```

Resposta esperada:

```json
{
  "id": 1,
  "eventId": 1,
  "userId": 1,
  "userName": "Maria Silva",
  "type": "WillParticipate",
  "createdAt": "2026-06-03T..."
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
    "id": 1,
    "eventId": 1,
    "userName": "Maria Silva",
    "type": "WillParticipate"
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
7.  PUT  /api/events/{EVENT_ID}  (publicar com eventStatus=Published)
8.  GET  /api/events
9.  GET  /api/events  (preencher campo status=Published no Swagger)
10. GET  /api/events/mine

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
20. Authorize com TOKEN_MARIA
21. POST /api/reactions
22. GET  /api/reactions?eventId=1
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

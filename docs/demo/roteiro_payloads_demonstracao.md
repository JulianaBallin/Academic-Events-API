# Roteiro de Demonstração: Payloads para Popular o Banco

URL:

```text
http://localhost:5000/swagger
```

## Antes de Começar

Subir o banco:

```bash
docker compose up -d
docker compose ps
``

```bash
docker compose down -v
docker compose up -d
```

Rodar a API:

```bash
cd AcademicEvents.API
dotnet run --urls http://localhost:5000
```

## PARTE 1: USUÁRIOS

### 1. Cadastrar o João

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

```text
TOKEN_JOAO = eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjEiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9lbWFpbGFkZHJlc3MiOiJqb2FvQHRlc3QuY29tIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZSI6Ikpvw6NvIFNvdXphIiwiZXhwIjoxNzgwNTM3OTgzLCJpc3MiOiJBY2FkZW1pY0V2ZW50c0FQSSIsImF1ZCI6IkFjYWRlbWljRXZlbnRzQ2xpZW50ZXMifQ.UoD4H-BAWx9fzfHMucncTGhbH-WpCwEQzAV-5wTg6pM
```

### 2. Cadastrar a Maria

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

### 3. Fazer login com a Maria

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

Anotar:

```text
TOKEN_MARIA = eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjIiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9lbWFpbGFkZHJlc3MiOiJtYXJpYUB0ZXN0LmNvbSIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWUiOiJNYXJpYSBTaWx2YSIsImV4cCI6MTc4MDUzODA2MCwiaXNzIjoiQWNhZGVtaWNFdmVudHNBUEkiLCJhdWQiOiJBY2FkZW1pY0V2ZW50c0NsaWVudGVzIn0.DV30rPCCvkS4A104cyOFgmoriSaoWewYe7ZgNUsGO2M
```

### 4. Autorizar no Swagger com o token da Maria

Clique em `Authorize` e cole o token da Maria.

## PARTE 2: EVENTOS

### 5. Criar evento

Precisa estar autorizado com `TOKEN_MARIA`.

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

### 6. Publicar o evento

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

### 7. Listar todos os eventos

Endpoint:

```text
GET /api/events
```

Não precisa de payload nem de token.


### 8. Filtrar eventos por status

Endpoint:

```text
GET /api/events?status=Published
```

Não precisa de payload.


### 9. Listar meus eventos

Precisa estar autorizado com `TOKEN_MARIA`.

Endpoint:

```text
GET /api/events/mine
```


## PARTE 3: ATIVIDADES

### 10. Criar primeira atividade, Cerimônia de Abertura

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

### 11. Listar atividades do evento

Endpoint:

```text
GET /api/activity/event/{EVENT_ID}
```

## PARTE 4: INSCRIÇÕES

### 12. Inscrever a Maria no evento

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

### 13. Tentar inscrição duplicada

Payload:

```json
{
  "eventId": 1
}
```

## PARTE 5: COMENTÁRIOS

### 14. Maria adiciona um comentário

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


### 15. Listar comentários do evento

Endpoint:

```text
GET /api/comments?eventId=1
```

## PARTE 6: REAÇÕES

### 16. Trocar para o token do João

```text
TOKEN_JOAO
```

### 17. João adiciona uma reação

Precisa estar autorizado com `TOKEN_JOAO`.

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


### 18. Listar reações do evento

Endpoint:

```text
GET /api/reactions?eventId=1
```

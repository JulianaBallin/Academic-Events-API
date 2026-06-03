# Roteiro de Demonstração: Payloads para Popular o Banco

Este roteiro é uma cola rápida para a demonstração prática da API no Swagger.

URL base esperada:

```text
http://localhost:5000/swagger
```

## Antes de Começar

Subir o banco:

```bash
docker compose up -d
docker compose ps
```

Se o banco já tiver sido usado antes e retornar erro de e-mail duplicado, limpe o volume:

```bash
docker compose down -v
docker compose up -d
```

Rodar a API:

```bash
cd AcademicEvents.API
dotnet run --urls http://localhost:5000
```

## Valores para Anotar

```text
TOKEN_JOAO  =
TOKEN_MARIA =
EVENT_ID    =
```

Se o banco estiver limpo, o primeiro evento normalmente terá `id` igual a `1`. Se vier outro valor na resposta, troque `1` pelo valor real do `EVENT_ID` nos próximos payloads.

## PARTE 1: USUÁRIOS

### 1. Cadastrar o João

Vou criar o João primeiro para guardar um segundo token. No fim ele será usado para reagir ao evento.

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

Retorno esperado:

```text
200 OK com token, name, email e expiresIn.
```

Anotar:

```text
TOKEN_JOAO = valor do campo token
```

### 2. Cadastrar a Maria

Maria será a usuária principal da demonstração.

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

Retorno esperado:

```text
200 OK com token, name, email e expiresIn.
```

### 3. Fazer login com a Maria

Vou fazer login para pegar um token novo da Maria.

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

Retorno esperado:

```text
200 OK com token, name, email e expiresIn.
```

Anotar:

```text
TOKEN_MARIA = valor do campo token
```

### 4. Autorizar no Swagger com o token da Maria

Clique em `Authorize` e cole o token da Maria.

```text
TOKEN_MARIA
```

### 5. Conferir o usuário autenticado

Endpoint:

```text
GET /api/me
```

Não precisa de payload.

Retorno esperado:

```json
{
  "id": "2",
  "email": "maria@test.com",
  "name": "Maria Silva"
}
```

Se a Maria tiver outro `id`, tudo bem. O importante é aparecer o e-mail e o nome dela.

## PARTE 2: EVENTOS

### 6. Criar evento

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

Retorno esperado:

```text
201 Created com id, status Draft, organizerId e organizerName Maria Silva.
```

Anotar:

```text
EVENT_ID = valor do campo id
```

### 7. Publicar o evento

A rota `PUT` atualiza o evento completo. Para publicar, mantenha os dados do evento e altere `eventStatus` para `Published`.

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

Retorno esperado:

```text
200 OK com status Published.
```

### 8. Listar todos os eventos

Endpoint:

```text
GET /api/events
```

Não precisa de payload nem de token.

Retorno esperado:

```text
200 OK com lista contendo o evento criado.
```

### 9. Filtrar eventos por status

No Swagger, use o mesmo endpoint `GET /api/events` e preencha o campo `status` com `Published`.

Endpoint:

```text
GET /api/events?status=Published
```

Não precisa de payload.

Retorno esperado:

```text
200 OK com lista contendo o evento publicado.
```

### 10. Listar meus eventos

Precisa estar autorizado com `TOKEN_MARIA`.

Endpoint:

```text
GET /api/events/mine
```

Retorno esperado:

```text
200 OK com os eventos criados pela Maria.
```

## PARTE 3: ATIVIDADES

### 11. Criar primeira atividade, Cerimônia de Abertura

Precisa estar autorizado com `TOKEN_MARIA`.

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

Retorno esperado:

```text
201 Created com os dados da atividade.
```

### 12. Criar segunda atividade, Workshop Técnico

Precisa estar autorizado com `TOKEN_MARIA`.

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

Retorno esperado:

```text
201 Created com os dados da atividade.
```

### 13. Listar atividades do evento

Endpoint:

```text
GET /api/activity/event/{EVENT_ID}
```

Exemplo:

```text
GET /api/activity/event/1
```

Retorno esperado:

```text
200 OK com Cerimônia de Abertura e Workshop ASP.NET Core.
```

## PARTE 4: INSCRIÇÕES

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

Retorno esperado:

```text
201 Created com status Pending e userName Maria Silva.
```

### 15. Tentar inscrição duplicada

Precisa continuar autorizado com `TOKEN_MARIA`.

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

Retorno esperado:

```text
400 Bad Request com a mensagem The user is already registered for this event.
```

## PARTE 5: COMENTÁRIOS

### 16. Maria adiciona um comentário

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

Retorno esperado:

```text
201 Created com userName Maria Silva e o conteúdo do comentário.
```

### 17. Listar comentários do evento

Endpoint:

```text
GET /api/comments?eventId=1
```

Não precisa de token.

Retorno esperado:

```text
200 OK com o comentário da Maria.
```

## PARTE 6: REAÇÕES

### 18. Trocar para o token do João

Clique em `Authorize`, remova o token da Maria e cole o token do João.

```text
TOKEN_JOAO
```

### 19. João adiciona uma reação

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

Retorno esperado:

```text
201 Created com userName João Souza e type WillParticipate.
```

### 20. Listar reações do evento

Endpoint:

```text
GET /api/reactions?eventId=1
```

Não precisa de token.

Retorno esperado:

```text
200 OK com a reação do João.
```

## Ordem Resumida para a Apresentação

```text
1.  POST /api/auth/register  (João)
2.  POST /api/auth/register  (Maria)
3.  POST /api/auth/login     (Maria)
4.  Authorize com TOKEN_MARIA
5.  GET  /api/me
6.  POST /api/events
7.  PUT  /api/events/{EVENT_ID}
8.  GET  /api/events
9.  GET  /api/events?status=Published
10. GET  /api/events/mine
11. POST /api/activity
12. POST /api/activity
13. GET  /api/activity/event/{EVENT_ID}
14. POST /api/registrations
15. POST /api/registrations  (erro de inscrição duplicada)
16. POST /api/comments       (Maria comenta)
17. GET  /api/comments?eventId=1
18. Authorize com TOKEN_JOAO
19. POST /api/reactions      (João reage)
20. GET  /api/reactions?eventId=1
```

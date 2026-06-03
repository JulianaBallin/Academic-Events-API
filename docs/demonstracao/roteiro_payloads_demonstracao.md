# Roteiro de Demonstracao - Payloads para Popular o Banco

Este roteiro foi pensado para usar no Swagger durante a apresentacao ao vivo.

URL base esperada:

```text
http://localhost:5000/swagger
```

Se a porta `5000` estiver ocupada, rode a API em outra porta, por exemplo `5002`, e troque a URL para:

```text
http://localhost:5002/swagger
```

---

## Antes de Comecar

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

## Variaveis para Anotar Durante a Demo

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

Se o banco ja tiver sido usado antes e retornar erro de email duplicado, troque os emails ou limpe o volume com:

```bash
docker compose down -v
docker compose up -d
```

---

## PARTE 1 - USUARIOS

### 1. Cadastrar a Maria (usuario principal)

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

Evidencia para print:

- Resposta 200 com token JWT
- Campos `Name`, `Email` e `ExpiresIn`

---

### 2. Cadastrar o Joao (segundo usuario para testar restricao de permissao)

Endpoint:

```text
POST /api/auth/register
```

Payload:

```json
{
  "Name": "Joao Souza",
  "Email": "joao@test.com",
  "Password": "Password123!"
}
```

O que anotar:

```text
TOKEN_JOAO = valor do campo "Token"
```

---

### 3. Fazer Login com a Maria

Repita o login para obter um token fresco, caso o cadastro tenha sido feito antes:

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

### 4. Autorizar no Swagger com o Token da Maria

Clique em `Authorize` e cole apenas o token, sem a palavra `Bearer`:

```text
TOKEN_MARIA
```

Em clientes HTTP externos o formato e `Authorization: Bearer TOKEN`.

---

### 5. Conferir o Usuario Autenticado

Endpoint:

```text
GET /api/me
```

Nao precisa de payload.

Resposta esperada:

```json
{
  "Id": "1",
  "Email": "maria@test.com",
  "Name": "Maria Silva"
}
```

Evidencia para print:

- Resposta 200 de endpoint protegido

---

## PARTE 2 - EVENTOS

### 6. Criar Evento

Endpoint:

```text
POST /api/events
```

Payload:

```json
{
  "Title": "Workshop de C# em Manaus",
  "Description": "Workshop pratico sobre ASP.NET Core, Entity Framework Core e construcao de APIs REST.",
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

Evidencia para print:

- Resposta 201
- Campo `Status` com valor `Draft`
- Campo `OrganizerName` preenchido

---

### 7. Publicar o Evento

Passo importante para o filtro `?status=Published` mostrar o evento.

Endpoint:

```text
PUT /api/events/{EVENT_ID}
```

Payload:

```json
{
  "Title": "Workshop de C# em Manaus - Atualizado",
  "Description": "Workshop pratico sobre ASP.NET Core, Entity Framework Core, autenticacao JWT e construcao de APIs REST.",
  "StartDate": "2026-06-10T09:00:00Z",
  "EndDate": "2026-06-10T18:00:00Z",
  "Location": "Bloco B - Auditorio",
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

Evidencia para print:

- Resposta 200
- `Status` igual a `Published`

---

### 8. Listar Todos os Eventos

Endpoint:

```text
GET /api/events
```

Nao precisa de payload nem de token.

Evidencia para print:

- Lista contendo o evento criado

---

### 9. Filtrar Eventos por Status

Endpoint:

```text
GET /api/events?status=Published
```

Nao precisa de payload.

Evidencia para print:

- Lista contendo apenas o evento publicado

---

### 10. Listar Meus Eventos

Endpoint:

```text
GET /api/events/meus
```

Precisa estar autorizado com `TOKEN_MARIA`.

Evidencia para print:

- Lista de eventos criados pela Maria

---

## PARTE 3 - ATIVIDADES

### 11. Criar Primeira Atividade (Cerimonia de Abertura)

Endpoint:

```text
POST /api/activity
```

Payload:

```json
{
  "EventId": 1,
  "Title": "Cerimonia de Abertura",
  "Description": "Abertura oficial do Workshop de C#.",
  "Type": "Opening",
  "StartAt": "2026-06-10T09:00:00Z",
  "EndedAt": "2026-06-10T09:30:00Z",
  "Location": "Auditorio Principal"
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
  "Title": "Cerimonia de Abertura",
  "Type": "Opening",
  "StartAt": "2026-06-10T09:00:00Z",
  "EndedAt": "2026-06-10T09:30:00Z",
  "Location": "Auditorio Principal"
}
```

---

### 12. Criar Segunda Atividade (Workshop Tecnico)

Endpoint:

```text
POST /api/activity
```

Payload:

```json
{
  "EventId": 1,
  "Title": "Workshop ASP.NET Core",
  "Description": "Introducao pratica ao ASP.NET Core.",
  "Type": "Workshop",
  "StartAt": "2026-06-10T10:00:00Z",
  "EndedAt": "2026-06-10T12:00:00Z",
  "Location": "Laboratorio 01"
}
```

---

### 13. Listar Atividades do Evento

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
    "Title": "Cerimonia de Abertura",
    "Type": "Opening",
    "StartAt": "2026-06-10T09:00:00Z",
    "EndedAt": "2026-06-10T09:30:00Z",
    "Location": "Auditorio Principal"
  },
  {
    "Id": 2,
    "EventId": 1,
    "Title": "Workshop ASP.NET Core",
    "Type": "Workshop",
    "StartAt": "2026-06-10T10:00:00Z",
    "EndedAt": "2026-06-10T12:00:00Z",
    "Location": "Laboratorio 01"
  }
]
```

Evidencia para print:

- Lista com as duas atividades ordenadas por horario de inicio

---

## PARTE 4 - INSCRICOES

### 14. Inscrever a Maria no Evento

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

Se o evento criado tiver outro id, trocar `1` pelo valor de `EVENT_ID`.

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

Evidencia para print:

- Resposta 201 com inscricao criada

---

### 15. Tentar Inscricao Duplicada

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

Resposta esperada (erro 400):

```text
Voce ja esta inscrito neste evento.
```

Evidencia para print:

- Resposta 400
- Mensagem de inscricao duplicada

---

## PARTE 5 - COMENTARIOS

### 16. Adicionar Comentario (como Maria)

Precisa estar autorizado com `TOKEN_MARIA`.

Endpoint:

```text
POST /api/comments
```

Payload:

```json
{
  "EventId": 1,
  "Content": "Otimo evento, vai ajudar muito na pratica com APIs em C#."
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
  "Content": "Otimo evento, vai ajudar muito na pratica com APIs em C#.",
  "CreatedAt": "2026-06-03T..."
}
```

Evidencia para print:

- Resposta 201 com campo `UserName` preenchido

---

### 17. Trocar para o Token do Joao

Clique em `Authorize` e substitua o token da Maria pelo token do Joao:

```text
TOKEN_JOAO
```

---

### 18. Joao Tenta Deletar o Comentario da Maria (erro 403)

Precisa estar autorizado com `TOKEN_JOAO`.

Endpoint:

```text
DELETE /api/comments/{COMMENT_ID}
```

Nao precisa de payload.

Resposta esperada (erro 403):

```text
Apenas o autor pode remover este comentario.
```

Evidencia para print:

- Resposta 403
- Mensagem de permissao negada

---

### 19. Voltar para o Token da Maria e Listar Comentarios

Clique em `Authorize` e volte a usar `TOKEN_MARIA`, depois acesse o endpoint publico:

Endpoint:

```text
GET /api/comments?eventId=1
```

Nao precisa de token.

Evidencia para print:

- Comentario da Maria visivel publicamente

---

## PARTE 6 - REACOES

### 20. Adicionar Reacao (como Maria)

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

Evidencia para print:

- Resposta 201 com `Type` igual a `WillParticipate`

---

### 21. Listar Reacoes do Evento

Endpoint:

```text
GET /api/reactions?eventId=1
```

Nao precisa de token.

Evidencia para print:

- Reacao da Maria visivel publicamente

---

## Ordem Resumida para a Apresentacao

```text
--- USUARIOS ---
1.  POST /api/auth/register  (Maria)
2.  POST /api/auth/register  (Joao)
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
11. POST /api/activity  (Cerimonia de Abertura)
12. POST /api/activity  (Workshop ASP.NET Core)
13. GET  /api/activity/event/{EVENT_ID}

--- INSCRICOES ---
14. POST /api/registrations
15. POST /api/registrations  (erro 400 - duplicada)

--- COMENTARIOS ---
16. POST /api/comments
17. Authorize com TOKEN_JOAO
18. DELETE /api/comments/{COMMENT_ID}  (erro 403)
19. GET  /api/comments?eventId=1

--- REACOES ---
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

### Token nao funciona no Swagger

Pare a API com `Ctrl+C`, rode novamente e gere um token novo.

No Swagger:

```text
Cole apenas o token, sem Bearer.
```

Em clientes HTTP externos:

```text
Authorization: Bearer TOKEN
```

# Demo Script - Payloads for Populating the Database

This script is designed for use in Swagger during the live presentation.

Expected base URL:

```text
http://localhost:5000/swagger
```

If port `5000` is busy, run the API on another port, for example `5002`, and change the URL to:

```text
http://localhost:5002/swagger
```

## Before Starting

Start the database:

```bash
docker compose up -d
docker compose ps
```

Run the API:

```bash
cd AcademicEvents.API
dotnet run --urls http://localhost:5000
```

If you need to use port `5002`:

```bash
cd AcademicEvents.API
dotnet run --urls http://localhost:5002
```

## Variables to Note During the Demo

Fill in these values as you get responses from Swagger:

```text
TOKEN_MARIA =
TOKEN_JOAO  =
EVENT_ID    =
ACTIVITY_ID =
COMMENT_ID  =
REACTION_ID =
REGISTRATION_ID =
```

If the database was used before and returns a duplicate email error, use new emails such as `maria1@test.com` and `joao1@test.com`, or clear the database volume with:

```bash
docker compose down -v
docker compose up -d
```

## 1. Register Main User

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

What to note:

```text
TOKEN_MARIA = value of the "Token" field
```

Evidence for screenshot:

- Response 200 with JWT token
- Fields `Name`, `Email` and `ExpiresIn`

## 2. Log In with Main User

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

What to note:

```text
TOKEN_MARIA = value of the "Token" field
```

Expected response:

```json
{
  "Token": "eyJhbGci...",
  "Name": "Maria Silva",
  "Email": "maria@test.com",
  "ExpiresIn": "2026-06-04T..."
}
```

Evidence for screenshot:

- Login returning 200
- JWT token returned by login

## 3. Authorize in Swagger

Click `Authorize`.

Paste only the token:

```text
TOKEN_MARIA
```

Note:

- In Swagger, paste only the token without writing `Bearer`.
- In external HTTP clients, the recommended format is `Authorization: Bearer TOKEN`.

## 4. Check the Authenticated User

Endpoint:

```text
GET /api/me
```

No payload needed.

Expected response:

```json
{
  "Id": "1",
  "Email": "maria@test.com",
  "Name": "Maria Silva"
}
```

Evidence for screenshot:

- 200 response from protected endpoint

## 5. Create Event

Endpoint:

```text
POST /api/events
```

Payload:

```json
{
  "Title": "C# Workshop in Manaus",
  "Description": "Hands-on workshop on ASP.NET Core, Entity Framework Core and REST API development.",
  "StartDate": "2026-06-10T09:00:00Z",
  "EndDate": "2026-06-10T18:00:00Z",
  "Location": "Block A - Room 201"
}
```

What to note:

```text
EVENT_ID = value of the "Id" field
```

Expected response:

```json
{
  "Id": 1,
  "Title": "C# Workshop in Manaus",
  "Description": "Hands-on workshop on ASP.NET Core, Entity Framework Core and REST API development.",
  "StartDate": "2026-06-10T09:00:00Z",
  "EndDate": "2026-06-10T18:00:00Z",
  "Location": "Block A - Room 201",
  "Status": "Draft",
  "OrganizerId": 1,
  "OrganizerName": "Maria Silva",
  "CreatedAt": "2026-06-03T..."
}
```

Evidence for screenshot:

- Response 201
- `Status` initial value as `Draft`
- `OrganizerName` populated

## 6. Publish the Event

This step is important so the `GET /api/events?status=Published` filter shows the event.

Endpoint:

```text
PUT /api/events/{EVENT_ID}
```

Payload:

```json
{
  "Title": "C# Workshop in Manaus - Updated",
  "Description": "Hands-on workshop on ASP.NET Core, Entity Framework Core, JWT authentication and REST API development.",
  "StartDate": "2026-06-10T09:00:00Z",
  "EndDate": "2026-06-10T18:00:00Z",
  "Location": "Block B - Auditorium",
  "EventStatus": "Published"
}
```

Expected response:

```json
{
  "Id": 1,
  "Title": "C# Workshop in Manaus - Updated",
  "Status": "Published",
  "OrganizerId": 1,
  "OrganizerName": "Maria Silva"
}
```

Evidence for screenshot:

- Response 200
- Status `Published`

## 7. Create Activities

Endpoint:

```text
POST /api/activity
```

First activity payload:

```json
{
  "EventId": 1,
  "Title": "Opening Ceremony",
  "Description": "Official opening of the C# Workshop.",
  "Type": "Opening",
  "StartAt": "2026-06-10T09:00:00Z",
  "EndedAt": "2026-06-10T09:30:00Z",
  "Location": "Main Auditorium"
}
```

What to note:

```text
ACTIVITY_ID = value of the "Id" field
```

Expected response:

```json
{
  "Id": 1,
  "EventId": 1,
  "Title": "Opening Ceremony",
  "Description": "Official opening of the C# Workshop.",
  "Type": "Opening",
  "StartAt": "2026-06-10T09:00:00Z",
  "EndedAt": "2026-06-10T09:30:00Z",
  "Location": "Main Auditorium"
}
```

Second activity payload:

```json
{
  "EventId": 1,
  "Title": "ASP.NET Core Workshop",
  "Description": "Hands-on introduction to ASP.NET Core.",
  "Type": "Workshop",
  "StartAt": "2026-06-10T10:00:00Z",
  "EndedAt": "2026-06-10T12:00:00Z",
  "Location": "Lab 01"
}
```

## 8. List Event Activities

Endpoint:

```text
GET /api/activity/event/{EVENT_ID}
```

Example:

```text
GET /api/activity/event/1
```

Expected response:

```json
[
  {
    "Id": 1,
    "Title": "Opening Ceremony",
    "Description": "Official opening of the C# Workshop.",
    "Type": "Opening",
    "StartAt": "2026-06-10T09:00:00Z",
    "EndedAt": "2026-06-10T09:30:00Z",
    "Location": "Main Auditorium",
    "EventId": 1
  },
  {
    "Id": 2,
    "Title": "ASP.NET Core Workshop",
    "Description": "Hands-on introduction to ASP.NET Core.",
    "Type": "Workshop",
    "StartAt": "2026-06-10T10:00:00Z",
    "EndedAt": "2026-06-10T12:00:00Z",
    "Location": "Lab 01",
    "EventId": 1
  }
]
```

Evidence for screenshot:

- List containing the two activities

## 9. List Public Events

Endpoint:

```text
GET /api/events
```

No payload needed.

Evidence for screenshot:

- List containing the created event

## 10. Filter Events by Status

Endpoint:

```text
GET /api/events?status=Published
```

No payload needed.

Evidence for screenshot:

- List containing the published event

## 11. List My Events

Endpoint:

```text
GET /api/events/meus
```

No payload needed.

Must be authorized with:

```text
TOKEN_MARIA
```

Evidence for screenshot:

- List of events created by Maria

## 12. Register for Event

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

If the created event has a different id, replace `1` with the value of `EVENT_ID`.

What to note:

```text
REGISTRATION_ID = value of the "Id" field
```

Expected response:

```json
{
  "Id": 1,
  "EventId": 1,
  "EventTitle": "C# Workshop in Manaus - Updated",
  "UserId": 1,
  "UserName": "Maria Silva",
  "Status": "Pending",
  "CreatedAt": "2026-06-03T..."
}
```

Evidence for screenshot:

- Response 201 with registration created

## 13. Try Duplicate Registration

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

If the created event has a different id, replace `1` with the value of `EVENT_ID`.

Expected response:

```text
Você já está inscrito neste evento.
```

Evidence for screenshot:

- Response 400
- Duplicate registration message

## 14. Add Comment

Endpoint:

```text
POST /api/comments
```

Payload:

```json
{
  "EventId": 1,
  "Content": "Great event, it will really help with API development in C#."
}
```

If the created event has a different id, replace `1` with the value of `EVENT_ID`.

What to note:

```text
COMMENT_ID = value of the "Id" field
```

Expected response:

```json
{
  "Id": 1,
  "EventId": 1,
  "UserId": 1,
  "UserName": "Maria Silva",
  "Content": "Great event, it will really help with API development in C#.",
  "CreatedAt": "2026-06-03T..."
}
```

Evidence for screenshot:

- Response 201
- Comment with `UserName`

## 15. Add Reaction

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

If the created event has a different id, replace `1` with the value of `EVENT_ID`.

What to note:

```text
REACTION_ID = value of the "Id" field
```

Expected response:

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

Evidence for screenshot:

- Response 201
- Reaction with `Type` equal to `WillParticipate`

## 16. Register Second User to Test 403

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

What to note:

```text
TOKEN_JOAO = value of the "Token" field
```

Then click `Authorize` and replace Maria's token with Joao's token:

```text
TOKEN_JOAO
```

## 17. Try Deleting Another User's Comment

Endpoint:

```text
DELETE /api/comments/{COMMENT_ID}
```

No payload needed.

Must be authorized with:

```text
TOKEN_JOAO
```

Expected response:

```text
Apenas o autor pode remover este comentário.
```

Evidence for screenshot:

- Response 403
- Permission denied message

## 18. Extra Evidence: List Comments

Endpoint:

```text
GET /api/comments?eventId=1
```

If the created event has a different id, replace `1` with the value of `EVENT_ID`.

No token needed.

Evidence for screenshot:

- Maria's comment visible publicly

## 19. Extra Evidence: List Reactions

Endpoint:

```text
GET /api/reactions?eventId=1
```

If the created event has a different id, replace `1` with the value of `EVENT_ID`.

No token needed.

Evidence for screenshot:

- Maria's reaction visible publicly

## 20. Summary Order for the Presentation

```text
1.  Open Swagger
2.  POST /api/auth/register with Maria
3.  POST /api/auth/login with Maria
4.  Authorize with TOKEN_MARIA
5.  GET /api/me
6.  POST /api/events
7.  POST /api/activity (Opening Ceremony)
8.  POST /api/activity (ASP.NET Core Workshop)
9.  GET /api/activity/event/{EVENT_ID}
10. PUT /api/events/{EVENT_ID} with status Published
11. GET /api/events
12. GET /api/events?status=Published
13. GET /api/events/meus
14. POST /api/registrations
15. POST /api/registrations again for error 400
16. POST /api/comments
17. POST /api/reactions
18. POST /api/auth/register with Joao
19. Authorize with TOKEN_JOAO
20. DELETE /api/comments/{COMMENT_ID} for error 403
```

## 21. Common Issues During Class

### Port 5432 busy

If local PostgreSQL is using the port:

```bash
sudo systemctl stop postgresql
docker compose up -d
```

After class:

```bash
sudo systemctl start postgresql
```

### Port 5000 busy

Run the API on port `5002`:

```bash
cd AcademicEvents.API
dotnet run --urls http://localhost:5002
```

Use:

```text
http://localhost:5002/swagger
```

### Token not working in Swagger

Stop the API with `Ctrl+C`, run it again and generate a new token.

In Swagger:

```text
Paste only the token, without Bearer.
```

In external HTTP clients:

```text
Authorization: Bearer TOKEN
```

# Roteiro de Demonstração - Payloads para Popular o Banco

Este roteiro foi pensado para usar no Swagger durante a apresentação ao vivo.

URL base esperada:

```text
http://localhost:5000/swagger
```

Se a porta `5000` estiver ocupada, rode a API em outra porta, por exemplo `5002`, e troque a URL para:

```text
http://localhost:5002/swagger
```

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

## Variáveis para Anotar Durante a Demo

Preencha estes valores conforme as respostas do Swagger:

```text
TOKEN_MARIA =
TOKEN_JOAO =
EVENTO_ID =
ACTIVITY_ID =
COMENTARIO_ID =
REACAO_ID =
INSCRICAO_ID =
```

Observação: a linha acima usa `REACAO_ID` sem acento para facilitar copiar e colar em terminal.

Se o banco já tiver sido usado antes e retornar erro de email duplicado, troque os emails por novos valores, por exemplo `maria1@teste.com` e `joao1@teste.com`, ou limpe o volume do banco com:

```bash
docker compose down -v
docker compose up -d
```

## 1. Cadastrar Usuário Principal

Endpoint:

```text
POST /api/auth/register
```

Payload:

```json
{
  "nome": "Maria Silva",
  "email": "maria@teste.com",
  "senha": "Senha123!"
}
```

O que anotar:

```text
TOKEN_MARIA = valor do campo "token"
```

Evidência para print:

- Resposta 200 com token JWT
- Campos `nome`, `email` e `expiraEm`

## 2. Fazer Login com o Usuário Principal

Endpoint:

```text
POST /api/auth/login
```

Payload:

```json
{
  "email": "maria@teste.com",
  "senha": "Senha123!"
}
```

O que anotar:

```text
TOKEN_MARIA = valor do campo "token"
```

Evidência para print:

- Login retornando 200
- Token JWT retornado pelo login

## 3. Autorizar no Swagger

Clique em `Authorize`.

Cole apenas o token:

```text
TOKEN_MARIA
```

Observação:

- No Swagger revisado, cole só o token, sem escrever `Bearer`.
- Em clientes HTTP externos, o formato recomendado continua sendo `Authorization: Bearer TOKEN`.

## 4. Conferir o Usuário Autenticado

Endpoint:

```text
GET /api/me
```

Não precisa de payload.

Resposta esperada:

```json
{
  "id": "1",
  "email": "maria@teste.com",
  "nome": "Maria Silva"
}
```

Evidência para print:

- Resposta 200 do endpoint protegido

## 5. Criar Evento

Endpoint:

```text
POST /api/events
```

Payload:

```json
{
  "titulo": "Workshop de C# em Manaus",
  "descricao": "Workshop prático sobre ASP.NET Core, Entity Framework Core e construção de APIs REST.",
  "dataInicio": "2026-06-10T09:00:00Z",
  "dataFim": "2026-06-10T18:00:00Z",
  "local": "Bloco A - Sala 201"
}
```

O que anotar:

```text
EVENTO_ID = valor do campo "id"
```

Resposta esperada:

```json
{
  "id": 1,
  "titulo": "Workshop de C# em Manaus",
  "descricao": "Workshop prático sobre ASP.NET Core, Entity Framework Core e construção de APIs REST.",
  "dataInicio": "2026-06-10T09:00:00Z",
  "dataFim": "2026-06-10T18:00:00Z",
  "local": "Bloco A - Sala 201",
  "status": "Rascunho",
  "organizadorId": 1,
  "nomeOrganizador": "Maria Silva"
}
```

Evidência para print:

- Resposta 201
- `status` inicial como `Rascunho`
- `nomeOrganizador` preenchido

## 6. Publicar o Evento

Este passo é importante para o filtro `GET /api/events?status=Publicado` mostrar o evento.

Endpoint:

```text
PUT /api/events/{EVENTO_ID}
```

Payload:

```json
{
  "titulo": "Workshop de C# em Manaus - Atualizado",
  "descricao": "Workshop prático sobre ASP.NET Core, Entity Framework Core, autenticação JWT e construção de APIs REST.",
  "dataInicio": "2026-06-10T09:00:00Z",
  "dataFim": "2026-06-10T18:00:00Z",
  "local": "Bloco B - Auditório",
  "status": "Publicado"
}
```

Resposta esperada:

```json
{
  "id": 1,
  "titulo": "Workshop de C# em Manaus - Atualizado",
  "status": "Publicado",
  "organizadorId": 1,
  "nomeOrganizador": "Maria Silva"
}
```

Evidência para print:

- Resposta 200
- Status `Publicado`

## 7. Criar atividades

Endpoint:

```text
POST /api/activities
```

Payload:

```json
{
  "eventId": 1,
  "titulo": "Cerimônia de Abertura",
  "descricao": "Abertura oficial do Workshop de C#.",
  "tipo": "Abertura",
  "dataInicio": "2026-06-10T09:00:00Z",
  "dataFim": "2026-06-10T09:30:00Z",
  "local": "Auditório Principal"
}
```

Resposta esperada:

```json
{
  "id": 1,
  "eventId": 1,
  "titulo": "Cerimônia de Abertura",
  "descricao": "Abertura oficial do Workshop de C#.",
  "tipo": "Abertura",
  "dataInicio": "2026-06-10T09:00:00Z",
  "dataFim": "2026-06-10T09:30:00Z",
  "local": "Auditório Principal"
}
```

Payload:

```json
{
  "eventId": 1,
  "titulo": "Workshop ASP.NET Core",
  "descricao": "Introdução prática ao ASP.NET Core.",
  "tipo": "Workshop",
  "dataInicio": "2026-06-10T10:00:00Z",
  "dataFim": "2026-06-10T12:00:00Z",
  "local": "Laboratório 01"
}
```

## 8. Listar atividades do Evento

Endpoint:
```text
GET /api/activities/event/{EVENTO_ID}
```

Exemplo:

```text
GET /api/activities/event/1
```

Resposta esperada:
```json
[
  {
    "id": 1,
    "titulo": "Cerimônia de Abertura",
    "descricao": "Abertura oficial do Workshop de C#.",
    "tipo": "Abertura",
    "dataInicio": "2026-06-10T09:00:00Z",
    "dataFim": "2026-06-10T09:30:00Z",
    "local": "Auditório Principal",
    "eventId": 1
  },
  {
    "id": 2,
    "titulo": "Workshop ASP.NET Core",
    "descricao": "Introdução prática ao ASP.NET Core.",
    "tipo": "Workshop",
    "dataInicio": "2026-06-10T10:00:00Z",
    "dataFim": "2026-06-10T12:00:00Z",
    "local": "Laboratório 01",
    "eventId": 1
  }
]
```

Evidência para print:

- Lista contendo as duas atividades


## 9. Listar Eventos Públicos

Endpoint:

```text
GET /api/events
```

Não precisa de payload.

Evidência para print:

- Lista contendo o evento criado

## 10. Filtrar Eventos por Status

Endpoint:

```text
GET /api/events?status=Publicado
```

Não precisa de payload.

Evidência para print:

- Lista contendo o evento publicado

## 11. Listar Meus Eventos

Endpoint:

```text
GET /api/events/meus
```

Não precisa de payload.

Precisa estar autorizado com:

```text
TOKEN_MARIA
```

Evidência para print:

- Lista de eventos criados pela Maria

## 12. Fazer Inscrição

Endpoint:

```text
POST /api/registrations
```

Payload:

```json
{
  "eventoId": 1
}
```

Se o evento criado tiver outro id, trocar `1` pelo valor de `EVENTO_ID`.

O que anotar:

```text
INSCRICAO_ID = valor do campo "id"
```

Resposta esperada:

```json
{
  "id": 1,
  "eventoId": 1,
  "tituloEvento": "Workshop de C# em Manaus - Atualizado",
  "usuarioId": 1,
  "nomeUsuario": "Maria Silva",
  "status": "Pendente"
}
```

Evidência para print:

- Resposta 201 com inscrição criada

## 13. Tentar Inscrição Duplicada

Endpoint:

```text
POST /api/registrations
```

Payload:

```json
{
  "eventoId": 1
}
```

Se o evento criado tiver outro id, trocar `1` pelo valor de `EVENTO_ID`.

Resposta esperada:

```text
Você já está inscrito neste evento.
```

Evidência para print:

- Resposta 400
- Mensagem de inscrição duplicada

## 14. Adicionar Comentário

Endpoint:

```text
POST /api/comments
```

Payload:

```json
{
  "eventoId": 1,
  "conteudo": "Ótimo evento, vai ajudar muito na prática com APIs em C#."
}
```

Se o evento criado tiver outro id, trocar `1` pelo valor de `EVENTO_ID`.

O que anotar:

```text
COMENTARIO_ID = valor do campo "id"
```

Resposta esperada:

```json
{
  "id": 1,
  "eventoId": 1,
  "usuarioId": 1,
  "nomeUsuario": "Maria Silva",
  "conteudo": "Ótimo evento, vai ajudar muito na prática com APIs em C#."
}
```

Evidência para print:

- Resposta 201
- Comentário com `nomeUsuario`

## 15. Adicionar Reação

Endpoint:

```text
POST /api/reactions
```

Payload:

```json
{
  "eventoId": 1,
  "tipo": "VouParticipar"
}
```

Se o evento criado tiver outro id, trocar `1` pelo valor de `EVENTO_ID`.

O que anotar:

```text
REACAO_ID = valor do campo "id"
```

Resposta esperada:

```json
{
  "id": 1,
  "eventoId": 1,
  "usuarioId": 1,
  "nomeUsuario": "Maria Silva",
  "tipo": "VouParticipar"
}
```

Evidência para print:

- Resposta 201
- Reação com `tipo` igual a `VouParticipar`

## 16. Cadastrar Segundo Usuário para Testar 403

Endpoint:

```text
POST /api/auth/register
```

Payload:

```json
{
  "nome": "João Souza",
  "email": "joao@teste.com",
  "senha": "Senha123!"
}
```

O que anotar:

```text
TOKEN_JOAO = valor do campo "token"
```

Depois clique em `Authorize` e substitua o token da Maria pelo token do João:

```text
TOKEN_JOAO
```

## 17. Tentar Deletar Comentário de Outro Usuário

Endpoint:

```text
DELETE /api/comments/{COMENTARIO_ID}
```

Não precisa de payload.

Precisa estar autorizado com:

```text
TOKEN_JOAO
```

Resposta esperada:

```text
Apenas o autor pode remover este comentário.
```

Evidência para print:

- Resposta 403
- Mensagem de permissão negada

## 18. Evidência Extra: Listar Comentários

Endpoint:

```text
GET /api/comments?eventoId=1
```

Se o evento criado tiver outro id, trocar `1` pelo valor de `EVENTO_ID`.

Não precisa de token.

Evidência para print:

- Comentário da Maria visível publicamente

## 19. Evidência Extra: Listar Reações

Endpoint:

```text
GET /api/reactions?eventoId=1
```

Se o evento criado tiver outro id, trocar `1` pelo valor de `EVENTO_ID`.

Não precisa de token.

Evidência para print:

- Reação da Maria visível publicamente

## 20. Ordem Resumida para a Apresentação

```text
1. Abrir Swagger
2. POST /api/auth/register com Maria
3. POST /api/auth/login com Maria
4. Authorize com TOKEN_MARIA
5. GET /api/me
6. POST /api/events
7. POST /api/activities
8. GET /api/activities/event/{EVENTO_ID}
9. PUT /api/events/{EVENTO_ID} com status Publicado
10. GET /api/events
11. GET /api/events?status=Publicado
12. GET /api/events/meus
13. POST /api/registrations
14. POST /api/registrations novamente para erro 400
15. POST /api/comments
16. POST /api/reactions
17. POST /api/auth/register com João
18. Authorize com TOKEN_JOAO
19. DELETE /api/comments/{COMENTARIO_ID} para erro 403
```

## 21. Problemas Comuns na Hora da Aula

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

No Swagger revisado:

```text
Cole apenas o token, sem Bearer.
```

Em clientes HTTP externos:

```text
Authorization: Bearer TOKEN
```

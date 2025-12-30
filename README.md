# HelpResponse AI

API platform to answer user questions using vector search (Claudia DB) and OpenAI models. The application receives a message, generates its embedding, queries a vector index to retrieve relevant sections, then produces an answer using chat completions. It also returns the retrieved sections and an indicator of whether a human handover is needed.

## Overview
- **Goal:** Provide contextual answers from indexed content using embeddings and OpenAI chat.
- **Tech Stack:** .NET 8, ASP.NET Core, Refit, Serilog, Swagger, HealthChecks, Docker.
- **Layers:**
	- Domain (`src/HelpResponse.AI.Domain`): Request/Response contracts and light validation.
	- Services (`src/HelpResponse.AI.Services`): conversation business rules.
	- Infra (`src/HelpResponse.AI.Infra.ClientHttp`): HTTP clients (OpenAI and Claudia DB) via Refit.
	- WebAPI (`src/HelpResponse.AI.WebApi`): endpoints, DI, logging, swagger.

## Architecture & Flow
1. Client calls `POST /ai/helpresponse/v1/conversations/completions` with a `ConversationRequest`.
2. The service generates the message embedding using the configured OpenAI model (Embeddings).
3. With the embedding vector, it queries Claudia DB (Azure Cognitive Search) on a vector index to retrieve relevant sections.
4. It selects the content with the highest score and builds the prompt for `chat/completions`, combining `system` and `user` messages from configured templates.
5. It generates the assistant answer with OpenAI and composes the `ConversationResponse` with:
	 - Messages (`user` and `assistant`)
	 - Retrieved sections (content + score)
	 - `handoverToHumanNeeded`: true if any section has `type` equal to `N2`.

### Key components
- Service: `ConversationService`
	- Generates embedding (`IOpenIAClientApi.SendEmbeddings`)
	- Queries vector index (`IClaudiaDbClientApi.Search`)
	- Builds chat (`ChatCompletionInput`) with `system` and `user` templates
	- Calls `SendChatCompletions`
	- Builds `ConversationResponse` adding messages and retrieved sections

- Infra: HTTP clients via Refit
	- OpenAI: `IOpenIAClientApi` with `/v1/embeddings` and `/v1/chat/completions`
	- Claudia DB: `IClaudiaDbClientApi` with `search` (vector query)
	- Authentication added via `DelegatingHandler` based on tokens from `appsettings`

- WebAPI & IoC
	- `Program.cs` registers `OpenAiApi` and `ClaudiaDbApi` in `IOptions<>`
	- `StartupExtensions` configures versioning, JSON, timeouts, Swagger, HealthChecks, and DI (`RegisterServices`, `RegisterClientHttp`)
	- Middleware `LogCorrelationIdMiddleware` adds `CorrelationId` to logs
	- Swagger exposed at `/ai/helpresponse/swagger`

## Endpoints
- `POST /ai/helpresponse/v1/conversations/completions`
	- Request (example):
		```json
		{
			"helpdeskId": 123,
			"projectName": "tesla_motors",
			"message": {
				"role": "user",
				"content": "What is the average range of the Model S?"
			}
		}
		```
	- Response (simplified example):
		```json
		{
			"messages": [
				{ "role": "USER", "content": "What is the average range of the Model S?" },
				{ "role": "ASSISTANT", "content": "The average range of the Model S is ..." }
			],
			"handoverToHumanNeeded": false,
			"sectionsRetrieved": [
				{ "score": 0.98, "content": "Section about range" }
			]
		}
		```

## Configuration
Parameters are defined in `appsettings.json`/`appsettings.Development.json`:
```json
{
	"ApiSettings": {
		"Title": "HelpResponse AI",
		"Description": "API for HelpResponse AI",
		"Version": "1.0.0",
		"DefaultRequestTimeout": "30",
		"PathBase": "/ai/helpresponse"
	},
	"ClientIntegrations": {
		"OpenAIApi": {
			"Token": "<OPENAI_API_KEY>",
			"Url": "https://api.openai.com",
			"Model": "text-embedding-3-large",
			"ChatCompletionModel": "gpt-4o",
			"PromptTemplates": [
				{ "Message": "... {0}", "Rule": "system" },
				{ "Message": "... {0}", "Rule": "user" }
			]
		},
		"ClaudiaDbApi": {
			"Token": "<AZURE_SEARCH_API_KEY>",
			"Url": "https://<your-service>.search.windows.net/indexes",
			"Count": true,
			"Select": "content, type",
			"Top": 10,
			"Filter": "projectName eq '<your-project>'",
			"Fields": "embeddings",
			"Kind": "vector"
		}
	}
}
```

> Important: Do not commit real keys. Use environment variables or `UserSecrets` in development.

## Local Run (Windows)
Prerequisites: .NET 8 SDK, Docker (optional), access to required keys.

### Run with .NET
```powershell
dotnet build HelpResponse.AI.Api.sln
dotnet run --project src/HelpResponse.AI.WebApi/HelpResponse.AI.WebApi.csproj
```

After startup:
- Swagger: `http://localhost:8080/ai/helpresponse/swagger`
- Health check: `http://localhost:8080/health-check`

### Run with Docker Compose
```powershell
docker compose up --build -d
```
By default, it exposes ports `8080` (HTTP) and `8081` (HTTPS). `docker-compose.yml` uses variables like `ASPNETCORE_HTTP_PORTS` and maps `UserSecrets` and HTTPS certificates.

## Quick Use (curl)
```bash
curl -X POST "http://localhost:8080/ai/helpresponse/v1/conversations/completions" \
	-H "Content-Type: application/json" \
	-d '{
		"helpdeskId": 123,
		"projectName": "tesla_motors",
		"message": { "role": "user", "content": "What is the average range of the Model S?" }
	}'
```

## Examples: Payloads & curl
- Battery replacement longevity
	- Payload:
		```json
		{
			"helpdeskId": 1001,
			"projectName": "tesla_motors",
			"message": {
				"role": "user",
				"content": "How long does a Tesla battery last before it needs to be replaced?"
			}
		}
		```
	- curl:
		```bash
		curl -X POST "http://localhost:8080/ai/helpresponse/v1/conversations/completions" \
			-H "Content-Type: application/json" \
			-d '{
				"helpdeskId": 1001,
				"projectName": "tesla_motors",
				"message": { "role": "user", "content": "How long does a Tesla battery last before it needs to be replaced?" }
			}'
		```

- Human support escalation intent
	- Payload:
		```json
		{
			"helpdeskId": 1002,
			"projectName": "tesla_motors",
			"message": {
				"role": "user",
				"content": "Regarding Tesla car batteries, I'm not sure about their durability. Could you help me with this by increasing the support to a human?"
			}
		}
		```
	- curl:
		```bash
		curl -X POST "http://localhost:8080/ai/helpresponse/v1/conversations/completions" \
			-H "Content-Type: application/json" \
			-d '{
				"helpdeskId": 1002,
				"projectName": "tesla_motors",
				"message": { "role": "user", "content": "Regarding Tesla car batteries, I\'m not sure about their durability. Could you help me with this by increasing the support to a human?" }
			}'
		```
	- Note: `handoverToHumanNeeded` is set by business logic when any retrieved section has `type` equal to `N2`. The intent to escalate in the message may influence retrieval, but the flag depends on indexed content types.

- Best Tesla model comparison
	- Payload:
		```json
		{
			"helpdeskId": 1003,
			"projectName": "tesla_motors",
			"message": {
				"role": "user",
				"content": "Which Tesla car model is the best and what sets it apart from the competition?"
			}
		}
		```
	- curl:
		```bash
		curl -X POST "http://localhost:8080/ai/helpresponse/v1/conversations/completions" \
			-H "Content-Type: application/json" \
			-d '{
				"helpdeskId": 1003,
				"projectName": "tesla_motors",
				"message": { "role": "user", "content": "Which Tesla car model is the best and what sets it apart from the competition?" }
			}'
		```

## Project Structure
- `src/HelpResponse.AI.Domain`: Conversation Requests/Responses and simple validations (e.g., `ConversationResponse.AddMessage`).
- `src/HelpResponse.AI.Services`: Conversation business logic (`ConversationService`) and DI (`ServicesExtensions`).
- `src/HelpResponse.AI.Infra.ClientHttp`: Refit clients for OpenAI and Claudia DB, configuration and authentication via handlers.
- `src/HelpResponse.AI.WebApi`: Startup (`Program.cs`), IoC (`StartupExtensions`, `SwaggerExtension`, `SerilogExtensions`), Middleware, Controllers.

## Notes
- `handoverToHumanNeeded` is true when any retrieved section has `type` equal to `N2`.
- The CORS policy is referenced by name (`"cors"`); configure it as needed.
- Adjust `ApiSettings.PathBase` to set the base prefix for endpoints.

---
Maintained for learning and demonstration of integrating vector search with chat completion.

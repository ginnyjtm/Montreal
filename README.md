### MONTREAL

## Montreal is also a living reference implementation. As I adopt new technologies (message queues, external integrations, and more), I integrate them here following the same architectural standards I use in production work, with documentation on the design decisions behind each one.
 
## Table of Contents
- Architecture
- Solution Structure
- Dependency Rules
- Getting Started
- How the Skeleton Was Built
- Supporting Multiple Clients
- Roadmap

## Why This Project Exists

Every new project starts with the same questions: where does the business logic live, what is allowed to depend on what, and how do we keep the framework from leaking into the core? Montreal answers those questions once, so every new project starts from a consistent, testable foundation instead of an ad-hoc folder layout.

## Goals

- Keep business rules independent of frameworks, databases, and UI.
- Make dependencies point inward, toward the domain.
- Keep the API host thin: configuration and wiring only.
- Make the codebase easy to extend with new infrastructure (queues, caches, third-party APIs) without touching business logic.

## Architecture

**Domain** Entities, value objects, domain rules, and core interfaces. No external dependencies.


**Application** Use cases (commands/queries), DTOs, validation, and abstractions that Infrastructure implements.


**Infrastructure** Implementations of Application abstractions: persistence, messaging, email, external services.


**Presentation** Everything HTTP/client-facing that isn't business logic: controllers, middleware, filters, request/response mapping.


**Api** The host. Composes the application: dependency injection, configuration, and startup. No controllers or middleware live here.

## Dependency Rules
Dependencies always point inward. An inner layer never knows about an outer one.

`Montreal.Domain`   Nothing


`Montreal.Application`   Domain  


`Montreal.Infrastructure`   Domain, Application


`Montreal.Presentation` Application only


`Montreal.Api`   Application, Infrastructure, Presentation

**Key points**
 
- **Presentation never references Infrastructure.** Controllers depend on Application abstractions, so swapping a database or message broker never touches the HTTP layer.
- **Api is purely a composition root.** It is the only project that sees every layer, because its only job is to wire them together.

## Getting Started
 
### Prerequisites
 
- [.NET SDK](https://dotnet.microsoft.com/download)
### Run locally
 
```bash
git clone https://github.com/<your-username>/Montreal.git
cd Montreal
dotnet restore
dotnet build
dotnet run --project Montreal.Api
```
 
---
 
## How the Skeleton Was Built
 
These are the exact steps used to scaffold the solution, so the template can be reproduced for any new project. Replace `Montreal` with your project name.

### Step 1: Create the solution and projects
 
```bash
dotnet new sln -n Montreal
 
dotnet new classlib -n Montreal.Domain
dotnet new classlib -n Montreal.Application
dotnet new classlib -n Montreal.Infrastructure
dotnet new classlib -n Montreal.Presentation
dotnet new webapi   -n Montreal.Api
```
 
> `Montreal.Api` uses the `webapi` template because it is the runnable host. Every other layer is a class library.
 
### Step 2: Add the projects to the solution
 
```bash
dotnet sln add Montreal.Domain Montreal.Application Montreal.Infrastructure Montreal.Presentation Montreal.Api
```
 
### Step 3: Wire up the project references
 
```bash
dotnet add Montreal.Application    reference Montreal.Domain
dotnet add Montreal.Infrastructure reference Montreal.Domain Montreal.Application
dotnet add Montreal.Presentation   reference Montreal.Application
dotnet add Montreal.Api            reference Montreal.Application Montreal.Infrastructure Montreal.Presentation
```
 
Because `Montreal.Presentation` is a class library that hosts controllers and middleware, it needs access to ASP.NET Core. Add this to `Montreal.Presentation.csproj`:
 
```xml
<ItemGroup>
  <FrameworkReference Include="Microsoft.AspNetCore.App" />
</ItemGroup>
```
 
### Step 4: First keystrokes in the Domain
 
Create the initial folders in `Montreal.Domain`. Git doesn't track empty folders, so each gets a `.gitkeep` until real code arrives:
 
```bash
mkdir Montreal.Domain/Entities Montreal.Domain/Interfaces
touch Montreal.Domain/Entities/.gitkeep Montreal.Domain/Interfaces/.gitkeep
```
 
---

## Supporting Multiple Clients
 
When different clients (Web, Mobile, gRPC) need different request/response shapes, split Presentation into one class library per client. All of them share a single host and call the **same** Application handlers:
 
```bash
dotnet new classlib -n Montreal.Presentation.Web
dotnet new classlib -n Montreal.Presentation.Mobile
dotnet new webapi   -n Montreal.Api   # Single host for all clients
```
 
Each Presentation library shapes its own contracts, while the business logic stays untouched no matter which client is consuming it.
 
---

## Roadmap
 
Montreal grows as I learn. Each item will ship as a working example with notes on the design decisions behind it.
 
- [x] Clean Architecture solution skeleton
- [x] Layer dependency rules and multi-client Presentation pattern
- [ ] CQRS with a mediator (commands, queries, pipeline behaviors)
- [ ] Persistence with Entity Framework Core and SQL Server
- [ ] Validation and global error handling
- [ ] Message queue integration (publish/consume domain events)
- [ ] Third-party API integration
- [ ] Unit and integration tests
- [ ] Docker support
- [ ] CI pipeline with GitHub Actions

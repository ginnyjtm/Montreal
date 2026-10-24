### MONTREAL

## This project was creating to be a personal standard of creating a .Net project skeleton using Clean architechture.
 
# Step 1 Scaffolding the skeleton of the project.

 - dotnet new sln -n {project_name}
 - dotnet new classlib -n {project_name}.Domain
 - dotnet new classlib -n {project_name}.Application
 - dotnet new classlib -n {project_name}.Infrastructure
 - dotnet new classlib -n {project_name}.Presentation
 - dotnet new classlib -n {project_name}.Api

 # In case of multiple Client Variant 
 The Presentation will handles everything HTTP/client-facing that isn't business logic. Hosting by Api:
 Rambo.Presentation/
  ├── Controllers/          # route handlers → sends to MediatR
  ├── Middleware/           # auth, error handling, logging
  ├── Filters/              # action filters
  └── Mappers/              # request/response shaping per client type   

  # Dependency Rule in case of having Presentation.
  ┌──────────────────────┬───────────────────────────────────────────┐
  │       Project        │                References                 │
  ├──────────────────────┼───────────────────────────────────────────┤
  │ Rambo.Domain         │ Nothing                                   │
  ├──────────────────────┼───────────────────────────────────────────┤
  │ Rambo.Application    │ Domain                                    │
  ├──────────────────────┼───────────────────────────────────────────┤
  │ Rambo.Infrastructure │ Domain, Application                       │
  ├──────────────────────┼───────────────────────────────────────────┤
  │ Rambo.Presentation   │ Application only                          │
  ├──────────────────────┼───────────────────────────────────────────┤
  │ Rambo.Api            │ Application, Infrastructure, Presentation │
  └──────────────────────┴───────────────────────────────────────────┘     

  # Key point
  Presentation references Application only, it never touches Infrastructure. 
  The Api project becomes purely a host: DI registration, configuration,  
  startup. No controllers or middleware live there.

  # If you need different shapes per client (Web, Mobile, gRPC)
  dotnet new classlib -n Rambo.Presentation.Web
  dotnet new classlib -n Rambo.Presentation.Mobile
  dotnet new webapi -n Rambo.Api                    # host for all 

  Each Presentation classlib shapes requests/responses differently but calls the **same Application handlers** the business logic stays untouched regardless of    
  which client is consuming it.


# Step 2 First keystroke
**In Domain project**
  - Creating Entities directory and put .gitkeep file in there.
  - Creating Interfaces directory and put .gitkeep file in there.
# .NET Clean Architecture Project

This project is demonstrating a multi-service backend system in Clean Architecture built with .NET 8 and ASP.NET Core.
The solution includes a secure REST API, a gRPC service, and a Razor Pages administrative panel.

It contains: 
- Domain Module,
- Application Module,
- Infrastructure Module,
- Presentation Module,
- REST API with JWT Authentication,
- gRPC,
- Razor Pages for the UI,
- Entity Framework Core (In-Memory Provider),
- Unit and E2E tests

## How to run

I've changed scripting to E2E testing because I have problems with running shell scripts on Windows so I needed more platform-bulletproof solution.

To test this project you can basically run `dotnet test` to run both unit and E2E tests.

This project was also tested manually on localhost so integration between components and UI is tested as well.

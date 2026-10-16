## RaceDayAPI – Programming 2B Formative 2

## Project Overview

RaceDayAPI is an ASP.NET Core Web API developed for the Programming 2B assignment. The system manages running events, event categories, participant registrations and race results.

The API uses C# and Entity Framework Core to communicate with a Microsoft SQL Server database. Swagger is used to view, test and interact with the available API endpoints.

## Technologies Used

- C#
- ASP.NET Core Web API
- Entity Framework Core
- Microsoft SQL Server
- Visual Studio
- Swagger / OpenAPI
- JWT authentication

## Main Features

- User registration and login.
- User and organizer account management.
- Creating, viewing, updating and deleting events.
- Managing event categories.
- Registering participants for events.
- Recording and updating race results.
- Role-based access control for protected endpoints.

## Project Structure

- **Controllers** – Handles HTTP requests and API operations.
- **Models** – Contains the classes representing users, events, categories, locations, registrations and results.
- **Data** – Contains the database context and Entity Framework configuration.
- **Migrations** – Contains database schema changes.
- **Program.cs** – Configures the application services, database connection and middleware.
- **appsettings.json** – Contains application configuration.

## Requirements

Before running the project, make sure you have:

- Visual Studio with ASP.NET and web development support.
- The .NET SDK version required by the project.
- Microsoft SQL Server or SQL Server Express.
- SQL Server Management Studio (SSMS), if you want to manage the database manually.

## How to Run the Project

1. Clone or download the repository.
2. Open `RaceDayAPI.sln` in Visual Studio.
3. Check the database connection string in `appsettings.json` and update it to match your SQL Server instance.
4. Make sure the `RaceDayDB` database exists and that the required migrations have been applied.
5. Restore the NuGet packages.
6. Build the solution.
7. Run the project using Visual Studio.
8. Open the Swagger URL displayed when the application starts to test the endpoints.

## API Endpoints

| Method | Endpoint                         | Purpose                             |
| ------ | -------------------------------- | ----------------------------------- |
| POST   | `/api/auth/login`                | Authenticate a user                 |
| GET    | `/api/Users`                     | Retrieve users                      |
| POST   | `/api/Users`                     | Register a user                     |
| GET    | `/api/Users/{id}`                | Retrieve a user by ID               |
| POST   | `/api/Users/login`               | User login                          |
| POST   | `/api/Users/admin`               | Create an administrator             |
| GET    | `/api/events`                    | Retrieve events                     |
| POST   | `/api/events`                    | Create an event                     |
| GET    | `/api/events/{id}`               | Retrieve an event                   |
| PUT    | `/api/events/{id}`               | Update an event                     |
| DELETE | `/api/events/{id}`               | Delete an event                     |
| GET    | `/api/events/{id}/registrations` | Retrieve registrations for an event |
| GET    | `/api/Category`                  | Retrieve categories                 |
| POST   | `/api/Category`                  | Create a category                   |
| GET    | `/api/Category/{id}`             | Retrieve a category                 |
| POST   | `/api/registrations`             | Register for an event               |
| GET    | `/api/registrations/{id}`        | Retrieve a registration             |
| POST   | `/api/results`                   | Record a race result                |
| GET    | `/api/results/{id}`              | Retrieve a result                   |
| PUT    | `/api/results/{id}`              | Update a result                     |

## Testing

The API can be tested using Swagger by selecting an endpoint, entering the required request data and executing the request. Protected endpoints may require a valid JWT bearer token and the appropriate user role.

Successful requests and error responses should be checked to confirm that the API behaves as expected.

## Conclusion

RaceDayAPI provides a RESTful API for managing race events and their related information. The project applies object-oriented programming, database integration, HTTP methods and API access control in an ASP.NET Core application.

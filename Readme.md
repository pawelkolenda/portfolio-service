# Portfolio Service

A modern .NET 8.0 microservice application for managing investment portfolios and stock information. This project follows clean architecture principles and is built with scalability and maintainability in mind.

## Project Structure

The solution is organized into several projects following clean architecture principles:

- **PortfolioService.Api**: The entry point of the application, containing controllers and API configurations
- **PortfolioService.Core**: Core business logic and interfaces
- **PortfolioService.Domain**: Domain models and business rules
- **PortfolioService.Infrastructure**: Implementation of interfaces and external services
- **StockService**: Service for handling stock-related operations
- **PortfolioService.Core.UnitTests**: Unit tests for the core functionality

## Technologies Used

- **.NET 8.0**: The latest version of .NET framework
- **ASP.NET Core**: For building the web API
- **Swagger/OpenAPI**: For API documentation and testing
- **Clean Architecture**: Following SOLID principles and separation of concerns
- **Unit Testing**: For ensuring code quality and reliability

## Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Visual Studio 2022](https://visualstudio.microsoft.com/vs/) (recommended) or Visual Studio Code
- Git for version control

## Getting Started

1. Clone the repository:
   ```bash
   git clone [repository-url]
   ```

2. Navigate to the solution directory:
   ```bash
   cd PortfolioService
   ```

3. Restore the NuGet packages:
   ```bash
   dotnet restore
   ```

4. Build the solution:
   ```bash
   dotnet build
   ```

5. Run the application:
   ```bash
   dotnet run --project PortfolioService.Api
   ```

The API will be available at `https://localhost:5001` or `http://localhost:5000`.

## API Documentation

Once the application is running, you can access the Swagger documentation at:
```
https://localhost:5001/swagger
```

## Testing

To run the unit tests:
```bash
dotnet test
```

## Project Architecture

The project follows clean architecture principles with the following layers:

- **Domain Layer**: Contains enterprise business rules and entities
- **Application Layer**: Contains business logic and interfaces
- **Infrastructure Layer**: Contains implementations of interfaces and external services
- **Presentation Layer**: Contains the API controllers

## License

This project is licensed under the MIT License - see the LICENSE file for details. 
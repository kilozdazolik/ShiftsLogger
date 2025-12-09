
# 🕒 Shifts Logger

A comprehensive Shift Logging application built with **.NET 10**. The project demonstrates a clean separation of concerns by splitting the solution into a RESTful Web API and a separate Console-based User Interface.

## 🚀 Features

### Core Functionality
- **Start & Stop Shifts:** Track working hours accurately. Automatically calculates duration.
- **Manage Shifts:** View history, update shift details, or delete records.
- **RESTful Architecture:** The UI communicates with the database strictly via HTTP requests to the API.

### User Experience (UI)
- **Interactive Console:** Built with `Spectre.Console` for a rich user experience.
- **No Manual IDs:** Users select shifts from interactive lists using arrow keys
- **Visual Feedback:** Color-coded tables, status messages, and prompts.

## 🛠️ Tech Stack

**Backend (API):**
- **.NET 10 Web API**
- **Entity Framework Core:** ORM for database interactions.
- **SQL Server:** (LocalDB) for persistent storage.

**Frontend (UI):**
- **Console Application**
- **Spectre.Console:** For rendering tables, selection prompts, and styled text.
- **HttpClient:** For API communication.

**Testing:**
- **xUnit:** Test runner.
- **FakeItEasy:** For mocking dependencies (Repositories/Services).
- **FluentAssertions:** For readable and expressive assertions.
- **EF Core In-Memory:** For isolated integration testing of the Service layer.

## 📂 Project Structure

- **`ShiftsLogger.API`**: Handles business logic, database connections, and HTTP endpoints.
- **`ShiftsLogger.UI`**: The client application that the user interacts with.
- **`ShiftsLogger.Tests`**: Contains Unit and Integration tests for the API.

## ⚙️ Getting Started

### Prerequisites
- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- SQL Server (or LocalDB installed with Visual Studio)

### Installation

1. **Clone the repository**
   ```bash
   git clone [https://github.com/YOUR-USERNAME/ShiftsLogger.git](https://github.com/YOUR-USERNAME/ShiftsLogger.git)
   cd ShiftsLogger
2.  **Database Setup** Update the connection string in `ShiftsLogger.API/appsettings.json` if necessary. Then apply migrations:
    
    ```
    cd ShiftsLogger.API
    dotnet ef database update
    ```
    
3.  **Run the API** Keep this terminal open!

    ```
    dotnet run
    ```
    
4.  **Run the UI** Open a new terminal window:
    ```
    cd ../ShiftsLogger.UI
    dotnet run
    ```
    

## 🧪 Testing


### Controller Tests

Unit tests for API Controllers use **FakeItEasy** to mock the Service layer, ensuring that the API endpoints return correct HTTP status codes (200 OK, 404 Not Found, 400 Bad Request) independently of the database logic.

### Service Tests

Service layer tests use an **In-Memory Database** to verify CRUD operations, ensuring that data is correctly added, modified, and removed without affecting the production database.

**Run the tests:**
```
dotnet test
```

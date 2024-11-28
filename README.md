# Bank Api

## Overview

A Bank API is a tool that lets apps and websites connect securely to your bank. It helps you check your accounts details, perform transactions(withdraws, deposits, transfers). 

## Features

**Authentication**
 - Registration - you can register yourself to system;
 - Login - you can sign in to created account.

**Account management**
 - Create new account.
 - Get account details by number-identifier;
 - Get list of user accounts with short overview.

**Transaction management**
 - perform one of 3 types of transactions:
    - withdraw funds;
    - deposit funds;
    - transfer funds to another account.

## Prerequisites

- [.NET Core SDK](https://dotnet.microsoft.com/download)
- [Postgres Database](https://www.postgresql.org/)
- [Visual Studio](https://visualstudio.microsoft.com/) or Command prompt

## Setup and Launch

### Configuration

1. AppSettings: Configure the connection string in `appsettings.json`.

### Building and Running the Application

1. **Build and Run**:
    - **Visual Studio** - open the solution in Visual Studio. Press `F5` or click on the `Start Debugging` button to build and run the application.
    - **Command prompt** - open cmd or powershell, navigate to main Api assembly and run `dotnet run`.

2. **Access the API**: Once the application is running, you can access the API at `https://localhost:5111`.

### Testing

 - **Visual Studio** - open the solution in Visual Studio. Right click on assembly with tests(Bank.Server.Tests) and then click **Run tests**.
 - **Command prompt** - open cmd or powershell, navigate to assembly with tests and run `dotnet test`.

### Swagger Documentation

- The API documentation is available via Swagger UI at `https://localhost:5111/swagger/index.html`.

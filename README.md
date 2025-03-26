# RPSSLGame REST API

## Overview

RPSSLGame is a REST API built with ASP.NET Core targeting .NET 8. It provides endpoints for playing a Rock-Paper-Scissors-Lizard-Spock game and managing the scoreboard.

## Architecture

This project follows the principles of Clean Architecture, which includes the following layers:

- **Domain**: Contains the core business logic and entities.
- **Application**: Contains the application logic, including commands and queries using CQRS and MediatR.
- **Infrastructure**: Contains the implementation details, such as logging, data access and external services.
- **Api**: Contains the REST API endpoints and configuration.
- **Tests**: Contains unit and functional tests.

### CQRS and MediatR

The project uses the Command Query Responsibility Segregation (CQRS) pattern to separate read and write operations. MediatR is used to handle commands and queries.

- **Commands**: Represent actions that change the state of the system.
- **Queries**: Represent actions that retrieve data without changing the state of the system.

### In-Memory Storage

For simplicity, an in-memory dictionary is used as the storage for the scoreboard. This can be replaced with a more persistent storage solution if needed.
  
## Global Exception Handling

The application has global exception handling configured to handle validation errors and other exceptions. It returns appropriate HTTP status codes and error messages.

## Swagger

Swagger is enabled for API documentation. You can access the Swagger UI at `/swagger`.

## Prerequisites

- .NET 8 SDK
- Visual Studio 2022 or later

## Getting Started

### Clone the Repository

https://github.com/sanjatesla/RPSSLGameAPI.git

### Build and Run the Application

1. Open the solution in Visual Studio.
2. Set `RPSSLGame.RESTApi` as the startup project.
3. Press `F5` to build and run the application.

or run "dotnet build"

### Running Tests

1. Open the Test Explorer in Visual Studio.
2. Run all tests to ensure everything is working correctly.

or run "dotnet test"

## Configuration

The application uses an `appsettings.json` file for configuration.

## API Endpoints

### Play Game

- **Endpoint**: `/play`
- **Method**: `POST`
- **Description**: Play a game with computer.
- **Request Body**:
  { "playerChoice": "Rock" }
- **Response**:
  { "player": "Rock", "computer": "Paper", "result": "Lose" }
  
### Play Game multiple players

- **Endpoint**: `/play-multiple`
- **Method**: `POST`
- **Description**: Play a game with multiple players.
- **Request Body**:
  { "players": [ { "name": "player1", "choice": "Rock" }, { "name": "player2", "choice": "Scissors" }, { "name": "player3", "choice": "Lizard" } ] }
- **Response**:
  { "winners": [{ "name": "player1", "choice": "Rock" }], "result": "player1 wins" }
  
### Get Scoreboard

- **Endpoint**: `/scoreboard`
- **Method**: `GET`
- **Description**: Retrieve the top 10 players from the scoreboard.
- **Response**:
  [ { "player": "winner1", "score": 2 }, { "player": "winner2", "score": 1 }, ... ]

### Reset Scoreboard

- **Endpoint**: `/reset-scoreboard`
- **Method**: `POST`
- **Description**: Reset the scoreboard.
- 
  ### Get Choices

- **Endpoint**: `/choices`
- **Method**: `GET`
- **Description**: Retrieve all the available choices.
- **Response**:
  [ { "id": 1, "name": "Rock" }, { "id": 2, "name": "Paper" }, ... ]

  ### Get Random Choice

- **Endpoint**: `/choice`
- **Method**: `GET`
- **Description**: Get randomly generated choice.
- **Response**:
  { "id": 1, "name": "Rock" }



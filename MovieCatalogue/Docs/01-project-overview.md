
# Movie Catalogue Management System

## 1. Project Overview

The Movie Catalogue Management System is a C# Console Application developed using Entity Framework Core and SQL Server.

The application allows users to maintain a collection of movies by performing CRUD operations, searching movies by title, and filtering movies by genre.

## 2. Technologies Used

- C#
- .NET 10
- Entity Framework Core 10
- SQL Server
- LINQ
- Dependency Injection
- Repository Pattern

## 3. Movie Entity

| Property | Data Type | Description |
|---|---|---|
| Id | int | Primary Key |
| Title | string | Movie title |
| Genre | string | Movie genre |
| Language | string | Movie language |
| DurationMinutes | int | Movie duration |
| ReleaseYear | int | Movie release year |

## 4. Application Features

1. Add a movie.
2. View all movies.
3. Get a movie by ID.
4. Update movie information.
5. Delete a movie.
6. Search movies by title.
7. Filter movies by genre.

## 5. Project Architecture

- Models: Contains movie entity classes.
- Data: Contains the EF Core DbContext.
- Migrations: Contains database schema changes.
- Repositories: Handles database operations.
- Services: Handles business logic and validation.
- Program.cs: Handles console input and application flow.

## 6. Database

Database Name: MovieCatalogueDB

Table Name: Movies

Entity Framework Core Code-First approach is used to create and manage the database schema.

## 7. Architecture Flow

Program.cs
    ↓
MovieService
    ↓
MovieRepository
    ↓
AppDbContext
    ↓
SQL Server

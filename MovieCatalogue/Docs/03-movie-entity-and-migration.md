
# Movie Entity and EF Core Migrations

## 1. Movie Entity

The Movie class represents a movie record in our application.

Properties:

- Id
- Title
- Genre
- Language
- DurationMinutes
- ReleaseYear

## 2. Entity Configuration

We use Fluent API inside OnModelCreating() to configure the Movie entity.

- Id is configured as the primary key.
- Title is required and has a maximum length of 150.
- Genre is required and has a maximum length of 50.
- Language is required and has a maximum length of 50.
- DurationMinutes is required.
- ReleaseYear is required.

## 3. What is Code-First Approach?

In the Code-First approach, we create C# entity classes and configure them before generating the database schema.

EF Core generates the database tables using migrations.

## 4. What are Migrations?

Migrations track changes made to entity classes and database configurations.

They help keep the database schema synchronized with the application model.

## 5. Migration Commands

Create migration:

dotnet ef migrations add InitialCreate

Apply migration:

dotnet ef database update

List migrations:

dotnet ef migrations list

Remove the latest unapplied migration:

dotnet ef migrations remove

## 6. Database Tables

Database: MovieCatalogueDB

Table: Movies

EF Core also maintains a __EFMigrationsHistory table to track applied migrations.

## 7. Conclusion

Migrations allow us to create and update database structures without manually writing CREATE TABLE statements.

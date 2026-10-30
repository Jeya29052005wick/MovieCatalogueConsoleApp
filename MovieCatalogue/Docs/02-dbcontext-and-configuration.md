
# DbContext and Database Configuration

## 1. Introduction

Entity Framework Core is an Object Relational Mapper (ORM) that allows developers to interact with databases using C# objects instead of writing SQL queries manually.

## 2. What is DbContext?

DbContext represents a session with the database.

It is responsible for:
- Managing database connections.
- Tracking entity changes.
- Executing database queries.
- Saving changes to the database.

## 3. AppDbContext

AppDbContext is our application's database context class.

It inherits from DbContext.

### DbSet

DbSet<Movie> represents the Movies table in SQL Server.

### OnConfiguring()

OnConfiguring() configures the database provider and connection string.

We use UseSqlServer() to connect to SQL Server.

### OnModelCreating()

OnModelCreating() is used to configure entity properties using Fluent API.

## 4. Fluent API

We use Fluent API instead of Data Annotations.

Important methods:

| Method | Purpose |
|---|---|
| HasKey() | Configures primary key |
| Property() | Configures an entity property |
| IsRequired() | Makes a column NOT NULL |
| HasMaxLength() | Sets maximum string length |
| HasIndex() | Creates a database index |
| HasCheckConstraint() | Configures database CHECK constraints |

## 5. Database Connection

Server: localhost\SQLEXPRESS

Database: MovieCatalogueDB

Provider: SQL Server

## 6. Dependency Injection

AppDbContext is registered using AddDbContext().

This allows the application to inject the database context wherever required.

## 7. Conclusion

AppDbContext acts as the bridge between our C# application and SQL Server.

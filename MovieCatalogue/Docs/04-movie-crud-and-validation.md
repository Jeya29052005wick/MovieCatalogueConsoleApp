
# Movie CRUD Operations and Validation

## 1. Introduction

CRUD stands for:

- Create
- Read
- Update
- Delete

These operations are implemented using the Repository and Service layers.

## 2. Create Operation

AddMovieAsync() adds a new movie to the database.

EF Core methods:
- AddAsync()
- SaveChangesAsync()

## 3. Read Operation

GetAllMoviesAsync() retrieves all movies.

GetMovieByIdAsync() retrieves a specific movie using its ID.

EF Core methods:
- ToListAsync()
- FirstOrDefaultAsync()
- AsNoTracking()

## 4. Update Operation

UpdateMovieAsync() modifies an existing movie.

EF Core methods:
- Update()
- SaveChangesAsync()

## 5. Delete Operation

DeleteMovieAsync() removes an existing movie.

EF Core methods:
- Remove()
- SaveChangesAsync()

## 6. Validation Rules

The application validates:

1. Title is required.
2. Genre is required.
3. Language is required.
4. Duration must be greater than zero.
5. Release year must be between 1888 and the current year plus five.

## 7. Service Layer

The Service layer handles business logic and validation before calling repository methods.

## 8. Repository Layer

The Repository layer handles database interactions through AppDbContext.

## 9. Error Handling

The application handles:
- Invalid movie details.
- Invalid movie IDs.
- Movie not found.
- Database operation failures.

## 10. Conclusion

The Repository and Service layers separate database operations from business logic and make the application easier to maintain.

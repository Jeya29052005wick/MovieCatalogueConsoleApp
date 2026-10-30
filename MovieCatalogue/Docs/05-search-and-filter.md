
# Movie Search and Filter Operations

## 1. Introduction

The application provides two additional operations to find movies easily.

1. Search movies by title.
2. Filter movies by genre.

## 2. Search by Title

Method: SearchMoviesAsync()

Repository method: SearchByTitleAsync()

LINQ method: Where()

Example:

Search term: Leo

Expected matching movie:

Leo

The search uses Contains() to find titles containing the supplied text.

## 3. Filter by Genre

Method: FilterMoviesByGenreAsync()

Repository method: FilterByGenreAsync()

LINQ method: Where()

Example:

Genre: Action

Expected matching movies:

- Leo
- Vikram

## 4. LINQ Queries

Search by title:

Movies.Where(m => m.Title.Contains(search))

Filter by genre:

Movies.Where(m => m.Genre == genre)

## 5. AsNoTracking()

AsNoTracking() is used for read-only queries.

It improves query efficiency by avoiding unnecessary change tracking.

## 6. Testing Scenarios

| Operation | Input | Expected Result |
|---|---|---|
| Search | Leo | Matching movie |
| Search | Avatar | No results |
| Filter | Action | Action movies |
| Filter | Comedy | No results |

## 7. Conclusion

Search and filter operations make it easier to retrieve specific movie records using LINQ and Entity Framework Core.

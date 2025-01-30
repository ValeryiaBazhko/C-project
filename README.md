# Library managing system

Swagger UI for easier API testing

Similarity search using **Levenshtein Distance**


## Setup instructions

Required: [.NET 8.0 SDK] https://dotnet.microsoft.com/en-us/download/dotnet/8.0


### Clone the repository
```sh
git clone https://github.com/ValeryiaBazhko/C-project/
cd Library
```

### Install dependencies
  
   ```sh
   dotnet restore
   ```

### Run the API

```sh
dotnet run --launch-profile https
```


## Example API request

### Fetch a paginated list of books
```sh
GET /api/Books?pageNum=1&pageSize=5
```
Query parameters: 
- pageNum: Page number
- pageSize: Number of books per page


### Add a new book
```sh
POST /api/Books
Content-Type: application/json

{
  "title": "Harry Potter",
  "publicationYear": 1997,
  "authorId": 1
}
```

### Update an existing book's details
```sh
{
  "id": 1,
  "title": "Harry Potter and the Chamber of Secrets",
  "publicationYear": 1998,
  "authorId": 1
}
```


###  Delete a book by its ID
```sh
DELETE /api/Books/1
```

###  Search for books by title using a similarity search

```sh
GET /api/Books/search?query=Harry
```
Query parameters:
- query: The title or a partial title of the book.

### Fetch a list of all authors
```sh
GET: api/Authors
```

### Add a new author
```sh
{
  "id": 1,
  "Name": "J. K. Rowling",
  "dateofbirth": "1965-07-31",
  "books": [
      {
        "id": 1,
        "title": "Harry Potter and the Chamber of Secrets",
        "publicationYear": 1998,
        "authorId": 1
      }
    ]
}
```

## Levenshtein Distance algorithm (Similarity Search)

- Helps find books even if there are minor typos in the search

- Levenshtein distance is a string metric for measuring the difference between two sequences
  
- The Levenshtein distance between two words is the minimum number of single-character edits (insertions, deletions or substitutions) required to change one word into the other

- If a book title has a Levenshtein distance ≤ 3 from the search query, it is considered a match






# Library managing system
Similarity search using **Levenshtein Distance and Jaccard Similarity**
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


### Configure database
```sh
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Port=5432;Database=LibraryDB;Username=postgres;Password=yourpassword"
}
```

### Apply database migrations
```sh
dotnet ef database update
```
### Run the application
```sh
dotnet run --launch-profile https
```
### Run the frontend
```sh
npm run dev
```

## Database Schema 
### Book Table 
```sh
CREATE TABLE Books (
Id PRIMARY KEY,
Title VARCHAR(255) NOT NULL,
PublicationYear INT NOT NULL,
AuthorId INT NOT NULL,
FOREIGN KEY (AuthorId) REFERENCES Authors(Id)
);
```
### Author Table
```sh
CREATE TABLE Authors(
Id PRIMARY KEY,
Name  VARCHAR(255) NOT NULL
);
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
## Levenshtein Distance + Jaccard Similarity algorithms (Similarity Search)

To enhance the book search functionality, this application utilizes Levenshtein Distance and Jaccard Similarity for typo-tolerant and relevance-based matching.

- Levenshtein distance is a string metric for measuring the difference between two sequences
- The Levenshtein distance between two words is the minimum number of single-character edits (insertions, deletions or substitutions) required to change one word into the other
- Levenshtein Distance is applied to each distinct word in a book title, allowing for more granular typo correction and partial matches.

- The Jaccard similarity measures the similarity between two sets by calculating the ratio of the intersection to the union.

- By leveraging both Levenshtein Distance and Jaccard Similarity, the system provides robust and fault-tolerant search capabilities.

## Performance optimization
-Pagination: the API implements server-side pagination using EF Core's .Skip().Take() method to fetch only the required subset of data.
-Indexed database queries for fast retrieval.


import { useState, useEffect } from "react";

const BookList = () => {
    const [books, setBooks] = useState([]);
    const [authors, setAuthors] = useState([]);
    const [pageNum, setPageNum] = useState(1);
    const [pageSize, setPageSize] = useState(5);
    const [totalPages, setTotalPages] = useState(1);
    const [search, setSearch] = useState("");




    useEffect(() => {
        if (search.trim() == "") {
            fetchBooks();
        }
        fetchAuthors();
    }, [pageNum]);


    const fetchBooks = async () => {
        try {
            const res = await fetch(`https://localhost:7053/api/books?pageNum=${pageNum}&pageSize=${pageSize}`);
            if (!res.ok) throw new Error("Failed to fetch books");

            const data = await res.json();
            setBooks(data.books);
            setTotalPages(data.TotalPages);
        } catch (error) {
            console.error("Error fetching books: ", error);
        }
    };

    const handleSearch = async () => {
        if (!search.trim()) {
            fetchBooks();
            return;
        }

        try {
            const res = await fetch(`https://localhost:7053/api/books/search?query=${search}`);
            if (!res.ok) throw new Error("Failed to search books");

            const data = await res.json();
            if (!data)
                setBooks(data);
            setTotalPages(1);
        } catch (error) {
            console.error("Error searching books: ", error);
        }
    };

    const fetchAuthors = async () => {
        try {
            const res = await fetch("https://localhost:7053/api/authors");
            if (!res.ok) throw new Error("Failed to fetch authors");

            const data = await res.json();
            const authorMap = [];
            data.forEach(author => {
                authorMap[author.id] = author.name;
            });
            setAuthors(authorMap);
        } catch (error) {
            console.error("Error fetching authors: ", error);
        }
    }

    const deleteBook = async (id) => {
        if (!window.confirm("Are you sure you want to delete this book?")) return;

        try {
            const res = await fetch(`https://localhost:7053/api/books/${id}`, { method: "DELETE" });
            if (!res.ok) throw new Error("Failed to delete book");

            setBooks(books.filter(book => book.id !== id));
        } catch (error) {
            console.error("Error deleting book: ", error);
        }
    };


    return (
        <div>
            <h2>Book List</h2>

            <input type="text" placeholder="Search books here" value={search} onChange={(e) => setSearch(e.target.value)} />
            <button onClick={handleSearch}>Search</button>
            <ul>
                {books?.length > 0 ? (
                    books.map((book) => (
                        <li key={book.id}>
                            <strong>{book.title}</strong> (Published: {book.publicationYear})
                            <br />
                            <span>Author: {book.authorName || "Unknown"}</span>
                            <button onClick={() => deleteBook(book.id)}>Delete</button>
                        </li>
                    ))
                ) : (
                    <p>No books found</p>
                )}
            </ul>
            {search.trim() == "" && (
                <div>
                    <button disabled={pageNum <= 1} onClick={() => setPageNum(pageNum - 1)}>Previous</button>
                    <span> Page {pageNum} of {totalPages} </span>
                    <button disabled={pageNum >= totalPages} onClick={() => setPageNum(pageNum + 1)}>Next</button>
                </div>
            )}
        </div>
    );
};

export default BookList;
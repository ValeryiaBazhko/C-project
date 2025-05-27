import { useState, useEffect } from "react";
import { Link, useNavigate } from "react-router-dom";
import "/src/styles/mystyle.css";

const BookList = () => {
    const [books, setBooks] = useState([]);
    const [authors, setAuthors] = useState({});
    const [pageNum, setPageNum] = useState(1);
    const [pageSize] = useState(5);
    const [totalPages, setTotalPages] = useState(1);
    const [search, setSearch] = useState("");
    const [noBooks, setNoBooks] = useState(false);
    const navigate = useNavigate();

    const BASE_URL = "http://localhost:5000";

    useEffect(() => {
        if (search.trim() === "") {
            fetchBooks();
        } else {
            handleSearch();
        }
        fetchAuthors();
    }, [search, pageNum]);

    const fetchBooks = async () => {
        try {
            const res = await fetch(`${BASE_URL}/api/books?pageNum=${pageNum}&pageSize=${pageSize}`);
            if (!res.ok) throw new Error("Failed to fetch books");

            const data = await res.json();
            setBooks(data.books);
            setTotalPages(data.totalPages);
            setNoBooks(false);
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
            const res = await fetch(`${BASE_URL}/api/books/search?query=${search}`);
            if (res.status === 404) {
                setBooks([]);
                setNoBooks(true);
                return;
            }

            if (res.ok) {
                const data = await res.json();
                setBooks(data);
                setNoBooks(false);
                setTotalPages(1);
            }
        } catch (error) {
            console.error("Error searching books: ", error);
        }
    };

    const fetchAuthors = async () => {
        try {
            const res = await fetch(`${BASE_URL}/api/authors`);
            if (!res.ok) throw new Error("Failed to fetch authors");

            const data = await res.json();
            const authorMap = {};
            data.forEach(author => {
                authorMap[author.id] = author.name;
            });
            setAuthors(authorMap);
        } catch (error) {
            console.error("Error fetching authors: ", error);
        }
    };

    const deleteBook = async (id) => {
        if (!window.confirm("Are you sure you want to delete this book?")) return;

        try {
            const res = await fetch(`${BASE_URL}/api/books/${id}`, { method: "DELETE" });
            if (!res.ok) throw new Error("Failed to delete book");

            setBooks(prev => prev.filter(book => book.id !== id));
        } catch (error) {
            console.error("Error deleting book: ", error);
        }
    };

    return (
        <div>
            <h2>Book List</h2>

            <input
                type="text"
                placeholder="Search books here"
                value={search}
                onChange={(e) => setSearch(e.target.value)}
            />
            <button onClick={handleSearch}>Search</button>

            <ul>
                {noBooks ? (
                    <p>No books found</p>
                ) : (
                    books.length > 0 ? (
                        books.map((book) => (
                            <li key={book.id}>
                                <strong>{book.title}</strong> (Published: {book.publicationYear})<br />
                                <em>Genre:</em> {book.genre}<br />
                                <span><strong>Author:</strong> {authors[book.authorId] || "Unknown"}</span><br />
                                <Link to={`books/edit/${book.id}`}>
                                    <button>Edit</button>
                                </Link>
                                <button onClick={() => deleteBook(book.id)}>Delete</button>
                            </li>
                        ))
                    ) : (
                        <p>No books available</p>
                    )
                )}
            </ul>

            {search.trim() === "" && (
                <div>
                    <button
                        disabled={pageNum <= 1}
                        onClick={() => setPageNum(prev => prev - 1)}
                        className="pages"
                    >
                        Previous
                    </button>
                    <span> Page {pageNum} of {totalPages} </span>
                    <button
                        disabled={pageNum >= totalPages}
                        onClick={() => setPageNum(prev => prev + 1)}
                        className="pages"
                    >
                        Next
                    </button>
                </div>
            )}
        </div>
    );
};

export default BookList;

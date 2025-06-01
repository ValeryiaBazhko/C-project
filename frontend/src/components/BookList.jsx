import { useState, useEffect } from "react";
import { Link, useNavigate } from "react-router-dom";
import { getAuthUser } from "../utils/auth";
import "/src/styles/mystyle.css";

const BookList = () => {
    const [books, setBooks] = useState([]);
    const [authors, setAuthors] = useState({});
    const [pageNum, setPageNum] = useState(1);
    const [pageSize] = useState(5);
    const [totalPages, setTotalPages] = useState(1);
    const [searchTitle, setSearchTitle] = useState("");
    const [searchAuthor, setSearchAuthor] = useState("");
    const [searchGenre, setSearchGenre] = useState("");
    const [noBooks, setNoBooks] = useState(false);
    const navigate = useNavigate();

    const user = getAuthUser();
    const isAdmin = user.role === true;

    const BASE_URL = "https://11f9-95-159-226-202.ngrok-free.app"; 

    useEffect(() => {
        fetchAuthors();
    }, []);

    useEffect(() => {
        // Automatically search when any field changes
        const timeoutId = setTimeout(() => {
            if (isSearchEmpty()) {
                fetchBooks();
            } else {
                handleSearch();
            }
        }, 300);

        return () => clearTimeout(timeoutId);
    }, [searchTitle, searchAuthor, searchGenre, pageNum]);

    const isSearchEmpty = () => {
        return searchTitle.trim() === "" && searchAuthor === "" && searchGenre.trim() === "";
    };

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
        try {
            // Handle author-only search (exact match)
            if (searchAuthor && !searchTitle.trim() && !searchGenre.trim()) {
                // Filter from all books, need to fetch them first if not available
                const res = await fetch(`${BASE_URL}/api/books?pageNum=1&pageSize=1000`); // Get all books
                if (res.ok) {
                    const data = await res.json();
                    const authorBooks = data.books.filter(book => book.authorId == searchAuthor);
                    setBooks(authorBooks);
                    setNoBooks(authorBooks.length === 0);
                    setTotalPages(1);
                }
                return;
            }
            
            if (searchTitle.trim() || searchGenre.trim()) {
                let query = '';
                if (searchTitle.trim()) {
                    query += searchTitle.trim();
                }
                if (searchGenre.trim()) {
                    query += (query ? ' ' : '') + searchGenre.trim();
                }

                console.log('Similarity search for:', query);

                const res = await fetch(`${BASE_URL}/api/books/search?query=${encodeURIComponent(query)}`);
                if (res.status === 404) {
                    setBooks([]);
                    setNoBooks(true);
                    return;
                }

                if (res.ok) {
                    let data = await res.json();
                    
                    if (searchAuthor) {
                        data = data.filter(book => book.authorId == searchAuthor);
                    }

                    console.log('Search results:', data);
                    setBooks(data);
                    setNoBooks(data.length === 0);
                    setTotalPages(1);
                } else {
                    console.error('Search failed with status:', res.status);
                    setBooks([]);
                    setNoBooks(true);
                }
                return;
            }
            
            if (isSearchEmpty()) {
                fetchBooks();
                return;
            }

        } catch (error) {
            console.error("Error searching books: ", error);
            setBooks([]);
            setNoBooks(true);
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

    const clearSearch = () => {
        setSearchTitle("");
        setSearchAuthor("");
        setSearchGenre("");
        setPageNum(1);
    };

    const deleteBook = async (id) => {
        if (!isAdmin) {
            alert("You don't have permission to delete books.");
            return;
        }

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
        <main className="main-content">
            <div className="form-container">
                <h2>Book List</h2>

                <div className="search-container">
                    <div className="search-fields">
                        <div className="form-group">
                            <label>Search by Title:</label>
                            <input
                                type="text"
                                className="form-input"
                                placeholder="Search books by title"
                                value={searchTitle}
                                onChange={(e) => setSearchTitle(e.target.value)}
                            />
                        </div>

                        <div className="form-group">
                            <label>Filter by Author:</label>
                            <select
                                className="form-input"
                                value={searchAuthor}
                                onChange={(e) => setSearchAuthor(e.target.value)}
                            >
                                <option value="">All Authors</option>
                                {Object.entries(authors).map(([id, name]) => (
                                    <option key={id} value={id}>{name}</option>
                                ))}
                            </select>
                        </div>

                        <div className="form-group">
                            <label>Search by Genre:</label>
                            <input
                                type="text"
                                className="form-input"
                                placeholder="Search books by genre"
                                value={searchGenre}
                                onChange={(e) => setSearchGenre(e.target.value)}
                            />
                        </div>
                    </div>

                    <div className="form-actions">
                        <button onClick={handleSearch} className="submit-button">
                            Search
                        </button>
                        <button onClick={clearSearch} className="cancel-button">
                            Clear Filters
                        </button>
                    </div>
                </div>

                <div className="book-list-container">
                    {noBooks ? (
                        <p className="text-center">No books found matching your search criteria</p>
                    ) : (
                        books.length > 0 ? (
                            <div className="book-cards">
                                {books.map((book) => (
                                    <div key={book.id} className="book-card">
                                        <div className="book-info">
                                            <h3 className="book-title">{book.title}</h3>
                                            <p className="book-year">Published: {book.publicationYear}</p>
                                            <p className="book-genre"><em>Genre:</em> {book.genre}</p>
                                            <p className="book-author"><strong>Author:</strong> {authors[book.authorId] || "Unknown"}</p>
                                        </div>

                                        {/* Show different buttons based on user role */}
                                        {isAdmin ? (
                                            <div className="form-actions">
                                                <Link to={`/books/edit/${book.id}`}>
                                                    <button className="submit-button">Edit</button>
                                                </Link>
                                                <button
                                                    className="cancel-button"
                                                    onClick={() => deleteBook(book.id)}
                                                    style={{backgroundColor: 'var(--danger-color)', color: 'white'}}
                                                >
                                                    Delete
                                                </button>
                                            </div>
                                        ) : (
                                            <div className="form-actions">
                                                <Link to={`/loans/borrow/${book.id}`}>
                                                    <button className="submit-button">Borrow Book</button>
                                                </Link>
                                            </div>
                                        )}
                                    </div>
                                ))}
                            </div>
                        ) : (
                            <p className="text-center">No books available</p>
                        )
                    )}
                </div>

                {isSearchEmpty() && (
                    <div className="form-actions mt-3">
                        <button
                            disabled={pageNum <= 1}
                            onClick={() => setPageNum(prev => prev - 1)}
                            className="cancel-button"
                        >
                            Previous
                        </button>
                        <span className="page-info"> Page {pageNum} of {totalPages} </span>
                        <button
                            disabled={pageNum >= totalPages}
                            onClick={() => setPageNum(prev => prev + 1)}
                            className="submit-button"
                        >
                            Next
                        </button>
                    </div>
                )}
            </div>
        </main>
    );
};

export default BookList;
import { useState, useEffect } from "react";
import { useParams, useNavigate } from "react-router-dom";
import "/src/styles/mystyle.css";

const UpdateForm = () => {
    const { id } = useParams();
    const [title, setTitle] = useState(``);
    const [publicationYear, setPublicationYear] = useState(``);
    const [authorId, setAuthorId] = useState(``);
    const [genre, setGenre] = useState(``);
    const [authors, setAuthors] = useState([]);
    const [errors, setErrors] = useState({
        id: ``,
        title: ``,
        publicationYear: ``,
        authorId: ``,
        genre: ``,
    });
    const nav = useNavigate();

    const BASE_URL = "https://11f9-95-159-226-202.ngrok-free.app";

    useEffect(() => {
        fetchBook();
        fetchAuthors();
    }, []);

    const fetchBook = async () => {
        try {
            const res = await fetch(`${BASE_URL}/api/books/${id}`);
            if (!res.ok) throw new Error("Failed to fetch book details");

            const data = await res.json();
            setTitle(data.title);
            setPublicationYear(data.publicationYear);
            setAuthorId(data.authorId);
            setGenre(data.genre);
        } catch (error) {
            console.error("Error fetching book details: ", error);
            setErrors(prev => ({ ...prev, general: "Error fetching book details" }));
        }
    };

    const fetchAuthors = async () => {
        try {
            const res = await fetch(`${BASE_URL}/api/authors`);
            if (!res.ok) throw new Error("Failed to fetch authors");

            const data = await res.json();
            setAuthors(data);
        } catch (error) {
            console.error("Error fetching authors: ", error);
        }
    };

    const validateForm = () => {
        const newErrors = {};

        if (!title) {
            newErrors.title = "Title is required";
        } else if (title.length > 100) {
            newErrors.title = "Title is too long";
        }

        if (!publicationYear || isNaN(publicationYear)) {
            newErrors.publicationYear = "Invalid publication year";
        }

        if (!authorId) {
            newErrors.authorId = "Author is required";
        }

        if (!genre) {
            newErrors.genre = "Genre is required";
        }

        setErrors(newErrors);
        return Object.keys(newErrors).length === 0;
    };

    const handleSubmit = async (e) => {
        e.preventDefault();

        if (!validateForm()) {
            return;
        }

        try {
            const res = await fetch(`${BASE_URL}/api/books/${id}`, {
                method: "PUT",
                headers: {
                    "Content-Type": "application/json",
                },
                body: JSON.stringify({
                    id: id,
                    title,
                    publicationYear,
                    genre,
                    authorId
                }),
            });

            if (!res.ok) throw new Error("Failed to update book");

            nav("/home");
        } catch (error) {
            console.error("Error updating book: ", error);
            setErrors(prev => ({ ...prev, general: "Error updating book" }));
        }
    };

    return (
        <main className="main-content">
            <div className="form-container">
                <h2>Edit Book</h2>

                {errors.general && (
                    <div className="error-message text-center mb-3">
                        {errors.general}
                    </div>
                )}

                <form onSubmit={handleSubmit} className="book-form">
                    <div className="form-group">
                        <label>Title:</label>
                        <input
                            type="text"
                            className={`form-input ${errors.title ? 'error' : ''}`}
                            value={title}
                            onChange={(e) => setTitle(e.target.value)}
                            placeholder="Enter book title"
                            required
                        />
                        {errors.title && <div className="error-message">{errors.title}</div>}
                    </div>

                    <div className="form-group">
                        <label>Publication Year:</label>
                        <input
                            type="number"
                            className={`form-input ${errors.publicationYear ? 'error' : ''}`}
                            value={publicationYear}
                            onChange={(e) => setPublicationYear(e.target.value)}
                            placeholder="Enter publication year"
                            required
                        />
                        {errors.publicationYear && <div className="error-message">{errors.publicationYear}</div>}
                    </div>

                    <div className="form-group">
                        <label>Genre:</label>
                        <input
                            type="text"
                            className={`form-input ${errors.genre ? 'error' : ''}`}
                            value={genre}
                            onChange={(e) => setGenre(e.target.value)}
                            placeholder="Enter book genre"
                            required
                        />
                        {errors.genre && <div className="error-message">{errors.genre}</div>}
                    </div>

                    <div className="form-group">
                        <label>Author:</label>
                        <select
                            className={`form-input ${errors.authorId ? 'error' : ''}`}
                            value={authorId}
                            onChange={(e) => setAuthorId(e.target.value)}
                            required
                        >
                            <option value="">Select an author</option>
                            {authors.map((author) => (
                                <option key={author.id} value={author.id}>
                                    {author.name}
                                </option>
                            ))}
                        </select>
                        {errors.authorId && <div className="error-message">{errors.authorId}</div>}
                    </div>

                    <div className="form-actions">
                        <button type="submit" className="submit-button">
                            Update Book
                        </button>
                        <button
                            type="button"
                            className="cancel-button"
                            onClick={() => nav("/home")}
                        >
                            Cancel
                        </button>
                    </div>
                </form>
            </div>
        </main>
    );
};

export default UpdateForm;
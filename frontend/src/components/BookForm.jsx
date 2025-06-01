import { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";

const BookForm = ({ onSubmit, initialData = null }) => {
    const [id, setId] = useState('');
    const [title, setTitle] = useState(``);
    const [genre, setGenre] = useState('');
    const [publicationYear, setPublicationYear] = useState(``);
    const [authorId, setAuthorId] = useState(``);
    const [authors, setAuthors] = useState([]);
    const [errors, setErrors] = useState({
        id: ``,
        title: ``,
        publicationYear: ``,
        authorId: ``,
        genre: ''
    });
    const navigate = useNavigate();
    const BASE_URL = "https://11f9-95-159-226-202.ngrok-free.app";

    useEffect(() => {
        fetch(`${BASE_URL}/api/authors`)
            .then((res) => res.json())
            .then((data) => setAuthors(data))
            .catch((err) => console.error("Error fetching authors: ", err))
    }, []);

    const handleSubmit = async (e) => {
        e.preventDefault();

        setErrors({});

        let newErrors = {};
        let isValid = true;

        if (!title) {
            isValid = false;
            newErrors.title = "Title is required";
        } else if (title.length > 100) {
            isValid = false;
            newErrors.title = "Title is too long";
        }

        if (!publicationYear || isNaN(publicationYear) || publicationYear <= 0) {
            isValid = false;
            newErrors.publicationYear = "Invalid publication year";
        }

        if (!authorId) {
            isValid = false;
            newErrors.authorId = "Author is required";
        }

        if (!genre) {
            isValid = false;
            newErrors.genre = "Genre is required";
        }

        console.log(isValid);

        if (!isValid) {
            setErrors(newErrors);
            return;
        }


        const bookData = {
            title,
            publicationYear,
            authorId,
            genre
        };

        console.log("Submitting book data: ", bookData);

        try {
            const res = await fetch(`${BASE_URL}/api/books`, {
                method: "POST",
                body: JSON.stringify(bookData),
                headers: {
                    "Content-type": "application/json; charset=UTF-8"
                }
            });

            if (!res.ok) {
                const error = await res.text();
                throw new Error(`Failed to add book: ${res.status}: ${error}`);
            }

            const data = await res.json();
            console.log("Books: ", data);
            setId(``);
            setTitle(``);
            setPublicationYear(``);
            setAuthorId(``);
            setGenre(``);
        } catch (error) {
            console.error("Error submitting form: ", error);
        }
    };

    return (
        <main className="main-content">
            <div className="form-container">
                <h2>{initialData ? `Edit Book` : `Add New Book`}</h2>
                <form onSubmit={handleSubmit} className="book-form">
                    <div className="form-group">
                        <label htmlFor="title">Title:</label>
                        <input
                            type="text"
                            id="title"
                            value={title}
                            onChange={(e) => setTitle(e.target.value)}
                            className={`form-input ${errors.title ? 'error' : ''}`}
                            required
                            placeholder="Enter book title"
                        />
                        {errors.title && <div className="error-message">{errors.title}</div>}
                    </div>

                    <div className="form-group">
                        <label htmlFor="publicationYear">Publication Year:</label>
                        <input
                            type="number"
                            id="publicationYear"
                            value={publicationYear}
                            onChange={(e) => setPublicationYear(e.target.value)}
                            className={`form-input ${errors.publicationYear ? 'error' : ''}`}
                            required
                            min="1000"
                            max={new Date().getFullYear()}
                            placeholder="YYYY"
                        />
                        {errors.publicationYear && <div className="error-message">{errors.publicationYear}</div>}
                    </div>

                    <div className="form-group">
                        <label htmlFor="genre">Genre:</label>
                        <input
                            type="text"
                            id="genre"
                            value={genre}
                            onChange={(e) => setGenre(e.target.value)}
                            className={`form-input ${errors.genre ? 'error' : ''}`}
                            required
                            placeholder="Enter book genre"
                        />
                        {errors.genre && <div className="error-message">{errors.genre}</div>}
                    </div>

                    <div className="form-group">
                        <label htmlFor="authorId">Select Author:</label>
                        <select
                            id="authorId"
                            value={authorId}
                            onChange={(e) => setAuthorId(e.target.value)}
                            className={`form-input ${errors.authorId ? 'error' : ''}`}
                            required
                        >
                            <option value="">Select an Author</option>
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
                            {initialData ? 'Update Book' : 'Add Book'}
                        </button>
                        <button
                            type="button"
                            onClick={() => navigate(-1)}
                            className="cancel-button"
                        >
                            Back
                        </button>
                    </div>
                </form>
            </div>
        </main>
    );
}
export default BookForm;

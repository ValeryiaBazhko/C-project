import { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";

const BookForm = ({ onSubmit, initialData = null }) => {
    const [id, setId] = useState('');
    const [title, setTitle] = useState(``);
    const [publicationYear, setPublicationYear] = useState(``);
    const [authorId, setAuthorId] = useState(``);
    const [authors, setAuthors] = useState([]);
    const [errors, setErrors] = useState({
        id: ``,
        title: ``,
        publicationYear: ``,
        authorId: ``
    });
    const navigate = useNavigate();

    useEffect(() => {
        fetch("https://localhost:7053/api/authors")
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

        if (!publicationYear || isNaN(publicationYear) || publicationYear <=0) {
            isValid = false;
            newErrors.publicationYear = "Invalid publication year";
        }

        if (!authorId) {
            isValid = false;
            newErrors.authorId = "Author is required";
        }

        console.log(isValid);

        if(!isValid){
            setErrors(newErrors);
            return;
        }


        const bookData = {
            title,
            publicationYear,
            authorId
        };

        console.log("Submitting book data: ", bookData);

        try {
            const res = await fetch(`https://localhost:7053/api/books`, {
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
        } catch (error) {
            console.error("Error submitting form: ", error);
        }
    };

    return (
        <div>
            <h2>{initialData ? `Edit Book` : `Add New Book`}</h2>
            <form onSubmit={handleSubmit}>
                <div>
                    <label htmlFor="title">Title:</label>
                    <input type="text" id="title" value={title} onChange={(e) => setTitle(e.target.value)} className="info" required />
                    {errors.title && <div style={{ color: `red` }}>{errors.title}</div>}
                </div>
                <div>
                    <label htmlFor="publicationYear">Publication Year:</label>
                    <input type="number" id="publicationYear" value={publicationYear} onChange={(e) => setPublicationYear(e.target.value)} className="info" required />
                    {errors.publicationYear && <div style={{ color: `red` }}>{errors.publicationYear}</div>}
                </div>
                <div>
                    <label htmlFor="authorId">Select Author:</label>
                    <select id="authorId" value={authorId} onChange={(e) => setAuthorId(e.target.value)} required>
                        <option value="">Select an Author</option>
                        {authors.map((author) => (
                            <option key={author.id} value={author.id}>
                                {author.name}
                            </option>
                        ))}
                    </select>
                    {errors.authorId && <div style={{ color: `red` }}>{errors.authorId}</div>}
                </div>
                <button type="submit">Add Book</button>
                <button onClick={() => navigate("/")}>Back</button>
            </form>
        </div>
    );
};

export default BookForm;

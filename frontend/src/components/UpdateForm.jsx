import { useState, useEffect } from "react";
import { useParams, useNavigate } from "react-router-dom";

const UpdateForm = () => {
    const { id } = useParams();
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
    const nav = useNavigate();

    useEffect(() => {
        fetchBook();
        fetchAuthors();
    }, []);


    const fetchBook = async () => {
        try {


            const res = await fetch(`https://localhost:7053/api/books/${id}`);
            if (!res.ok) throw new Error("Failed to fetch book details");


            const data = await res.json();
            setTitle(data.title);
            setPublicationYear(data.publicationYear);
            setAuthorId(data.authorId);
        } catch (error) {
            setErrors("Error fetching book details: ", error);
        }
    };

    const fetchAuthors = async () => {
        try {
            const res = await fetch(`https://localhost:7053/api/authors`);
            if (!res.ok) throw new Error("Failed to fetch authors");

            const data = await res.json();
            setAuthors(data);
        } catch (error) {
            console.error("Error fetching authros: ", error)
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

        setErrors(newErrors);
        return Object.keys(newErrors).length === 0;
    };


    const handleSubmit = async (e) => {
        e.preventDefault();

        if (!validateForm()) {
            return;
        }

        try {
            const res = await fetch(`https://localhost:7053/api/books/${id}`, {
                method: "PUT",
                headers: {
                    "Content-Type": "application/json",
                },
                body: JSON.stringify({
                    id: id,
                    title,
                    publicationYear,
                    authorId
                }),
            });

            if (!res.ok) throw new Error("Failed to update book");

            alert("Book updated successfully!");
            nav("/");
        } catch (error) {
            console.error("Error updating book: ", error);
        }
    };

    return (
        <div>
            <h2>Edit book:</h2>
            <form onSubmit={handleSubmit}>
                <div>
                    <label>Title:</label>
                    <input type="text" value={title} onChange={(e) => setTitle(e.target.value)}
                        required />
                </div>

                <div>
                    <label>Publication Year:</label>
                    <input type="number" value={publicationYear}
                        onChange={(e) => setPublicationYear(e.target.value)}
                        required />
                </div>

                <div>
                    <label>Author:</label>
                    <select value={authorId} onChange={(e) => setAuthorId(e.target.value)} required>
                        <option value="">Select an author</option>
                        {authors.map((author) => (
                            <option key={author.id} value={author.id}>{author.name}</option>
                        ))}
                    </select>
                </div>

                <button type="submit">Update Book</button>
                <button type="button" onClick={() => nav("/")}>Cancel</button>
            </form>
        </div>
    );
};

export default UpdateForm; 
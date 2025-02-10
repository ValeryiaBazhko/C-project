import { useState, useEffect } from "react";

const BookForm = ({ onSubmit, initialData = null }) => {
    const [id, setId] = useState(initialData?.id || ``);
    const [title, setTitle] = useState(initialData?.title || ``);
    const [publicationYear, setPublicationYear] = useState(initialData?.publicationYear || ``);
    const [authorId, setAuthorId] = useState(initialData?.authorId || ``);
    const [authors, setAuthors] = useState([]);
    const [pageNum, setPageNum] = useState(1);
    const [pageSize, setPageSize] = useState(5);


    const [errors, setErrors] = useState({
        title: ``,
        publicationYear: ``,
        authorId: ``
    });

    useEffect(() => {
        fetch("https://localhost:7053/api/authors")
            .then((res) => res.json())
            .then((data) => setAuthors(data))
            .catch((err) => console.error("Error fetching authors: ", err))
    }, []);


    const handleSubmit = async (e) => {
        e.preventDefault();

        setErrors({});

        let isValid = true;
        let newErrors = {};

        if (!title) {
            isValid = false;
            newErrors.title = "Title is required";
        } else if (title.length > 100) {
            isValid = false;
            newErrors.title = "Title is too long";
        }

        if (!publicationYear || isNaN(publicationYear)) {
            isValid = false;
            newErrors.publicationYear = "Invalid publication year";
        }

        if (!isValid) {
            setErrors(newErrors);
            return;
        }

        try {
            const res = await fetch(`https://localhost:7053/api/books?pageNum=${pageNum}&pageSize=${pageSize}`, {
                method: "POST",
                body: JSON.stringify({
                    title,
                    publicationYear,
                    authorId
                }),
                headers: {
                    "Content-type": "application/json; charset=UTF-8"
                }
            });

            if (!res.ok) {
                throw new Error("Failed to add book")
            }
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
                    <input type="text" id="title" value={title} onChange={(e) => setTitle(e.target.value)} />
                    {errors.title && <div style={{ color: `red` }}>{errors.title}</div>}
                </div>

                <div>
                    <label htmlFor="publicationYear">Publication Year:</label>
                    <input type="number" id="publicationYear" value={publicationYear} onChange={(e) => setPublicationYear(e.target.value)} />
                    {errors.publicationYear && <div style={{ color: `red` }}>{errors.publicationYear}</div>}
                </div>

                <div>
                    <label htmlFor="authorId">Select Author:</label>
                    <select id="authorId" value={authorId} onChange={(e) => setAuthorId(e.target.value)}>
                        <option>Select an Author</option>
                        {authors.map((author) => (
                            <option key={author.id} value={author.id}>
                                {author.name}
                            </option>
                        ))}
                    </select>
                    {errors.authorId && <div style={{ color: `red` }}>{errors.authorId}</div>}
                </div>

                <button type="submit">{initialData ? `Update Book` : `Add Book`}</button>

            </form>
        </div>
    );
};

export default BookForm;
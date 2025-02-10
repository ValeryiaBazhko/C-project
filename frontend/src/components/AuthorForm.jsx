import { useState, useEffect } from "react";

const AuthorForm = ({ onSubmit, initialData = null }) => {
    const [id, setId] = useState('');
    const [name, setName] = useState('');
    const [dateofbirth, setDateofbirth] = useState('');

    const [errors, setErrors] = useState({
        id: ``,
        name: ``,
        dateofbirth: ``
    });



    const handleSubmit = async (e) => {
        e.preventDefault();

        setErrors({});

        let isValid = true;
        let newErrors = {};

        if (!name) {
            isValid = false;
            newErrors.name = "Name is required";
        } else if (name.length > 100) {
            isValid = false;
            newErrors.name = "Title is too long";
        }

        if (!dateofbirth) { //todo check if valid date
            isValid = false;
            newErrors.dateofbirth = "Invalid date of birth";
        }

        if (!isValid) {
            setErrors(newErrors);
            return;
        }


        try {
            const res = await fetch("https://localhost:7053/api/authors", {
                method: "POST",
                body: JSON.stringify({
                    name,
                    dateofbirth
                }),
                headers: {
                    "Content-type": "application/json; charset=UTF-8"
                }
            });

            if (!res.ok) {
                throw new Error("Failed to add author");
            }

            setId('');
            setName('');
            setDateofbirth('');

        } catch (error) {
            console.error("Error submitting form:", error);
        }


    };

    return (
        <div>
            <h2> Add new Author: </h2>
            <form onSubmit={handleSubmit}>
                <div>
                    <label htmlFor="name">Name:</label>
                    <input type="text" id="name" value={name} onChange={(e) => setName(e.target.value)} />
                    {errors.name && <div style={{ color: 'red' }}>{errors.name}</div>}
                </div>

                <div>
                    <label htmlFor="dateofbirth">Date of Birth:</label>
                    <input type="date" id="dateofbirth" value={dateofbirth} onChange={(e) => setDateofbirth(e.target.value)} />
                    {errors.dateofbirth && <div style={{ color: 'red' }}>{errors.dateofbirth}</div>}
                </div>

                <button type="submit">Add the Author</button>

            </form>
        </div>
    );
};

export default AuthorForm;

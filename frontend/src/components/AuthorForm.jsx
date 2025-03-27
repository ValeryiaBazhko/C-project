import { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";

const AuthorForm = ({ onSubmit, initialData = null }) => {
    const [id, setId] = useState('');
    const [name, setName] = useState('');
    const [dateofbirth, setDateofbirth] = useState('');
    const navigate = useNavigate();
    const [errors, setErrors] = useState({
        id: ``,
        name: ``,
        dateofbirth: ``
    });

    const BASE_URL = "https://localhost:5001";



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

        const date = new Date();
        console.log(dateofbirth.toString());
        console.log(date.toISOString().substr(0, 10));

        if (!dateofbirth || dateofbirth.toString() > date.toISOString().substring(0, 10)) { //todo check if valid date
            isValid = false;
            newErrors.dateofbirth = "Invalid date of birth";
        }


        if (!dateofbirth || dateofbirth.toString() > date.toISOString().substring(0, 10)) {
            isValid = false;
            newErrors.dateofbirth = "Invalid date of birth";
        }

        if (!isValid) {
            setErrors(newErrors);
            return;
        }

        const dateofbirthutc = new Date(dateofbirth).toISOString().split("T")[0];

        const bookData = {
            name,
            dateofbirth: dateofbirthutc,
        };

        console.log("Submitting book data: ", bookData);

        try {
            const res = await fetch(`${BASE_URL}/api/authors`, {
                method: "POST",
                body: JSON.stringify(bookData),
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
                    <input type="text" id="name" value={name} onChange={(e) => setName(e.target.value)} className="info" />
                    {errors.name && <div style={{ color: 'red' }}>{errors.name}</div>}
                </div>

                <div>
                    <label htmlFor="dateofbirth" >Date of Birth:</label>
                    <input type="date" id="dateofbirth" value={dateofbirth} onChange={(e) => setDateofbirth(e.target.value)} className="info" />
                    {errors.dateofbirth && <div style={{ color: 'red' }}>{errors.dateofbirth}</div>}
                </div>

                <button type="submit">Add the Author</button>
                <button onClick={() => navigate("/")}> Back</button>
            </form>

        </div>
    );
};

export default AuthorForm;

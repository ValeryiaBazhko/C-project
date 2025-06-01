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

    const BASE_URL = "https://11f9-95-159-226-202.ngrok-free.app";



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
        <main className="main-content">
            <div className="form-container">
                <h2>Add New Author</h2>
                <form onSubmit={handleSubmit} className="author-form">
                    <div className="form-group">
                        <label htmlFor="name">Name:</label>
                        <input
                            type="text"
                            id="name"
                            value={name}
                            onChange={(e) => setName(e.target.value)}
                            className={`form-input ${errors.name ? 'error' : ''}`}
                            placeholder="Enter author's name"
                        />
                        {errors.name && <div className="error-message">{errors.name}</div>}
                    </div>

                    <div className="form-group">
                        <label htmlFor="dateofbirth">Date of Birth:</label>
                        <input
                            type="date"
                            id="dateofbirth"
                            value={dateofbirth}
                            onChange={(e) => setDateofbirth(e.target.value)}
                            className={`form-input ${errors.dateofbirth ? 'error' : ''}`}
                            max={new Date().toISOString().split('T')[0]}
                        />
                        {errors.dateofbirth && <div className="error-message">{errors.dateofbirth}</div>}
                    </div>

                    <div className="form-actions">
                        <button type="submit" className="submit-button">
                            {initialData ? 'Update Author' : 'Add Author'}
                        </button>
                        <button
                            type="button"
                            onClick={() => navigate(-1)}
                            className="cancel-button"
                        >
                            Cancel
                        </button>
                    </div>

                    {errors.submit && <div className="error-message submit-error">{errors.submit}</div>}
                </form>
            </div>
        </main>
    );
};

export default AuthorForm;

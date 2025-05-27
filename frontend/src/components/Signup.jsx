import { useState } from "react";
import { useNavigate } from "react-router-dom";

const SignUpForm = ({onAuthSuccess = false}) => {
    const [formData, setFormData] = useState({
        firstName: "",
        lastName: "",
        email: "",
        password: "",
        isAdmin: false
    });
    const [error, setError] = useState("");
    const navigate = useNavigate();

    const BASE_URL = "https://localhost:5001";

    const handleChange = (e) => {
        const { name, value, type, checked } = e.target;
        setFormData(prev => ({
            ...prev,
            [name]: type === "checkbox" ? checked : value
        }));
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        setError("");

        try {
            const response = await fetch(`${BASE_URL}/api/auth/register`, {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                    "Accept": "application/json"
                },
                body: JSON.stringify({
                    firstName: formData.firstName,
                    lastName: formData.lastName,
                    email: formData.email,
                    password: formData.password,
                    role: formData.isAdmin
                })
            });

            const data = await response.json();
            onAuthSuccess(data);

            if (!response.ok) {
                throw new Error(data.message || "Registration failed");
            }
            
            localStorage.setItem("user", JSON.stringify(data));
            navigate("/home");

        } catch (err) {
            setError(err.message);
            console.error("Registration error:", err);
        }
    };

    return (
        <div className="signup-form-container">
            <h2>Create Account</h2>
            <form onSubmit={handleSubmit}>
                <input
                    type="text"
                    name="firstName"
                    placeholder="First Name"
                    value={formData.firstName}
                    onChange={handleChange}
                    required
                />

                <input
                    type="text"
                    name="lastName"
                    placeholder="Last Name"
                    value={formData.lastName}
                    onChange={handleChange}
                    required
                />

                <input
                    type="email"
                    name="email"
                    placeholder="Email"
                    value={formData.email}
                    onChange={handleChange}
                    required
                />

                <input
                    type="password"
                    name="password"
                    placeholder="Password (min 6 characters)"
                    value={formData.password}
                    onChange={handleChange}
                    minLength={6}
                    required
                />

                <button type="submit">Sign Up</button>

                {error && <p style={{ color: "red" }}>{error}</p>}
            </form>
        </div>
    );
};

export default SignUpForm;
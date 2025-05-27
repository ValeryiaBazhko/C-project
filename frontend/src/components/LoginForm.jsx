import { useState } from "react";
import { useNavigate } from "react-router-dom";

const LoginForm = () => {
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [error, setError] = useState("");
    const navigate = useNavigate();

    const BASE_URL = "https://localhost:5001";

    const handleLogin = async (e) => {
        e.preventDefault();
        setError("");

        try {
            const response = await fetch(`${BASE_URL}/api/auth/login`, {
                method: "POST",
                body: JSON.stringify({ email, password }),
                headers: {
                    "Content-Type": "application/json",
                    "Accept": "application/json" // Explicitly ask for JSON response
                },
                credentials: 'include' // If using cookies/sessions
            });

            if (!response.ok) {
                // Try to get error message from response
                const errorText = await response.text();
                throw new Error(errorText || "Invalid email or password");
            }

            // Check if response has content before parsing
            const contentLength = response.headers.get('content-length');
            if (contentLength && parseInt(contentLength) > 0) {
                const data = await response.json();
                localStorage.setItem("user", JSON.stringify(data));

                if (data.role === true) {
                    console.log("Logged in as admin");
                }
            }

            navigate("/home"); // Redirect to home page
        } catch (err) {
            setError(err.message);
        }
    };

    return (
        <div className="login-form-container">
            <h2>Login</h2>
            <form onSubmit={handleLogin}>
                <input
                    type="email"
                    placeholder="Email"
                    value={email}
                    required
                    onChange={(e) => setEmail(e.target.value)}
                />

                <input
                    type="password"
                    placeholder="Password"
                    value={password}
                    required
                    onChange={(e) => setPassword(e.target.value)}
                />

                <button type="submit">Login</button>

                {error && <p style={{ color: "red" }}>{error}</p>}
            </form>
        </div>
    );
};

export default LoginForm;
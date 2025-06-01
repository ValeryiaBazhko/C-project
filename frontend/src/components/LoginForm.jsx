import { useState } from "react";
import { useNavigate } from "react-router-dom";

const LoginForm = ({onAuthSuccess=false}) => {
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [error, setError] = useState("");
    const navigate = useNavigate();

    const BASE_URL = "https://11f9-95-159-226-202.ngrok-free.app";

    const handleLogin = async (e) => {
        e.preventDefault();
        setError("");

        try {
            const response = await fetch(`${BASE_URL}/api/auth/login`, {
                method: "POST",
                body: JSON.stringify({ email, password }),
                headers: {
                    "Content-Type": "application/json",
                    "Accept": "application/json" 
                }
            });

            if (!response.ok) {
                const errorText = await response.text();
                throw new Error(errorText || "Invalid email or password");
            }
            
            const data = await response.json();
            onAuthSuccess(data);
            
            const contentLength = response.headers.get('content-length');
            if (contentLength && parseInt(contentLength) > 0) {
                const data = await response.json();
                localStorage.setItem("user", JSON.stringify(data));

                if (data.role === true) {
                    console.log("Logged in as admin");
                }
            }

            navigate("/home"); 
        } catch (err) {
            setError(err.message);
        }
    };

    return (
        <main className="main-content">
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
            <p>Don't have an account? <a href="/signup">Sign up</a></p>
        </div>
        </main>
    );
};

export default LoginForm;
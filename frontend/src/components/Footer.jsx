import React from "react";
import { Link, useNavigate } from "react-router-dom";
import { getAuthUser } from "../utils/auth";
import "/src/styles/footer.css";

const Footer = () => {
    const user = getAuthUser();
    const isAdmin = user.role === true;
    const navigate = useNavigate();

    const handleLogout = () => {
        localStorage.removeItem('user');
        localStorage.removeItem('token');
        sessionStorage.removeItem('user');
        sessionStorage.removeItem('token');
        
        navigate('/');
    };

    return (
        <footer className="footer">
            <nav className="footer-nav">
                <Link to="/home" className="footer-link">HOME</Link>
                {isAdmin && (
                    <>
                        <Link to="/books/add" className="footer-link">Add Book</Link>
                        <Link to="/authors/add" className="footer-link">Add Author</Link>
                    </>
                )}
                <Link to="/loans" className="footer-link">Loans</Link>
                <Link to="/" className="footer-link" onClick={handleLogout}>LOGOUT</Link>
            </nav>
        </footer>
    );
};

export default Footer;
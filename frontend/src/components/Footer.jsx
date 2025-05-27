import React from "react";
import { Link } from "react-router-dom";
import "/src/styles/footer.css";

const Footer = () => {
    return (
        <footer className="footer">
            <nav className="footer-nav">
                <Link to="/home" className="footer-link">HOME</Link>
                <Link to="/books/add" className="footer-link">Add Book</Link>
                <Link to="/authors/add" className="footer-link">Add author</Link>
            </nav>
        </footer>
    );
};

export default Footer;

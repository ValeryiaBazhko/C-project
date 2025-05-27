import BookList from './components/BookList';
import LoginForm from "./components/LoginForm";
import BookForm from './components/BookForm';
import AuthorForm from './components/AuthorForm';
import UpdateForm from './components/UpdateForm';
import SignUpForm from './components/Signup';
import { Routes, Route, useNavigate } from 'react-router-dom';
import React, { useState, useEffect } from 'react';
import {getAuthUser, isAuthenticated, setAuthUser} from './utils/auth';
import "/src/styles/mystyle.css";
import Footer from './components/Footer';

function App() {
    const [authChecked, setAuthChecked] = useState(false);
    const [showFooter, setShowFooter] = useState(false);
    const navigate = useNavigate();
    
    useEffect(() => {
        const user = getAuthUser();
        const authStatus = !!user;
        setShowFooter(authStatus);
        setAuthChecked(true);
        
        if (!authStatus && !['/', '/signup'].includes(location.pathname)) {
            navigate('/');
        }
    }, [navigate, location.pathname]);

    const handleAuthSuccess = (userData) => {
        setAuthUser(userData);
        setShowFooter(true);
        navigate('/home');
    };

    
    if (!authChecked) {
        return null; 
    }

    return (
        <div>
            <Routes>
                <Route path="/" element={<LoginForm onAuthSuccess={handleAuthSuccess} />} />
                <Route path="/signup" element={<SignUpForm onAuthSuccess={handleAuthSuccess} />} />
                
                <Route path="/home" element={<BookList />} />
                <Route path="/books/add" element={<BookForm />} />
                <Route path="/authors/add" element={<AuthorForm />} />
                <Route path="/books/edit/:id" element={<UpdateForm />} />
            </Routes>

            {showFooter && <Footer />}
        </div>
    );
}

export default App;
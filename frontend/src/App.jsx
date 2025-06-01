import React, { useState, useEffect } from 'react';
import { Routes, Route, useNavigate, useLocation } from 'react-router-dom';
import BookList from './components/BookList';
import LoginForm from "./components/LoginForm";
import BookForm from './components/BookForm';
import AuthorForm from './components/AuthorForm';
import UpdateForm from './components/UpdateForm';
import SignUpForm from './components/Signup';
import { getAuthUser, isAuthenticated, setAuthUser } from './utils/auth';
import "/src/styles/mystyle.css";
import Footer from './components/Footer';
import BorrowBook from "./components/BorrowBookForm.jsx";
import LoansView from "./components/LoansView.jsx";

function App() {
    const [authChecked, setAuthChecked] = useState(false);
    const [showFooter, setShowFooter] = useState(false);
    const navigate = useNavigate();
    const location = useLocation(); 

    useEffect(() => {
        const user = getAuthUser();
        const authStatus = !!user;
        const isLoginOrSignup = ['/', '/signup'].includes(location.pathname);

        setShowFooter(authStatus && !isLoginOrSignup); 
        setAuthChecked(true);

        if (!authStatus && !isLoginOrSignup) {
            navigate('/');
        }
    }, [navigate, location.pathname]); 

    const handleAuthSuccess = (userData) => {
        setAuthUser(userData);
        setShowFooter(true);
        navigate('/home');
    };

    if (!authChecked) return null;

    return (
        <div>
            <Routes>
                <Route path="/" element={<LoginForm onAuthSuccess={handleAuthSuccess} />} />
                <Route path="/signup" element={<SignUpForm onAuthSuccess={handleAuthSuccess} />} />
                <Route path="/home" element={<BookList />} />
                <Route path="/books/add" element={<BookForm />} />
                <Route path="/authors/add" element={<AuthorForm />} />
                <Route path="/books/edit/:id" element={<UpdateForm />} />
                <Route path="/loans/borrow/:bookId" element={<BorrowBook />} />
                <Route path="/loans" element={<LoansView />} />
            </Routes>

            {showFooter && <Footer />}
        </div>
    );
}

export default App;


import React from 'react';
import BookList from './components/BookList';
import BookForm from './components/BookForm';
import AuthorForm from './components/AuthorForm';
import UpdateForm from './components/UpdateForm';
import { Router, Routes, Route } from 'react-router-dom';
import "/src/styles/mystyle.css";
import Footer from './components/Footer';

function App() {
  return (
    <div>
      <Routes>
        <Route path="/" element={<BookList />} />
        <Route path="/books/add" element={<BookForm />} />
        <Route path="/authors/add" element={<AuthorForm />} />
        <Route path="/books/edit/:id" element={<UpdateForm />} />

      </Routes>
      <Footer />
    </div>
  );
};


export default App;

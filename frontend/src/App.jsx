
import React from 'react';
import BookList from './components/BookList';
import BookForm from './components/BookForm';
import AuthorForm from './components/AuthorForm';

const App = () => {
  return (
    <div>
      <h1>Library Management</h1>
      <BookList />
      <BookForm />
      <AuthorForm />
    </div>
  );
};


export default App;

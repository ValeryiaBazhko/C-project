import { useState, useEffect } from "react";
import { useParams, useNavigate } from "react-router-dom";
import { getAuthUser } from "../utils/auth";
import "/src/styles/mystyle.css";

const BorrowBook = () => {
    const { bookId } = useParams();
    const [book, setBook] = useState(null);
    const [fromDate, setFromDate] = useState("");
    const [dueDate, setDueDate] = useState("");
    const [loading, setLoading] = useState(false);
    const [errors, setErrors] = useState({
        fromDate: "",
        dueDate: "",
        general: ""
    });
    const navigate = useNavigate();
    const user = getAuthUser();

    const BASE_URL = "https://11f9-95-159-226-202.ngrok-free.app"; 

    useEffect(() => {
        fetchBook();
        // Set default dates
        const today = new Date();
        const twoWeeksLater = new Date();
        twoWeeksLater.setDate(today.getDate() + 14);

        setFromDate(today.toISOString().split('T')[0]);
        setDueDate(twoWeeksLater.toISOString().split('T')[0]);
    }, [bookId]);

    const fetchBook = async () => {
        try {
            const res = await fetch(`${BASE_URL}/api/books/${bookId}`);
            if (!res.ok) throw new Error("Failed to fetch book details");

            const data = await res.json();
            setBook(data);
        } catch (error) {
            console.error("Error fetching book details: ", error);
            setErrors(prev => ({ ...prev, general: "Error fetching book details" }));
        }
    };

    const validateForm = () => {
        const newErrors = {};
        const today = new Date();
        const fromDateObj = new Date(fromDate);
        const dueDateObj = new Date(dueDate);

        if (!fromDate) {
            newErrors.fromDate = "From date is required";
        } else if (fromDateObj < today) {
            newErrors.fromDate = "From date cannot be in the past";
        }

        if (!dueDate) {
            newErrors.dueDate = "Due date is required";
        } else if (dueDateObj <= fromDateObj) {
            newErrors.dueDate = "Due date must be after from date";
        }
        
        if (fromDateObj && dueDateObj) {
            const diffDays = (dueDateObj - fromDateObj) / (1000 * 60 * 60 * 24);
            if (diffDays > 30) {
                newErrors.dueDate = "Loan period cannot exceed 30 days";
            }
        }

        setErrors(newErrors);
        return Object.keys(newErrors).length === 0;
    };

    const handleSubmit = async (e) => {
        e.preventDefault();

        if (!validateForm()) {
            return;
        }

        setLoading(true);

        try {
            console.log('Current user object:', user); 
            
            const loanData = {
                id: 0, // For new loans
                userId: user.id,
                bookId: parseInt(bookId),
                fromDate: fromDate + "T00:00:00.000Z",
                dueDate: dueDate + "T23:59:59.000Z",
                checkoutDate: new Date().toISOString(),
                status: "Active",
                returnDate: "0001-01-01T00:00:00.000Z"
            };

            console.log('Sending loan request:', loanData);

            const res = await fetch(`${BASE_URL}/api/loans`, {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                },
                body: JSON.stringify(loanData),
            });

            if (!res.ok) {
                const errorText = await res.text();
                throw new Error(`Failed to create loan: ${errorText}`);
            }
            
            navigate("/home");
        } catch (error) {
            console.error("Error creating loan: ", error);
            setErrors(prev => ({ ...prev, general: error.message || "Error creating loan" }));
        } finally {
            setLoading(false);
        }
    };

    if (!book) {
        return (
            <main className="main-content">
                <div className="form-container">
                    <p className="text-center">Loading book details...</p>
                </div>
            </main>
        );
    }

    return (
        <main className="main-content">
            <div className="form-container">
                <h2>Borrow Book</h2>

                {/* Book Details */}
                <div className="book-details mb-3">
                    <h3 className="book-title">{book.title}</h3>
                    <p className="book-info">Published: {book.publicationYear}</p>
                    <p className="book-info">Genre: {book.genre}</p>
                </div>

                {errors.general && (
                    <div className="error-message text-center mb-3">
                        {errors.general}
                    </div>
                )}

                <form onSubmit={handleSubmit} className="book-form">
                    <div className="form-group">
                        <label>From Date:</label>
                        <input
                            type="date"
                            className={`form-input ${errors.fromDate ? 'error' : ''}`}
                            value={fromDate}
                            onChange={(e) => setFromDate(e.target.value)}
                            required
                        />
                        {errors.fromDate && <div className="error-message">{errors.fromDate}</div>}
                    </div>

                    <div className="form-group">
                        <label>Due Date:</label>
                        <input
                            type="date"
                            className={`form-input ${errors.dueDate ? 'error' : ''}`}
                            value={dueDate}
                            onChange={(e) => setDueDate(e.target.value)}
                            required
                        />
                        {errors.dueDate && <div className="error-message">{errors.dueDate}</div>}
                    </div>

                    <div className="loan-summary mb-3">
                        <h4>Loan Summary:</h4>
                        <p><strong>Book:</strong> {book.title}</p>
                        <p><strong>Borrower:</strong> {user.firstName || user.FirstName || 'Unknown'} {user.lastName || user.LastName || 'User'}</p>
                        <p><strong>Loan Period:</strong> {fromDate} to {dueDate}</p>
                        {fromDate && dueDate && (
                            <p><strong>Duration:</strong> {Math.ceil((new Date(dueDate) - new Date(fromDate)) / (1000 * 60 * 60 * 24))} days</p>
                        )}
                    </div>

                    <div className="form-actions">
                        <button
                            type="submit"
                            className="submit-button"
                            disabled={loading}
                        >
                            {loading ? "Creating Loan..." : "Confirm Borrow"}
                        </button>
                        <button
                            type="button"
                            className="cancel-button"
                            onClick={() => navigate("/home")}
                            disabled={loading}
                        >
                            Cancel
                        </button>
                    </div>
                </form>
            </div>
        </main>
    );
};

export default BorrowBook;
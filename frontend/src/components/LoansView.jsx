import { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { getAuthUser } from "../utils/auth";
import "/src/styles/mystyle.css";

const LoansView = () => {
    const [loans, setLoans] = useState([]);
    const [authors, setAuthors] = useState({});
    const [pageNum, setPageNum] = useState(1);
    const [pageSize] = useState(10);
    const [totalPages, setTotalPages] = useState(1);
    const [loading, setLoading] = useState(true);
    const [statusFilter, setStatusFilter] = useState("");
    const navigate = useNavigate();

    const user = getAuthUser();
    const isAdmin = user.role === true;

    const BASE_URL = "https://11f9-95-159-226-202.ngrok-free.app"; 

    useEffect(() => {
        fetchLoans();
        fetchAuthors();
    }, [pageNum, statusFilter]);

    const fetchLoans = async () => {
        try {
            setLoading(true);
            let url;

            if (isAdmin) {
                if (statusFilter) {
                    url = `${BASE_URL}/api/loans/status/${statusFilter}`;
                } else {
                    url = `${BASE_URL}/api/loans?pageNum=${pageNum}&pageSize=${pageSize}`;
                }
            } else {
                url = `${BASE_URL}/api/loans/user/${user.id}`;
            }

            console.log('Fetching loans from:', url);

            const res = await fetch(url);
            if (!res.ok) throw new Error("Failed to fetch loans");

            const data = await res.json();

            if (isAdmin && !statusFilter) {
                setLoans(data.loans || []);
                setTotalPages(data.totalPages || 1);
            } else {
                setLoans(Array.isArray(data) ? data : []);
                setTotalPages(1);
            }
        } catch (error) {
            console.error("Error fetching loans: ", error);
            setLoans([]);
        } finally {
            setLoading(false);
        }
    };

    const fetchAuthors = async () => {
        try {
            const res = await fetch(`${BASE_URL}/api/authors`);
            if (!res.ok) throw new Error("Failed to fetch authors");

            const data = await res.json();
            const authorMap = {};
            data.forEach(author => {
                authorMap[author.id] = author.name;
            });
            setAuthors(authorMap);
        } catch (error) {
            console.error("Error fetching authors: ", error);
        }
    };

    const returnLoan = async (loanId) => {
        if (!window.confirm("Are you sure you want to mark this loan as returned?")) return;

        try {
            const res = await fetch(`${BASE_URL}/api/loans/${loanId}/return`, {
                method: "PUT",
                headers: {
                    "Content-Type": "application/json",
                }
            });

            if (!res.ok) throw new Error("Failed to return loan");

            // Refresh the loans list
            fetchLoans();
        } catch (error) {
            console.error("Error returning loan: ", error);
            alert("Error returning loan. Please try again.");
        }
    };

    const deleteLoan = async (loanId) => {
        if (!window.confirm("Are you sure you want to delete this loan?")) return;

        try {
            const res = await fetch(`${BASE_URL}/api/loans/${loanId}`, {
                method: "DELETE",
                headers: {
                    "Content-Type": "application/json",
                }
            });

            if (!res.ok) throw new Error("Failed to delete loan");

            // Refresh the loans list
            fetchLoans();
        } catch (error) {
            console.error("Error deleting loan: ", error);
            alert("Error deleting loan. Please try again.");
        }
    };

    const formatDate = (dateString) => {
        const date = new Date(dateString);
        return date.toLocaleDateString();
    };

    const isOverdue = (loan) => {
        const today = new Date();
        const dueDate = new Date(loan.dueDate);
        return dueDate < today && loan.status.toLowerCase() !== 'returned';
    };

    const getStatusColor = (loan) => {
        if (loan.status.toLowerCase() === 'returned') return 'var(--success-color)';
        if (isOverdue(loan)) return 'var(--danger-color)';
        return 'var(--primary-color)';
    };

    if (loading) {
        return (
            <main className="main-content">
                <div className="form-container">
                    <p className="text-center">Loading loans...</p>
                </div>
            </main>
        );
    }

    return (
        <main className="main-content">
            <div className="form-container">
                <h2>{isAdmin ? "All Loans" : "My Loans"}</h2>

                {isAdmin && (
                    <div className="search-container">
                        <div className="form-group">
                            <label>Filter by Status:</label>
                            <select
                                className="form-input"
                                value={statusFilter}
                                onChange={(e) => setStatusFilter(e.target.value)}
                            >
                                <option value="">All Statuses</option>
                                <option value="Active">Active</option>
                                <option value="Returned">Returned</option>
                                <option value="Overdue">Overdue</option>
                                <option value="Renewed">Renewed</option>
                            </select>
                        </div>
                    </div>
                )}

                <div className="loans-list-container">
                    {loans.length > 0 ? (
                        <div className="loan-cards">
                            {loans.map((loan) => (
                                <div key={loan.id} className="loan-card">
                                    <div className="loan-info">
                                        <h3 className="loan-book-title">{loan.book?.title || "Unknown Book"}</h3>
                                        <p className="loan-detail">
                                            <strong>Author:</strong> {authors[loan.book?.authorId] || "Unknown"}
                                        </p>
                                        <p className="loan-detail">
                                            <strong>Borrower:</strong> {loan.user?.firstName} {loan.user?.lastName}
                                        </p>
                                        <p className="loan-detail">
                                            <strong>From:</strong> {formatDate(loan.fromDate)}
                                            <strong> To:</strong> {formatDate(loan.dueDate)}
                                        </p>
                                        <p className="loan-detail">
                                            <strong>Checkout:</strong> {formatDate(loan.checkoutDate)}
                                        </p>
                                        {loan.returnDate && loan.returnDate !== "0001-01-01T00:00:00" && (
                                            <p className="loan-detail">
                                                <strong>Returned:</strong> {formatDate(loan.returnDate)}
                                            </p>
                                        )}
                                        <div className="loan-status">
                                            <span
                                                className="status-badge"
                                                style={{ backgroundColor: getStatusColor(loan) }}
                                            >
                                                {isOverdue(loan) ? 'OVERDUE' : loan.status.toUpperCase()}
                                            </span>
                                        </div>
                                    </div>

                                    <div className="form-actions">
                                        {loan.status.toLowerCase() !== 'returned' && (
                                            <button
                                                className="submit-button"
                                                onClick={() => returnLoan(loan.id)}
                                            >
                                                Return Book
                                            </button>
                                        )}
                                        {isAdmin && (
                                            <button
                                                className="cancel-button"
                                                onClick={() => deleteLoan(loan.id)}
                                                style={{backgroundColor: 'var(--danger-color)', color: 'white'}}
                                            >
                                                Delete
                                            </button>
                                        )}
                                    </div>
                                </div>
                            ))}
                        </div>
                    ) : (
                        <p className="text-center">
                            {isAdmin ? "No loans found" : "You have no loans"}
                        </p>
                    )}
                </div>

                {isAdmin && !statusFilter && (
                    <div className="form-actions mt-3">
                        <button
                            disabled={pageNum <= 1}
                            onClick={() => setPageNum(prev => prev - 1)}
                            className="cancel-button"
                        >
                            Previous
                        </button>
                        <span className="page-info"> Page {pageNum} of {totalPages} </span>
                        <button
                            disabled={pageNum >= totalPages}
                            onClick={() => setPageNum(prev => prev + 1)}
                            className="submit-button"
                        >
                            Next
                        </button>
                    </div>
                )}
            </div>
        </main>
    );
};

export default LoansView;
export const getAuthUser = () => {
    const user = localStorage.getItem('user');
    return user ? JSON.parse(user) : null;
};

export const isAuthenticated = () => {
    return !!getAuthUser();
};

export const setAuthUser = (userData) => {
    localStorage.setItem('user', JSON.stringify(userData));
};

export const clearAuth = () => {
    localStorage.removeItem('user');
};
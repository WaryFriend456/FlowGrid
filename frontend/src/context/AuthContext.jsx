import { createContext, useState, useContext, use } from "react";

const AuthContext = createContext(null);

export const AuthProvider = ({ children }) => {
    const [user, setUser] = useState(null); // user object with details
    const [token, setToken] = useState(localStorage.getItem("token") || null); // JWT token

    const login = (newToken) => {
        localStorage.setItem("token", newToken);
        setToken(newToken);
        // Fetch user details using the token
    }

    const logout = () => {
        localStorage.removeItem("token");
        setToken(null);
        setUser(null);
    }

    const authContextValue = {
        user,
        token,
        login,
        logout
    }

    return (
        <AuthContext.Provider value={authContextValue}>
            {children}
        </AuthContext.Provider>
    );
}

export const useAuth = () => {
    return useContext(AuthContext);
};
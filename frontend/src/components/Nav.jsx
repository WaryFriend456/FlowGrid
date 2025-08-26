import { Link, useNavigate } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';

function Nav() {

    const { token, logout } = useAuth();
    const navigate = useNavigate();

    const handleLogout = () => {
        logout();
        navigate('/login');
    };

    return (
        <nav style={{ padding: '1rem', borderBottom: '1px solid #ccc' }}>
            {token ? (
                <>
                    <Link to="/dashboard" style={{ marginRight: '1rem', border: '2px solid white', padding: "1rem" }}>Home</Link>
                    <button onClick={handleLogout}> Logout </button>
                </>
            ) : (
                <>
                    <Link to="/login" style={{ marginRight: '1rem', border: '2px solid white', padding: "1rem" }}>Login</Link>
                    <Link to="/register" style={{ marginRight: '1rem', border: '2px solid white', padding: "1rem" }}>Register</Link>
                </>
            )}
        </nav>
    );
}
export default Nav;
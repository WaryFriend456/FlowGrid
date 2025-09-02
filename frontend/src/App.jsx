import './App.css'
import { Routes, Route, Navigate } from 'react-router-dom'
import Nav from './components/Nav'
import Login from './pages/Login'
import Register from './pages/Register'
import Dashboard from './pages/Dashboard'
import { useAuth } from './context/AuthContext'
import BoardPage from './pages/BoardPage'

function App() {

  const { token } = useAuth();

  return (
    <>
      <Nav />
      <main/>
      <Routes>
        <Route path="/" element={token ? <Navigate to="/dashboard" /> : <Navigate to="/login" />} />
        <Route path="/login" element={<Login />} />
        <Route path="/register" element={<Register />} />
        <Route path="/dashboard" element={token ? <Dashboard /> : <Navigate to="/login" />} />
        <Route path='/board/:boardId' element={token ? <BoardPage /> : <Navigate to="/login" />} />
        <Route path="*" element={<Navigate to={token ? "/dashboard" : "/login"} />} />
      </Routes>
    </>
  )
}

export default App

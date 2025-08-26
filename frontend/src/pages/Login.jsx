import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { useAuth } from "../context/AuthContext";

function Login() {
    const [formData, setFormData] = useState({
        username: "",
        password: "",
    });

    const navigate = useNavigate();
    const { login } = useAuth();

    const handleChange = (e) => {
        const {name, value} = e.target;
        setFormData({
            ...formData,
            [name]: value,
        });
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        try{
            const response = await fetch("https://localhost:7226/api/Auth/login", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                },
                body: JSON.stringify(formData),
            });

            const data = await response.json();

            if(!response.ok){
                alert(`Login Failed : ${data.message}`);
                return;
            }

            console.log("Login successful:", data.token);
            alert("Login Successful");
            login(data.token);
            navigate("/dashboard");
        }catch(error){
            // const errorMessages = errorData.map(error => error.description).join('\n');
            console.log("Login failed Please try again:", error);
        }
    }
  return(
    <>
    <h2>Login</h2>
    <div>
    <form onSubmit={handleSubmit}>
        <input 
        type="text" 
        name="username" 
        value={formData.username} 
        onChange={handleChange} 
        placeholder="Username" 
        />
        <input 
        type="password" 
        name="password" 
        value={formData.password} 
        onChange={handleChange} 
        placeholder="Password" />
        <button type="submit">Login</button>
    </form>
    </div>
    </>
  );
}
export default Login;
import { useState } from "react";
import { useNavigate } from "react-router-dom";

function Register() {
    const [formData, setFormData] = useState({
        username: "",
        email: "",
        password: "",
    });

    const navigate = useNavigate();

    const handleChange = (e) => {
        const {name ,value} = e.target;
        setFormData({
            ...formData,
            [name]: value,
        });
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        try{
            console.log("Submitting form data:", formData);
            const response = await fetch("https://localhost:7226/api/Auth/register", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                },
                body: JSON.stringify(formData),
            });

            if(!response.ok){
                const errorData = await response.json();
                const errorMessages = errorData.map(error => error.description).join('\n');
                alert(`Registration Failed : "${errorMessages}"`);
                return;
            }

            const data = await response.json();
            console.log("Registration successful:", data.token);
            alert("Registration Successful");
            navigate("/login");

        }catch(error){
            console.log("Error during registration:", error);
            alert("Registration Failed, please try again later.");
        }
    };

    return(
        <>
        <div>
            <h2>Registration</h2>
            <form onSubmit={handleSubmit}>
                <input 
                type="text" 
                name="username"
                placeholder="Username"
                value={formData.username}
                onChange={handleChange}
                required
                />
                <input 
                type="email" 
                name="email"
                placeholder="Email"
                value={formData.email}
                onChange={handleChange}
                required
                />
                <input 
                type="password"
                name="password"
                placeholder="Password"
                value={formData.password}
                onChange={handleChange}
                required
                />
                <button type="submit">Register</button>
            </form>
        </div>
        </>
    );
}
export default Register;
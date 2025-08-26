import { authFetch } from "../services/api.js";
import { useState, useEffect } from "react";

function Dashboard() {
    const [message, setMessage] = useState("");
    const [error, setError] = useState("");

    useEffect(() => {
        const fetchData = async () => {
            try {
                const data = await authFetch("https://localhost:7226/api/Auth/protected");
                console.log(data);
                setMessage(data.message);
            }catch(error){
                setError(error.message);
            }
        };

        fetchData();
    }, []);

  return (
    <div>
        <h1>Dashboard</h1>
      {message && <p style={{color: 'green'}}>{message}</p>}
      {error && <p style={{color: 'red'}}>{error}</p>}
    </div>
  );
}
export default Dashboard;
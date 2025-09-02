import { useState } from "react";
import { authFetch } from "../services/api";

export default function AddListForm({ boardId, onListAdded}){

    const [title, setTitle] = useState("");
    const [error, setError] = useState("");

    const handleSubmit = async (e) => {
        e.preventDefault();
        if(!title.trim()){
            return;
        }

        try{
            await authFetch(`https://localhost:7226/api/board/${boardId}/tasklist`, {
                method: "POST",
                body: JSON.stringify({ title}),
            });
            setTitle("");
            onListAdded(); // Notify parent to refresh lists
        } catch (error) {
            setError(error.message);
        }
    }

    return (
        <form onSubmit={handleSubmit} style={{ margin: "1rem", minWidth: "250px"}}>
            <input type="text"
            value={title}
            onChange={(e) => setTitle(e.target.value)}
            placeholder="Enter List Title..." />
            <button type="submit">Add List</button>
            {error && <p style={{color: "red"}}>{error}</p>}
        </form>
    );
}
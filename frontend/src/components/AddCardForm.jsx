import { useState } from "react";
import { authFetch } from "../services/api";

export default function AddCardForm({ listId, onCardAdded }) {

    const [title, setTitle] = useState("");
    const [description, setDescription] = useState("");
    const [isAdding, setIsAdding] = useState(false); // State to track if the form is being displayed

    const handleSubmit = async (e) => {
        e.preventDefault();
        if(!title.trim()) return;

        try{
            await authFetch(`https://localhost:7226/api/tasklist/${listId}/card`,
                {
                    method: "POST",
                    body: JSON.stringify({ title, description }),
                }
            );
            setTitle("");
            setDescription("");
            setIsAdding(false);
            onCardAdded(); // Notify parent to refresh cards
        } catch (err){
            console.error("Error adding card:", err);
        }
    };

    if(!isAdding){
        return (
            <button onClick={ () => setIsAdding(true)} style={{marginTop: "1rem", width: "100%"}}>
                + add a card
            </button>
        );
    }

    return (
        <form onSubmit={handleSubmit}
        style={{
            marginTop: "1rem",
            display: "flex",
            flexDirection: "column",
            gap: "0.5rem",
        }}>
            <input
            type="text"
            value={title}
            onChange={(e) => setTitle(e.target.value)}
            placeholder="Card Title"
            />
            <textarea
            value={description}
            onChange={(e) => setDescription(e.target.value)}
            placeholder="Card Description"
            />
            <div>
                <button type="submit">Add Card</button>
                <button type="button" onClick={() => setIsAdding(false)}>Cancel</button>
            </div>
        </form>
    );

}
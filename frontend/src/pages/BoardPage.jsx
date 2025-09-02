import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { authFetch } from "../services/api";
import TaskList from "../components/TaskList";
import AddListForm from "../components/AddListForm";

export default function BoardPage() {
    const { boardId } = useParams();
    const [board, setBoard] = useState(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState("");

    const fetchBoard = async () => {
        try {
            const data = await authFetch(`https://localhost:7226/api/board/${boardId}`);
            setBoard(data);
        } catch (err) {
            setError(err.message);
        } finally {
            setLoading(false);
        }
    };
    useEffect(() => {
        fetchBoard();
    }, [boardId]); // Re-fetch board data when boardId changes

    if (loading) return <p>Loading...</p>;
    if (error) return <p style={{ color: "red" }}>Error: {error}</p>;

    return (
        <>
            <div>
                <h1>Board: {board?.title}</h1>
                <div style={{ display: "flex", flexDirection: "row", overflowX: "auto" }}>
                    <AddListForm boardId={boardId} onListAdded={fetchBoard} />
                    {board?.lists.map(list => (
                        <TaskList key={list.id} list={list} />
                    ))}
                </div>
            </div>
        </>
    );
}
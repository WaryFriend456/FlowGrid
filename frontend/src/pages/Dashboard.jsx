import { authFetch } from "../services/api.js";
import { useState, useEffect } from "react";
import { Link } from "react-router-dom";

function Dashboard() {
  const [boards, setBoards] = useState([]);
  const [newBoardTitle, setNewBoardTitle] = useState("");
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");

  // GET Boards
  const fetchBoards = async () => {
    try {
      setLoading(true);
      const data = await authFetch("https://localhost:7226/api/Board");
      setBoards(data);
    } catch (err) {
      setError(err.message);
    } finally {
      setLoading(false);
    }
  };

  // Fetch boards on component when first rendered/loads
  useEffect(() => {
    fetchBoards();
  }, []);

  // handle create board
  const handleCreateBoard = async (e) => {
    e.preventDefault();
    if(!newBoardTitle.trim()) return;

    try{
      await authFetch("https://localhost:7226/api/Board", {
        method : "POST",
        body : JSON.stringify({ title: newBoardTitle }),
      });
      setNewBoardTitle("");
      fetchBoards();
    } catch (err) {
      setError(err.message);
    }
  };

  if (loading) return <p>Loading...</p>;
  if (error) return <p style={{ color: 'red' }}>Error : {error}</p>;

  return (
    <>
      <div>
        <h1>Your Boards</h1>
        <form onSubmit={handleCreateBoard}>
          <input
          type="text"
          value={newBoardTitle}
          onChange={(e) => setNewBoardTitle(e.target.value)}
          placeholder="New Board Title"/>
          <button type="submit">Create Board</button>
        </form>
        <ul>
          {boards.map(board => (
            <li key={board.id}>
              <Link to={`/board/${board.id}`}>{board.title}</Link>
              {/* {board.title} */}
            </li>
          ))}
        </ul>
      </div>
    </>
  );
}
export default Dashboard;
export default function Card({ card }){
    return (
        <>
        <div style={{ border: "1px solid grey", padding: "0.5rem", margin: "0.5rem", backgroundColor: "#444"}}>
            <h4>{card.title}</h4>
            <p>{card.description}</p>
        </div>
        </>
    );
}
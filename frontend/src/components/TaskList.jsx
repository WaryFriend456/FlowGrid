import Card from "./Card";

export default function TaskList( { list } ){
    return (
        <>
        <div style={{ border: "1px solid grey", padding: "1rem", margin: "1rem", minWidth: "250px"}}>
            <h3>{list.title}</h3>
            <div>
                {list.cards.map(card => (
                    <Card key={card.id} card={card} />
                ))}
            </div>
        </div>
        </>
    );
}
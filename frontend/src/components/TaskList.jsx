import AddCardForm from "./AddCardForm";
import Card from "./Card";

export default function TaskList( { list, onBoardUpdate } ){
    return (
        <>
        <div style={{ border: "1px solid grey", padding: "1rem", margin: "1rem", minWidth: "250px"}}>
            <h3>{list.title}</h3>
            <div className="cards-container">
                {list.cards.map(card => (
                    <Card key={card.id} card={card} />
                ))}
            </div>
            <AddCardForm listId={list.id} onCardAdded={onBoardUpdate} />
        </div>
        </>
    );
}
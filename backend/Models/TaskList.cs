using System.ComponentModel.DataAnnotations;

namespace backend.Models
{
    public class TaskList
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public int Order { get; set; }

        // Foreign key to the Board it belongs to
        public int BoardId { get; set; }
        public Board Board { get; set; }

        // Navigation property to the cards in this list
        public List<Card> Cards { get; set; } = new();
    }
}

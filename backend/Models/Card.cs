using System.ComponentModel.DataAnnotations;

namespace backend.Models
{
    public class Card
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public string? Description { get; set; }

        // Foreign key to the list it belongs to
        public int ListId { get; set; }
        public TaskList List {  get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace backend.Models
{
    public class Board
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        // Foreign key to the User who owns the board
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }

        // Navigation property to the lists in this board
        public List<TaskList> Lists { get; set; } = new();
    }
}

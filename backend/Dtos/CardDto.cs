using System.ComponentModel.DataAnnotations;

namespace backend.Dtos
{
    public class CardDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public int ListId { get; set; }
    }

    public class CreateCardDto
    {
        [Required]
        public string Title { get; set; }
        public string? Description { get; set; }
    }

    public class UpdateCardDto
    {
        [Required]
        public string Title { get; set; }
        public string? Description { get; set; }
    }
}

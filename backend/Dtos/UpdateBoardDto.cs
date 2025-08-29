using System.ComponentModel.DataAnnotations;

namespace backend.Dtos
{
    public class UpdateBoardDto
    {
        [Required]
        [MinLength(3, ErrorMessage = "Title must be at least 3 characters long.")]
        public string Title { get; set; }
    }
}

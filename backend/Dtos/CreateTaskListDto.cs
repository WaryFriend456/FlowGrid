using System.ComponentModel.DataAnnotations;

namespace backend.Dtos
{
    public class CreateTaskListDto
    {
        [Required]
        public string Title { get; set; }
    }
}

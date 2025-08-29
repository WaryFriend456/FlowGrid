namespace backend.Dtos
{
    public class BoardDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public List<TaskListDto> Lists { get; set; } = new();
    }
}

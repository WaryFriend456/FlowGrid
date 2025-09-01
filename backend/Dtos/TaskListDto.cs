namespace backend.Dtos
{
    public class TaskListDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Order { get; set; }
        public int BoardId { get; set; }
        public List<CardDto> Cards { get; set; } = new();
    }
}

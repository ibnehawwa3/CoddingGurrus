
namespace CoddingGurrus.Core.Dtos.Tutorials
{
    public class ContentDto
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public long TopicId { get; set; }
        public int TotalRecords { get; set; }
    }
}

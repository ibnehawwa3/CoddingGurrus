
namespace CoddingGurrus.Core.Models.Tutorials
{
    public class ContentModel
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public long TopicId { get; set; }
    }

    public class ContentViewModel : ContentModel
    {
        public long CourseId { get; set; }
    }
}

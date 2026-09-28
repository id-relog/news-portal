namespace WebAppUI.Models.Analytics
{
    public class AnalyticsIndexViewModel
    {
        public int Days { get; set; }
        public int Take { get; set; }
        public List<TopArticleViewItem> Items { get; set; } = new();
    }

    public class TopArticleViewItem
    {
        public Guid ArticleId { get; set; }
        public string Title { get; set; } = string.Empty;
        public int Views { get; set; }
    }
}


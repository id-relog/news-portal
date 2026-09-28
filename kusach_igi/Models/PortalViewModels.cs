namespace kusach_igi.Models;

public class CategoryViewModel
{
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int ArticleCount { get; set; }
}

public class ArticleCardViewModel
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string Summary { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DateTime PublishedAt { get; set; }
    public int ReadMinutes { get; set; }
    public int Views { get; set; }
    public int CommentsCount { get; set; }
    public bool IsFeatured { get; set; }
    public List<string> Tags { get; set; } = new();
}

public class HomePageViewModel
{
    public List<ArticleCardViewModel> FeaturedArticles { get; set; } = new();
    public List<ArticleCardViewModel> LatestArticles { get; set; } = new();
    public List<CategoryViewModel> Categories { get; set; } = new();
    public List<string> EditorialSteps { get; set; } = new();
}

public class NewsListViewModel
{
    public string? SelectedCategory { get; set; }
    public string SearchQuery { get; set; } = string.Empty;
    public List<CategoryViewModel> Categories { get; set; } = new();
    public List<ArticleCardViewModel> Articles { get; set; } = new();
}

public class NewsDetailsViewModel
{
    public ArticleCardViewModel Article { get; set; } = new();
    public List<ArticleCardViewModel> RelatedArticles { get; set; } = new();
}

using kusach_igi.Models;

namespace kusach_igi.Data;

public static class PortalContentRepository
{
    private static readonly List<ArticleCardViewModel> Articles =
    [
        new()
        {
            Id = 1,
            Title = "Цифровая редакция университета: как устроен учебный новостной портал",
            Slug = "digital-campus-newsroom",
            Category = "Университет",
            Author = "Анна Коваль",
            Summary = "Обзор структуры портала: публикации, модерация, аналитика и удобная навигация без отдельного API.",
            Content = "Портал объединяет редакторов, корреспондентов и читателей в одном MVC-приложении. Контент проходит через понятный жизненный цикл: создание материала, редакторская проверка, публикация и последующий анализ отклика аудитории. Главная задача системы — показать новости университета в структурированном и подробном формате, сохраняя удобство работы без отдельного API-слоя.",
            PublishedAt = DateTime.Today.AddDays(-1).AddHours(-3),
            ReadMinutes = 6,
            Views = 842,
            CommentsCount = 14,
            IsFeatured = true,
            Tags = ["MVC", "Редакция", "Учебный проект"]
        },
        new()
        {
            Id = 2,
            Title = "Как организовать модерацию статей и комментариев в одном приложении",
            Slug = "moderation-workflow",
            Category = "Модерация",
            Author = "Илья Новик",
            Summary = "Практическая схема для ролей администратора, модератора и автора с понятными этапами проверки.",
            Content = "Внутри одного веб-приложения можно реализовать полный цикл модерации: статус материала, замечания, повторную отправку и отслеживание изменений. Такой подход особенно удобен для учебного проекта, когда важно показать бизнес-логику и интерфейс, а не разносить всё по нескольким сервисам.",
            PublishedAt = DateTime.Today.AddDays(-2).AddHours(-5),
            ReadMinutes = 5,
            Views = 613,
            CommentsCount = 9,
            Tags = ["Модерация", "Workflow", "ASP.NET Core"]
        },
        new()
        {
            Id = 3,
            Title = "Раздел аналитики: какие показатели полезны редактору",
            Slug = "analytics-for-editor",
            Category = "Аналитика",
            Author = "Мария Гончар",
            Summary = "Просмотры, вовлечённость, время публикации и категории, которые привлекают больше внимания.",
            Content = "Даже в учебном портале аналитика должна быть читаемой: список популярных публикаций, трендовые категории, ключевые показатели по вовлечённости и простые сравнительные блоки. Это помогает показать, что приложение не только хранит данные, но и даёт редакции материал для решений.",
            PublishedAt = DateTime.Today.AddDays(-3).AddHours(-7),
            ReadMinutes = 4,
            Views = 521,
            CommentsCount = 6,
            IsFeatured = true,
            Tags = ["Аналитика", "Dashboards", "Контент"]
        },
        new()
        {
            Id = 4,
            Title = "Категории, теги и поиск: как сделать навигацию по новостям удобной",
            Slug = "navigation-categories-tags-search",
            Category = "Навигация",
            Author = "Олег Романов",
            Summary = "Подробный пример структуры каталога новостей с фильтрацией по разделам и быстрым поиском.",
            Content = "Чтобы пользователь быстро находил нужные материалы, новости нужно описывать через категории и теги. Категория отвечает за крупный раздел, а тег помогает объединить материалы по теме. В интерфейсе это удобно показывать карточками, фильтрами и блоком связанных публикаций.",
            PublishedAt = DateTime.Today.AddDays(-4).AddHours(-2),
            ReadMinutes = 7,
            Views = 468,
            CommentsCount = 11,
            Tags = ["Поиск", "Категории", "UX"]
        },
        new()
        {
            Id = 5,
            Title = "Подробная карточка новости в MVC: что показать пользователю",
            Slug = "detailed-news-page-mvc",
            Category = "Интерфейс",
            Author = "София Левченко",
            Summary = "Состав подробной страницы: текст, метаданные, связанные материалы, теги и показатели статьи.",
            Content = "Детальная страница публикации должна содержать не только основной текст, но и дополнительный контекст: автора, дату, время чтения, количество просмотров, теги и ссылки на похожие новости. Такой экран делает приложение заметно богаче даже без интеграции с внешним API.",
            PublishedAt = DateTime.Today.AddDays(-5).AddHours(-1),
            ReadMinutes = 6,
            Views = 395,
            CommentsCount = 5,
            Tags = ["UI", "Страница статьи", "Razor"]
        }
    ];

    private static readonly List<CategoryViewModel> Categories =
    [
        new() { Name = "Университет", Slug = "university", Description = "Новости кампуса, кафедр и студенческой жизни.", ArticleCount = 12 },
        new() { Name = "Модерация", Slug = "moderation", Description = "Материалы про проверку контента и роли пользователей.", ArticleCount = 8 },
        new() { Name = "Аналитика", Slug = "analytics", Description = "Показатели публикаций, просмотры и вовлечённость.", ArticleCount = 6 },
        new() { Name = "Навигация", Slug = "navigation", Description = "Поиск, фильтры, категории и пользовательские сценарии.", ArticleCount = 9 },
        new() { Name = "Интерфейс", Slug = "ui", Description = "Экранные формы, карточки новостей и удобство чтения.", ArticleCount = 7 }
    ];

    public static HomePageViewModel GetHomePage()
    {
        return new HomePageViewModel
        {
            FeaturedArticles = Articles.Where(a => a.IsFeatured).OrderByDescending(a => a.PublishedAt).ToList(),
            LatestArticles = Articles.OrderByDescending(a => a.PublishedAt).Take(4).ToList(),
            Categories = Categories,
            EditorialSteps =
            [
                "Автор готовит материал и описывает его через категорию, теги и краткое описание.",
                "Модератор проверяет содержание, оформление и готовность публикации.",
                "После публикации материал попадает на главную страницу и в тематические разделы.",
                "Редакция отслеживает просмотры и вовлечённость для следующих выпусков."
            ]
        };
    }

    public static NewsListViewModel GetNews(string? category, string? query)
    {
        var items = Articles.AsEnumerable();

        if (!string.IsNullOrWhiteSpace(category))
        {
            items = items.Where(a => a.Category.Equals(category, StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrWhiteSpace(query))
        {
            items = items.Where(a =>
                a.Title.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                a.Summary.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                a.Tags.Any(t => t.Contains(query, StringComparison.OrdinalIgnoreCase)));
        }

        return new NewsListViewModel
        {
            SelectedCategory = category,
            SearchQuery = query ?? string.Empty,
            Categories = Categories,
            Articles = items.OrderByDescending(a => a.PublishedAt).ToList()
        };
    }

    public static NewsDetailsViewModel? GetArticle(string slug)
    {
        var article = Articles.FirstOrDefault(a => a.Slug.Equals(slug, StringComparison.OrdinalIgnoreCase));
        if (article is null)
        {
            return null;
        }

        return new NewsDetailsViewModel
        {
            Article = article,
            RelatedArticles = Articles
                .Where(a => a.Slug != slug && (a.Category == article.Category || a.Tags.Intersect(article.Tags).Any()))
                .OrderByDescending(a => a.PublishedAt)
                .Take(3)
                .ToList()
        };
    }
}

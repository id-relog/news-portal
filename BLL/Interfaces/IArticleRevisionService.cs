using BLL.DTO;

namespace BLL.Interfaces
{
    public interface IArticleRevisionService : IService<ArticleRevisionDTO>
    {
        List<ArticleRevisionDTO> GetForArticle(Guid articleId);
        ArticleRevisionDTO? CreateRevision(Guid articleId, string editedByUserId, string title, string? summary, string content);
    }
}


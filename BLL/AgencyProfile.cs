using AutoMapper;
using BLL.DTO;
using DAL.entity;

namespace BLL
{
    public class AgencyProfile : Profile
    {
        public AgencyProfile()
        {
            CreateMap<NewsCategory, NewsCategoryDTO>().ReverseMap();

            CreateMap<NewsArticle, NewsArticleDTO>()
                .ForMember(d => d.CategoryName, opt => opt.MapFrom(s => s.Category.Name))
                .ForMember(d => d.Status, opt => opt.MapFrom(s => (ArticleStatusDTO)s.Status))
                .ForMember(d => d.Tags, opt => opt.MapFrom(s => s.Tags.Select(t => t.Tag.Name).ToList()));

            CreateMap<NewsArticleDTO, NewsArticle>()
                .ForMember(d => d.Category, opt => opt.Ignore())
                .ForMember(d => d.Tags, opt => opt.Ignore())
                .ForMember(d => d.Status, opt => opt.MapFrom(s => (ArticleStatus)s.Status));

            CreateMap<ArticleComment, ArticleCommentDTO>()
                .ForMember(d => d.Status, opt => opt.MapFrom(s => (CommentStatusDTO)s.Status));

            CreateMap<ArticleCommentDTO, ArticleComment>()
                .ForMember(d => d.Article, opt => opt.Ignore())
                .ForMember(d => d.Status, opt => opt.MapFrom(s => (CommentStatus)s.Status));

            CreateMap<NewsTag, NewsTagDTO>().ReverseMap();
            CreateMap<NewsSource, NewsSourceDTO>().ReverseMap();

            CreateMap<ArticleRevision, ArticleRevisionDTO>().ReverseMap()
                .ForMember(d => d.Article, opt => opt.Ignore());

            CreateMap<CategorySubscription, CategorySubscriptionDTO>()
                .ForMember(d => d.CategoryName, opt => opt.MapFrom(s => s.Category.Name));

            CreateMap<CategorySubscriptionDTO, CategorySubscription>()
                .ForMember(d => d.Category, opt => opt.Ignore());

            CreateMap<UserBan, UserBanDTO>().ReverseMap();
        }
    }
}
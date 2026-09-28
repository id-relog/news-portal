using BLL.Interfaces;
using BLL.Services;
using DAL.Configurations;
using Microsoft.Extensions.DependencyInjection;

namespace BLL
{
    public static class ConfigurationExtensions
    {
        public static void ConfigureBLL(this IServiceCollection services, string connection)
        {
            services.ConfigureDAL(connection);

            services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<AgencyProfile>();
            });

            services.AddScoped<INewsCategoryService, NewsCategoryService>();
            services.AddScoped<INewsArticleService, NewsArticleService>();
            services.AddScoped<IArticleCommentService, ArticleCommentService>();

            services.AddScoped<INewsTagService, NewsTagService>();
            services.AddScoped<INewsSourceService, NewsSourceService>();
            services.AddScoped<IArticleRevisionService, ArticleRevisionService>();
            services.AddScoped<IArticleAnalyticsService, ArticleAnalyticsService>();
            services.AddScoped<IArticleReactionService, ArticleReactionService>();
            services.AddScoped<ICategorySubscriptionService, CategorySubscriptionService>();
            services.AddScoped<IUserBanService, UserBanService>();
        }
    }
}
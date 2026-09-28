using DAL.Interfaces;
using DAL.repositoryes;
using DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DAL.Configurations
{
    public static class ConfigurationExtensions
    {
        public static void ConfigureDAL(this IServiceCollection services, string connection)
        {
            services.AddDbContext<AgencyContext>(options =>
            {
                options.UseNpgsql(connection);

        });

            services.AddScoped<INewsCategoryRepository, NewsCategoryRepository>();
            services.AddScoped<INewsArticleRepository, NewsArticleRepository>();
            services.AddScoped<IArticleCommentRepository, ArticleCommentRepository>();

            services.AddScoped<INewsTagRepository, NewsTagRepository>();
            services.AddScoped<INewsSourceRepository, NewsSourceRepository>();
            services.AddScoped<IArticleRevisionRepository, ArticleRevisionRepository>();
            services.AddScoped<IArticleViewEventRepository, ArticleViewEventRepository>();
            services.AddScoped<IArticleReactionsRepository, ArticleReactionsRepository>();
            services.AddScoped<ICategorySubscriptionRepository, CategorySubscriptionRepository>();
            services.AddScoped<IUserBanRepository, UserBanRepository>();
        }
    }
}
using DAL.entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Configuration;
using Microsoft.Extensions.Configuration.Json;

namespace DAL
{
    public class AgencyContext : IdentityDbContext
    {
        public DbSet<NewsCategory> NewsCategories { get; set; }
        public DbSet<NewsArticle> NewsArticles { get; set; }
        public DbSet<ArticleComment> ArticleComments { get; set; }
        public DbSet<NewsTag> NewsTags { get; set; }
        public DbSet<ArticleTag> ArticleTags { get; set; }
        public DbSet<NewsSource> NewsSources { get; set; }
        public DbSet<ArticleRevision> ArticleRevisions { get; set; }
        public DbSet<ArticleViewEvent> ArticleViewEvents { get; set; }
        public DbSet<ArticleReaction> ArticleReactions { get; set; }
        public DbSet<CategorySubscription> CategorySubscriptions { get; set; }
        public DbSet<UserBan> UserBans { get; set; }

        public AgencyContext() { }

        public AgencyContext(DbContextOptions<AgencyContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .Build();

                optionsBuilder.UseNpgsql(configuration.GetConnectionString("postgresConnection"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                var currentTableName = entity.GetTableName();
                if (currentTableName != null)
                {
                    entity.SetTableName(currentTableName.ToLowerInvariant());
                }

                foreach (var property in entity.GetProperties())
                {
                    var currentColumnName = property.GetColumnName(StoreObjectIdentifier.Table(entity.GetTableName()!, entity.GetSchema()));
                    if (currentColumnName != null)
                    {
                            property.SetColumnName(currentColumnName.ToLowerInvariant());
                    }
                }
            }
            modelBuilder.Entity<NewsCategory>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Name).IsRequired().HasMaxLength(128);
                entity.Property(x => x.Slug).IsRequired().HasMaxLength(128);
                entity.HasIndex(x => x.Slug).IsUnique();
            });

            modelBuilder.Entity<NewsArticle>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Title).IsRequired().HasMaxLength(256);
                entity.Property(x => x.Slug).IsRequired().HasMaxLength(256);
                entity.HasIndex(x => x.Slug).IsUnique();
                entity.Property(x => x.Content).IsRequired();
                entity.Property(x => x.ImageUrl).HasMaxLength(512);
                entity.Property(x => x.AuthorUserId).IsRequired().HasMaxLength(450);
                entity.Property(x => x.ReviewedByUserId).HasMaxLength(450);
                entity.Property(x => x.ReviewNote).HasMaxLength(1024);
                entity.HasOne(x => x.Category)
                    .WithMany()
                    .HasForeignKey(x => x.CategoryId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.Source)
                    .WithMany()
                    .HasForeignKey(x => x.SourceId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity<ArticleComment>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.DisplayName).IsRequired().HasMaxLength(128);
                entity.Property(x => x.Body).IsRequired().HasMaxLength(4000);
                entity.Property(x => x.AuthorUserId).HasMaxLength(450);
                entity.Property(x => x.ModeratedByUserId).HasMaxLength(450);
                entity.HasOne(x => x.Article)
                    .WithMany()
                    .HasForeignKey(x => x.ArticleId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<NewsTag>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Name).IsRequired().HasMaxLength(128);
                entity.Property(x => x.Slug).IsRequired().HasMaxLength(128);
                entity.HasIndex(x => x.Slug).IsUnique();
            });

            modelBuilder.Entity<ArticleTag>(entity =>
            {
                entity.HasKey(x => new { x.ArticleId, x.TagId });
                entity.HasOne(x => x.Article)
                    .WithMany(a => a.Tags)
                    .HasForeignKey(x => x.ArticleId)
                    .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(x => x.Tag)
                    .WithMany()
                    .HasForeignKey(x => x.TagId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<NewsSource>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Name).IsRequired().HasMaxLength(128);
                entity.Property(x => x.Url).HasMaxLength(512);
            });

            modelBuilder.Entity<ArticleRevision>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Title).IsRequired().HasMaxLength(256);
                entity.Property(x => x.Content).IsRequired();
                entity.Property(x => x.EditedByUserId).IsRequired().HasMaxLength(450);
                entity.HasIndex(x => new { x.ArticleId, x.RevisionNumber }).IsUnique();
                entity.HasOne(x => x.Article)
                    .WithMany()
                    .HasForeignKey(x => x.ArticleId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<ArticleViewEvent>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.ViewerUserId).HasMaxLength(450);
                entity.Property(x => x.IpHash).HasMaxLength(128);
                entity.Property(x => x.UserAgent).HasMaxLength(512);
                entity.HasOne(x => x.Article)
                    .WithMany()
                    .HasForeignKey(x => x.ArticleId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<ArticleReaction>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.UserId).IsRequired().HasMaxLength(450);
                entity.HasIndex(x => new { x.ArticleId, x.UserId }).IsUnique();
                entity.HasOne(x => x.Article)
                    .WithMany()
                    .HasForeignKey(x => x.ArticleId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<CategorySubscription>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.UserId).IsRequired().HasMaxLength(450);
                entity.HasIndex(x => new { x.UserId, x.CategoryId }).IsUnique();
                entity.HasOne(x => x.Category)
                    .WithMany()
                    .HasForeignKey(x => x.CategoryId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<UserBan>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.UserId).IsRequired().HasMaxLength(450);
                entity.Property(x => x.BannedByUserId).IsRequired().HasMaxLength(450);
                entity.Property(x => x.Reason).IsRequired().HasMaxLength(256);
                entity.Property(x => x.EvidenceMessage).IsRequired().HasMaxLength(4000);
                entity.HasIndex(x => new { x.UserId, x.IsActive });
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
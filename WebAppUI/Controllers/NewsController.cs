using BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text;
using WebAppUI.Auth;
using WebAppUI.Models.News;

namespace WebAppUI.Controllers
{
    public class NewsController : Controller
    {
        private readonly INewsArticleService _articleService;
        private readonly INewsCategoryService _categoryService;
        private readonly IArticleCommentService _commentService;
        private readonly IArticleAnalyticsService _analyticsService;
        private readonly IArticleReactionService _reactionService;
        private readonly IUserBanService _banService;
        private readonly IWebHostEnvironment _environment;

        public NewsController(
            INewsArticleService articleService,
            INewsCategoryService categoryService,
            IArticleCommentService commentService,
            IArticleAnalyticsService analyticsService,
            IArticleReactionService reactionService,
            IUserBanService banService,
            IWebHostEnvironment environment)
        {
            _articleService = articleService;
            _categoryService = categoryService;
            _commentService = commentService;
            _analyticsService = analyticsService;
            _reactionService = reactionService;
            _banService = banService;
            _environment = environment;
        }

        [AllowAnonymous]
        [HttpGet("/news")]
        public IActionResult Index(string? q, string? category, int page = 1, int pageSize = 10)
        {
            var articles = _articleService.GetPublished(50);
            if (!string.IsNullOrWhiteSpace(q))
            {
                articles = articles
                    .Where(a => a.Title.Contains(q, StringComparison.OrdinalIgnoreCase)
                        || (a.Summary ?? string.Empty).Contains(q, StringComparison.OrdinalIgnoreCase)
                        || a.Tags.Any(t => t.Contains(q, StringComparison.OrdinalIgnoreCase)))
                    .ToList();
            }

            if (!string.IsNullOrWhiteSpace(category))
            {
                articles = articles
                    .Where(a => string.Equals(a.CategoryName, category, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            foreach (var article in articles)
            {
                article.ViewsCount = _analyticsService.GetTotalViews(article.Id);
                var reactions = _reactionService.GetCounts(article.Id);
                article.LikesCount = reactions.likes;
                article.DislikesCount = reactions.dislikes;
            }

            if (pageSize < 5) pageSize = 5;
            if (pageSize > 30) pageSize = 30;
            if (page < 1) page = 1;
            var totalCount = articles.Count;

            var paged = articles
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return View(new NewsIndexViewModel
            {
                Articles = paged,
                Query = q,
                Category = category,
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount,
                Categories = _categoryService.Get()
            });
        }

        [AllowAnonymous]
        [HttpGet("/news/{slug}")]
        public IActionResult Details(string slug)
        {
            var article = _articleService.GetBySlug(slug);
            if (article == null) return NotFound();
            var canWorkWithProcessing =
                User.IsInRole(AppRoles.Administrator)
                || User.IsInRole(AppRoles.Manager)
                || User.IsInRole(AppRoles.Correspondent)
                || User.IsInRole(AppRoles.Moderator);
            if (article.Status != BLL.DTO.ArticleStatusDTO.Published && !canWorkWithProcessing)
                return Forbid();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var cookieName = $"viewed_{article.Id}";
            if (Request.Cookies[cookieName] == null)
            {
                _analyticsService.TrackView(article.Id, userId, null, Request.Headers.UserAgent.ToString());
                Response.Cookies.Append(cookieName, "1", new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddHours(24),
                    HttpOnly = true,
                    IsEssential = true
                });
            }
            article.ViewsCount = _analyticsService.GetTotalViews(article.Id);
            var reactions = _reactionService.GetCounts(article.Id);
            article.LikesCount = reactions.likes;
            article.DislikesCount = reactions.dislikes;

            var comments = _commentService.GetForArticle(article.Id);
            return View(new NewsDetailsViewModel { Article = article, Comments = comments });
        }

        [Authorize(Roles = $"{AppRoles.Administrator},{AppRoles.Manager},{AppRoles.Correspondent},{AppRoles.Moderator}")]
        [HttpGet("/news/manage")]
        public IActionResult Manage()
        {
            var articles = _articleService.Get();
            return View(new NewsManageViewModel { Articles = articles });
        }

        [Authorize(Roles = $"{AppRoles.Administrator},{AppRoles.Manager},{AppRoles.Correspondent}")]
        [HttpGet("/news/create")]
        public IActionResult Create()
        {
            return View("Editor", BuildEditorModel(new NewsEditorViewModel()));
        }

        [Authorize(Roles = $"{AppRoles.Administrator},{AppRoles.Manager},{AppRoles.Correspondent}")]
        [HttpPost("/news/create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NewsEditorViewModel model)
        {
            model = BuildEditorModel(model);
            if (!ModelState.IsValid) return View("Editor", model);

            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(currentUserId)) return Challenge();

            var slug = string.IsNullOrWhiteSpace(model.Slug) ? Slugify(model.Title) : Slugify(model.Slug);

            string? imageUrl;
            if (!string.IsNullOrWhiteSpace(model.ImageUrl))
            {
                imageUrl = model.ImageUrl.Trim();
            }
            else
            {
                imageUrl = await SaveImageAsync(model.ImageFile);
            }

            var article = new BLL.DTO.NewsArticleDTO
            {
                Id = Guid.NewGuid(),
                Title = model.Title.Trim(),
                Slug = slug,
                Summary = model.Summary?.Trim(),
                Content = model.Content.Trim(),
                CategoryId = model.CategoryId,
                AuthorUserId = currentUserId,
                ImageUrl = imageUrl,
                Tags = ParseTags(model.Tags)
            };

            if (!_articleService.Add(article))
            {
                ModelState.AddModelError(string.Empty, "Не удалось создать новость.");
                return View("Editor", model);
            }

            TempData["SuccessMessage"] = "Новость создана как черновик.";
            return RedirectToAction(nameof(Manage));
        }

        [Authorize(Roles = $"{AppRoles.Administrator},{AppRoles.Manager},{AppRoles.Correspondent}")]
        [HttpGet("/news/edit/{id:guid}")]
        public IActionResult Edit(Guid id)
        {
            var article = _articleService.Get(id);
            if (article == null) return NotFound();

            var model = new NewsEditorViewModel
            {
                Id = article.Id,
                Title = article.Title,
                Slug = article.Slug,
                Summary = article.Summary,
                Content = article.Content,
                CategoryId = article.CategoryId,
                Tags = string.Join(", ", article.Tags),
                ImageUrl = article.ImageUrl,
                CurrentImageUrl = article.ImageUrl
            };
            return View("Editor", BuildEditorModel(model));
        }

        [Authorize(Roles = $"{AppRoles.Administrator},{AppRoles.Manager},{AppRoles.Correspondent}")]
        [HttpPost("/news/edit/{id:guid}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, NewsEditorViewModel model)
        {
            var existing = _articleService.Get(id);
            if (existing == null) return NotFound();

            model = BuildEditorModel(model);
            if (!ModelState.IsValid) return View("Editor", model);

            var slug = string.IsNullOrWhiteSpace(model.Slug) ? Slugify(model.Title) : Slugify(model.Slug);

            string? imageUrl = existing.ImageUrl;
            if (!string.IsNullOrWhiteSpace(model.ImageUrl))
            {
                imageUrl = model.ImageUrl.Trim();
            }
            else
            {
                var uploadedImage = await SaveImageAsync(model.ImageFile);
                if (!string.IsNullOrWhiteSpace(uploadedImage))
                {
                    imageUrl = uploadedImage;
                }
            }

            existing.Title = model.Title.Trim();
            existing.Slug = slug;
            existing.Summary = model.Summary?.Trim();
            existing.Content = model.Content.Trim();
            existing.CategoryId = model.CategoryId;
            existing.ImageUrl = imageUrl;
            existing.Tags = ParseTags(model.Tags);

            if (!_articleService.Edit(existing))
            {
                ModelState.AddModelError(string.Empty, "Не удалось сохранить изменения.");
                return View("Editor", model);
            }

            TempData["SuccessMessage"] = "Новость обновлена.";
            return RedirectToAction(nameof(Manage));
        }

        [Authorize(Roles = $"{AppRoles.Administrator},{AppRoles.Manager}")]
        [HttpPost("/news/{id:guid}/submit")]
        [ValidateAntiForgeryToken]
        public IActionResult Submit(Guid id)
        {
            _articleService.SubmitForReview(id);
            return RedirectToAction(nameof(Manage));
        }

        [Authorize(Roles = $"{AppRoles.Administrator},{AppRoles.Moderator}")]
        [HttpPost("/news/{id:guid}/approve")]
        [ValidateAntiForgeryToken]
        public IActionResult Approve(Guid id)
        {
            var reviewerId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
            _articleService.Approve(id, reviewerId);
            return RedirectToAction(nameof(Manage));
        }

        [Authorize(Roles = $"{AppRoles.Administrator},{AppRoles.Moderator}")]
        [HttpPost("/news/{id:guid}/reject")]
        [ValidateAntiForgeryToken]
        public IActionResult Reject(Guid id, string? note)
        {
            var reviewerId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
            _articleService.Reject(id, reviewerId, note ?? "Требуются исправления");
            return RedirectToAction(nameof(Manage));
        }

        [Authorize(Roles = $"{AppRoles.Administrator},{AppRoles.Manager}")]
        [HttpPost("/news/{id:guid}/archive")]
        [ValidateAntiForgeryToken]
        public IActionResult Archive(Guid id)
        {
            _articleService.Archive(id);
            return RedirectToAction(nameof(Manage));
        }

        [Authorize(Roles = $"{AppRoles.Administrator},{AppRoles.Manager}")]
        [HttpPost("/news/{id:guid}/delete")]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Guid id)
        {
            _articleService.Delete(id);
            TempData["SuccessMessage"] = "Новость удалена.";
            return RedirectToAction(nameof(Manage));
        }

        [Authorize(Roles = AppRoles.Administrator)]
        [HttpGet("/news/categories")]
        public IActionResult Categories()
        {
            var categories = _categoryService.Get();
            return View(categories);
        }

        [Authorize(Roles = AppRoles.Administrator)]
        [HttpGet("/news/categories/create")]
        public IActionResult CreateCategory()
        {
            return View("CategoryEditor", new CategoryEditorViewModel());
        }

        [Authorize(Roles = AppRoles.Administrator)]
        [HttpPost("/news/categories/create")]
        [ValidateAntiForgeryToken]
        public IActionResult CreateCategory(CategoryEditorViewModel model)
        {
            if (!ModelState.IsValid) return View("CategoryEditor", model);

            var category = new BLL.DTO.NewsCategoryDTO
            {
                Id = Guid.NewGuid(),
                Name = model.Name.Trim(),
                Slug = string.IsNullOrWhiteSpace(model.Slug) ? Slugify(model.Name) : Slugify(model.Slug),
                CreatedAtUtc = DateTime.UtcNow
            };

            if (!_categoryService.Add(category))
            {
                ModelState.AddModelError(string.Empty, "Не удалось создать категорию.");
                return View("CategoryEditor", model);
            }

            TempData["SuccessMessage"] = "Категория добавлена.";
            return RedirectToAction(nameof(Categories));
        }

        [Authorize(Roles = AppRoles.Administrator)]
        [HttpGet("/news/categories/edit/{id:guid}")]
        public IActionResult EditCategory(Guid id)
        {
            var category = _categoryService.Get(id);
            if (category == null) return NotFound();

            return View("CategoryEditor", new CategoryEditorViewModel
            {
                Id = category.Id,
                Name = category.Name,
                Slug = category.Slug
            });
        }

        [Authorize(Roles = AppRoles.Administrator)]
        [HttpPost("/news/categories/edit/{id:guid}")]
        [ValidateAntiForgeryToken]
        public IActionResult EditCategory(Guid id, CategoryEditorViewModel model)
        {
            if (!ModelState.IsValid) return View("CategoryEditor", model);

            var category = _categoryService.Get(id);
            if (category == null) return NotFound();

            category.Name = model.Name.Trim();
            category.Slug = string.IsNullOrWhiteSpace(model.Slug) ? Slugify(model.Name) : Slugify(model.Slug);

            if (!_categoryService.Edit(category))
            {
                ModelState.AddModelError(string.Empty, "Не удалось сохранить категорию.");
                return View("CategoryEditor", model);
            }

            TempData["SuccessMessage"] = "Категория обновлена.";
            return RedirectToAction(nameof(Categories));
        }

        [Authorize(Roles = AppRoles.Administrator)]
        [HttpPost("/news/categories/delete/{id:guid}")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteCategory(Guid id)
        {
            var hasArticles = _articleService.Get().Any(x => x.CategoryId == id);
            if (hasArticles)
            {
                TempData["SuccessMessage"] = "Нельзя удалить категорию: есть связанные новости.";
                return RedirectToAction(nameof(Categories));
            }

            _categoryService.Delete(id);
            TempData["SuccessMessage"] = "Категория удалена.";
            return RedirectToAction(nameof(Categories));
        }

        [Authorize]
        [HttpPost("/news/{slug}/comment")]
        [ValidateAntiForgeryToken]
        public IActionResult AddComment(string slug, [Bind(Prefix = "NewComment")] AddCommentViewModel newComment)
        {
            var article = _articleService.GetBySlug(slug);
            if (article == null) return NotFound();

            article.ViewsCount = _analyticsService.GetTotalViews(article.Id);
            var reactions = _reactionService.GetCounts(article.Id);
            article.LikesCount = reactions.likes;
            article.DislikesCount = reactions.dislikes;

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
            if (_banService.IsBanned(userId))
            {
                TempData["ErrorMessage"] = "Вы не можете комментировать: ваш аккаунт заблокирован.";
                return RedirectToAction(nameof(Details), new { slug });
            }

            var comments = _commentService.GetForArticle(article.Id);
            if (!ModelState.IsValid)
            {
                return View("Details", new NewsDetailsViewModel
                {
                    Article = article,
                    Comments = comments,
                    NewComment = newComment
                });
            }

            var displayName = User.Identity?.Name
                           ?? User.FindFirstValue(ClaimTypes.Email)
                           ?? User.FindFirstValue(ClaimTypes.NameIdentifier)
                           ?? "Пользователь";

            var commentDto = new BLL.DTO.ArticleCommentDTO
            {
                Id = Guid.NewGuid(),
                ArticleId = article.Id,
                AuthorUserId = userId,
                DisplayName = displayName,
                Body = newComment.Body.Trim(),
                Status = BLL.DTO.CommentStatusDTO.Approved,
                CreatedAtUtc = DateTime.UtcNow
            };

            if (!_commentService.Add(commentDto))
            {
                TempData["ErrorMessage"] = "Ошибка при публикации комментария.";
                return RedirectToAction(nameof(Details), new { slug });
            }

            TempData["SuccessMessage"] = "Комментарий успешно опубликован.";
            return RedirectToAction(nameof(Details), new { slug });
        }

        [Authorize(Roles = $"{AppRoles.Administrator},{AppRoles.Moderator}")]
        [HttpPost("comment/{commentId:guid}/delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteComment(Guid commentId, string slug)
        {
            _commentService.Delete(commentId);
            TempData["SuccessMessage"] = "Комментарий удалён модератором.";
            return RedirectToAction(nameof(Details), new { slug });
        }

        [Authorize(Roles = $"{AppRoles.Administrator},{AppRoles.Moderator}")]
        [HttpPost("user/{userId}/ban")]
        [ValidateAntiForgeryToken]
        public IActionResult BanUser(string userId, string slug, Guid? commentId)
        {
            var moderatorId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
            _banService.BanUser(
                userId: userId,
                bannedByUserId: moderatorId,
                reason: "Нарушение правил в комментариях",
                evidenceMessage: "Ручная блокировка через админ-панель",
                evidenceCommentId: commentId
            );
            TempData["SuccessMessage"] = "Пользователь заблокирован.";
            return RedirectToAction(nameof(Details), new { slug });
        }

        [Authorize]
        [HttpPost("/news/{slug}/react")]
        [ValidateAntiForgeryToken]
        public IActionResult React(string slug, bool isLike)
        {
            var article = _articleService.GetBySlug(slug);
            if (article == null) return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(userId)) return Challenge();

            _reactionService.SetReaction(article.Id, userId, isLike);
            return RedirectToAction(nameof(Details), new { slug });
        }

        private NewsEditorViewModel BuildEditorModel(NewsEditorViewModel model)
        {
            model.Categories = _categoryService.Get();
            return model;
        }

        private async Task<string?> SaveImageAsync(IFormFile? file)
        {
            if (file == null || file.Length == 0) return null;

            var uploadsRoot = Path.Combine(_environment.WebRootPath, "uploads", "news");
            Directory.CreateDirectory(uploadsRoot);

            var extension = Path.GetExtension(file.FileName);
            if (string.IsNullOrWhiteSpace(extension))
            {
                extension = ".jpg";
            }

            var fileName = $"{Guid.NewGuid()}{extension.ToLowerInvariant()}";
            var targetPath = Path.Combine(uploadsRoot, fileName);

            await using var stream = new FileStream(targetPath, FileMode.Create);
            await file.CopyToAsync(stream);

            return $"/uploads/news/{fileName}";
        }

        private static List<string> ParseTags(string? tagsInput) =>
            (tagsInput ?? string.Empty)
                .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Select(x => x.Trim())
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

        private static string Slugify(string input)
        {
            var value = (input ?? string.Empty).Trim().ToLowerInvariant();
            if (string.IsNullOrWhiteSpace(value)) return Guid.NewGuid().ToString("N");

            var sb = new StringBuilder();
            var prevDash = false;
            foreach (var ch in value)
            {
                if (char.IsLetterOrDigit(ch))
                {
                    sb.Append(ch);
                    prevDash = false;
                    continue;
                }

                if (!prevDash)
                {
                    sb.Append('-');
                    prevDash = true;
                }
            }

            return sb.ToString().Trim('-');
        }
    }
}
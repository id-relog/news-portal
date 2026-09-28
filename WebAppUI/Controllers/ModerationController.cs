using BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebAppUI.Auth;
using WebAppUI.Models.Moderation;

namespace WebAppUI.Controllers
{
    [Authorize(Roles = $"{AppRoles.Moderator},{AppRoles.Administrator}")]
    public class ModerationController : Controller
    {
        private readonly IArticleCommentService _commentService;
        private readonly IUserBanService _banService;

        public ModerationController(IArticleCommentService commentService, IUserBanService banService)
        {
            _commentService = commentService;
            _banService = banService;
        }

        public IActionResult Index()
        {
            var pending = _commentService.GetPending(200);
            return View(new ModerationQueueViewModel { PendingComments = pending });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Approve(Guid id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
            _commentService.Approve(id, userId);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Reject(Guid id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
            _commentService.Reject(id, userId);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Ban(Guid id, string reason = "Нарушение правил")
        {
            var moderatorId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
            var comment = _commentService.Get(id);
            if (comment == null) return RedirectToAction(nameof(Index));

            if (string.IsNullOrWhiteSpace(comment.AuthorUserId))
            {
                _commentService.Delete(id);
                return RedirectToAction(nameof(Index));
            }

            _banService.BanUser(comment.AuthorUserId, moderatorId, reason, comment.Body, comment.Id);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Guid id)
        {
            _commentService.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}


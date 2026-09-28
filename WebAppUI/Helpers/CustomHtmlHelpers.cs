using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebAppUI.Helpers
{
    public static class CustomHtmlHelpers
    {
        public static IHtmlContent RoleBadge(this IHtmlHelper html, string roleName)
        {
            var badgeClass = roleName switch
            {
                "Admin" => "bg-danger",
                "Manager" => "bg-warning",
                "Client" => "bg-success",
                _ => "bg-secondary"
            };

            var span = new TagBuilder("span");
            span.AddCssClass($"badge {badgeClass} me-1");
            span.InnerHtml.Append(roleName);
            return span;
        }

        public static IHtmlContent HighlightMatch(this IHtmlHelper html, string? text, string? searchTerm)
        {
            text ??= string.Empty;
            if (string.IsNullOrEmpty(searchTerm) || !text.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                return html.Raw(text);

            var index = text.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase);
            var highlighted = text.Insert(index + searchTerm.Length, "</strong>")
                                  .Insert(index, "<strong class='text-danger'>");
            return html.Raw(highlighted);
        }
    }
}
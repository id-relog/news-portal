using System.ComponentModel.DataAnnotations;

namespace WebAppUI.Models.News
{
    public class CategoryEditorViewModel
    {
        public Guid? Id { get; set; }

        [Required]
        [Display(Name = "Название")]
        [MaxLength(128)]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Slug (URL)")]
        [MaxLength(128)]
        public string? Slug { get; set; }
    }
}

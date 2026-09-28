using BLL.DTO;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace WebAppUI.Models.News
{
    public class NewsEditorViewModel
    {
        public Guid? Id { get; set; }

        [Required]
        [Display(Name = "Заголовок")]
        [MaxLength(256)]
        public string Title { get; set; } = string.Empty;

        [Display(Name = "Slug (URL)")]
        [MaxLength(256)]
        public string? Slug { get; set; }

        [Display(Name = "Краткое описание")]
        [MaxLength(512)]
        public string? Summary { get; set; }

        [Required]
        [Display(Name = "Текст новости")]
        public string Content { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Категория")]
        public Guid CategoryId { get; set; }

        [Display(Name = "Теги (через запятую)")]
        public string? Tags { get; set; }

        [Display(Name = "URL изображения (ссылка из интернета)")]
        [MaxLength(512)]
        public string? ImageUrl { get; set; }

        [Display(Name = "Или загрузить файл с компьютера")]
        public IFormFile? ImageFile { get; set; }

        public string? CurrentImageUrl { get; set; }
        public List<NewsCategoryDTO> Categories { get; set; } = new();
    }
}
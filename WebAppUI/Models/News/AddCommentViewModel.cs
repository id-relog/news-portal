using System.ComponentModel.DataAnnotations;

namespace WebAppUI.Models.News
{
    public class AddCommentViewModel
    {
        [Required(ErrorMessage = "Комментарий обязателен")]
        [MaxLength(4000)]
        [Display(Name = "Комментарий")]
        public string Body { get; set; } = string.Empty;
    }
}
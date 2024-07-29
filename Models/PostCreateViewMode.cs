using System.ComponentModel.DataAnnotations;
using BlogApp.Entity;

namespace BlogApp.Models
{
    public class PostCreateViewModel
    {
        [Required]
        [Display(Name = "Title")]

        public string? Title { get; set; }

        [Required]
        [Display(Name = "Description")]

        public string? Description { get; set; }

        [Required]
        [Display(Name = "Content")]

        public string? Content { get; set; }


        [Display(Name = "Image")]
        public IFormFile? ImageFile { get; set; }

        public string? Image { get; set; }


        public List<Tag> Tags { get; set; } = new();
    }
}
using System.ComponentModel.DataAnnotations;

namespace ShopApp.Models;

public class Category
{
    public int Id { get; set; }

    [Required(ErrorMessage = "اسم التصنيف مطلوب")]
    [StringLength(60, MinimumLength = 2, ErrorMessage = "الاسم لازم يكون بين 2 و 60 حرف")]
    [Display(Name = "اسم التصنيف")]
    public string Name { get; set; } = string.Empty;

    [StringLength(250)]
    [Display(Name = "الوصف")]
    public string? Description { get; set; }

    [Display(Name = "تاريخ الإنشاء")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<Product> Products { get; set; } = new List<Product>();
}

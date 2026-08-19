using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopApp.Models;

public class Product
{
    public int Id { get; set; }

    [Required(ErrorMessage = "اسم المنتج مطلوب")]
    [StringLength(100)]
    [Display(Name = "اسم المنتج")]
    public string Name { get; set; } = string.Empty;

    [Range(0.01, 1_000_000, ErrorMessage = "السعر لازم يكون أكبر من صفر")]
    [Column(TypeName = "decimal(18,2)")]
    [Display(Name = "السعر")]
    public decimal Price { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "الكمية لا يمكن أن تكون سالبة")]
    [Display(Name = "الكمية بالمخزن")]
    public int Stock { get; set; }

    [Display(Name = "متاح للبيع")]
    public bool IsAvailable { get; set; } = true;

    [Required(ErrorMessage = "لازم تختار تصنيف")]
    [Display(Name = "التصنيف")]
    public int CategoryId { get; set; }

    public Category? Category { get; set; }
}

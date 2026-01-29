using Domain_Layer.DTOs.Attribute;
using Domain_Layer.DTOs.ProductVariantDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_Layer.DTOs.ProductDtos
{
    public class ProductDetailsDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }

        // الخيارات المجمعة (الكاتالوج) عشان الـ User ينقي منها (مثلاً: Color, Size)
        public List<AttributeGroupDto> AllOptions { get; set; } = new();

        // لستة التوليفات الجاهزة (عشان السعر والستوك لكل اختيار)
        public IEnumerable<ProductVariantDto> Variants { get; set; } = new List<ProductVariantDto>();
    }
}

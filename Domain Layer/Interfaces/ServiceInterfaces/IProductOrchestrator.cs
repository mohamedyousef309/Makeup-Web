using Domain_Layer.DTOs.ProductDtos;
using Domain_Layer.Respones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_Layer.Interfaces.ServiceInterfaces
{
    public interface IProductOrchestrator
    {
        Task<RequestRespones<ProductWithVariantsDto>> CreateProduct(CreateProductDto dto);
        Task<RequestRespones<ProductWithVariantsDto>> UpdateProduct(UpdateProductDto dto);
        Task<RequestRespones<bool>> DeleteProduct(int id);
        Task<RequestRespones<List<ProductWithVariantsDto>>> GetAllProducts();
        Task<RequestRespones<ProductWithVariantsDto>> GetProductById(int id);
    }
}

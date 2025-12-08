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
        Task<RequestRespones<ProductDto>> CreateProduct(CreateProductDto dto);
        Task<RequestRespones<ProductDto>> UpdateProduct(UpdateProductDto dto);
        Task<RequestRespones<bool>> DeleteProduct(int id);
        Task<RequestRespones<List<ProductDto>>> GetAllProducts();
        Task<RequestRespones<ProductDto>> GetProductById(int id);
    }
}

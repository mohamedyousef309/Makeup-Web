using Domain_Layer.DTOs.ProductDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_Layer.Interfaces.ServiceInterfaces
{
    public interface IProductService
    {
        // Create
        Task<ProductDto> CreateProductAsync(CreateProductDto dto);

        // Update
        Task<ProductDto> UpdateProductAsync(UpdateProductDto dto);

        // Delete
        Task<bool> DeleteProductAsync(int id);

        // Get
        Task<ProductDto> GetByIdAsync(int id);

        // Get All
        Task<IEnumerable<ProductDto>> GetAllAsync();

        // Check Exists
        Task<bool> ExistsAsync(int id);
    }
}

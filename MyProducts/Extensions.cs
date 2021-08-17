using MyProducts.Dtos;
using MyProducts.Entities;

namespace MyProducts
{
    /// <summary>
    /// Classe que cuida das extensões convertendo de Entidade para um formato DTO de retorno
    /// </summary>
    public static class Extensions
    {
        public static UserReturnDto AsUserReturnDto(this User user)
        {
            return new UserReturnDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email
            };
        }
        public static ProductReturnDto AsProductReturnDto(this Product product)
        {
            return new ProductReturnDto
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Status = product.Status
            };
        }
    }
}
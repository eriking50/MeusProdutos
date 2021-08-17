using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyProducts.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyProducts.Entities;
using MyProducts.Dtos;
using Microsoft.AspNetCore.Authorization;

namespace MyProducts.Controllers
{
    /// <summary>
    /// Classe que cuida das rotas de produtos
    /// </summary>
    [Route("meusprodutos/produtos")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly MyProductsContext _context;
        public ProductsController(MyProductsContext context)
        {
            _context = context;
        }
        /// <summary>
        /// Retorna os produtos cadastrados no sistema
        /// </summary>
        /// <returns>Produtos cadastrados no sistema</returns>
        /// <response code="200">Lista de produtos retornada com sucesso</response>
        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<IEnumerable<ProductReturnDto>> GetProductsAsync()
        {
            return (await _context.Products.ToListAsync()).Select(product => product.AsProductReturnDto());
        }
        /// <summary>
        /// Retorna um produto com o Id correspondente
        /// </summary>
        /// <returns>Um produto baseado no Id</returns>
        /// <response code="200">Produto retornado com sucesso</response>
        /// <response code="404">O Id não foi encontrado no sistema</response>
        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        
        public async Task<ActionResult<ProductReturnDto>> GetProductAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if(product == null)
            {
                return NotFound();
            }
            return product.AsProductReturnDto();
        }
        /// <summary>
        /// Cria um produto no banco de dados
        /// </summary>
        /// <returns>Produto criado</returns>
        /// <response code="201">Produtos cadastrado com sucesso</response>
        /// <response code="400">Algum dos dados informados não segue o padrão especificado</response>
        /// <response code="401">Você não tem autorização para esta ação</response>
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [Authorize]
        public async Task<ActionResult<ProductReturnDto>> CreateProductAsync(ProductDto product)
        {
            int lastId = await _context.Products.Select(u => u.Id).DefaultIfEmpty().MaxAsync();
            Product crtProduct = new()
            {
                Id = ++lastId,
                Name = product.Name,
                Price = product.Price,
                Status = product.Status
            };

            _context.Products.Add(crtProduct);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetProductAsync), new {id = crtProduct.Id }, crtProduct.AsProductReturnDto());
        }
        /// <summary>
        /// Atualiza o produto com o Id correspondente
        /// </summary>
        /// <response code="204">Produto atualizado com sucesso</response>
        /// <response code="400">O Id não foi encontrado no sistema ou algum dos dados informado não segue o padrão especificado</response>
        /// <response code="401">Você não tem autorização para esta ação</response>
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [Authorize]
        public async Task<ActionResult> UpdateProductAsync(int id, ProductDto product)
        {
            bool hasProduct = await _context.Products.AnyAsync(p => p.Id == id);
            if (!hasProduct)
            {
                return BadRequest("Não existe produto com este ID");
            }
            Product updProduct = new()
            {
                Id = id,
                Name = product.Name,
                Price = product.Price,
                Status = product.Status
            };
            _context.Products.Update(updProduct);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        /// <summary>
        /// Deleta o produto com o Id correspondente
        /// </summary>
        /// <response code="204">Produto deletado com sucesso</response>
        /// <response code="400">O Id não foi encontrado no sistema</response>
        /// <response code="401">Você não tem autorização para esta ação</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [Authorize]
        public async Task<ActionResult> DeleteProductAsync(int id)
        {
            bool hasProduct = await _context.Products.AnyAsync(p => p.Id == id);
            if (!hasProduct)
            {
                return BadRequest("Não existe produto com este ID");
            }
            var dltProduct = await _context.Products.FindAsync(id);
            _context.Products.Remove(dltProduct);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}

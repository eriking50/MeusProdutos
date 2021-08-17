using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyProducts.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyProducts.Entities;
using MyProducts.Dtos;

namespace MyProducts.Controllers
{
    [Route("meusprodutos/produtos")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly MyProductsContext _context;
        public ProductsController(MyProductsContext context)
        {
            _context = context;
        }
        //GET meusprodutos/produtos
        [HttpGet]
        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
            return await _context.Products.ToListAsync();
        }
        //GET meusprodutos/produtos/id
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProductAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if(product == null)
            {
                return NotFound();
            }
            return product;
        }
        //POST meusprodutos/produtos
        [HttpPost]
        public async Task<ActionResult<Product>> CreateProductAsync(ProductDto product)
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
            return CreatedAtAction(nameof(GetProductAsync), new {id = crtProduct.Id }, crtProduct);
        }
        //PUT meusprodutos/produtos/id
        [HttpPut("{id}")]
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
        //DELETE meusprodutos/produtos/id
        [HttpDelete("{id}")]
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

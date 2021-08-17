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
    [Route("meusprodutos/usuarios")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly MyProductsContext _context;
        public UsersController(MyProductsContext context)
        {
            _context = context;
        }
        //GET meusprodutos/usuarios
        [HttpGet]
        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }
        //GET meusprodutos/usuarios/id
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if(user == null)
            {
                return NotFound();
            }
            return user;
        }
        //POST meusprodutos/usuarios
        [HttpPost]
        public async Task<ActionResult<User>> CreateUserAsync(UserDto user)
        {
            bool hasEmail = await _context.Users.AnyAsync(us => us.Email == user.Email);
            if (hasEmail)
            {
                return BadRequest("Esse email já se encontra no banco de dados");
            }
            int lastId = await _context.Users.Select(u => u.Id).DefaultIfEmpty().MaxAsync();
            User crtUser = new()
            {
                Id = ++lastId,
                Name = user.Name,
                Email = user.Email,
                Password = user.Password
            };

            _context.Users.Add(crtUser);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetUserAsync), new {id = crtUser.Id }, crtUser);
        }
        //PUT meusprodutos/usuarios/id
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateUserAsync(int id, UserDto user)
        {
            bool hasUser = await _context.Users.AnyAsync(user => user.Id == id);
            if (!hasUser)
            {
                return BadRequest("Não existe usuário com este ID");
            }
            bool hasEmail = await _context.Users.AnyAsync(us => us.Email == user.Email);
            if (hasEmail)
            {
                return BadRequest("Esse email já se encontra no banco de dados");
            }
            User updUser = new()
            {
                Id = id,
                Name = user.Name,
                Email = user.Email,
                Password = user.Password
            };
            _context.Users.Update(updUser);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        //DELETE meusprodutos/usuarios/id
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUserAsync(int id)
        {
            bool hasUser = await _context.Users.AnyAsync(user => user.Id == id);
            if (!hasUser)
            {
                return BadRequest("Não existe usuário com este ID");
            }
            var dltUser = await _context.Users.FindAsync(id);
            _context.Users.Remove(dltUser);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}

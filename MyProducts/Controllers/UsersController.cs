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
    /// Classe que cuida das rotas de usuários
    /// </summary>
    [Route("meusprodutos/usuarios")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly MyProductsContext _context;
        private readonly PasswordHashing _hashing;
        public UsersController(MyProductsContext context)
        {
            _context = context;
            _hashing = new PasswordHashing();
        }
        /// <summary>
        /// Retorna os usuários cadastrados no sistema
        /// </summary>
        /// <returns>Usuários cadastrados no sistema</returns>
        /// <response code="200">Lista de usuários cadastrados retornada com sucesso</response>
        /// <response code="401">Você não tem autorização para esta ação</response>
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [Authorize]
        public async Task<IEnumerable<UserReturnDto>> GetUsersAsync()
        {
            return (await _context.Users.ToListAsync()).Select(user => user.AsUserReturnDto());
        }
        /// <summary>
        /// Retorna um usuário com o Id correspondente
        /// </summary>
        /// <returns>Um usuário baseado no Id</returns>
        /// <response code="200">Usuário retornado com sucesso</response>
        /// <response code="404">O Id informado não foi encontrado no sistema</response>
        /// <response code="401">Você não tem autorização para esta ação</response>
        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        [Authorize]
        public async Task<ActionResult<UserReturnDto>> GetUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if(user == null)
            {
                return NotFound();
            }
            return user.AsUserReturnDto();
        }
        /// <summary>
        /// Cria um usuário no banco de dados
        /// </summary>
        /// <returns>Usuário criado</returns>
        /// <response code="201">Usuário cadastrado com sucesso</response>
        /// <response code="400">Algum dos dados informados não segue o padrão especificado</response>
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [AllowAnonymous]
        public async Task<ActionResult<UserReturnDto>> CreateUserAsync(UserDto user)
        {
            var hash = _hashing.GetHash(user.Password);
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
                Password = hash.ToString()
            };

            _context.Users.Add(crtUser);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetUserAsync), new {id = crtUser.Id }, crtUser.AsUserReturnDto());
        }
        /// <summary>
        /// Atualiza o usuário com o Id correspondente
        /// </summary>
        /// <response code="204">Usuário atualizado com sucesso</response>
        /// <response code="400">O Id não foi encontrado no sistema ou algum dos dados informado não segue o padrão especificado</response>
        /// <response code="401">Você não tem autorização para esta ação</response>
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [Authorize]
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
            var hash = _hashing.GetHash(user.Password);
            User updUser = new()
            {
                Id = id,
                Name = user.Name,
                Email = user.Email,
                Password = hash
            };
            _context.Users.Update(updUser);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        /// <summary>
        /// Deleta o usuário com o Id correspondente
        /// </summary>
        /// <response code="204">Usuário deletado com sucesso</response>
        /// <response code="400">O Id não foi encontrado no sistema</response>
        /// <response code="401">Você não tem autorização para esta ação</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [Authorize]
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

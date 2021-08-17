using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyProducts.Data;
using MyProducts.Entities;
using MyProducts.Services;
using System.Linq;
using MyProducts.Dtos;
using Microsoft.AspNetCore.Authorization;

namespace MyProducts.Controllers
{
    /// <summary>
    /// Classe que cuida da rota de login
    /// </summary>
    public class LoginController : ControllerBase
    {
        private readonly MyProductsContext _context;
        private readonly TokenService _tokenService;
        private readonly PasswordHashing _hashing;
        public LoginController(MyProductsContext context, TokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
            _hashing = new PasswordHashing();
        }
        /// <summary>
        /// Faz Login usando usuário e senha
        /// </summary>
        /// <returns>Um token de autorização</returns>
        /// <response code="200">Login efetuado com sucesso</response>
        /// <response code="404">Usuário ou senha inválidos</response>
        [HttpPost]
        [Route("login")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [AllowAnonymous]
        public async Task<ActionResult<LoginReturnDto>> Authenticate([FromBody]LoginDto loginUser)
        {
            //Gera o hash da senha enviada
            string passHash = _hashing.GetHash(loginUser.Password);
            //Busca o usuário no banco de dados e valida 
            User user = await _context.Users.SingleOrDefaultAsync(u => u.Name == loginUser.Name && u.Password == passHash);
            if (user == null)
                return NotFound(new { message = "Usuário ou senha inválidos" });

            // Gera o Token
            var token = _tokenService.GenerateToken(user);

            return new LoginReturnDto
            {
                Name = user.Name,
                Token = token
            };
        }
    }
}
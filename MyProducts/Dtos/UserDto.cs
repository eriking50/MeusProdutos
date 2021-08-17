using System.ComponentModel.DataAnnotations;

namespace MyProducts.Dtos
{
    public class UserDto
    {
        [Required(ErrorMessage="O nome do usuário é obrigatório")]
        [MinLength(3, ErrorMessage ="O tamanho mínimo é 3"), MaxLength(20, ErrorMessage ="O tamanho máximo é 20")]
        public string Name { get; set; }

        [Required(ErrorMessage="O email do usuário é obrigatório")]
        [EmailAddress]
        public string Email { get; set; }
        
        [Required(ErrorMessage="A senha é obrigatória")]
        [MinLength(8, ErrorMessage ="O tamanho mínimo é 8"), MaxLength(20, ErrorMessage ="O tamanho máximo é 20")]
        public string Password { get; set; }
    }
}

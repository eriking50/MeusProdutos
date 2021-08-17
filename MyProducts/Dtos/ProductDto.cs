using System.ComponentModel.DataAnnotations;
using System;

namespace MyProducts.Dtos
{
    public class ProductDto
    {
        [Required(ErrorMessage="O nome do produto é obrigatório")]
        [MinLength(3, ErrorMessage ="O tamanho mínimo é 3"), MaxLength(80, ErrorMessage ="O tamanho máximo é 80")]
        public string Name { get; set; }

        [Required(ErrorMessage="O preço do produto é obrigatório")]
        [Range(0, Int32.MaxValue, ErrorMessage = "O preço não pode ser negativo")]
        public double Price { get; set; }

        [Required(ErrorMessage="O status do produto é obrigatório")]
        public bool Status { get; set; }
    }
}
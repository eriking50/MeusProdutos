using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyProducts.Entities
{
    /// <summary>
    /// Entidade de produtos
    /// </summary>
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public bool Status { get; set; }
    }
}

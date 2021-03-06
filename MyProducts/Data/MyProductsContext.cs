using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyProducts.Entities;

namespace MyProducts.Data
{
    /// <summary>
    /// Classe que define o contexto de banco de dados
    /// </summary>
    public class MyProductsContext : DbContext
    {
        public MyProductsContext(DbContextOptions<MyProductsContext> options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
    }
}

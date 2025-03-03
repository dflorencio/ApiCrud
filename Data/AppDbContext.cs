using ApiCrud.Produtos;
using Microsoft.EntityFrameworkCore;


namespace ApiCrud.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Produto> Produtos { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=localhost\\\\SQLEXPRESS,1433; Database=Produtos;User Id=sa; Password=Venda@159; Integrated Security=False;TrustServerCertificate=True;");
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.LogTo(Console.WriteLine);
        }
    }
}
namespace APIClientes.Data
{
    using APIClientes.Models;
    using Microsoft.EntityFrameworkCore;

    public class ApplicationDBContext:DbContext
    {

        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {

        }

        public DbSet<Client> Clients { get; set; }
        public DbSet<User> Users { get; set; }
    }
}

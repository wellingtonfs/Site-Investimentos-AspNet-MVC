using Microsoft.EntityFrameworkCore;
using Sistemas_Distribuidos.Models;
using Sistemas_Distribuidos.Models.Hg;

namespace Sistemas_Distribuidos.Data
{
    public class BancoContext : DbContext
    {
        public BancoContext(DbContextOptions<BancoContext> options) : base(options)
        {
        }

        // Tabelas presentes no banco de dados SQL:

        public DbSet<UserModel> Users { get; set; }
        public DbSet<MoedaModel> Moedas { get; set; }
        public DbSet<IndiceModel> Indices { get; set; }
        public DbSet<CorretoraModel> Corretoras { get; set; }
        public DbSet<TaxaModel> Taxas { get; set; }
    }
}

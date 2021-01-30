using gestionecentralino.Core;
using Microsoft.EntityFrameworkCore;

namespace gestionecentralino.Db
{
    public class CentralinoDbContext : DbContext
    {
        private readonly DbConfiguration _configuration;
        public DbSet<PhoneCallLine> Calls { get; set; }

        public CentralinoDbContext(): base() {}

        public CentralinoDbContext(DbConfiguration configuration) : base()
        {
            _configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            switch (_configuration.Db)
            {
                case DbEnum.SqlServer:
                    optionsBuilder.UseSqlServer(_configuration.ConnectionString);
                    break;

                case DbEnum.MySql:
                    optionsBuilder.UseMySQL(_configuration.ConnectionString);
                    break;

            }
        }
    }
}
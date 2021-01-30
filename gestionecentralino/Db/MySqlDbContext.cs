using gestionecentralino.Core;
using Microsoft.EntityFrameworkCore;

namespace gestionecentralino.Db
{
    public class MySqlDbContext: CentralinoDbContext
    {
        private readonly DbConfiguration _configuration;
        public override DbSet<PhoneCallLine> Calls { get; set; }
        public override void Migrate() => Database.Migrate();

        public MySqlDbContext(DbConfiguration configuration) : base()
        {
            _configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => 
            optionsBuilder.UseMySQL(_configuration.ConnectionString);
    }
}
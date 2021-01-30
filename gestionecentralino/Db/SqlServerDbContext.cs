using gestionecentralino.Core;
using Microsoft.EntityFrameworkCore;

namespace gestionecentralino.Db
{
    public class SqlServerDbContext: CentralinoDbContext
    {
        private readonly DbConfiguration _configuration;
        public override DbSet<PhoneCallLine> Calls { get; set; }
        public override void Migrate() => Database.Migrate();

        public SqlServerDbContext(): this(new DbConfiguration()){}

        public SqlServerDbContext(DbConfiguration configuration) : base()
        {
            _configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (string.IsNullOrWhiteSpace(_configuration.ConnectionString))
            {
                optionsBuilder.UseSqlServer();
            }
            else
            {
                optionsBuilder.UseSqlServer(_configuration.ConnectionString);
            }
        }
    }
}
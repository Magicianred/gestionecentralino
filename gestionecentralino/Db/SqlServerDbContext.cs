using Microsoft.EntityFrameworkCore;

namespace gestionecentralino.Db
{
    public class SqlServerDbContext : CentralinoDbContext
    {
        private readonly string _connectionString;

        public SqlServerDbContext(): base() {}

        public SqlServerDbContext(string connectionString): base()
        {
            _connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }

        public override DbSet<PhoneCallLine> Calls { get; set; }
    }
}
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace gestionecentralino.Db
{
    public class CentralinoDbContext: DbContext
    {
        private readonly string _connectionString;

        private DbSet<PhoneCallLine> Calls { get; set; }

        public CentralinoDbContext(): base()
        {
        }

        public CentralinoDbContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }
    }
}

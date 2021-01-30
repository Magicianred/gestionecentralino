using System.Collections.Generic;
using System.Text;
using gestionecentralino.Core;
using Microsoft.EntityFrameworkCore;

namespace gestionecentralino.Db
{
    public class MySqlDbContext: CentralinoDbContext
    {
        private readonly string _connectionString;

        public MySqlDbContext() : base() {}

        public MySqlDbContext(string connectionString) : base()
        {
            _connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL(_connectionString);
        }

        public override DbSet<PhoneCallLine> Calls { get; set; }
    }
}

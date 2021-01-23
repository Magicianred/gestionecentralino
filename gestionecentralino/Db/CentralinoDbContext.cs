using System.Collections.Generic;
using System.Text;
using gestionecentralino.Core;
using Microsoft.EntityFrameworkCore;

namespace gestionecentralino.Db
{
    public class CentralinoDbContext: DbContext
    {
        private readonly DbConfiguration _configuration;

        private DbSet<PhoneCallLine> Calls { get; set; }

        public CentralinoDbContext(): base()
        {
        }

        public CentralinoDbContext(DbConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_configuration.ConnectionString);
        }
    }
}

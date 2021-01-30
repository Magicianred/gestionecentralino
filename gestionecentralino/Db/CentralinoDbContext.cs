using Microsoft.EntityFrameworkCore;

namespace gestionecentralino.Db
{
    public abstract class CentralinoDbContext : DbContext
    {
        public abstract DbSet<PhoneCallLine> Calls { get; set; }

        public CentralinoDbContext(): base() {}
    }
}
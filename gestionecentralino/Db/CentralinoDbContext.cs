using System.Diagnostics;
using System.Reflection.Metadata;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace gestionecentralino.Db
{
    abstract public class CentralinoDbContext : DbContext
    {
        public abstract DbSet<PhoneCallLine> Calls { get; set; }
        public CentralinoDbContext() : base() {}
    }
}
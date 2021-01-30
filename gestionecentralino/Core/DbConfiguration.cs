using gestionecentralino.Db;

namespace gestionecentralino.Core
{
    public class DbConfiguration
    {
        public DbConfiguration()
        {
            ConnectionString = "";
            Db = DbEnum.SqlServer;
        }

        public string ConnectionString { get; set; }
        public DbEnum Db { get; set; }
    }
}
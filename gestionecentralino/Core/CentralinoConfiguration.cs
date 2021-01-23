namespace gestionecentralino.Core
{
    public class CentralinoConfiguration
    {
        public CentralinoConfiguration(string host, int port, string username, string password)
        {
            Host = host;
            Port = port;
            Username = username;
            Password = password;
            DbConfiguration = new DbConfiguration();
        }

        public string Host { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public DbConfiguration DbConfiguration { get; set; }
    }
}
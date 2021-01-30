using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;
using gestionecentralino.Db;
using LanguageExt;
using static LanguageExt.Prelude;

namespace gestionecentralino.Core
{
    public class CentralinoConfiguration
    {
        private CentralinoConfiguration() : this("127.0.0.1", 2300, "SMDR", "SMDR")
        {
            
        }

        public CentralinoConfiguration(string host, int port, string username, string password)
        {
            Host = host;
            Port = port;
            Username = username;
            Password = password;
            DbConfiguration = new DbConfiguration();
            Show = false;
            Sede = SedeEnum.Darfo;
        }

        public SedeEnum Sede { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public DbConfiguration DbConfiguration { get; set; }

        private static CentralinoConfiguration FromXml(string filePath)
        {
            try
            {
                var reader = new XmlSerializer(typeof(CentralinoConfiguration));
                using var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                using var streamreader = new StreamReader(stream);
                var config = (CentralinoConfiguration)reader.Deserialize(streamreader);
                return config;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"The configuration file is invalid. Reason:");
                Console.WriteLine(ex);
                throw;
            }
        }

        public static Option<CentralinoConfiguration> FromCommanLine(string[] cmdArgs)
        {
            try
            {
                var cfg = new CentralinoConfiguration();
                var options = new OptionSet()
                    .Add("cfg|config-file=", "nome del file di configurazione in cui specificare tutte le opzioni di questo help", v => { cfg = FromXml(v); })

                    .Add("H|hostname=", $"hostname del centralino server. Default {cfg.Host}", v => cfg.Host = v)
                    .Add("P|port=", $"porta del centralino server. Default {cfg.Port}", v => cfg.Port = ToInt(v))
                    .Add("u|username=", $"username. Default {cfg.Username}", v => cfg.Username = v)
                    .Add("p|password=", $"password. Default {cfg.Password}", v => cfg.Password = v)
                    .Add("s|sede=", $"sede di riferimento. Default {cfg.Sede}", v =>
                    {
                        Enum.TryParse(v, ignoreCase: true, out SedeEnum sede);
                        cfg.Sede = sede;
                    })

                    .Add("db=", $"db in uso. Scegliere tra {DbEnum.MySql}|{DbEnum.SqlServer} .Default {cfg.DbConfiguration.Db}", v =>
                    {
                        Enum.TryParse(v, ignoreCase: true, out DbEnum dbType);
                        cfg.DbConfiguration.Db = dbType;
                    })
                    ;

                options.Add("h|help", "Mostra help", v =>
                {
                    cfg.Show = true;
                    options.WriteOptionDescriptions(Console.Out);
                });
                options.Add("v|version", "Stampa il numero di versione", v =>
                {
                    cfg.Show = true;
                    Console.Out.WriteLine(Assembly.GetExecutingAssembly().GetName().Version);
                });

                options.Parse(cmdArgs);

                return cfg.Show
                    ? None
                    : Some(cfg);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return None;
            }
        }

        public bool Show { get; set; }

        private static bool IsPresent(string v) => v != null;

        private static int ToInt(string v) => Int32.Parse(v);

        public override string ToString() => ToStringProperties(this);

        private string ToStringProperties(object o, string tabs = "   ") =>
            o.GetType()
                .GetProperties()
                .Aggregate("", (acc, prop) => $"{acc}\n{tabs}{prop.Name}: { (IsClass(prop) ? prop.PropertyType.Name + ToStringProperties(prop.GetValue(o), $"{tabs}   ") : prop.GetValue(o)) }");


        private bool IsClass(PropertyInfo prop) => prop.PropertyType.IsClass && prop.PropertyType != typeof(string);
    }
}
using System.Configuration;
using System.IO;
using System.Reflection;

namespace UTEC.FB.Lead.App_Start
{
    public class AppSettings
    {
        private static readonly string _IdListFileName;
        static AppSettings()
        {
            string Apppath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            var IdListFileName = ConfigurationManager.AppSettings["Id_Lead.FileName"];
            _IdListFileName = Path.Combine(Apppath, IdListFileName);
        }
        public static string Id_Lead { get => _IdListFileName; }

    }
}
using System;
using System.Linq;
using System.Configuration;
using System.IO.Compression;
using System.IO;

namespace ConsoleApp2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //stringify settings
            string srce = ConfigurationManager.AppSettings.Get("srce");
            string dest = ConfigurationManager.AppSettings.Get("dest");
            int mnth = int.Parse(ConfigurationManager.AppSettings.Get("mnth"));
            string type = ConfigurationManager.AppSettings.Get("type");

            //specify source
            DirectoryInfo DirInfo = new DirectoryInfo(@srce);

            //specify desired month
            DateTime zipmnth = DateTime.UtcNow.AddMonths(-mnth);
            DateTime zipmnth2 = new DateTime(zipmnth.Year, zipmnth.Month, 1);
            DateTime zipmnth3 = new DateTime(zipmnth.Year, zipmnth.Month, DateTime.DaysInMonth(zipmnth.Year,zipmnth.Month));

            //create folder naming schema
            string schema = zipmnth.Year + "_" + zipmnth.Month + "_" + type;
            //create folder to be zipped
            string getzipped = Path.Combine(dest, schema);
            Directory.CreateDirectory(getzipped);

            //find files 
            var files = from f in DirInfo.EnumerateFiles()
                        where f.CreationTimeUtc >= zipmnth2
                        where f.CreationTimeUtc <= zipmnth3
                        select f;

            //move files
            foreach (var f in files)
            {
                string filename = Path.Combine(getzipped, f.Name);
                //list files
                Console.WriteLine("{0}", f.Name);
                File.Move(f.FullName, filename);
            }
            //zip the folder
            string zipdest = Path.Combine(dest, schema + ".zip");
            ZipFile.CreateFromDirectory(getzipped, zipdest);
            Directory.Delete(getzipped, true);

        }
    }
}

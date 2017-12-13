using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.IO;

namespace Backups
{
    class Program
    {
        private static string BackupFileName
        {
            get
            {
                return Path.Combine(ConfigurationManager.AppSettings["MySqlBackupFolder"], string.Concat("{0} - ", DateTime.Now.ToString("yyyy-MM-dd -- HH_mm_ss"), ".sql"));
            }
        }

        static void Main(string[] args)
        {
            BackupSelectedDatabase(ConfigurationManager.AppSettings["KodiVideos"]);
            BackupSelectedDatabase(ConfigurationManager.AppSettings["KodiMusic"]);
            BackupSelectedDatabase(ConfigurationManager.AppSettings["PasteBin"]);
            BackupSelectedDatabase(ConfigurationManager.AppSettings["FAQ"]);
            BackupSelectedDatabase(ConfigurationManager.AppSettings["Traccar"]);
            BackupSelectedDatabase(ConfigurationManager.AppSettings["Xwiki"]);
        }

        private static void BackupSelectedDatabase(string databaseName)
        {
            using (MySqlConnection connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    using (MySqlBackup mb = new MySqlBackup(cmd))
                    {
                        cmd.Connection = connection;
                        connection.Open();
                        connection.ChangeDatabase(databaseName);

                        mb.ExportToFile(string.Format(BackupFileName, connection.Database));
                    }
                }
            }
        }
    }
}

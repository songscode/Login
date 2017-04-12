using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Hosting;
using FluentMigrator.Runner.Announcers;
using FluentMigrator.Runner.Initialization;
using Login.Common.PetaPoco;

namespace Login
{
    public static partial class SiteInitialization
    {
        private static string[] databaseKeys = new[] {
            "Default"
        };

        /// <summary>
        /// Automatically creates a database for the template if it doesn't already exists.
        /// You might delete this method to disable auto create functionality.
        /// </summary>
        private static void EnsureDatabase(string databaseKey)
        {
            var contextDb = new ContextDB(databaseKey);
            var serverType = contextDb.Provider.ServerType;
            bool isSql = serverType.StartsWith("SqlServer", StringComparison.OrdinalIgnoreCase);
            bool isPostgres = !isSql & serverType.StartsWith("Postgres", StringComparison.OrdinalIgnoreCase);
            bool isMySql = !isSql && !isPostgres && serverType.StartsWith("MySql", StringComparison.OrdinalIgnoreCase);
            bool isSqlite = !isSql && !isPostgres && !isMySql && serverType.StartsWith("Sqlite", StringComparison.OrdinalIgnoreCase);
            if (!isSql && !isPostgres && !isMySql && !isSqlite)
                return;

            var cb = contextDb.Provider.GetFactory().CreateConnectionStringBuilder();
            string catalogKey = "?";

            if (isSqlite)
            {
                catalogKey = "Data Source";
                if (!cb.ContainsKey(catalogKey))
                    return;

                var dataFile = cb[catalogKey] as string;
                if (string.IsNullOrEmpty(dataFile))
                    return;

                dataFile = dataFile.Replace("|DataDirectory|", HostingEnvironment.MapPath("~/App_Data/"));
                if (File.Exists(dataFile))
                    return;

                Directory.CreateDirectory(Path.GetDirectoryName(dataFile));
                using (var sqliteConn = contextDb.Connection)
                {
                    var createFile = sqliteConn.GetType().GetMethod("CreateFile", BindingFlags.Static);
                    if (createFile != null)
                        createFile.Invoke(null, new object[] { dataFile });
                }

                SqlConnection.ClearAllPools();
                return;
            }

            foreach (var ck in new[] {"Initial Catalog", "Database", "AttachDBFilename"})
            {
                if (cb.ContainsKey(ck))
                {
                    catalogKey = ck;
                    break;
                }

            }

            var catalog = cb[catalogKey] as string;
            cb[catalogKey] = null;

            string databasesQuery = "SELECT * FROM sys.databases WHERE NAME = @name";
            string createDatabaseQuery = @"CREATE DATABASE [{0}]";

            if (isPostgres)
            {
                databasesQuery = "select * from postgres.pg_catalog.pg_database where datname = @0";
                createDatabaseQuery = "CREATE DATABASE \"{0}\"";
            }
            else if (isMySql)
            {
                databasesQuery = "SELECT * FROM INFORMATION_SCHEMA.SCHEMATA WHERE SCHEMA_NAME = @0";
                createDatabaseQuery = "CREATE DATABASE `{0}`";
            }

            if (contextDb.Query<object>(databasesQuery, catalog).Any())
                return;

            var isLocalServer = isSql &&
                contextDb.ConnectionString.IndexOf(@"(localdb)\", StringComparison.OrdinalIgnoreCase) >= 0 ||
                contextDb.ConnectionString.IndexOf(@".\") >= 0;

            string command;
            if (isLocalServer)
            {
                var filename = Path.Combine(HostingEnvironment.MapPath("~/App_Data"), catalog);
                command = String.Format(@"CREATE DATABASE [{0}] ON PRIMARY (Name = N'{0}', FILENAME = '{1}.mdf') LOG ON (NAME = N'{0}_log', FILENAME = '{1}.ldf')",
                    catalog, filename);

                if (File.Exists(filename + ".mdf"))
                    command += " FOR ATTACH";
            }
            else
            {
                command = String.Format(createDatabaseQuery, catalog);
            }
            contextDb.Execute(command);
            SqlConnection.ClearAllPools();
        }

        public static bool SkippedMigrations { get; private set; }

        private static void RunMigrations(string databaseKey)
        {
            var contextDb = new ContextDB(databaseKey);
            var serverType = contextDb.Provider.GetFactory().GetType().Name;
            bool isSqlServer = serverType.StartsWith("SqlServer", StringComparison.OrdinalIgnoreCase);
            bool isOracle = !isSqlServer && serverType.StartsWith("Oracle", StringComparison.OrdinalIgnoreCase);

            // safety check to ensure that we are not modifying an arbitrary database.
            // remove these lines if you want Login migrations to run on your DB.
            if (!isOracle && contextDb.ConnectionString.IndexOf(typeof(SiteInitialization).Namespace +
                    @"_" + databaseKey + "_v2", StringComparison.OrdinalIgnoreCase) < 0)
            {
                SkippedMigrations = true;
                return;
            }

            string databaseType = isOracle ? "OracleManaged" : serverType;
            var connectionString = contextDb.ConnectionString;

            using (var sw = new StringWriter())
            {
                Announcer announcer = isOracle ?
                    new TextWriterAnnouncer(sw) { ShowSql = true } :
                    new TextWriterWithGoAnnouncer(sw) { ShowSql = true };

                var runner = new RunnerContext(announcer)
                {
                    Database = databaseType,
                    Connection = connectionString,
                    Targets = new string[] { typeof(SiteInitialization).Assembly.Location },
                    Task = "migrate:up",
                    WorkingDirectory = Path.GetDirectoryName(typeof(SiteInitialization).Assembly.Location),
                    Namespace = "Login.Core.Migrations." + databaseKey + "DB",
                    Timeout = 90
                };

                try
                {
                    new TaskExecutor(runner).Execute();
                }
                catch (Exception ex)
                {
                    throw new Exception("Error executing migration:\r\n" +
                        sw.ToString(), ex);
                }
            }
        }
    }
}

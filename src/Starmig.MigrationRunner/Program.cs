using DbUp;
using DbUp.Helpers;
using System.Reflection;

namespace Starmig.MigrationRunner
{
    static class Program
    {
        static int Main(string[] args)
        {
            var noWait = false;

            if (args.Length == 1 && args[0] == "--nowait")
            {
                noWait = true;
            }

            int exitCode = 0;

            try
            {
                WriteToConsole($"Database Migration Runner.");

                // OBS: GARANTIR QUE OS ARQUIVOS DE SCRIPTS ESTEJAM MARCADOS COMO EMBEDDED
                // https://www.c-sharpcorner.com/uploadfile/40e97e/saving-an-embedded-file-in-C-Sharp/
                WriteToConsole("\nIMPORTANT: Please ensure your scripts are EMBEDDED in the executable.");

                var baseNamespace = typeof(Program).Namespace;

                var connectionString = "Server=BUZATTOPC;Database=Starmig;User Id=sa;Password=buzatto;";

                WriteToConsole("\nListing variables...\n");
                var variables = new Dictionary<string, string>();

                if (!noWait)
                {
                    Console.Write("\nPress return to run scripts...");
                    Console.ReadLine();
                }

                // Pre deployments
                WriteToConsole("Start executing predeployment scripts...");
                string preDeploymentScriptsPath = baseNamespace + ".PreDeployment";
                RunMigrations(connectionString,
                    preDeploymentScriptsPath, variables, true);


                // Migrations
                WriteToConsole("Start executing migration scripts...");
                var migrationScriptsPath = baseNamespace + ".Migrations";
                RunMigrations(connectionString,
                    migrationScriptsPath, variables, false);

                // Post deployments
                WriteToConsole("Start executing postdeployment scripts...");
                string postdeploymentScriptsPath = baseNamespace + ".PostDeployment";
                RunMigrations(connectionString,
                    postdeploymentScriptsPath, variables, true);

            }
            catch (Exception e)
            {
                WriteToConsole(e.Message, ConsoleColor.Red);

                exitCode = -1;
            }

            if (!noWait)
            {
                Console.Write("Press return key to exit...");
                Console.ResetColor();
                Console.ReadKey();
            }

            return exitCode;
        }

        private static int RunMigrations(string connectionString,
            string @namespace,
            Dictionary<string, string> variables,
            bool alwaysRun = false)
        {
            WriteToConsole($"Executing scripts in {@namespace}");

            var builder = DeployChanges.To
                .SqlDatabase(connectionString)
                .WithVariables(variables)
                .WithScriptsEmbeddedInAssembly(
                    Assembly.GetExecutingAssembly(), file =>
                    {
                        return file
                            .ToLower()
                            .StartsWith(@namespace.ToLower());
                    })
                .LogToConsole();

            builder = alwaysRun ?
                 builder.JournalTo(new NullJournal()) :
                 builder.JournalToSqlTable("dbo", "DatabaseMigrations");

            var executor = builder.Build();
            var result = executor.PerformUpgrade();

            if (!result.Successful)
            {
                throw new Exception(result.Error.ToString());
            }

            ShowSuccess();
            return 0;
        }

        private static void ShowSuccess()
        {
            WriteToConsole("Success!", ConsoleColor.Green);
        }

        private static void WriteToConsole(string msg,
             ConsoleColor color = ConsoleColor.Green)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(msg);
            Console.ResetColor();
        }
    }
}
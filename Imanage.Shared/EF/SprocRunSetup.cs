using System;
using System.IO;

namespace Imanage.Shared.EF
{
    public static class SprocRunSetup
    {
        public static void ApplyStoredProcedures(this IDbContext context, string[] sprocs)
        {
            foreach (var i in sprocs)
            {
                context.ExecuteSqlCommand($@"IF EXISTS (SELECT * FROM dbo.sysobjects 
                                        where id = object_id(N'[dbo].[{i}]')
                                        and OBJECTPROPERTY(id, N'IsProcedure') = 1)
                                        DROP PROCEDURE [dbo].[{i}];");

                string content = GetFileContentWithName($"{i}.sql");
                context.ExecuteSqlCommand(content);
            }
        }

        public static String GetFileContentWithName(string filePath)
        {
            string sqlContent = "";
            var baseDir = $@"{AppDomain.CurrentDomain.BaseDirectory}";
            if (Directory.Exists($"{baseDir}\bin"))
                sqlContent = File.ReadAllText(String.Format(@"{0}\bin\SProcs\{1}", baseDir, filePath));
            else
                sqlContent = File.ReadAllText(String.Format(@"{0}\SProcs\{1}", baseDir, filePath));

            return sqlContent;
        }
    }
}

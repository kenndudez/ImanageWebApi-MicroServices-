using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imanage.Shared.EF
{
    public abstract class ImanageBaseMigration : Migration
    {

        public void DropStoredProcedure(MigrationBuilder builder, string sprocName)
        {
            string script = $@"IF EXISTS (SELECT * FROM dbo.sysobjects 
                                        where id = object_id(N'[dbo].[{sprocName}]')
                                        and OBJECTPROPERTY(id, N'IsProcedure') = 1)
                                        DROP PROCEDURE [dbo].[{sprocName}];";
            builder.Sql(script);
        }
    }
}

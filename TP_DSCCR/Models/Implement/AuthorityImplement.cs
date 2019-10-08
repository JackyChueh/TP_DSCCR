using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using TP_DSCCR.Models.Access;
using TP_DSCCR.Models.Entity;
using TP_DSCCR.Models.Data;

namespace TP_DSCCR.Models.Implement
{
    public class AuthorityImplement : EnterpriseLibrary
    {
        public AuthorityImplement(string connectionStringName) : base(connectionStringName) { }

        public List<Main.SidebarItem> UserFunctionAuthority()
        {
            List<Main.SidebarItem> items = new List<Main.SidebarItem>();
            try
            {
                string sql = @"
 SELECT DISTINCT F.SN, F.NAME, F.CATEGORY, F.URL, F.PARENT_SN, F.SORT, F.IMG,
	CASE WHEN A.ROLES_SN IS NOT NULL THEN 1 WHEN A.USERS_SN IS NOT NULL THEN 1 ELSE 0 END AS ALLOW
	FROM FUNCTIONS F LEFT JOIN AUTHORITY A ON F.SN=A.FUNCTIONS_SN AND (A.USERS_SN=0 OR A.ROLES_SN=1)
        WHERE F.MODE='Y'
	ORDER BY F.SORT
";
                using (DbCommand cmd = Db.GetSqlStringCommand(sql))
                {
                    //Db.AddInParameter(cmd, "ROLES_SN", DbType.String, 0);
                    using (IDataReader reader = this.Db.ExecuteReader(cmd))
                    {
                        while (reader.Read())
                        {
                            Main.SidebarItem item = new Main.SidebarItem
                            {
                                //USERS_SN = reader["USERS_SN"] as Int16? ?? null,
                                //FUNCTIONS_SN = reader["SN"] as Int16? ?? null,
                                //ROLES_SN = reader["ROLES_SN"] as Int32? ?? null,
                                //ROLES = new ROLES()
                                //{
                                //    SN = reader["ROLES_SN"] as Int16? ?? null,
                                //},
                                //USERS = new USERS()
                                //{
                                //    SN = reader["USERS_SN"] as Int16? ?? null,
                                //},
                                FUNCTIONS = new FUNCTIONS()
                                {
                                    SN = reader["SN"] as Int16? ?? null,
                                    NAME = reader["NAME"] as string,
                                    CATEGORY = reader["CATEGORY"] as string,
                                    URL = reader["URL"] as string,
                                    PARENT_SN = reader["PARENT_SN"] as Int16? ?? null,
                                    SORT = reader["SORT"] as Int16? ?? null,
                                    IMG = reader["IMG"] as string
                                },
                                ALLOW = reader["ALLOW"] as int? ?? null
                            };
                            items.Add(item);
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
            return items;
        }
    }
}
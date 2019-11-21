using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using TP_DSCCR.Models.Access;
using TP_DSCCR.Models.Entity;
using TP_DSCCR.Models.Data;
using TP_DSCCR.ViewModels;

namespace TP_DSCCR.Models.Implement
{
    public class AuthorityImplement : EnterpriseLibrary
    {
        public AuthorityImplement(string connectionStringName) : base(connectionStringName) { }

        public LoginCheckRes LoginCheck(LoginCheckReq LoginCheckReq)
        {
            LoginCheckRes LoginCheckRes = new LoginCheckRes();
            USERS USERS = null;
            GRANTS GRANTS = null;

            string sql;
            sql = @"
SELECT U.SN, U.NAME, U.PASSWORD
    FROM USERS U 
    WHERE U.ID=@ID AND U.MODE='Y'
";
            using (DbCommand cmd = Db.GetSqlStringCommand(sql))
            {
                Db.AddInParameter(cmd, "ID", DbType.String, LoginCheckReq.USERS.ID);
                using (IDataReader reader = this.Db.ExecuteReader(cmd))
                {
                    if (reader.Read() && reader["PASSWORD"] as string == LoginCheckReq.USERS.PASSWORD)
                    {
                        USERS = new USERS()
                        {
                            SN = reader["SN"] as int? ?? null,
                            ID = LoginCheckReq.USERS.ID,
                            NAME = reader["NAME"] as string
                        };
                        LoginCheckRes.USERS = USERS;
                    }
                }
            }

            if (USERS != null)
            {
                sql = @"
SELECT ROLES_SN
    FROM GRANTS
    WHERE USERS_SN=@USERS_SN
";
                using (DbCommand cmd = Db.GetSqlStringCommand(sql))
                {
                    Db.AddInParameter(cmd, "USERS_SN", DbType.String, USERS.SN);
                    using (IDataReader reader = this.Db.ExecuteReader(cmd))
                    {
                        if (reader.Read())
                        {
                            GRANTS = new GRANTS()
                            {
                                ROLES_SN = reader["ROLES_SN"] as int? ?? null,
                            };
                            LoginCheckRes.GRANTS = GRANTS;
                        }
                    }
                }
            }
            return LoginCheckRes;
        }

        public List<Main.SidebarItem> UserFunctionAuthority(int? ROLES_SN)
        {
            List<Main.SidebarItem> items = new List<Main.SidebarItem>();
            try
            {
                string sql = @"
 SELECT DISTINCT F.SN, F.NAME, F.CATEGORY, F.URL, F.PARENT_SN, F.SORT, F.IMG,
	CASE WHEN A.ROLES_SN IS NOT NULL THEN 1 WHEN A.USERS_SN IS NOT NULL THEN 1 ELSE 0 END AS ALLOW
	FROM FUNCTIONS F LEFT JOIN AUTHORITY A ON F.SN=A.FUNCTIONS_SN AND (A.ROLES_SN=@ROLES_SN)
        WHERE F.MODE='Y'
	ORDER BY F.SORT
";
                using (DbCommand cmd = Db.GetSqlStringCommand(sql))
                {
                    Db.AddInParameter(cmd, "ROLES_SN", DbType.Int32, ROLES_SN);

                    using (IDataReader reader = this.Db.ExecuteReader(cmd))
                    {
                        while (reader.Read())
                        {
                            Main.SidebarItem item = new Main.SidebarItem
                            {
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
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using TP_DSCCR.Models.Access;
using TP_DSCCR.Models.Entity;
using TP_DSCCR.ViewModels;
using System.IO;

namespace TP_DSCCR.Models.Implement
{
    public class UsersImplement : EnterpriseLibrary
    {
        public UsersImplement(string connectionStringName) : base(connectionStringName) { }

        public UsersRetrieveRes PaginationRetrieve(UsersRetrieveReq Req)
        {
            UsersRetrieveRes res = new UsersRetrieveRes()
            {
                USERS = new List<USERS>(),
                Pagination = new Pagination
                {
                    PageCount = 0,
                    RowCount = 0,
                    PageNumber = 0,
                    MinNumber = 0,
                    MaxNumber = 0,
                    StartTime = DateTime.Now
                }
            };

            using (DbCommand cmd = Db.CreateConnection().CreateCommand())
            {
                string sql = @"
SELECT TOP(@TOP) SN,ID,NAME,PASSWORD,EMAIL,dbo.PHRASE_NAME('mode',MODE,default) AS MODE,MEMO,CDATE,CUSER,MDATE,MUSER
    FROM USERS
    {0}";
                //Db.AddInParameter(cmd, "TOP", DbType.Int32, 1000);

                string where = "";
                Db.AddInParameter(cmd, "TOP", DbType.Int32, 1000);

                if (Req.USERS.SN != null)
                {
                    where += " AND SN=@SN";
                    Db.AddInParameter(cmd, "SN", DbType.Int16, Req.USERS.SN);
                }
                if (!string.IsNullOrEmpty(Req.USERS.ID))
                {
                    where += " AND ID LIKE @ID";
                    Db.AddInParameter(cmd, "ID", DbType.String, "%" + Req.USERS.ID + "%");
                }
                if (!string.IsNullOrEmpty(Req.USERS.NAME))
                {
                    where += " AND NAME LIKE @NAME";
                    Db.AddInParameter(cmd, "NAME", DbType.String, "%" + Req.USERS.NAME + "%");
                }
                if (!string.IsNullOrEmpty(Req.USERS.MODE))
                {
                    where += " AND MODE=@MODE";
                    Db.AddInParameter(cmd, "MODE", DbType.String, Req.USERS.MODE);
                }
                if (where.Length > 0)
                {
                    where = " WHERE" + where.Substring(4);
                }

                sql = String.Format(sql, where);
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sql;
                using (DataTable dt = Db.ExecuteDataSet(cmd).Tables[0])
                {
                    res.Pagination.RowCount = dt.Rows.Count;
                    res.Pagination.PageCount = Convert.ToInt32(Math.Ceiling(1.0 * res.Pagination.RowCount / Req.PageSize));
                    res.Pagination.PageNumber = Req.PageNumber < 1 ? 1 : Req.PageNumber;
                    res.Pagination.PageNumber = Req.PageNumber > res.Pagination.PageCount ? res.Pagination.PageCount : res.Pagination.PageNumber;
                    res.Pagination.MinNumber = (res.Pagination.PageNumber - 1) * Req.PageSize + 1;
                    res.Pagination.MaxNumber = res.Pagination.PageNumber * Req.PageSize;
                    res.Pagination.MaxNumber = res.Pagination.MaxNumber > res.Pagination.RowCount ? res.Pagination.RowCount : res.Pagination.MaxNumber;

                    if (dt.Rows.Count > 0)
                    {
                        for (int i = res.Pagination.MinNumber - 1; i < res.Pagination.MaxNumber; i++)
                        {
                            var row = new USERS
                            {
                                SN = (int)dt.Rows[i]["SN"],
                                ID = dt.Rows[i]["ID"] as string,
                                NAME = dt.Rows[i]["NAME"] as string,
                                PASSWORD = dt.Rows[i]["PASSWORD"] as string,
                                EMAIL = dt.Rows[i]["EMAIL"] as string,
                                MODE = dt.Rows[i]["MODE"] as string,
                                MEMO = dt.Rows[i]["MEMO"] as string,
                                CDATE = dt.Rows[i]["CDATE"] as DateTime? ?? null,
                                CUSER = dt.Rows[i]["CUSER"] as string,
                                MDATE = dt.Rows[i]["MDATE"] as DateTime? ?? null,
                                MUSER = dt.Rows[i]["MUSER"] as string,
                            };
                            res.USERS.Add(row);
                        }
                    }
                }
            }
            res.Pagination.EndTime = DateTime.Now;

            return res;
        }

        public UsersModifyRes ModificationQuery(UsersModifyReq Req)
        {
            UsersModifyRes res = new UsersModifyRes();
            
            using (DbCommand cmd = Db.CreateConnection().CreateCommand())
            {
                string sql = @"
        SELECT U.SN,U.ID,U.NAME,U.PASSWORD,U.EMAIL,U.MODE,U.MEMO,U.CDATE,U.CUSER,U.MDATE,U.MUSER,G.ROLES_SN
            FROM USERS U LEFT JOIN GRANTS G ON U.SN=G.USERS_SN
            WHERE U.SN=@SN
        ";
                Db.AddInParameter(cmd, "SN", DbType.Int32, Req.USERS.SN);

                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sql;
                using (IDataReader reader = Db.ExecuteReader(cmd))
                {
                    if (reader.Read())
                    {
                        USERS USERS = new USERS
                        {
                            SN = reader["SN"] as int? ?? null,
                            ID = reader["ID"] as string,
                            NAME = reader["NAME"] as string,
                            //PASSWORD = reader["PASSWORD"] as string,
                            EMAIL = reader["EMAIL"] as string,
                            MODE = reader["MODE"] as string,
                            MEMO = reader["MEMO"] as string,
                            CDATE = reader["CDATE"] as DateTime?,
                            CUSER = reader["CUSER"] as string,
                            MDATE = reader["MDATE"] as DateTime?,
                            MUSER = reader["MUSER"] as string
                        };
                        res.USERS = USERS;

                        GRANTS GRANTS = new GRANTS
                        {
                            ROLES_SN = reader["ROLES_SN"] as int? ?? null
                        };
                        res.GRANTS = GRANTS;
                    }
                }
            }

            return res;
        }

        public bool DataCreate(UsersModifyReq req)
        {
            int effect = 0;
            using (DbConnection conn = Db.CreateConnection())
            {
                conn.Open();
                DbTransaction trans = conn.BeginTransaction();
                try
                {
                    using (DbCommand cmd = conn.CreateCommand())
                    {
                        //Int64? USERS_SN = new Sequence("SCC").GetSeqBigInt("USERS");
                        string sql = @"
SET @SN = NEXT VALUE FOR [USERS_SEQ]
INSERT USERS (SN,ID,NAME,PASSWORD,EMAIL,MODE,MEMO,CDATE,CUSER,MDATE,MUSER)
    VALUES (@SN,@ID,@NAME,@PASSWORD,@EMAIL,@MODE,@MEMO,GETDATE(),@CUSER,GETDATE(),@MUSER);
        ";
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = sql;

                        Db.AddInParameter(cmd, "ID", DbType.String, req.USERS.ID);
                        Db.AddInParameter(cmd, "NAME", DbType.String, req.USERS.NAME);
                        Db.AddInParameter(cmd, "PASSWORD", DbType.String, req.USERS.PASSWORD);
                        Db.AddInParameter(cmd, "EMAIL", DbType.String, req.USERS.EMAIL);
                        Db.AddInParameter(cmd, "MODE", DbType.String, req.USERS.MODE);
                        Db.AddInParameter(cmd, "MEMO", DbType.String, req.USERS.MEMO);
                        Db.AddInParameter(cmd, "CUSER", DbType.String, req.USERS.CUSER);
                        Db.AddInParameter(cmd, "MUSER", DbType.String, req.USERS.MUSER);
                        Db.AddOutParameter(cmd, "SN", DbType.Int32, 1);

                        effect = Db.ExecuteNonQuery(cmd);
                        req.USERS.SN = Db.GetParameterValue(cmd, "SN") as Int32? ?? null;
                    }
                    using (DbCommand cmd = conn.CreateCommand())
                    {
                        string sql = @"
  INSERT GRANTS (ROLES_SN,USERS_SN,CDATE,CUSER,MDATE,MUSER)
	VALUES (@ROLES_SN,@USERS_SN,GETDATE(),@CUSER,GETDATE(),@MUSER);
        ";
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = sql;

                        Db.AddInParameter(cmd, "ROLES_SN", DbType.Int32, req.GRANTS.ROLES_SN);
                        Db.AddInParameter(cmd, "USERS_SN", DbType.String, req.USERS.SN);
                        Db.AddInParameter(cmd, "CUSER", DbType.String, req.USERS.CUSER);
                        Db.AddInParameter(cmd, "MUSER", DbType.String, req.USERS.MUSER);

                        effect = Db.ExecuteNonQuery(cmd);
                    }

                    trans.Commit();
                    return true;
                    
                }
                catch
                {
                    trans.Rollback();
                }
            }
            return false;
        }

        public bool DataUpdate(UsersModifyReq req)
        {
            int effect = 0;
            using (DbConnection conn = Db.CreateConnection())
            {
                conn.Open();
                DbTransaction trans = conn.BeginTransaction();
                try
                {
                    using (DbCommand cmd = conn.CreateCommand())
                    {
                        string sql = @"
UPDATE USERS
    SET ID=@ID,NAME=@NAME,EMAIL=@EMAIL,MODE=@MODE,MEMO=@MEMO,MDATE=GETDATE(),MUSER=@MUSER
        {0}
    WHERE SN=@SN;
";
                        string password = "";
                        if (!string.IsNullOrEmpty(req.USERS.PASSWORD))
                        {
                            password = ",PASSWORD=@PASSWORD";

                            Db.AddInParameter(cmd, "PASSWORD", DbType.String, req.USERS.PASSWORD);
                        }
                        sql = string.Format(sql, password);

                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = sql;

                        Db.AddInParameter(cmd, "SN", DbType.String, req.USERS.SN);
                        Db.AddInParameter(cmd, "ID", DbType.String, req.USERS.ID);
                        Db.AddInParameter(cmd, "NAME", DbType.String, req.USERS.NAME);

                        Db.AddInParameter(cmd, "EMAIL", DbType.String, req.USERS.EMAIL);
                        Db.AddInParameter(cmd, "MODE", DbType.String, req.USERS.MODE);
                        Db.AddInParameter(cmd, "MEMO", DbType.String, req.USERS.MEMO);
                        Db.AddInParameter(cmd, "MUSER", DbType.String, req.USERS.MUSER);
                        effect = Db.ExecuteNonQuery(cmd);
                    }

                    using (DbCommand cmd = conn.CreateCommand())
                    {
                        string sql = @"
  UPDATE GRANTS 
    SET ROLES_SN=@ROLES_SN,MDATE=GETDATE(),MUSER=@MUSER
    WHERE USERS_SN=@USERS_SN
        ";
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = sql;

                        Db.AddInParameter(cmd, "ROLES_SN", DbType.Int32, req.GRANTS.ROLES_SN);
                        Db.AddInParameter(cmd, "USERS_SN", DbType.String, req.USERS.SN);
                        Db.AddInParameter(cmd, "MUSER", DbType.String, req.USERS.MUSER);

                        effect = Db.ExecuteNonQuery(cmd);
                    }

                    trans.Commit();
                    return true;

                }
                catch
                {
                    trans.Rollback();
                }
            }

            using (DbCommand cmd = Db.CreateConnection().CreateCommand())
            {
    
            }
            return false;
        }

        public int DataDelete(UsersModifyReq req)
        {
            int count = 0;
            using (DbCommand cmd = Db.CreateConnection().CreateCommand())
            {
                string sql = @"
        DELETE FROM USERS
            WHERE SN=@SN;
        ";
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sql;
                Db.AddInParameter(cmd, "SN", DbType.String, req.USERS.SN);
                count = Db.ExecuteNonQuery(cmd);
            }
            return count;
        }

        public int DataReset(UsersModifyReq req)
        {
            int count = 0;
            using (DbCommand cmd = Db.CreateConnection().CreateCommand())
            {
                string sql = @"
        UPDATE USERS
        	SET PASSWORD=@PASSWORD, FORCE_PWD=@FORCE_PWD, MDATE=GETDATE(), MUSER=@MUSER
            WHERE SN=@SN;
        ";
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sql;

                Db.AddInParameter(cmd, "SN", DbType.String, req.USERS.SN);
                Db.AddInParameter(cmd, "PASSWORD", DbType.String, req.USERS.PASSWORD);
                Db.AddInParameter(cmd, "FORCE_PWD", DbType.Int16, req.USERS.FORCE_PWD);
                Db.AddInParameter(cmd, "MUSER", DbType.String, req.USERS.MUSER);
                count = Db.ExecuteNonQuery(cmd);
            }
            return count;
        }

    }
}
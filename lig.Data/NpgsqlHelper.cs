using Npgsql;
using System;
using System.Data;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace lig.Data
{
    public class NpgsqlHelper
    {
        private static string connectionStringName = "";
        public static string _ConnectionStringName
        {
            get
            {
                if (string.IsNullOrEmpty(connectionStringName))
                {
                    throw new Exception("你必须调用DbConfig.init()初始化数据库配置！");
                }
                else
                {
                    return connectionStringName;
                }
            }
            set { connectionStringName = value; }
        }


        public static void Init(string name)
        {
            connectionStringName = name;

        }

        public static string GetConnectionString()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
            //创建配置根对象
            var configurationRoot = builder.Build();
            string connectionString = configurationRoot.GetSection("ConnectionStrings").GetSection("DefaultConnection").Path;

            return connectionString;
        }


        public static DataTable SqlQueryTable(string queryStr)
        {
            string sql = queryStr.Replace("@", ":").Replace("?", ":").Replace("[", "\"").Replace("]", "\"");

            DataSet ds = new DataSet();

            string connString = NpgsqlHelper.GetConnectionString();
            using (var sqlConn = new NpgsqlConnection(connString))
            {
                if (sqlConn.State == ConnectionState.Closed)
                {
                    sqlConn.Open();
                }

                NpgsqlDataAdapter sqldap = new NpgsqlDataAdapter(sql, sqlConn);
                sqldap.Fill(ds);
                sqldap.Dispose();
            }
            return ds.Tables[0];


        }


        /// <summary>
        ///  执行查询，返回首行首列
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static object ExecuteScalar(string queryStr)
        {
            string sql = queryStr.Replace("@", ":").Replace("?", ":").Replace("[", "\"").Replace("]", "\"");

            object ret = null;
            string connString = NpgsqlHelper.GetConnectionString();
            using (var sqlConn = new NpgsqlConnection(connString))
            {

                if (sqlConn.State == ConnectionState.Closed)
                {
                    sqlConn.Open();
                }
                NpgsqlCommand command = new NpgsqlCommand(sql, sqlConn);
                ret = command.ExecuteScalar();
                return ret;

            }

        }


        /// <summary>
        /// 执行sql
        /// </summary>
        /// <returns></returns>
        public static void ExecuteSQL(string queryStr)
        {
            string sql = queryStr.Replace("@", ":").Replace("?", ":").Replace("[", "\"").Replace("]", "\"");
            string connString = NpgsqlHelper.GetConnectionString();

            using (var sqlConn = new NpgsqlConnection(connString))
            {
                if (sqlConn.State == ConnectionState.Closed)
                {
                    sqlConn.Open();
                }
                NpgsqlCommand command = new NpgsqlCommand(sql, sqlConn);
                command.ExecuteNonQuery();
                command.Dispose();

            }


        }


        public static DataTable SqlQueryPages(string sql, PageOption page)
        {
            StringBuilder sqlPage = new StringBuilder();

            sqlPage.Append(sql);

            if (page.Sort.Key != null)
            {

                sqlPage.Append($" order by  {page.Sort.Key} {page.Sort.Value} ");

            }

            sqlPage.Append($" limit { page.PageSize} offset {(page.Page - 1) * page.PageSize}");


            var dt = SqlQueryTable(sqlPage.ToString());
            string sqlCount = $@"select COUNT(1) num from  ( {sql} )  b";
            page.Total = Convert.ToInt32(NpgsqlHelper.ExecuteScalar(sqlCount));

            return dt;

        }

    }
}

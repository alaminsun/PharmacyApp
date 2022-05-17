using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhramacyApp.Helpers
{
    public class IDGenerated
    {

        //private readonly IApplicationDbContext _dbContext;

        //public IDGenerated()
        //{
        //}

        ////private readonly IDateTimeService _dateTime;
        ////private readonly IDGenerated _iDGenerated;
        //public IDGenerated(IApplicationDbContext dbContext)
        //{
        //    //_dateTime = dateTime;
        //    _dbContext = dbContext;
        //    //_iDGenerated = iDGenerated;
        //}
        private readonly IConfiguration configuration;
        public IDGenerated(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public Int64 getMAXSL(string tableName, string columnName)
        {
            Int64 MAXID = 0;
            string QueryString = "select ISNULL(MAX(" + columnName + "),0)+1 id from " + tableName + "";
            using (SqlConnection sqlConnection = new SqlConnection(configuration.GetConnectionString("ApplicationConnection")))
            {
                sqlConnection.Open();
                using (SqlCommand sqlCommand = new SqlCommand(QueryString, sqlConnection))
                {
                    using (SqlDataReader rdr = sqlCommand.ExecuteReader())
                    {
                        if (rdr.Read())
                        {
                            MAXID = Convert.ToInt64(rdr["id"].ToString());
                        }
                    }
                }
            }
            return MAXID;
        }

        public int getMAXSLFIN(string tableName, string columnName)
        {
            int MAXID = 0;
            string QueryString = "select ISNULL(MAX(" + columnName + "),0)+1 id from " + tableName + "";
            using (SqlConnection sqlConnection = new SqlConnection(configuration.GetConnectionString("ApplicationConnection")))
            {
                sqlConnection.Open();
                using (SqlCommand sqlCommand = new SqlCommand(QueryString, sqlConnection))
                {
                    using (SqlDataReader rdr = sqlCommand.ExecuteReader())
                    {
                        if (rdr.Read())
                        {
                            MAXID = Convert.ToInt32(rdr["id"].ToString());
                        }
                    }
                }
            }
            return MAXID;
        }

        //public Int64 getMAXSL(string tableName, string columnName, string ConnString)
        //{
        //    Int64 MAXID = 0;
        //    string QueryString = "select ISNULL(MAX(" + columnName + "),0)+1 id from " + tableName + "";
        //    using (var connection = _dbContext)
        //    {
        //        sqlConnection.Open();
        //        using (SqlCommand sqlCommand = new SqlCommand(QueryString, sqlConnection))
        //        {
        //            using (SqlDataReader rdr = sqlCommand.ExecuteReader())
        //            {
        //                if (rdr.Read())
        //                {
        //                    MAXID = Convert.ToInt64(rdr["id"].ToString());
        //                }
        //            }
        //        }
        //    }
        //    return MAXID;
        //}

        //public string getMAXID(string tableName, string columnName, string fm9, string ConnString)
        //{
        //    string MAXID = "";
        //    string QueryString = "select to_char((select ISNULL(MAX(" + columnName + "),0)+1 from " + tableName + " ), '" + fm9 + "') id from dual";
        //    using (SqlConnection sqlConnection = new SqlConnection(dbConnection.SAConnStrReader(ConnString)))
        //    {
        //        sqlConnection.Open();
        //        using (SqlCommand sqlCommand = new SqlCommand(QueryString, sqlConnection))
        //        {
        //            using (SqlDataReader rdr = sqlCommand.ExecuteReader())
        //            {
        //                if (rdr.Read())
        //                {
        //                    MAXID = rdr[0].ToString();
        //                }
        //            }
        //        }
        //    }
        //    return MAXID;
        //}
    }
}

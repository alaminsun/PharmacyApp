using SPAs.DAL.Gateway;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;
using System.Linq;
using System.Web;

namespace SPAs.DAL.DAO
{
    public class IDGenerated
    {

        DBConnection dbConnection = new DBConnection();
        DBHelper dbHelper = new DBHelper();
        public Int64 getMAXSL(string columnName, string tableName)
        {
            Int64 MAXID = 0;
            string QueryString = "select NVL(MAX(" + columnName + "),0)+1 id from " + tableName + "";
            using (OracleConnection oracleConnection = new OracleConnection(dbConnection.StringRead()))
            {
                oracleConnection.Open();
                OracleCommand oracleCommand = new OracleCommand(QueryString, oracleConnection);
                using (OracleDataReader rdr = oracleCommand.ExecuteReader())
                {
                    if (rdr.Read())
                    {
                        MAXID = Convert.ToInt64(rdr["id"].ToString());
                    }
                }
            }
            return MAXID;
        }

        public Int64 getMAXChallanNO(string columnName, string tableName)
        {
            Int64 MAXID = 0;
            string QueryString = "select NVL(MAX(TO_NUMBER(" + columnName + ")),0)+1 id from " + tableName + "";
            using (OracleConnection oracleConnection = new OracleConnection(dbConnection.StringRead()))
            {
                oracleConnection.Open();
                OracleCommand oracleCommand = new OracleCommand(QueryString, oracleConnection);
                using (OracleDataReader rdr = oracleCommand.ExecuteReader())
                {
                    if (rdr.Read())
                    {
                        MAXID = Convert.ToInt64(rdr["id"].ToString());
                    }
                }
            }
            return MAXID;
        }


        public string getMAXID(string columnName, string tableName, string fm9)
        {
            string MAXID = "";
            string QueryString = "select to_char((select NVL(MAX(" + columnName + "),0)+1 from " + tableName + " ), '" + fm9 + "') id from dual";
            using (OracleConnection oracleConnection = new OracleConnection(dbConnection.StringRead()))
            {
                oracleConnection.Open();
                OracleCommand oracleCommand = new OracleCommand(QueryString, oracleConnection);
                using (OracleDataReader rdr = oracleCommand.ExecuteReader())
                {
                if (rdr.Read())
                {
                    MAXID = rdr[0].ToString();
                }
                }
            }
            return MAXID;
        }

        public string getRequisitionMAXID(string columnName, string tableName, string fm9)
        {
            string MAXID = "";
            string QueryString = "select to_char((select NVL(MAX(" + columnName + "),0)+1 from " + tableName + " ), '" + fm9 + "') id from dual";
            using (OracleConnection oracleConnection = new OracleConnection(dbConnection.StringRead()))
            {
                oracleConnection.Open();
                OracleCommand oracleCommand = new OracleCommand(QueryString, oracleConnection);
               using(OracleDataReader rdr = oracleCommand.ExecuteReader())
               {
                if (rdr.Read())
                {
                    MAXID = rdr[0].ToString();
                }
            }
        }
            return MAXID;
        }

    }
}
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace SPAs.DAL.Common
{
    public static  class ExtensionMethod
    {
        //public static void Convert<T>(this DataColumn column, Func<object, T> conversion)
        //{
        //    foreach (DataRow row in column.Table.Rows)
        //    {
        //        row[column] = conversion(row[column]);
        //    }
        //}

        //public static DataTable GetFormatDt(DataTable dt)
        //{
        //    dt.Columns["Invoice_Date"].Convert(val => string.Parse(val.ToString()).ToString().Substring(0,10));
        //    dt.Columns["Delivery_Date"].Convert(val => string.Parse(val.ToString()).ToString().Substring(0, 10));

        //    return dt;
        //}
        public static bool ChangeColumnDataType(DataTable table, string columnname, Type newtype)
        {
            if (table.Columns.Contains(columnname) == false)
                return false;

            DataColumn column = table.Columns[columnname];
            if (column.DataType == newtype)
                return true;

            try
            {
                DataColumn newcolumn = new DataColumn("temporary", newtype);
                table.Columns.Add(newcolumn);
                foreach (DataRow row in table.Rows)
                {
                    try
                    {
                        row["temporary"] = Convert.ChangeType(row[columnname], newtype).ToString().Substring(0,10);
                    }
                    catch
                    {
                    }
                }
                table.Columns.Remove(columnname);
                newcolumn.ColumnName = columnname;
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
    }
}
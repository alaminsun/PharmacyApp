using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;

namespace SPAs.DAL.Common
{
    public class Export
    {
        public virtual string CompanyName { get; set; }
        public virtual string ReportName { get; set; }
        public virtual string FromDate { get; set; }
        public virtual string ToDate { get; set; }
        public void ExportToExcel(DataTable table)
        {
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.ClearHeaders();
            HttpContext.Current.Response.Buffer = true;
           // HttpContext.Current.Response.ContentType = "application/text";           
            HttpContext.Current.Response.ContentType = "application/ms-excel";
           // HttpContext.Current.Response.ContentType = "application/ms-word";
           // HttpContext.Current.Response.Write(@"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.0 Transitional//EN"">");
//text/csv

            HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=Reports.xls");
           // HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=Reports.csv");
           // HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=Reports.doc");
            HttpContext.Current.Response.Charset = "utf-8";
            HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("windows-1250");
            HttpContext.Current.Response.Write("<font style='font-size:10.0pt; font-family:Calibri;'>");
            HttpContext.Current.Response.Write("<BR><BR><BR>");
            HttpContext.Current.Response.Write("<Table border='1' bgColor='#ffffff' borderColor='#000000' cellSpacing='0' cellPadding='0' style='font-size:10.0pt; font-family:Calibri; background:white;'> <TR>");
            int columnscount = table.Columns.Count;

            for (int j = 0; j < columnscount; j++)
            {
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write("<B>");
                HttpContext.Current.Response.Write(table.Columns[j].ToString());
                HttpContext.Current.Response.Write("</B>");
                HttpContext.Current.Response.Write("</Td>");
            }
            HttpContext.Current.Response.Write("</TR>");
            foreach (DataRow row in table.Rows)
            {
                HttpContext.Current.Response.Write("<TR>");
                for (int i = 0; i < table.Columns.Count; i++)
                {
                    HttpContext.Current.Response.Write("<Td>");
                    HttpContext.Current.Response.Write(row[i].ToString());
                    HttpContext.Current.Response.Write("</Td>");
                }

                HttpContext.Current.Response.Write("</TR>");
            }
            HttpContext.Current.Response.Write("</Table>");
            HttpContext.Current.Response.Write("</font>");
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();
        }
        public void ExportToExcel(DataTable table, string rptName)
        {
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.ClearHeaders();
            HttpContext.Current.Response.Buffer = true;
            // HttpContext.Current.Response.ContentType = "application/text";           
            HttpContext.Current.Response.ContentType = "application/ms-excel";
            // HttpContext.Current.Response.ContentType = "application/ms-word";
            // HttpContext.Current.Response.Write(@"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.0 Transitional//EN"">");
            //text/csv

            HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename="+ rptName +".xls");
            // HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=Reports.csv");
            // HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=Reports.doc");
            HttpContext.Current.Response.Charset = "utf-8";
            HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("windows-1250");
            HttpContext.Current.Response.Write("<font style='font-size:10.0pt; font-family:Calibri;'>");
            HttpContext.Current.Response.Write("<B>");
            HttpContext.Current.Response.Write("<DIV>");
            HttpContext.Current.Response.Write(CompanyName);
            HttpContext.Current.Response.Write("</DIV>");
            HttpContext.Current.Response.Write("<DIV>");
            HttpContext.Current.Response.Write(ReportName);
            HttpContext.Current.Response.Write("</DIV>");
            HttpContext.Current.Response.Write("<DIV>");
            HttpContext.Current.Response.Write("Date: From ");
            HttpContext.Current.Response.Write(FromDate);
            HttpContext.Current.Response.Write(" To ");
            HttpContext.Current.Response.Write(ToDate);
            HttpContext.Current.Response.Write("</DIV>");
            HttpContext.Current.Response.Write("</B>");
            HttpContext.Current.Response.Write("<BR>");
            HttpContext.Current.Response.Write("<Table border='1' bgColor='#ffffff' borderColor='#000000' cellSpacing='0' cellPadding='0' style='font-size:10.0pt; font-family:Calibri; background:white;'> <TR>");
            int columnscount = table.Columns.Count;

            for (int j = 0; j < columnscount; j++)
            {
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write("<B>");
                HttpContext.Current.Response.Write(table.Columns[j].ToString());
                HttpContext.Current.Response.Write("</B>");
                HttpContext.Current.Response.Write("</Td>");
            }
            HttpContext.Current.Response.Write("</TR>");
            foreach (DataRow row in table.Rows)
            {
                HttpContext.Current.Response.Write("<TR>");
                for (int i = 0; i < table.Columns.Count; i++)
                {
                    HttpContext.Current.Response.Write("<Td>");
                    HttpContext.Current.Response.Write(row[i].ToString());
                    HttpContext.Current.Response.Write("</Td>");
                }

                HttpContext.Current.Response.Write("</TR>");
            }
            HttpContext.Current.Response.Write("</Table>");
            HttpContext.Current.Response.Write("</font>");
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();
        }

        public void ExportToExcel2(DataTable table, string rptName)
        {
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.ClearHeaders();
            HttpContext.Current.Response.Buffer = true;
            // HttpContext.Current.Response.ContentType = "application/text";           
            HttpContext.Current.Response.ContentType = "application/ms-excel";
            // HttpContext.Current.Response.ContentType = "application/ms-word";
            // HttpContext.Current.Response.Write(@"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.0 Transitional//EN"">");
            //text/csv

            HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + rptName + ".xls");
            // HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=Reports.csv");
            // HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=Reports.doc");
            HttpContext.Current.Response.Charset = "utf-8";
            HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("windows-1250");
            HttpContext.Current.Response.Write("<font style='font-size:10.0pt; font-family:Calibri;'>");
            HttpContext.Current.Response.Write("<B>");
            HttpContext.Current.Response.Write("<DIV>");
            HttpContext.Current.Response.Write(CompanyName);
            HttpContext.Current.Response.Write("</DIV>");
            HttpContext.Current.Response.Write("<DIV>");
            HttpContext.Current.Response.Write(ReportName);
            HttpContext.Current.Response.Write("</DIV>");
            HttpContext.Current.Response.Write("</B>");
            HttpContext.Current.Response.Write("<BR>");
            HttpContext.Current.Response.Write("<Table border='1' bgColor='#ffffff' borderColor='#000000' cellSpacing='0' cellPadding='0' style='font-size:10.0pt; font-family:Calibri; background:white;'> <TR>");
            int columnscount = table.Columns.Count;

            for (int j = 0; j < columnscount; j++)
            {
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write("<B>");
                HttpContext.Current.Response.Write(table.Columns[j].ToString());
                HttpContext.Current.Response.Write("</B>");
                HttpContext.Current.Response.Write("</Td>");
            }
            HttpContext.Current.Response.Write("</TR>");
            foreach (DataRow row in table.Rows)
            {
                HttpContext.Current.Response.Write("<TR>");
                for (int i = 0; i < table.Columns.Count; i++)
                {
                    HttpContext.Current.Response.Write("<Td>");
                    HttpContext.Current.Response.Write(row[i].ToString());
                    HttpContext.Current.Response.Write("</Td>");
                }

                HttpContext.Current.Response.Write("</TR>");
            }
            HttpContext.Current.Response.Write("</Table>");
            HttpContext.Current.Response.Write("</font>");
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();
        }

        public void ExportToExcelMethod2(DataTable dtSource)
        {
            StringBuilder sbDocBody = new StringBuilder(); ;
            try
            {
                // Declare Styles
                sbDocBody.Append("<style>");
                sbDocBody.Append(".Header {  background-color:Navy; color:#ffffff; font-weight:bold;font-family:Verdana; font-size:12px;}");
                sbDocBody.Append(".SectionHeader { background-color:#8080aa; color:#ffffff; font-family:Verdana; font-size:12px;font-weight:bold;}");
                sbDocBody.Append(".Content { background-color:#ccccff; color:#000000; font-family:Verdana; font-size:12px;text-align:left}");
                sbDocBody.Append(".Label { background-color:#ccccee; color:#000000; font-family:Verdana; font-size:12px; text-align:right;}");
                sbDocBody.Append("</style>");
                //
                StringBuilder sbContent = new StringBuilder(); ;
                sbDocBody.Append("<br><table align=\"center\" cellpadding=1 cellspacing=0 style=\"background-color:#000000;\">");
                sbDocBody.Append("<tr><td width=\"500\">");
                sbDocBody.Append("<table width=\"100%\" cellpadding=1 cellspacing=2 style=\"background-color:#ffffff;\">");
                //
                if (dtSource.Rows.Count > 0)
                {
                    sbDocBody.Append("<tr><td>");
                    sbDocBody.Append("<table width=\"600\" cellpadding=\"0\" cellspacing=\"2\"><tr><td>");
                    //
                    // Add Column Headers
                    sbDocBody.Append("<tr><td width=\"25\"> </td></tr>");
                    sbDocBody.Append("<tr>");
                    sbDocBody.Append("<td> </td>");
                    for (int i = 0; i < dtSource.Columns.Count; i++)
                    {
                        sbDocBody.Append("<td class=\"Header\" width=\"120\">" + dtSource.Columns[i].ToString().Replace(".", "<br>") + "</td>");
                    }
                    sbDocBody.Append("</tr>");
                    //
                    // Add Data Rows
                    for (int i = 0; i < dtSource.Rows.Count; i++)
                    {
                        sbDocBody.Append("<tr>");
                        sbDocBody.Append("<td> </td>");
                        for (int j = 0; j < dtSource.Columns.Count; j++)
                        {
                            sbDocBody.Append("<td class=\"Content\">" + dtSource.Rows[i][j].ToString() + "</td>");
                        }
                        sbDocBody.Append("</tr>");
                    }
                    sbDocBody.Append("</table>");
                    sbDocBody.Append("</td></tr></table>");
                    sbDocBody.Append("</td></tr></table>");
                }
                //
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.Buffer = true;
                //
                HttpContext.Current.Response.AppendHeader("Content-Type", "application/ms-excel");
                HttpContext.Current.Response.AppendHeader("Content-disposition", "attachment; filename=EmployeeDetails.xls");
                HttpContext.Current.Response.Write(sbDocBody.ToString());
                HttpContext.Current.Response.End();
            }
            catch (Exception ex)
            {
                // Ignore this error as this is caused due to termination of the Response Stream.
            }
        }

        public class ListtoDataTableConverter
        {
            public DataTable ToDataTable<T>(List<T> items)
            {
                DataTable dataTable = new DataTable(typeof(T).Name);
                PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
                foreach (PropertyInfo prop in Props)
                {
                    dataTable.Columns.Add(prop.Name, prop.PropertyType);
                }

                foreach (T item in items)
                {
                    var values = new object[Props.Length];
                    for (int i = 0; i < Props.Length; i++)
                    {
                        values[i] = Props[i].GetValue(item, null);
                    }
                    dataTable.Rows.Add(values);
                }

                //put a breakpoint here and check datatable
                return dataTable;
            }
        }



    }

    public class Student
    {
        public string Name { get; set; }
        public int StudentId { get; set; }
        public int Age { get; set; }

    }
}
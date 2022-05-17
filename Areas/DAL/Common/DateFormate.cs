using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SPAs.DAL.Common
{
    public class DateFormate
    {
        string date="";
      

         public string StringDateDdMonYYYY(string ddMMyyyy)
         {
             if (ddMMyyyy.Length == 10)
             {
                 string monthName = "";
                 string[] monthNumber = ddMMyyyy.Split('/');

                 if ((monthNumber[1] == "01") || (monthNumber[1] == "1"))
                     monthName = "Jan";
                 else if ((monthNumber[1] == "02") || (monthNumber[1] == "2"))
                     monthName = "Feb";
                 else if ((monthNumber[1] == "03") || (monthNumber[1] == "3"))
                     monthName = "Mar";
                 else if ((monthNumber[1] == "04") || (monthNumber[1] == "4"))
                     monthName = "Apr";
                 else if ((monthNumber[1] == "05") || (monthNumber[1] == "5"))
                     monthName = "May";
                 else if ((monthNumber[1] == "06") || (monthNumber[1] == "6"))
                     monthName = "Jun";
                 else if ((monthNumber[1] == "07") || (monthNumber[1] == "7"))
                     monthName = "Jul";
                 else if ((monthNumber[1] == "08") || (monthNumber[1] == "8"))
                     monthName = "Aug";
                 else if ((monthNumber[1] == "09") || (monthNumber[1] == "9"))
                     monthName = "Sep";
                 else if (monthNumber[1] == "10")
                     monthName = "Oct";
                 else if (monthNumber[1] == "11")
                     monthName = "Nov";
                 else if (monthNumber[1] == "12")
                     monthName = "Dec";
                 else
                     monthName = string.Empty;

                 ddMMyyyy = monthNumber[0] + "-" + monthName + "-" + monthNumber[2];
                 date = ddMMyyyy;
             }
             else
             {
                  date = DateTime.Now.ToString("dd/MM/yyyy");
             }
            return date;
        }


         public string StringDateddMMyyyy(string ddMMMyyyy)
         {
             if (ddMMMyyyy.Length == 11)
             {
                 string monthName = "";
                 string[] monthNumber = ddMMMyyyy.Split('-');

                 if (monthNumber[1] == "Jan")
                     monthName = "01";
                 else if (monthNumber[1] == "Feb") 
                     monthName = "02";
                 else if (monthNumber[1] == "Mar") 
                     monthName = "03";
                 else if (monthNumber[1] == "Apr") 
                     monthName = "04";
                 else if (monthNumber[1] == "May")
                     monthName = "05";
                 else if (monthNumber[1] == "Jun") 
                     monthName = "06";
                 else if (monthNumber[1] == "Jul") 
                     monthName = "07";
                 else if (monthNumber[1] == "Aug") 
                     monthName = "08";
                 else if (monthNumber[1] == "Sep")
                     monthName = "09";
                 else if (monthNumber[1] == "Oct")
                     monthName = "10";
                 else if (monthNumber[1] == "Nov")
                     monthName = "11";
                 else if (monthNumber[1] == "Dec")
                     monthName = "12";
                 else
                     monthName = string.Empty;

                 ddMMMyyyy = monthNumber[0] + "/" + monthName + "/" + monthNumber[2];
                 date = ddMMMyyyy;
             }
             else
             {
                 date = DateTime.Now.ToString("dd/MM/yyyy");
             }
             return date;
         }
         public string StringDateYYYYmmDD(string ddMMyyyy)
         {
             if (ddMMyyyy.Length == 10)
             {
                 string[] monthNumber = ddMMyyyy.Split('-');
                 // ddMMyyyy = monthNumber[2] + "/" + monthName[1] + "/" + monthNumber[0];
                 string t1 = monthNumber[0].ToString();
                 string t2 = monthNumber[1].ToString(); ;
                 string t3 = monthNumber[2].ToString();
                 string ddMMyyyy2 = t3 + "/" + t2 + "/" + t1;
                 date = ddMMyyyy2;
             }
             else
             {
                 date = DateTime.Now.ToString("dd/MM/yyyy");
             }
             return date;
         }
         public string StringOneDateAdd(string ddMMyyyy)
         {
             if (ddMMyyyy.Length == 10)
             {
                 string[] monthNumber = ddMMyyyy.Split('-');
                 // ddMMyyyy = monthNumber[2] + "/" + monthName[1] + "/" + monthNumber[0];
                 Int64 day = Convert.ToInt64(monthNumber[2].ToString()) + 1;
                 string t1 = monthNumber[0].ToString();
                 string t2 = monthNumber[1].ToString(); ;
                 string t3 = day.ToString().Length == 1 ? "0" + day.ToString() : day.ToString();

                 string ddMMyyyy2 = t3 + "/" + t2 + "/" + t1;
                 date = ddMMyyyy2;
             }
             else
             {
                 date = DateTime.Now.ToString("dd/MM/yyyy");
             }
             return date;
         }
    }
}
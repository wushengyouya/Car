using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public static class DateTimeExtensions
    {
        public static string ToFriendlyDateTimeString(this DateTime Date)
        {
            return FriendlyDate(Date) + " @ " + Date.ToString("t").ToLower();
        }

        public static string ToFriendlyDateString(this DateTime Date)
        {
            return FriendlyDate(Date);
        }

       
        public static string FriendlyDate(DateTime date)
        {
            string formattedDate = "";

            //if(DateTime.Now.Hour-date.Hour>=1)
            //{
            //    formattedDate = "1小时前";
            //}

            //else if (DateTime.Now.Hour - date.Hour >= 3)
            //{
            //    formattedDate = "3小时前";
            //}
            //else if (DateTime.Now.Hour - date.Hour >= 5)
            //{
            //    formattedDate = "5小时前";
            //}
            //else if (DateTime.Now.Hour - date.Hour >= 10)
            //{
            //    formattedDate = "10小时前";
            //}
            //else if (date.Date == DateTime.Today.AddDays(-1))
            //{
            //    formattedDate = "昨天";
            //}
            //else if (date.Date > DateTime.Today.AddDays(-6))
            //{
            //    // *** Show the Day of the week
            //    formattedDate = date.ToString("dddd").ToString();
            //}
            //else
            //{
            //    formattedDate = date.ToString("yyyy年/MM月/dd日");
            //}
            formattedDate = date.ToString("yyyy年/MM月/dd日");
            return formattedDate;
        }
    }
}

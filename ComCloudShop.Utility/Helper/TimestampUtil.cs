using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComCloudShop.Utility.Helper
{
   public class TimestampUtil
    {
        /// <summary>
        /// 时间戳转换日期
        /// </summary>
        /// <param name="time">时间戳</param>
        /// <returns></returns>
        public static DateTime ConvertTimeToDate(long time)
        {
            DateTime time2 = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(0x7b2, 1, 1));
            long ticks = long.Parse(time.ToString() + "0000000");
            TimeSpan span = new TimeSpan(ticks);
            return time2.Add(span);
        }

        /// <summary>
        /// 根据时间转换时间戳
        /// </summary>
        /// <param name="d">时间</param>
        /// <returns></returns>
        public static long GenerateTimeStamp(DateTime d)
        {
            DateTime time = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(0x7b2, 1, 1));
            string str = DateTime.Parse(d.ToLocalTime().ToString()).Subtract(time).Ticks.ToString();
            return Convert.ToInt64(str.Substring(0, str.Length - 7));
        }
    }
}

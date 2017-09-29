using System;

namespace TaskAutoRunManager.DataAccess
{
    public class XConvert
    {
        public static DateTime ToDateTime(object o)
        {
            try
            {
                return Convert.ToDateTime(o);
            }
            catch
            {
                return DateTime.MinValue;
            }
        }

        public static string ToString(object o)
        {
            try
            {
                return Convert.ToString(o);
            }
            catch
            {
                return "";
            }
        }


        public static int ToInt32(object o)
        {
            try
            {
                return Convert.ToInt32(o);
            }
            catch
            {
                return -1;
            }
        }



        public static long ToInt64(object o)
        {
            try
            {
                return Convert.ToInt64(o);
            }
            catch
            {
                return -1;
            }
        }


        public static short ToInt16(object o)
        {
            try
            {
                return Convert.ToInt16(o);
            }
            catch
            {
                return -1;
            }
        }




    }
}
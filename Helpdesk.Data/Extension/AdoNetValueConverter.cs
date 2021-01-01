using System;
using System.Collections.Generic;
using System.Text;

namespace Helpdesk.Data.Extension
{
    public class AdoNetValueConverter
    {
        public T GetValue<T>(object obj)
        {
            if (typeof(DBNull) != obj.GetType())
            {
                return (T)Convert.ChangeType(obj, typeof(T));
            }
            return default(T);
        }

        public T GetValue<T>(object obj, object defaultValue)
        {
            if (typeof(DBNull) != obj.GetType())
            {
                return (T)Convert.ChangeType(obj, typeof(T));
            }
            return (T)defaultValue;
        }

        public T GetValue<T>(object obj, object defaultValue, IFormatProvider formatProvider)
        {
            if (typeof(DBNull) != obj.GetType())
            {
                return (T)Convert.ChangeType(obj, typeof(T), formatProvider);
            }
            return (T)defaultValue;
        }
    }
}

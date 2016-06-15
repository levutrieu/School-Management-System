using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DATN.TTS.BUS
{
    public static class TableUtil
    {
        public static DataTable ConvertToTable(Dictionary<string, Type> defition)
        {
            try
            {
                DataTable dt = new DataTable();

                foreach (var x in defition)
                {
                    dt.Columns.Add(x.Key, x.Value);
                }
                DataRow dr = dt.NewRow();
                dt.Rows.Add(dr);
                return dt;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static DataTable ConvertDictionaryToTable(Dictionary<string, Type> defition, bool allowAddnew)
        {
            try
            {
                DataTable dt = new DataTable();

                foreach (var x in defition)
                {
                    dt.Columns.Add(x.Key, x.Value);
                }
                if (allowAddnew)
                {
                    DataRow dr = dt.NewRow();
                    dt.Rows.Add(dr);
                }
                return dt;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static DataTable LinqToDataTable<T>(IEnumerable<T> linqlist)
        {
            DataTable dt = new DataTable();

            PropertyInfo[] columns = null;

            if (linqlist == null) return dt;

            foreach (T record in linqlist)
            {

                if (columns == null)
                {
                    columns = ((Type) record.GetType()).GetProperties();
                    foreach (PropertyInfo getProperty in columns)
                    {
                        Type colType = getProperty.PropertyType;

                        if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition()
                                                        == typeof (Nullable<>)))
                        {
                            colType = colType.GetGenericArguments()[0];
                        }

                        dt.Columns.Add(new DataColumn(getProperty.Name, colType));
                    }
                }

                DataRow dr = dt.NewRow();

                foreach (PropertyInfo pinfo in columns)
                {
                    dr[pinfo.Name] = pinfo.GetValue(record, null) ?? DBNull.Value;
                }

                dt.Rows.Add(dr);
            }
            return dt;
        }
   }
}

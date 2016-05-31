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

        public static int AutoIncreaseID(DataTable iDataSoure)
        {
            int res = 0;
            try
            {
                if (iDataSoure.Rows.Count <=0)
                {
                    res = 1;
                }
                else
                {
                    if (iDataSoure.Rows.Count == 1)
                    {
                        res = int.Parse(iDataSoure.Rows[0]["ID_NHANVIEN"].ToString());
                    }
                    else
                    {
                        for (int i = 0; i < iDataSoure.Rows.Count; i++)
                        {
                            int min = int.Parse(iDataSoure.Rows[i]["ID_NHANVIEN"].ToString());
                            for (int j = i + 1; j < iDataSoure.Rows.Count; j++)
                            {
                                int max = int.Parse(iDataSoure.Rows[j]["ID_NHANVIEN"].ToString());
                                if (max > min)
                                {
                                    res = max;
                                }
                            }
                        }
                    }
                   
                    res = res + 1;
                }
               return res;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static string KiemTra(ArrayList _MangMa, string _ma)
        {
            if (_MangMa.Count <= 0)//chua co ma cam do nao
            {
                return _ma + "001";
            }
            else
            {
                int max = TimMax(_MangMa);

                ArrayList _machuaco = new ArrayList();
                for (int i = 1; i <= max; i++)
                {
                    _machuaco.Add(i);
                }

                foreach (Object x in _MangMa)
                {
                    for (int i = 1; i <= max; i++)
                    {
                        if (i == int.Parse(x.ToString()))
                        {
                            _machuaco.Remove(x);
                            break;
                        }
                    }

                }

                if (_machuaco.Count > 0)
                {
                    int min = TimMin(_machuaco);
                    return _ma + min.ToString("000");//lap day ma cam do con trong khoang tu 1-->cuoi
                }
                else
                {
                    return _ma + (max + 1).ToString("000");
                }
            }
        }

        public static int TimMax(ArrayList mang)
        {
            int max = int.Parse(mang[0].ToString());
            for (int i = 0; i < mang.Count; i++)
            {
                if (int.Parse(mang[i].ToString()) > max)
                    max = int.Parse(mang[i].ToString());
            }
            return max;
        }

        public static int TimMin(ArrayList mang)
        {
            int min = int.Parse(mang[0].ToString());
            for (int i = 0; i < mang.Count; i++)
            {
                if (int.Parse(mang[i].ToString()) < min)
                    min = int.Parse(mang[i].ToString());
            }
            return min;
        }
    }
}

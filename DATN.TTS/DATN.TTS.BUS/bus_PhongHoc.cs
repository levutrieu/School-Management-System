using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DATN.TTS.DATA;

namespace DATN.TTS.BUS
{
    public class bus_PhongHoc
    {
        db_ttsDataContext db = new db_ttsDataContext();
        public DataTable GetAll()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ID_PHONG");
            dt.Columns.Add("MA_PHONG");
            dt.Columns.Add("TEN_PHONG");
            dt.Columns.Add("SUCCHUA");
            dt.Columns.Add("GHICHU");
            dt.Columns.Add("LOAIPHONG");
            dt.Columns.Add("IS_MAYCHIEU");
            dt.Columns.Add("IS_MAYTINH");
            dt.Columns.Add("DAY");
            dt.Columns.Add("TANG");
            dt.Columns.Add("TINHTRANG");
            dt.Columns.Add("HIENTRANG");
            dt.Columns.Add("MAYCHIEU");
            dt.Columns.Add("MAYTINH");
            try
            {
                var phong = from p in db.tbl_PHONGHOCs where p.IS_DELETE == 0 select p;
                foreach (var p in phong)
                {
                    DataRow r = dt.NewRow();
                    r["ID_PHONG"] = p.ID_PHONG;
                    r["MA_PHONG"] = p.MA_PHONG;
                    r["TEN_PHONG"] = p.TEN_PHONG;
                    r["SUCCHUA"] = p.SUCCHUA;
                    r["GHICHU"] = p.GHICHU;
                    r["LOAIPHONG"] = p.LOAIPHONG;
                    r["IS_MAYCHIEU"] = p.IS_MAYCHIEU;
                    r["IS_MAYTINH"] = p.IS_MAYTINH;
                    r["DAY"] = p.DAY;
                    r["TANG"] = p.TANG;
                    r["TINHTRANG"] = p.TINHTRANG;
                    if (p.TINHTRANG == 1)
                    {
                        r["HIENTRANG"] = "Đang sử dụng";
                        r["TINHTRANG"] = "True";
                    }
                    else
                    {
                        r["HIENTRANG"] = "Đang sữa chữa";
                        r["TINHTRANG"] = "False";
                    }
                    if (p.IS_MAYCHIEU == 1)
                    {
                        r["MAYCHIEU"] = "True";
                    }
                    else
                    {
                        r["MAYCHIEU"] = "False";
                    }
                    if (p.IS_MAYTINH == 1)
                    {
                        r["MAYTINH"] = "True";
                    }
                    else
                    {
                        r["MAYTINH"] = "False";
                    }
                    dt.Rows.Add(r);
                    dt.AcceptChanges();
                }
                return dt;

            }
            catch (Exception)
            {
                
                throw;
            }
        }

        public bool Insert_Phong(params object[] param)
        {
            try
            {
                DataTable dt = (DataTable) param[0];
                DataRow r = dt.Rows[0];
                tbl_PHONGHOC p = new tbl_PHONGHOC();
                p.MA_PHONG = r["MA_PHONG"].ToString();
                p.TEN_PHONG = r["TEN_PHONG"].ToString();
                p.SUCCHUA = Convert.ToInt32(r["SUCCHUA"].ToString());
                p.GHICHU = r["GHICHU"].ToString();
                p.LOAIPHONG = r["LOAIPHONG"].ToString();
                p.IS_MAYCHIEU = Convert.ToInt32(r["IS_MAYCHIEU"].ToString());
                p.IS_MAYTINH = Convert.ToInt32(r["IS_MAYTINH"].ToString());
                p.DAY = r["DAY"].ToString();
                p.TANG = Convert.ToInt32(r["TANG"].ToString());
                p.TINHTRANG = Convert.ToInt32(r["TINHTRANG"].ToString());
                p.CREATE_USER = r["USER"].ToString();
                p.CREATE_TIME = System.DateTime.Today;
                p.IS_DELETE = 0;

                db.tbl_PHONGHOCs.InsertOnSubmit(p);
                db.SubmitChanges();
                if (!p.ID_PHONG.GetTypeCode().Equals(TypeCode.DBNull))
                {
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        public void Update_Phong(params object[] param)
        {
            try
            {
                DataTable dt = (DataTable)param[0];
                DataRow r = dt.Rows[0];
                tbl_PHONGHOC p = db.tbl_PHONGHOCs.Single(t => t.ID_PHONG.Equals(int.Parse(r["ID_PHONG"].ToString())));
                p.MA_PHONG = r["MA_PHONG"].ToString();
                p.TEN_PHONG = r["TEN_PHONG"].ToString();
                p.SUCCHUA = Convert.ToInt32(r["SUCCHUA"].ToString());
                p.GHICHU = r["GHICHU"].ToString();
                p.LOAIPHONG = r["LOAIPHONG"].ToString();
                p.IS_MAYCHIEU = Convert.ToInt32(r["IS_MAYCHIEU"].ToString());
                p.IS_MAYTINH = Convert.ToInt32(r["IS_MAYTINH"].ToString());
                p.DAY = r["DAY"].ToString();
                p.TANG = Convert.ToInt32(r["TANG"].ToString());
                p.TINHTRANG = Convert.ToInt32(r["TINHTRANG"].ToString());
                p.UPDATE_USER = r["USER"].ToString();
                p.UPDATE_TIME = System.DateTime.Today;
                p.IS_DELETE = 0;
                
                db.SubmitChanges();
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        public void Delete_Phong(params object[] param)
        {
            try
            {
                DataTable dt = (DataTable)param[0];
                DataRow r = dt.Rows[0];
                tbl_PHONGHOC p = db.tbl_PHONGHOCs.Single(t => t.ID_PHONG.Equals(int.Parse(r["ID_PHONG"].ToString())));
                p.UPDATE_USER = r["USER"].ToString();
                p.UPDATE_TIME = System.DateTime.Today;
                p.IS_DELETE = 1;

                db.SubmitChanges();
            }
            catch (Exception)
            {
                
                throw;
            }
        }
    }
}

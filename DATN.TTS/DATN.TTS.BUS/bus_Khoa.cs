using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DATN.TTS.DATA;

namespace DATN.TTS.BUS
{
    public class bus_Khoa
    {
        db_ttsDataContext db = new db_ttsDataContext();
        public DataTable GetAllKhoa()
        {
            try
            {
                DataTable dt = null;
                var kh = from k in db.tbl_KHOAs where k.IS_DELETE == 0 select k;
                dt = TableUtil.LinqToDataTable(kh);
                return dt;
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        public bool Insert_Khoa(params object[] param)
        {
            try
            {
                DataTable dt = (DataTable) param[0];
                DataRow r = dt.Rows[0];
                tbl_KHOA k = new tbl_KHOA();
                k.MA_KHOA = r["MA_KHOA"].ToString();
                k.TEN_KHOA = r["TEN_KHOA"].ToString();
                k.DIENTHOAI = r["DIENTHOAI"].ToString();
                k.EMAIL = r["EMAIL"].ToString();
                k.GHICHU = r["GHICHU"].ToString();
                k.CREATE_USER = r["USER"].ToString();
                k.CREATE_TIME = System.DateTime.Today;
                k.IS_DELETE = 0;
                db.tbl_KHOAs.InsertOnSubmit(k);
                db.SubmitChanges();

                if (k.ID_KHOA.ToString().Equals(string.Empty))
                {
                    return false;
                }
                return true;
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        public void Update_Khoa(params object[] param)
        {
            try
            {
                DataTable dt = (DataTable) param[0];
                DataRow r = dt.Rows[0];
                tbl_KHOA k = db.tbl_KHOAs.Single(t => t.ID_KHOA.Equals(int.Parse(r["ID_KHOA"].ToString())));
                k.MA_KHOA = r["MA_KHOA"].ToString();
                k.TEN_KHOA = r["TEN_KHOA"].ToString();
                k.DIENTHOAI = r["DIENTHOAI"].ToString();
                k.EMAIL = r["EMAIL"].ToString();
                k.GHICHU = r["GHICHU"].ToString();
                k.UPDATE_USER = r["USER"].ToString();
                k.UPDATE_TIME = System.DateTime.Today;
                db.SubmitChanges();
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        public void Delete_Khoa(params object[] param)
        {
            try
            {
                DataTable dt = (DataTable)param[0];
                DataRow r = dt.Rows[0];
                tbl_KHOA k = db.tbl_KHOAs.Single(t => t.ID_KHOA.Equals(int.Parse(r["ID_KHOA"].ToString())));
                k.IS_DELETE = 0;
                k.UPDATE_USER = r["USER"].ToString();
                k.UPDATE_TIME = System.DateTime.Today;
                db.SubmitChanges();
            }
            catch (Exception)
            {
                
                throw;
            }
        }
    }
}

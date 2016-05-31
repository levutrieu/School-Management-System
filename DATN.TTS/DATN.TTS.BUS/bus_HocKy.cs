using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using DATN.TTS.DATA;

namespace DATN.TTS.BUS
{
    public class bus_HocKy
    {
        db_ttsDataContext db = new db_ttsDataContext();

        public DataTable GetAll()
        {
            try
            {
                

                DataTable dt = null;
                var hk = (from hki in db.tbl_HOCKies where hki.IS_DELETE == 0 select hki);
                dt = TableUtil.LinqToDataTable(hk);
                return dt;
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        public bool Insert_HocKy(params object[] param)
        {
            try
            {
                DataTable dt = (DataTable) param[0];
                DataRow r = dt.Rows[0];
                tbl_HOCKY hk = new tbl_HOCKY();
                hk.MA_HOCKY = r["MA_HOCKY"].ToString();
                hk.TEN_HOCKY = r["TEN_HOCKY"].ToString();
                hk.TRANGTHAI = r["TRANGTHAI"].ToString();
                hk.GHICHU = r["GHICHU"].ToString();
                hk.CREATE_USER = r["USER"].ToString();
                hk.CREATE_TIME = System.DateTime.Today;
                hk.IS_DELETE = 0;
                db.tbl_HOCKies.InsertOnSubmit(hk);
                db.SubmitChanges();
                if (hk.ID_HOCKY.ToString().Equals(string.Empty))
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

        public void Update_HocKy(params object[] param)
        {
            try
            {
                DataTable dt = (DataTable)param[0];
                DataRow r = dt.Rows[0];
                tbl_HOCKY hk = db.tbl_HOCKies.Single(t => t.ID_HOCKY.ToString().Equals(r["ID_HOCKY"].ToString()));
                hk.MA_HOCKY = r["MA_HOCKY"].ToString();
                hk.TEN_HOCKY = r["TEN_HOCKY"].ToString();
                hk.TRANGTHAI = r["TRANGTHAI"].ToString();
                hk.GHICHU = r["GHICHU"].ToString();
                hk.UPDATE_USER = r["USER"].ToString();
                hk.UPDATE_TIME = System.DateTime.Today;
                db.SubmitChanges();
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        public void Delete_HocKy(params object[] param)
        {
            try
            {
                DataTable dt = (DataTable)param[0];
                DataRow r = dt.Rows[0];
                tbl_HOCKY hk = db.tbl_HOCKies.Single(t => t.ID_HOCKY.ToString().Equals(r["ID_HOCKY"].ToString()));
                hk.IS_DELETE = 1;
                hk.UPDATE_TIME = System.DateTime.Today;
                hk.UPDATE_USER = r["USER"].ToString();
                db.SubmitChanges();
            }
            catch (Exception)
            {
                
                throw;
            }
        }
    }
}

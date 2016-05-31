using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DATN.TTS.DATA;

namespace DATN.TTS.BUS
{
    public class bus_TietHoc
    {
        db_ttsDataContext db = new db_ttsDataContext();

        public DataTable GetAll()
        {
            try
            {
                DataTable dt = null;
                var th = from t in db.tbl_TIETHOCs where t.IS_DELETE == 0 select t;
                dt = TableUtil.LinqToDataTable(th);
                return dt;
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
                DataTable dt = (DataTable) param[0];
                DataRow r = dt.Rows[0];
                tbl_TIETHOC th = db.tbl_TIETHOCs.Single(t => t.ID_TIETHOC.ToString().Equals(r["ID_TIETHOC"].ToString()));
                th.IS_DELETE = 1;
                th.UPDATE_TIME = System.DateTime.Today;
                th.UPDATE_USER = r["USER"].ToString();
                db.SubmitChanges();
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
                DataTable dt = (DataTable)param[0];
                DataRow r = dt.Rows[0];
                tbl_TIETHOC th = new tbl_TIETHOC();
                th.TEN_TIETHOC = r["TEN_TIETHOC"].ToString();
                th.GIO_BD = r["GIO_BD"].ToString();
                th.GIO_KT = r["GIO_KT"].ToString();
                th.CA = r["CA"].ToString();
                th.IS_DELETE = 0;
                th.CREATE_TIME = System.DateTime.Today;
                th.CREATE_USER = r["USER"].ToString();
                db.tbl_TIETHOCs.InsertOnSubmit(th);
                db.SubmitChanges();
                if (th.ID_TIETHOC.ToString().Equals(string.Empty))
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
                DataTable dt = (DataTable)param[0];
                DataRow r = dt.Rows[0];
                tbl_TIETHOC th = db.tbl_TIETHOCs.Single(t => t.ID_TIETHOC.ToString().Equals(r["ID_TIETHOC"].ToString()));
                th.TEN_TIETHOC = r["TEN_TIETHOC"].ToString();
                th.GIO_BD = r["GIO_BD"].ToString();
                th.GIO_KT = r["GIO_KT"].ToString();
                th.CA = r["CA"].ToString();
                th.IS_DELETE = 0;
                th.UPDATE_TIME = System.DateTime.Today;
                th.UPDATE_USER = r["USER"].ToString();
                db.SubmitChanges();
            }
            catch (Exception)
            {
                
                throw;
            }
        }

       
    }
}

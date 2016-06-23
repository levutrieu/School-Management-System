using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DATN.TTS.DATA;

namespace DATN.TTS.BUS
{
    public class bus_ChucVu 
    {
        db_ttsDataContext db = new db_ttsDataContext();

        public DataTable GetAll()
        {
            try
            {
                var cv = from cvu in db.tbl_CHUCVUs where cvu.IS_DELETE == 0 select cvu;
                return TableUtil.LinqToDataTable(cv);
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        public bool Insert_CVU(params object[] param)
        {
            try
            {
                DataTable dt = (DataTable) param[0];
                DataRow r = dt.Rows[0];
                tbl_CHUCVU cv = new tbl_CHUCVU();
                cv.MA_CHUCVU = r["MA_CHUCVU"].ToString();
                cv.TEN_CHUCVU = r["TEN_CHUCVU"].ToString();
                cv.GHICHU = r["GHICHU"].ToString();
                cv.CREATE_USER = r["USER"].ToString();
                cv.CREATE_TIME = System.DateTime.Today;
                cv.IS_DELETE = 0;
                db.tbl_CHUCVUs.InsertOnSubmit(cv);
                db.SubmitChanges();
                if (!cv.ID_CHUCVU.GetTypeCode().Equals(TypeCode.DBNull))
                {
                    return true;
                }
                return false;
            }
            catch (Exception err)
            {
                
                throw err;
            }
        }

        public bool Update_CVU(params object[] param)
        {
            try
            {
                DataTable dt = (DataTable)param[0];
                DataRow r = dt.Rows[0];
                tbl_CHUCVU cv =  db.tbl_CHUCVUs.Single(t => t.ID_CHUCVU.Equals(Convert.ToInt32(r["ID_CHUCVU"].ToString())));
                cv.MA_CHUCVU = r["MA_CHUCVU"].ToString();
                cv.TEN_CHUCVU = r["TEN_CHUCVU"].ToString();
                cv.GHICHU = r["GHICHU"].ToString();
                cv.UPDATE_USER = r["USER"].ToString();
                cv.UPDATE_TIME = System.DateTime.Today;

                db.SubmitChanges();
                if (!cv.ID_CHUCVU.GetTypeCode().Equals(TypeCode.DBNull))
                {
                    return true;
                }
                return false;
            }
            catch (Exception err)
            {

                throw err;
            }
        }

        public bool Delete_CVU(params object[] param)
        {
            try
            {
                DataTable dt = (DataTable)param[0];
                DataRow r = dt.Rows[0];
                tbl_CHUCVU cv = db.tbl_CHUCVUs.Single(t => t.ID_CHUCVU.Equals(Convert.ToInt32(r["ID_CHUCVU"].ToString())));
                cv.IS_DELETE = 1;
                cv.GHICHU = r["GHICHU"].ToString();
                cv.UPDATE_USER = r["USER"].ToString();
                cv.UPDATE_TIME = System.DateTime.Today;

                db.SubmitChanges();
                if (!cv.ID_CHUCVU.GetTypeCode().Equals(TypeCode.DBNull))
                {
                    return true;
                }
                return false;
            }
            catch (Exception err)
            {

                throw err;
            }
        }
    }
}

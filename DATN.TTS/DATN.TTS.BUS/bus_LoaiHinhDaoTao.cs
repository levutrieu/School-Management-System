using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DATN.TTS.DATA;

namespace DATN.TTS.BUS
{
    public class bus_LoaiHinhDaoTao
    {
        db_ttsDataContext db = new db_ttsDataContext();

        public DataTable GetAll()
        {
            try
            {
                DataTable dt = null;
                var lhdt = from ldt in db.tbl_LOAIHINH_DTAOs where ldt.IS_DELETE == 0 select ldt;
                dt = TableUtil.LinqToDataTable(lhdt);
                return dt;
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        public bool Insert_LoaiHinhDaoTao(params object[] param)
        {
            try
            {
                DataTable dt = (DataTable) param[0];
                DataRow r = dt.Rows[0];
                tbl_LOAIHINH_DTAO lhdt = new tbl_LOAIHINH_DTAO();
                lhdt.MA_LOAIHINH_DTAO = r["MA_LOAIHINH_DTAO"].ToString();
                lhdt.TEN_LOAIHINH_DTAO = r["TEN_LOAIHINH_DTAO"].ToString();
                lhdt.TRANGTHAI = r["TRANGTHAI"].ToString();
                lhdt.GHICHU = r["GHICHU"].ToString();
                lhdt.CREATE_USER = r["USER"].ToString();
                lhdt.CREATE_TIME = System.DateTime.Today;
                lhdt.IS_DELETE = 0;
                db.tbl_LOAIHINH_DTAOs.InsertOnSubmit(lhdt);
                db.SubmitChanges();
                if (lhdt.ID_LOAIHINH_DTAO.ToString().Equals(string.Empty))
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

        public void Update_LoaiHinhDaoTao(params object[] param)
        {
            try
            {
                DataTable dt = (DataTable) param[0];
                DataRow r = dt.Rows[0];

                tbl_LOAIHINH_DTAO lhdt = db.tbl_LOAIHINH_DTAOs.Single(t => t.ID_LOAIHINH_DTAO.ToString().Equals(r["ID_LOAIHINH_DTAO"].ToString()));
                lhdt.MA_LOAIHINH_DTAO = r["MA_LOAIHINH_DTAO"].ToString();
                lhdt.TEN_LOAIHINH_DTAO = r["TEN_LOAIHINH_DTAO"].ToString();
                lhdt.TRANGTHAI = r["TRANGTHAI"].ToString();
                lhdt.GHICHU = r["GHICHU"].ToString();
                lhdt.UPDATE_USER = r["USER"].ToString();
                lhdt.UPDATE_TIME = System.DateTime.Today;
                db.SubmitChanges();
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        public void Delete_LoaiHinhDaoTao(params object[] param)
        {
            try
            {
                DataTable dt = (DataTable)param[0];
                DataRow r = dt.Rows[0];

                tbl_LOAIHINH_DTAO lhdt = db.tbl_LOAIHINH_DTAOs.Single(t => t.ID_LOAIHINH_DTAO.ToString().Equals(r["ID_LOAIHINH_DTAO"].ToString()));
                lhdt.UPDATE_USER = r["USER"].ToString();
                lhdt.UPDATE_TIME = System.DateTime.Today;
                lhdt.IS_DELETE = 1;
                db.SubmitChanges();
            }
            catch (Exception)
            {
                
                throw;
            }
        }
    }
}

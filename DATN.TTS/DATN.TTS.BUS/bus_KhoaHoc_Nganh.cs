using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DATN.TTS.DATA;

namespace DATN.TTS.BUS
{
    public class bus_KhoaHoc_Nganh
    {
        db_ttsDataContext db = new db_ttsDataContext();
        public DataTable GetAllKhoaHoc()
        {
            try
            {
                var khoahoc = from kh in db.tbl_KHOAHOCs where kh.IS_DELETE == 0 select kh;
                return TableUtil.LinqToDataTable(khoahoc);
            }
            catch (Exception err)
            {
                throw err;
            }
        }

        public DataTable GetAllNganh()
        {
            try
            {
                var nganh = from ng in db.tbl_NGANHs where ng.IS_DELETE == 0 select ng;
                return TableUtil.LinqToDataTable(nganh);
            }
            catch (Exception err)
            {
                throw err;
            }
        }

        public DataTable GetAllKhoaNganh()
        {
            try
            {
                var khoanganh = from khoanganhs in db.tbl_KHOAHOC_NGANHs where khoanganhs.IS_DELETE == 0
                                join k in db.tbl_KHOAHOCs on khoanganhs.ID_KHOAHOC equals k.ID_KHOAHOC where k.IS_DELETE == 0
                                join ng in db.tbl_NGANHs on khoanganhs.ID_NGANH equals ng.ID_NGANH where ng.IS_DELETE == 0
                                select new
                                {
                                    khoanganhs.ID_KHOAHOC_NGANH,
                                    khoanganhs.ID_KHOAHOC,
                                    khoanganhs.ID_NGANH,
                                    khoanganhs.SO_HKY,
                                    khoanganhs.HOCKY_TRONGKHOA,
                                    khoanganhs.GHICHU,
                                    khoanganhs.SO_LOP,
                                    khoanganhs.SO_SINHVIEN_DK,
                                    k.TEN_KHOAHOC,
                                    ng.TEN_NGANH
                                };

                return TableUtil.LinqToDataTable(khoanganh);
            }
            catch (Exception)
            {
                throw;
            }
        }


        public bool Insert_Khoa_Nganh(params object[] oParams)
        {
            try
            {
                DataTable dt = (DataTable) oParams[0];
                DataRow r = dt.Rows[0];
                tbl_KHOAHOC_NGANH kngNganh = new tbl_KHOAHOC_NGANH();
                kngNganh.ID_KHOAHOC = Convert.ToInt32(r["ID_KHOAHOC"].ToString());
                kngNganh.ID_NGANH = Convert.ToInt32(r["ID_NGANH"].ToString());
                kngNganh.SO_HKY = Convert.ToInt32(r["SO_HKY"].ToString());
                kngNganh.SO_LOP = Convert.ToInt32(r["SO_LOP"].ToString());
                kngNganh.SO_SINHVIEN_DK = Convert.ToInt32(r["SO_SINHVIEN_DK"].ToString());
                kngNganh.HOCKY_TRONGKHOA = r["HOCKY_TRONGKHOA"].ToString();
                kngNganh.GHICHU = r["GHICHU"].ToString();
                kngNganh.CREATE_USER = r["USER"].ToString();
                kngNganh.CREATE_TIME = DateTime.Today;
                kngNganh.IS_DELETE = 0;

                db.tbl_KHOAHOC_NGANHs.InsertOnSubmit(kngNganh);
                db.SubmitChanges();

                if (!kngNganh.ID_KHOAHOC_NGANH.GetTypeCode().Equals(TypeCode.DBNull))
                    return true;
                return false;
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        public void Delete_Khoa_Nganh(params object[] oParams)
        {
            try
            {
                DataTable dt = (DataTable)oParams[0];
                DataRow r = dt.Rows[0];
                tbl_KHOAHOC_NGANH kngNganh = db.tbl_KHOAHOC_NGANHs.Single(t => t.ID_KHOAHOC_NGANH == Convert.ToInt32(r["ID_KHOAHOC_NGANH"].ToString()));
                kngNganh.IS_DELETE = 1;
                kngNganh.UPDATE_TIME = DateTime.Today;
                kngNganh.CREATE_USER = r["USER"].ToString();
                db.SubmitChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Update_Khoa_Nganh(params object[] oParams)
        {
            try
            {
                DataTable dt = (DataTable)oParams[0];
                DataRow r = dt.Rows[0];
                tbl_KHOAHOC_NGANH kngNganh = db.tbl_KHOAHOC_NGANHs.Single(t => t.ID_KHOAHOC_NGANH == Convert.ToInt32(r["ID_KHOAHOC_NGANH"].ToString()));
                kngNganh.ID_KHOAHOC = Convert.ToInt32(r["ID_KHOAHOC"].ToString());
                kngNganh.ID_NGANH = Convert.ToInt32(r["ID_NGANH"].ToString());
                kngNganh.SO_HKY = Convert.ToInt32(r["SO_HKY"].ToString());
                kngNganh.SO_LOP = Convert.ToInt32(r["SO_LOP"].ToString());
                kngNganh.SO_SINHVIEN_DK = Convert.ToInt32(r["SO_SINHVIEN_DK"].ToString());
                kngNganh.HOCKY_TRONGKHOA = r["HOCKY_TRONGKHOA"].ToString();
                kngNganh.GHICHU = r["GHICHU"].ToString();
                kngNganh.UPDATE_TIME = DateTime.Today;
                kngNganh.CREATE_USER = r["USER"].ToString();
                db.SubmitChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}

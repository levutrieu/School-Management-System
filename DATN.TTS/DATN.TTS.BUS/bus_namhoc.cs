using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using DATN.TTS.DATA;

namespace DATN.TTS.BUS
{
    public class bus_namhoc
    {
        db_ttsDataContext db = new db_ttsDataContext();

        public DataTable GetAll()
        {
            try
            {
                DataTable dt = new DataTable();
                var namhoc = from nh in db.tbl_NAMHOC_HIENTAIs where nh.IS_DELETE == 0 select nh;
                dt = TableUtil.LinqToDataTable(namhoc);
                return dt;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool SetNamHocHienTai(int pID_NAMHOC_HIENTAI)
        {
            try
            {
                tbl_NAMHOC_HIENTAI namhoc = db.tbl_NAMHOC_HIENTAIs.Single(t => t.ID_NAMHOC_HIENTAI == pID_NAMHOC_HIENTAI);
                namhoc.IS_HIENTAI = 1;
                db.SubmitChanges();
                var nhoc = from nam in db.tbl_NAMHOC_HIENTAIs.Where(t => t.ID_NAMHOC_HIENTAI != pID_NAMHOC_HIENTAI && (t.IS_DELETE != 1 || t.IS_DELETE == null)) select nam;
                DataTable dt = new DataTable();
                dt = TableUtil.LinqToDataTable(nhoc);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow r in dt.Rows)
                    {
                        tbl_NAMHOC_HIENTAI sethien = db.tbl_NAMHOC_HIENTAIs.Single(t => t.ID_NAMHOC_HIENTAI == Convert.ToInt32(r["ID_NAMHOC_HIENTAI"].ToString()));
                        sethien.IS_HIENTAI = 0;
                        db.SubmitChanges();
                    }
                }
                db.SubmitChanges();
                if (namhoc.ID_NAMHOC_HIENTAI > 0)
                    return true;
                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool Insert_NamHocHienTai(DataTable iDataSoure)
        {
            try
            {
                DataRow r = iDataSoure.Rows[0];
                tbl_NAMHOC_HIENTAI namhoc = new tbl_NAMHOC_HIENTAI();
                namhoc.NAMHOC_TU = Convert.ToInt32(r["NAMHOC_TU"]);
                namhoc.NAMHOC_DEN = Convert.ToInt32(r["NAMHOC_DEN"]);
                namhoc.NGAY_BATDAU = Convert.ToDateTime(r["NGAY_BATDAU"]);
                namhoc.SO_TUAN = Convert.ToInt32(r["SO_TUAN"]);
                namhoc.SO_HKY_TRONGNAM = Convert.ToInt32(r["SO_HKY_TRONGNAM"]);
                //namhoc.IS_HIENTAI = Convert.ToInt32(r[""]);
                //namhoc.IS_DELETE = Convert.ToInt32(r[""]);
                namhoc.CREATE_USER = r["USER"].ToString();
                namhoc.CREATE_TIME = DateTime.Now;

                db.tbl_NAMHOC_HIENTAIs.InsertOnSubmit(namhoc);
                db.SubmitChanges();
                if (!namhoc.ID_NAMHOC_HIENTAI.GetTypeCode().Equals(TypeCode.DBNull))
                    return true;
                return false;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public bool Delete_NamHocHienTai(int pID_NAMHOC_HIENTAI, string pUSER)
        {
            try
            {
                tbl_NAMHOC_HIENTAI namhoc = db.tbl_NAMHOC_HIENTAIs.Single(t => t.ID_NAMHOC_HIENTAI == pID_NAMHOC_HIENTAI);
                namhoc.IS_DELETE = 1;
                namhoc.UPDATE_USER = pUSER;
                namhoc.UPDATE_TIME = DateTime.Now;
                db.SubmitChanges();
                db.SubmitChanges();
                if (namhoc.ID_NAMHOC_HIENTAI > 0)
                    return true;
                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool Update_NamHocHienTai(DataTable iDataSoure)
        {
            try
            {
                DataRow r = iDataSoure.Rows[0];
                tbl_NAMHOC_HIENTAI namhoc = db.tbl_NAMHOC_HIENTAIs.Single(t => t.ID_NAMHOC_HIENTAI == Convert.ToInt32(r["ID_NAMHOC_HIENTAI"].ToString()));
                namhoc.NAMHOC_TU = Convert.ToInt32(r["NAMHOC_TU"]);
                namhoc.NAMHOC_DEN = Convert.ToInt32(r["NAMHOC_DEN"]);
                namhoc.NGAY_BATDAU = Convert.ToDateTime(r["NGAY_BATDAU"]);
                namhoc.SO_TUAN = Convert.ToInt32(r["SO_TUAN"]);
                namhoc.SO_HKY_TRONGNAM = Convert.ToInt32(r["SO_HKY_TRONGNAM"]);
                namhoc.UPDATE_USER = r["USER"].ToString();
                namhoc.UPDATE_TIME = DateTime.Now;

                db.SubmitChanges();
                if (namhoc.ID_NAMHOC_HIENTAI > 0)
                    return true;
                return false;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

    }
}

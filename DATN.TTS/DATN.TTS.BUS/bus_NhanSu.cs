using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DATN.TTS.DATA;

namespace DATN.TTS.BUS
{
    public class bus_NhanSu
    {
        db_ttsDataContext db = new db_ttsDataContext();

        public DataTable GetAllNhanSu()
        {
            try
            {
                DataTable dt = null;

                var ns = from nsu in db.tbl_NhanSus where nsu.IS_DELETE == 0 select nsu ;

                dt = TableUtil.LinqToDataTable(ns);

                return dt;
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        protected int GetLastID()
        {
            try
            {
                int lasts = 0;
                var last = (from ns in db.tbl_NhanSus select ns).OrderByDescending(t =>t.ID_NHANVIEN).FirstOrDefault();

                if (last != null)
                {
                  lasts   = int.Parse(last.ID_NHANVIEN.ToString());
                }
                return lasts + 1;
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        public bool Insert_NhanSu(params object[] oParam)
        {
            bool res;
            try
            {
                DataTable iDataSoure = (DataTable) oParam[0];

                DataRow dr = iDataSoure.Rows[0];

                tbl_NhanSu ns = new tbl_NhanSu();
                //ns.ID_NHANVIEN = GetLastID();
                ns.MA_NHANVIEN = dr["MA_NHANVIEN"].ToString();
                ns.HOTEN = dr["HOTEN"].ToString();
                ns.GIOI_TINH = int.Parse(dr["GIOI_TINH"].ToString());
                ns.NGAY_SINH = DateTime.Parse(dr["NGAY_SINH"].ToString());
                ns.DIACHI = dr["DIACHI"].ToString();
                ns.CMND = dr["CMND"].ToString();
                ns.NGAYCAP = DateTime.Parse(dr["NGAYCAP"].ToString());
                ns.NOICAP = dr["NOICAP"].ToString();
                ns.DIENTHOAI = dr["DIENTHOAI"].ToString();
                ns.EMAIL = dr["EMAIL"].ToString();
                ns.TRANGTHAI = dr["TRANGTHAI"].ToString();
                ns.NAM_LAMVIEC = int.Parse(dr["NAM_LAMVIEC"].ToString());
                ns.ID_HE_SOLUONG = int.Parse(dr["ID_HE_SOLUONG"].ToString());
                ns.CREATE_USER = dr["USER"].ToString();
                ns.CREATE_TIME = System.DateTime.Now;
                ns.CHOOHIENTAI = dr["CHOOHIENTAI"].ToString();
                ns.LUONGCB = float.Parse(dr["LUONGCB"].ToString());
                ns.IS_DELETE = 0;
                db.tbl_NhanSus.InsertOnSubmit(ns);
                db.SubmitChanges();
                if (string.IsNullOrEmpty(ns.ID_NHANVIEN.ToString()))
                {
                    res = false;
                }
                else
                {
                    res = true;
                }
                return res;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Delete_NhanSu(params object[] param)
        {
            try
            {
                DataTable dt = (DataTable) param[0];
                DataRow dr = dt.Rows[0];
                tbl_NhanSu ns = db.tbl_NhanSus.Single(t => t.ID_NHANVIEN == int.Parse(dr["ID_NHANVIEN"].ToString()));
                ns.IS_DELETE = 1;
                db.SubmitChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Update_NhanSu(params object[] param)
        {
            try
            {
                DataTable dt = (DataTable) param[0];
                DataRow dr = dt.Rows[0];
                tbl_NhanSu ns = db.tbl_NhanSus.Single(t => t.ID_NHANVIEN == int.Parse(dr["ID_NHANVIEN"].ToString()));
                ns.MA_NHANVIEN = dr["MA_NHANVIEN"].ToString();
                ns.HOTEN = dr["HOTEN"].ToString();
                ns.GIOI_TINH = int.Parse(dr["GIOI_TINH"].ToString());
                ns.NGAY_SINH = DateTime.Parse(dr["NGAY_SINH"].ToString());
                ns.DIACHI = dr["DIACHI"].ToString();
                ns.CMND = dr["CMND"].ToString();
                ns.NGAYCAP = DateTime.Parse(dr["NGAYCAP"].ToString());
                ns.NOICAP = dr["NOICAP"].ToString();
                ns.DIENTHOAI = dr["DIENTHOAI"].ToString();
                ns.EMAIL = dr["EMAIL"].ToString();
                ns.TRANGTHAI = dr["TRANGTHAI"].ToString();
                ns.NAM_LAMVIEC = int.Parse(dr["NAM_LAMVIEC"].ToString());
                ns.ID_HE_SOLUONG = int.Parse(dr["ID_HE_SOLUONG"].ToString());
                ns.UPDATE_USRER = dr["USER"].ToString();
                ns.UPDATE_TIME = System.DateTime.Now;
                ns.CHOOHIENTAI = dr["CHOOHIENTAI"].ToString();
                ns.LUONGCB = int.Parse(dr["LUONGCB"].ToString());
                db.SubmitChanges();
            }
            catch (Exception)
            {
                
                throw;
            }
        }

    }
}

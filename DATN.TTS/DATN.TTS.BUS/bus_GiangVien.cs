using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DATN.TTS.DATA;

namespace DATN.TTS.BUS
{
    public class bus_GiangVien 
    {
        db_ttsDataContext db = new db_ttsDataContext();
        public DataTable GetAllChucVu()
        {
            var cv = from c in db.tbl_CHUCVUs where c.IS_DELETE == 0 select c;

            DataTable dt = TableUtil.LinqToDataTable(cv);
            return dt;
        }

        public DataTable GetAllKhoa()
        {
            try
            {
                var khoa = from k in db.tbl_KHOAs where k.IS_DELETE == 0
                    select new
                    {
                        k.ID_KHOA,
                        k.TEN_KHOA,
                    };
                return TableUtil.LinqToDataTable(khoa);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DataTable GetAllGiangVien()
        {
            try
            {
                DataTable dt = new DataTable();
                var gv = from g in db.tbl_GIANGVIENs where g.IS_DELETE == 0
                    join k in db.tbl_KHOAs on g.ID_KHOA equals k.ID_KHOA where k.IS_DELETE == 0
                    join cv in db.tbl_CHUCVUs on g.ID_CHUCVU equals cv.ID_CHUCVU where cv.IS_DELETE == 0
                    select new
                    {
                        g.ID_GIANGVIEN,
                        g.ID_CHUCVU,
                        g.ID_KHOA,
                        g.MA_GIANGVIEN,
                        g.TEN_GIANGVIEN,
                        g.NGAYSINH,
                        g.NOISINH,
                        g.GIOITINH,
                        g.HE_SOLUONG,
                        g.CMND,
                        g.NGAYCAP,
                        g.NOICAP,
                        g.NAM_LAMVIEC,
                        g.NAM_KETTHUC,
                        g.PATH_ANH,
                        g.TRANGTHAI,
                        g.DIACHI,
                        g.DIENTHOAI,
                        g.EMAIL,
                        g.GHICHU,
                        k.TEN_KHOA,
                        cv.TEN_CHUCVU
                    };
                dt = TableUtil.LinqToDataTable(gv);
                return dt;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool CheckMaGV(string pMaGV)
        {
            try
            {
                var gv = from g in db.tbl_GIANGVIENs where g.IS_DELETE == 0 && g.MA_GIANGVIEN == pMaGV select g;
                DataTable dt = TableUtil.LinqToDataTable(gv);
                if (dt.Rows.Count > 0)
                    return false;
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool Insert_Giangvien(params object[] oParams)
        {
            try
            {
                DataTable dt = (DataTable) oParams[0];
                DataRow r = dt.Rows[0];
                tbl_GIANGVIEN gv = new tbl_GIANGVIEN();
                gv.ID_KHOA = Convert.ToInt32(r["ID_KHOA"].ToString());
                gv.MA_GIANGVIEN = r["MA_GIANGVIEN"].ToString();
                gv.TEN_GIANGVIEN = r["TEN_GIANGVIEN"].ToString();
                gv.MA_GIANGVIEN = r["MA_GIANGVIEN"].ToString();
                gv.PATH_ANH = r["PATH_ANH"].ToString();
                gv.NGAYSINH = Convert.ToDateTime(r["NGAYSINH"].ToString());
                gv.GIOITINH = Convert.ToBoolean(r["GIOITINH"].ToString());
                gv.DIACHI = r["DIACHI"].ToString();
                gv.DIENTHOAI = r["DIENTHOAI"].ToString();
                gv.EMAIL = r["EMAIL"].ToString();
                gv.CMND = r["CMND"].ToString();
                gv.NGAYCAP = Convert.ToDateTime(r["NGAYCAP"].ToString());
                gv.NOICAP = r["NOICAP"].ToString();
                gv.TRANGTHAI = r["TRANGTHAI"].ToString();
                gv.GHICHU = r["GHICHU"].ToString();
                gv.NOISINH = r["NOISINH"].ToString();
                gv.NAM_LAMVIEC = Convert.ToDateTime(r["NAM_LAMVIEC"].ToString());
                gv.NAM_KETTHUC = Convert.ToDateTime(r["NAM_KETTHUC"].ToString());
                gv.ID_CHUCVU = Convert.ToInt32(r["ID_CHUCVU"].ToString());
                gv.HE_SOLUONG = Convert.ToDouble(r["HE_SOLUONG"].ToString());
                gv.IS_DELETE = 0;
                gv.CREATE_TIME = System.DateTime.Today;
                gv.UPDATE_USER = r["USER"].ToString();
                db.tbl_GIANGVIENs.InsertOnSubmit(gv);
                db.SubmitChanges();
                if (!gv.ID_GIANGVIEN.GetTypeCode().Equals(TypeCode.DBNull))
                    return true;
                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool Update_GiangVien(params object[] oParams)
        {
            try
            {
                int res = 0;
                DataTable dt = (DataTable)oParams[0];
                DataRow r = dt.Rows[0];
                tbl_GIANGVIEN gv = db.tbl_GIANGVIENs.Single(t => t.ID_GIANGVIEN == Convert.ToInt32(r["ID_GIANGVIEN"].ToString()));
                gv.ID_KHOA = Convert.ToInt32(r["ID_KHOA"].ToString());
                gv.MA_GIANGVIEN = r["MA_GIANGVIEN"].ToString();
                gv.TEN_GIANGVIEN = r["TEN_GIANGVIEN"].ToString();
                gv.MA_GIANGVIEN = r["MA_GIANGVIEN"].ToString();
                gv.PATH_ANH = r["PATH_ANH"].ToString();
                gv.NGAYSINH = Convert.ToDateTime(r["NGAYSINH"].ToString());
                gv.GIOITINH = Convert.ToBoolean(r["GIOITINH"].ToString());
                gv.DIACHI = r["DIACHI"].ToString();
                gv.DIENTHOAI = r["DIENTHOAI"].ToString();
                gv.EMAIL = r["EMAIL"].ToString();
                gv.CMND = r["CMND"].ToString();
                gv.NGAYCAP = Convert.ToDateTime(r["NGAYCAP"].ToString());
                gv.NOICAP = r["NOICAP"].ToString();
                gv.TRANGTHAI = r["TRANGTHAI"].ToString();
                gv.GHICHU = r["GHICHU"].ToString();
                gv.NOISINH = r["NOISINH"].ToString();
                gv.NAM_LAMVIEC = Convert.ToDateTime(r["NAM_LAMVIEC"].ToString());
                gv.NAM_KETTHUC = Convert.ToDateTime(r["NAM_KETTHUC"].ToString());
                gv.ID_CHUCVU = Convert.ToInt32(r["ID_CHUCVU"].ToString());
                gv.HE_SOLUONG = Convert.ToDouble(r["HE_SOLUONG"].ToString());
                gv.UPDATE_TIME = System.DateTime.Today;
                gv.UPDATE_USER = r["USER"].ToString();
                db.SubmitChanges();
                res = gv.ID_GIANGVIEN;
                if (res > 0)
                    return true;
                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool Delete_GiangVien(params object[] oParams)
        {
            try
            {
                int res = 0;
                DataTable dt = (DataTable)oParams[0];
                DataRow r = dt.Rows[0];
                tbl_GIANGVIEN gv = db.tbl_GIANGVIENs.Single(t => t.ID_GIANGVIEN == Convert.ToInt32(r["ID_GIANGVIEN"].ToString()));
                gv.IS_DELETE = 1;
                gv.UPDATE_TIME = System.DateTime.Today;
                gv.UPDATE_USER = r["USER"].ToString();
                db.SubmitChanges();
                res = gv.ID_GIANGVIEN;
                if (res > 0)
                    return true;
                return false;
            }
            catch (Exception)
            {
                
                throw;
            }
        }
    }
}

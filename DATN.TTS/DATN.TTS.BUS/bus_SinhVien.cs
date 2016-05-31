using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DATN.TTS.DATA;

namespace DATN.TTS.BUS
{
    public class bus_SinhVien
    {
        db_ttsDataContext db = new db_ttsDataContext();
        public DataTable GetAllLop()
        {
            try
            {
                DataTable dt = new DataTable();
                var lop = from l in db.tbl_LOPHOCs
                    where l.IS_DELETE == 0
                    select new
                    {
                        l.ID_LOPHOC,
                        l.TEN_LOP
                    };
                dt = TableUtil.LinqToDataTable(lop);
                return dt;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool CheckMSSV(string pMaSoSV)
        {
            try
            {
                DataTable dt = new DataTable();
                var sv = (from s in db.tbL_SINHVIENs where s.IS_DELETE == 0 && s.MA_SINHVIEN == pMaSoSV select s);
                dt = TableUtil.LinqToDataTable(sv);
                if (dt.Rows.Count > 0)
                    return false;
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DataTable GetAllSinhVien()
        {
            try
            {
                DataTable dt = new DataTable();
                var sv = from svien in db.tbL_SINHVIENs where svien.IS_DELETE == 0 
                         join l in db.tbl_LOPHOCs on svien.ID_LOPHOC equals l.ID_LOPHOC where l.IS_DELETE == 0
                         select new
                        {
                            svien.ID_SINHVIEN,
                            svien.ID_LOPHOC,
                            svien.TRANGTHAI,
                            svien.MA_SINHVIEN,
                            svien.TEN_SINHVIEN,
                            svien.NGAYSINH,
                            svien.NOICAP,
                            svien.GIOITINH,
                            svien.IS_DOANVIEN,
                            svien.NGAYCAP,
                            svien.NGAY_RATRUONG,
                            svien.NOISINH,
                            svien.NGAY_VAODOAN,
                            svien.NGAY_VAOTRUONG,
                            svien.PATH_ANH,
                            svien.THONGTIN_NGOAITRU,
                            svien.CMND,
                            svien.DIACHI, 
                            svien.DIENTHOAI,
                            svien.DIENTHOAI_GD,
                            svien.EMAIL,

                            l.TEN_LOP

                        };

                dt = TableUtil.LinqToDataTable(sv);

                return dt;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool Insert_SinhVien(params object[] oParams)
        {
            try
            {
                DataTable dt = (DataTable)oParams[0];
                DataRow r = dt.Rows[0];
                tbL_SINHVIEN sv = new tbL_SINHVIEN();
                sv.ID_LOPHOC = Convert.ToInt32(r["ID_LOPHOC"].ToString());
                sv.TRANGTHAI = r["TRANGTHAI"].ToString();
                sv.MA_SINHVIEN = r["MA_SINHVIEN"].ToString();
                sv.TEN_SINHVIEN = r["TEN_SINHVIEN"].ToString();
                sv.PATH_ANH = r["PATH_ANH"].ToString();
                sv.GIOITINH = Convert.ToBoolean(r["GIOITINH"]);
                sv.DIENTHOAI = r["DIENTHOAI"].ToString();
                sv.DIENTHOAI_GD = r["DIENTHOAI_GD"].ToString();
                sv.DIACHI = r["DIACHI"].ToString();
                sv.EMAIL = r["EMAIL"].ToString();
                sv.CMND = r["CMND"].ToString();
                sv.NGAYCAP = DateTime.Parse(r["NGAYCAP"].ToString());
                sv.NOICAP = r["NOICAP"].ToString();
                sv.NGAYSINH = DateTime.Parse(r["NGAYSINH"].ToString());
                sv.NOISINH = r["NOISINH"].ToString();
                sv.THONGTIN_NGOAITRU = r["THONGTIN_NGOAITRU"].ToString();
                sv.IS_DOANVIEN = Convert.ToBoolean(r["IS_DOANVIEN"].ToString());
                sv.NGAY_VAODOAN = DateTime.Parse(r["NGAY_VAODOAN"].ToString());
                sv.NGAY_VAOTRUONG = DateTime.Parse(r["NGAY_VAOTRUONG"].ToString());
                sv.NGAY_RATRUONG = DateTime.Parse(r["NGAY_RATRUONG"].ToString());
                sv.IS_DELETE = 0;
                sv.CREATE_USER = r["USER"].ToString();

                sv.PASSWORD = r["MA_SINHVIEN"].ToString(); // pass word mặc định mỗi khi thêm 1 sinh viên

                sv.CREATE_TIME = System.DateTime.Today;

                db.tbL_SINHVIENs.InsertOnSubmit(sv);
                db.SubmitChanges();
                if (!sv.ID_SINHVIEN.GetTypeCode().Equals(TypeCode.DBNull))
                    return true;
                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int Update_SinhVien(params object[] oParams)
        {
            try
            {
                int res = 0;
                DataTable dt = (DataTable)oParams[0];
                DataRow r = dt.Rows[0];
                tbL_SINHVIEN sv = db.tbL_SINHVIENs.Single(t => t.ID_SINHVIEN.Equals(Convert.ToInt32(r["ID_SINHVIEN"].ToString())));
                sv.ID_LOPHOC = Convert.ToInt32(r["ID_LOPHOC"].ToString());
                sv.TRANGTHAI = r["TRANGTHAI"].ToString();
                sv.MA_SINHVIEN = r["MA_SINHVIEN"].ToString();
                sv.TEN_SINHVIEN = r["TEN_SINHVIEN"].ToString();
                sv.PATH_ANH = r["PATH_ANH"].ToString();
                sv.GIOITINH = Convert.ToBoolean(r["GIOITINH"]);
                sv.DIENTHOAI = r["DIENTHOAI"].ToString();
                sv.DIENTHOAI_GD = r["DIENTHOAI_GD"].ToString();
                sv.DIACHI = r["DIACHI"].ToString();
                sv.EMAIL = r["EMAIL"].ToString();
                sv.CMND = r["CMND"].ToString();
                sv.NGAYCAP = DateTime.Parse(r["NGAYCAP"].ToString());
                sv.NOICAP = r["NOICAP"].ToString();
                sv.NGAYSINH = DateTime.Parse(r["NGAYSINH"].ToString());
                sv.NOISINH = r["NOISINH"].ToString();
                sv.THONGTIN_NGOAITRU = r["THONGTIN_NGOAITRU"].ToString();
                sv.IS_DOANVIEN = Convert.ToBoolean(r["IS_DOANVIEN"].ToString());
                sv.NGAY_VAODOAN = DateTime.Parse(r["NGAY_VAODOAN"].ToString());
                sv.NGAY_VAOTRUONG = DateTime.Parse(r["NGAY_VAOTRUONG"].ToString());
                sv.NGAY_RATRUONG = DateTime.Parse(r["NGAY_RATRUONG"].ToString());

                sv.IS_DELETE = 0;
                sv.UPDATE_USER = r["USER"].ToString();
                sv.UPDATE_TIME = System.DateTime.Today;
                db.SubmitChanges();

                res = sv.ID_SINHVIEN;
                return res;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public int Delete_SinhVien(params object[] oParam)
        {
            try
            {
                int res = 0;
                DataTable dt = (DataTable) oParam[0];
                DataRow r = dt.Rows[0];
                tbL_SINHVIEN sv = db.tbL_SINHVIENs.Single(t => t.ID_SINHVIEN.Equals(Convert.ToInt32(r["ID_SINHVIEN"].ToString())));
                sv.IS_DELETE = 1;
                sv.UPDATE_USER = r["USER"].ToString();
                sv.UPDATE_TIME = System.DateTime.Today;
                db.SubmitChanges();
                res = sv.ID_SINHVIEN;
                return res;
            }
            catch (Exception)
            {
                
                throw;
            }
        }
    }
}

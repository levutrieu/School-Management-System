using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using DATN.TTS.DATA;

namespace DATN.TTS.BUS
{
    public class bus_LopHoc
    {
        db_ttsDataContext db = new db_ttsDataContext();

        public bool CheckTrungMaLop(string malop)
        {
            try
            {
                var lop = from l in db.tbl_LOPHOCs where l.MA_LOP == malop select l;
                DataTable dt = new DataTable();
                dt = TableUtil.LinqToDataTable(lop);
                if (dt.Rows.Count > 0)
                    return false;
                return true;
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
                DataTable dt = new DataTable();
                dt.Columns.Add("ID_KHOAHOC_NGANH", typeof(int));
                dt.Columns.Add("KHOA_NGANH", typeof(string));
                var khNganh = from kng in db.tbl_KHOAHOC_NGANHs where (kng.IS_DELETE != 1 || kng.IS_DELETE == null)
                              join kh in db.tbl_KHOAHOCs on kng.ID_KHOAHOC equals kh.ID_KHOAHOC where (kh.IS_DELETE !=1 || kh.IS_DELETE == null)
                              join nganh in db.tbl_NGANHs on kng.ID_NGANH equals nganh.ID_NGANH where (nganh.IS_DELETE !=1 || nganh.IS_DELETE == null)
                              select new
                              {
                                  kng.ID_KHOAHOC_NGANH,
                                  kh.TEN_KHOAHOC,
                                  nganh.TEN_NGANH
                              
                              };
                foreach (var x in khNganh)
                {
                    DataRow r = dt.NewRow();
                    r["ID_KHOAHOC_NGANH"] = x.ID_KHOAHOC_NGANH;
                    r["KHOA_NGANH"] = x.TEN_NGANH + "_" + x.TEN_KHOAHOC;

                    dt.Rows.Add(r);
                    dt.AcceptChanges();
                }
                return dt;
            }
            catch (Exception err)
            {
                throw err;
            }
        }

        public DataTable GetGV()
        {
            try
            {
                var gv = from gvien in db.tbl_GIANGVIENs where gvien.IS_DELETE == 0 select new {gvien.ID_GIANGVIEN, gvien.TEN_GIANGVIEN};
                return TableUtil.LinqToDataTable(gv);
            }
            catch (Exception err)
            {
                throw err;
            }
        }

        public DataTable GetAllLop()
        {
            try
            {
                DataTable dt = new DataTable();
                var lop = from l in db.tbl_LOPHOCs where (l.IS_DELETE != 1 || l.IS_DELETE == null)
                          join khoanganh in db.tbl_KHOAHOC_NGANHs on l.ID_KHOAHOC_NGANH equals khoanganh.ID_KHOAHOC_NGANH where (khoanganh.IS_DELETE != 1 || khoanganh.IS_DELETE == null)
                          join kh in db.tbl_KHOAHOCs on khoanganh.ID_KHOAHOC equals kh.ID_KHOAHOC where (kh.IS_DELETE != 1 || kh.IS_DELETE == null)
                          join nganh in db.tbl_NGANHs on khoanganh.ID_NGANH equals nganh.ID_NGANH where (nganh.IS_DELETE != 1 || nganh.IS_DELETE == null)
                          join gv in db.tbl_GIANGVIENs on l.ID_GIANGVIEN_CN equals gv.ID_GIANGVIEN where (gv.IS_DELETE != 1 || gv.IS_DELETE == null)
                    select new
                    {
                        l.ID_LOPHOC,
                        l.ID_KHOAHOC_NGANH,
                        l.MA_LOP,
                        l.TEN_LOP,
                        l.SOLUONG_SV,
                        l.GHICHU,
                        l.ID_GIANGVIEN_CN,
                        kh.TEN_KHOAHOC,
                        nganh.TEN_NGANH,
                        gv.TEN_GIANGVIEN
                    };
                dt = TableUtil.LinqToDataTable(lop);
                return dt;
            }
            catch (Exception err)
            {
                throw err;
            }
        }

        public bool Insert_Lop(params object[] oParams)
        {
            try
            {
                DataTable dt = (DataTable) oParams[0];
                DataRow r = dt.Rows[0];
                tbl_LOPHOC lop = new tbl_LOPHOC();
                lop.ID_KHOAHOC_NGANH = Convert.ToInt32(r["ID_KHOAHOC_NGANH"].ToString());
                lop.MA_LOP = r["MA_LOP"].ToString();
                lop.TEN_LOP = r["TEN_LOP"].ToString();
                lop.GHICHU = r["GHICHU"].ToString();
                lop.ID_GIANGVIEN_CN = Convert.ToInt32(r["ID_GIANGVIEN"].ToString());// giáo viên quản lý lớp
                lop.SOLUONG_SV = Convert.ToInt32(r["SOLUONG_SV"].ToString());
                lop.CREATE_USER = r["USER"].ToString();
                lop.CREATE_TIME = DateTime.Today;
                lop.IS_DELETE = 0;

                db.tbl_LOPHOCs.InsertOnSubmit(lop);
                db.SubmitChanges();
                if (!lop.ID_LOPHOC.GetTypeCode().Equals(TypeCode.DBNull))
                    return true;
                return false;
            }
            catch (Exception err)
            {
                throw err;
            }
        }

        public bool Update_Lop(params object[] oParams)
        {
            try
            {
                DataTable dt = (DataTable)oParams[0];
                DataRow r = dt.Rows[0];

                tbl_LOPHOC lop = db.tbl_LOPHOCs.Single(t => t.ID_LOPHOC.Equals(Convert.ToInt32(r["ID_LOPHOC"].ToString())));
                lop.ID_KHOAHOC_NGANH = Convert.ToInt32(r["ID_KHOAHOC_NGANH"].ToString());
                lop.MA_LOP = r["MA_LOP"].ToString();
                lop.TEN_LOP = r["TEN_LOP"].ToString();
                lop.GHICHU = r["GHICHU"].ToString();
                lop.ID_GIANGVIEN_CN = Convert.ToInt32(r["ID_GIANGVIEN"].ToString());// giáo viên quản lý lớp
                lop.SOLUONG_SV = Convert.ToInt32(r["SOLUONG_SV"].ToString());
                lop.UPDATE_USER = r["USER"].ToString();
                lop.UPDATE_TIME = System.DateTime.Today;
                db.SubmitChanges();
                if (!lop.ID_LOPHOC.GetTypeCode().Equals(TypeCode.DBNull))
                    return true;
                return false;
            }
            catch (Exception err)
            {
                throw err;
            }
        }

        public bool Delete_Lop(params object[] oParams)
        {
            try
            {
                DataTable dt = (DataTable)oParams[0];
                DataRow r = dt.Rows[0];

                tbl_LOPHOC lop = db.tbl_LOPHOCs.Single(t => t.ID_LOPHOC.Equals(Convert.ToInt32(r["ID_LOPHOC"].ToString())));
                lop.UPDATE_USER = r["USER"].ToString();
                lop.UPDATE_TIME = System.DateTime.Today;
                lop.IS_DELETE = 1;

                db.SubmitChanges();
                if (!lop.ID_LOPHOC.GetTypeCode().Equals(TypeCode.DBNull))
                    return true;
                return false;
            }
            catch (Exception err)
            {
                throw err;
            }
        }

        public DataTable GetAllLopWhere(int idnganh)
        {
            try
            {
                DataTable dt = null;
                if (idnganh > 0)
                {
                    var lop = from l in db.tbl_LOPHOCs
                              where (l.IS_DELETE != 1 || l.IS_DELETE == null) && l.ID_KHOAHOC_NGANH == idnganh
                              join khoanganh in db.tbl_KHOAHOC_NGANHs on l.ID_KHOAHOC_NGANH equals khoanganh.ID_KHOAHOC_NGANH
                              where (khoanganh.IS_DELETE != 1 || khoanganh.IS_DELETE == null)
                              join kh in db.tbl_KHOAHOCs on khoanganh.ID_KHOAHOC equals kh.ID_KHOAHOC
                              where (kh.IS_DELETE != 1 || kh.IS_DELETE == null)
                              join nganh in db.tbl_NGANHs on khoanganh.ID_NGANH equals nganh.ID_NGANH
                              where (nganh.IS_DELETE != 1 || nganh.IS_DELETE == null)
                              join gv in db.tbl_GIANGVIENs on l.ID_GIANGVIEN_CN equals gv.ID_GIANGVIEN
                              where (gv.IS_DELETE != 1 || gv.IS_DELETE == null)
                              select new
                              {
                                  l.ID_LOPHOC,
                                  l.ID_KHOAHOC_NGANH,
                                  l.MA_LOP,
                                  l.TEN_LOP,
                                  l.SOLUONG_SV,
                                  l.GHICHU,
                                  l.ID_GIANGVIEN_CN,
                                  kh.TEN_KHOAHOC,
                                  nganh.TEN_NGANH,
                                  gv.TEN_GIANGVIEN
                              };
                    dt = TableUtil.LinqToDataTable(lop);
                }
                return dt;
            }
            catch (Exception err)
            {
                throw err;
            }
        }
    }
}

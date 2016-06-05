using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DATN.TTS.DATA;

namespace DATN.TTS.BUS
{
    public class bus_LapKeHoachDaoTaoKhoa:bus_PhanCong_TKB
    {
        db_ttsDataContext db = new db_ttsDataContext();

        public DataTable GetKhoaWhereHDT(int IdHeDaoTao)
        {
            try
            {
                DataTable dt = null;
                var hdt = from h in db.tbl_KHOAHOCs
                    where (h.IS_DELETE != 1 || h.IS_DELETE == null) && h.ID_HE_DAOTAO == IdHeDaoTao
                    select new
                    {
                        h.ID_KHOAHOC,
                        h.TEN_KHOAHOC
                    };
                dt = TableUtil.LinqToDataTable(hdt);
                return dt;

            }
            catch (Exception err)
            {
               throw err;
            }
        }

        public DataTable GetNganhWhereHDT()
        {
            try
            {
                DataTable dt = new DataTable();
                    var nganh = (from ng in db.tbl_NGANHs
                        where (ng.IS_DELETE != 1 || ng.IS_DELETE == null)
                        join k in db.tbl_KHOAs on ng.ID_KHOA equals k.ID_KHOA
                        where (k.IS_DELETE != 1 || k.IS_DELETE == null)
                        select new
                        {
                            ng.ID_NGANH,
                            ng.MA_NGANH,
                            ng.TEN_NGANH,
                            ng.KYHIEU,
                            ng.TRANGTHAI,
                            ng.GHICHU,
                            ng.CAP_NGANH,
                            ng.ID_KHOA,
                            k.TEN_KHOA
                        }
                        );

                    dt = TableUtil.LinqToDataTable(nganh);
                return dt;
            }
            catch (Exception er)
            {
                throw er;
            }
        }

        public DataTable GetAllKhoaNganh(int pID_KHOAHOC)
        {
            try
            {
                DataTable dt = new DataTable();
                if (pID_KHOAHOC != 0 || !string.IsNullOrEmpty(pID_KHOAHOC.ToString()))
                {
                    var khoanganh = from khoanganhs in db.tbl_KHOAHOC_NGANHs
                                    where (khoanganhs.IS_DELETE != 1 || khoanganhs.IS_DELETE == null) && khoanganhs.ID_KHOAHOC == pID_KHOAHOC
                                    join k in db.tbl_KHOAHOCs on khoanganhs.ID_KHOAHOC equals k.ID_KHOAHOC
                                    where k.IS_DELETE == 0
                                    join ng in db.tbl_NGANHs on khoanganhs.ID_NGANH equals ng.ID_NGANH
                                    where ng.IS_DELETE == 0
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
                    dt =TableUtil.LinqToDataTable(khoanganh);
                }
                return dt;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DataTable GetAllKhoaHoc(int pID_HEDAOTAO)
        {
            try
            {
                DataTable dt = null;
                if (pID_HEDAOTAO > 0)
                {
                    var kh = from khoc in db.tbl_KHOAHOCs
                             join hdt in db.tbl_HEDAOTAOs on khoc.ID_HE_DAOTAO equals hdt.ID_HE_DAOTAO
                             where (khoc.IS_DELETE != 1 || khoc.IS_DELETE == null) && (hdt.IS_DELETE != 1 || hdt.IS_DELETE == null) && khoc.ID_HE_DAOTAO == pID_HEDAOTAO
                             select new
                             {
                                 khoc.ID_KHOAHOC,
                                 khoc.MA_KHOAHOC,
                                 khoc.ID_HE_DAOTAO,
                                 khoc.KYHIEU,
                                 khoc.NAM_BD,
                                 khoc.NAM_KT,
                                 khoc.SO_HKY_1NAM,
                                 khoc.SO_HKY,
                                 khoc.TEN_KHOAHOC,
                                 khoc.TRANGTHAI,
                                 hdt.TEN_HE_DAOTAO

                             };
                    dt = TableUtil.LinqToDataTable(kh);
                }
                else
                {
                    var kh = from khoc in db.tbl_KHOAHOCs
                             join hdt in db.tbl_HEDAOTAOs on khoc.ID_HE_DAOTAO equals hdt.ID_HE_DAOTAO
                             where (khoc.IS_DELETE != 1 || khoc.IS_DELETE == null) && (hdt.IS_DELETE != 1 || hdt.IS_DELETE == null)
                             select new
                             {
                                 khoc.ID_KHOAHOC,
                                 khoc.MA_KHOAHOC,
                                 khoc.ID_HE_DAOTAO,
                                 khoc.KYHIEU,
                                 khoc.NAM_BD,
                                 khoc.NAM_KT,
                                 khoc.SO_HKY_1NAM,
                                 khoc.SO_HKY,
                                 khoc.TEN_KHOAHOC,
                                 khoc.TRANGTHAI,
                                 hdt.TEN_HE_DAOTAO

                             };
                    dt = TableUtil.LinqToDataTable(kh);
                }
                
                return dt;

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
                int res = 0;
                DataTable dt = (DataTable)oParams[0];
                string pUser = (string) oParams[1];
                foreach (DataRow r in dt.Rows)
                {
                    if (r["ID_KHOAHOC_NGANH"].ToString() == "0")
                    {
                        if (r["CHK"].ToString() == "False" || r["CHK"].ToString() == string.Empty)
                        {
                            #region Thực hiện thêm mới

                            try
                            {
                                tbl_KHOAHOC_NGANH kngNganh = new tbl_KHOAHOC_NGANH();
                                kngNganh.ID_KHOAHOC = Convert.ToInt32(r["ID_KHOAHOC"].ToString());
                                kngNganh.ID_NGANH = Convert.ToInt32(r["ID_NGANH"].ToString());
                                kngNganh.SO_HKY = Convert.ToInt32(r["SO_HKY"].ToString());
                                kngNganh.SO_LOP = Convert.ToInt32(r["SO_LOP"].ToString());
                                kngNganh.SO_SINHVIEN_DK = Convert.ToInt32(r["SO_SINHVIEN_DK"].ToString());
                                kngNganh.HOCKY_TRONGKHOA = r["HOCKY_TRONGKHOA"].ToString();
                                kngNganh.GHICHU = r["GHICHU"].ToString();
                                kngNganh.CREATE_USER = pUser;
                                kngNganh.CREATE_TIME = DateTime.Today;
                                kngNganh.IS_DELETE = 0;
                                db.tbl_KHOAHOC_NGANHs.InsertOnSubmit(kngNganh);
                                db.SubmitChanges();
                                res++;
                            }
                            catch (Exception err)
                            {
                                return false;
                                throw err;
                            }

                            #endregion
                        }
                    }
                    else
                    {
                        if (r["CHK"].ToString() == "False" || r["CHK"].ToString() == string.Empty)
                        {
                            #region Thực hiện cập nhật

                            try
                            {
                                tbl_KHOAHOC_NGANH kngNganh = db.tbl_KHOAHOC_NGANHs.Single(t => t.ID_KHOAHOC_NGANH == Convert.ToInt32(r["ID_KHOAHOC_NGANH"].ToString()));
                                kngNganh.ID_KHOAHOC = Convert.ToInt32(r["ID_KHOAHOC"].ToString());
                                kngNganh.ID_NGANH = Convert.ToInt32(r["ID_NGANH"].ToString());
                                kngNganh.SO_HKY = Convert.ToInt32(r["SO_HKY"].ToString());
                                kngNganh.SO_LOP = Convert.ToInt32(r["SO_LOP"].ToString());
                                kngNganh.SO_SINHVIEN_DK = Convert.ToInt32(r["SO_SINHVIEN_DK"].ToString());
                                kngNganh.HOCKY_TRONGKHOA = r["HOCKY_TRONGKHOA"].ToString();
                                kngNganh.GHICHU = r["GHICHU"].ToString();
                                kngNganh.UPDATE_TIME = DateTime.Today;
                                kngNganh.CREATE_USER = pUser;
                                db.SubmitChanges();
                                res++;
                            }
                            catch (Exception err)
                            {
                                return false;
                                throw err;
                            }

                            #endregion
                        }
                        if (r["CHK"].ToString() == "True")
                        {
                            tbl_KHOAHOC_NGANH kngNganh = db.tbl_KHOAHOC_NGANHs.Single(t => t.ID_KHOAHOC_NGANH == Convert.ToInt32(r["ID_KHOAHOC_NGANH"].ToString()));
                            kngNganh.IS_DELETE = 1;
                            kngNganh.UPDATE_TIME = DateTime.Today;
                            kngNganh.CREATE_USER = pUser;
                            db.SubmitChanges();    
                        }
                    }
                }
                if (res > 0)
                    return true;
                return false;
            }
            catch (Exception err)
            {
                throw err;
            }
        }

        public bool Delete_Khoa_Nganh(params object[] oParams)
        {
            try
            {
                int res = 0;
                DataTable dt = (DataTable)oParams[0];
                string pUSER = (string) oParams[1];
                foreach (DataRow r in dt.Rows)
                {
                    try
                    {
                        if (r["CHK"].ToString() == "True")
                        {
                            tbl_KHOAHOC_NGANH kngNganh = db.tbl_KHOAHOC_NGANHs.Single(t => t.ID_KHOAHOC_NGANH == Convert.ToInt32(r["ID_KHOAHOC_NGANH"].ToString()));
                            kngNganh.IS_DELETE = 1;
                            kngNganh.UPDATE_TIME = DateTime.Today;
                            kngNganh.CREATE_USER = pUSER;
                            db.SubmitChanges();
                            res++;
                        }
                    }
                    catch (Exception err)
                    {
                        return false;
                        throw err;
                    }
                }
                if (res > 0)
                    return true;
                return false;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public DataTable GetData(int pID_bomon, int pID_loaimon)
        {
            try
            {
                DataTable dt = null;



                return dt;
            }
            catch (Exception err)
            {
                throw err;
            }
        }

        public DataTable GetData_1()
        {
            try
            {
                DataTable dt = null;
                var mh = from mhoc in db.tbl_MONHOCs where (mhoc.IS_DELETE != 1 || mhoc.IS_DELETE == null) 
                 select new
                {
                    mhoc.ID_MONHOC,
                    mhoc.MA_MONHOC,
                    mhoc.TEN_MONHOC,
                    mhoc.KY_HIEU,
                    mhoc.SO_TC,
                    mhoc.ISBATBUOC,
                    mhoc.IS_THUHOCPHI,
                    mhoc.IS_THUCHANH,
                    mhoc.IS_LYTHUYET,
                    mhoc.IS_TINHDIEM,
                    mhoc.ID_MONHOC_SONGHANH,
                    mhoc.TRANGTHAI
                };
                dt = TableUtil.LinqToDataTable(mh);
                return dt;
            }
            catch (Exception err)
            {
                throw err;
            }
        }

        public DataTable GetAllKhoaNganhCT(int pID_KHOAHOC_NGANH)
        {
            try
            {
                DataTable dt = null;
                var knct = from ct in db.tbl_KHOAHOC_NGANH_CTIETs
                           where
                             (ct.IS_DELETE != 1 ||
                             ct.IS_DELETE == null) &&
                             ct.ID_KHOAHOC_NGANH == pID_KHOAHOC_NGANH
                           select new
                           {
                               ct.ID_KHOAHOC_NGANH_CTIET,
                               ct.ID_MONHOC,
                               ct.ID_HE_DAOTAO,
                               ct.ID_KHOAHOC_NGANH,
                               ct.SO_TC,
                               ct.HOCKY,
                               ct.SOTIET_LT,
                               ct.SOTIET_TH,
                               ct.ID_MONHOC_TRUOC,
                               ct.ID_MONHOC_SONGHANH,
                               ct.MONHOC_TIENQUYET,
                               TEN_MONHOC =
                                 ((from mh in db.tbl_MONHOCs
                                   where
                                     mh.ID_MONHOC == ct.ID_MONHOC
                                   select new
                                   {
                                       mh.TEN_MONHOC
                                   }).First().TEN_MONHOC)
                           };
                dt = TableUtil.LinqToDataTable(knct);
                return dt;
            }
            catch (Exception err)
            {
                throw err;
            }
        }

        public DataTable GetMonHocTruoc()
        {
            try
            {
                DataTable dt = new DataTable();
                var monhoctruoc = from x in db.tbl_MONHOCs
                    where (x.IS_DELETE != 1 || x.IS_DELETE == null)
                    select x;
                dt = TableUtil.LinqToDataTable(monhoctruoc);
                return dt;
            }
            catch (Exception err)
            {
                throw err;
            }
        }

        public DataTable GetMonHocSongHanh()
        {
            try
            {
                DataTable dt = new DataTable();
                var monhoctruoc = from x in db.tbl_MONHOCs
                                  where (x.IS_DELETE != 1 || x.IS_DELETE == null)
                                  select x;
                dt = TableUtil.LinqToDataTable(monhoctruoc);
                return dt;
            }
            catch (Exception err)
            {
                throw err;
            }
        }

        public DataTable GetMonHocTienQuyet()
        {
            try
            {
                DataTable dt = new DataTable();
                var monhoctruoc = from x in db.tbl_MONHOCs
                                  where (x.IS_DELETE != 1 || x.IS_DELETE == null)
                                  select x;
                dt = TableUtil.LinqToDataTable(monhoctruoc);
                return dt;
            }
            catch (Exception err)
            {
                throw err;
            }
        }

        public bool Insert_KhoaNganhCT(params object[] oParams)
        {
            try
            {
                int res = 0;
                DataTable dt = (DataTable) oParams[0];
                string pUSER = (string) oParams[1];
                foreach (DataRow r in dt.Rows)
                {
                    if (r["ID_KHOAHOC_NGANH_CTIET"].ToString() == "0")
                    {
                        if (r["CHK"].ToString() == "False" || r["CHK"].ToString() == string.Empty)
                        {
                            #region Thêm mới
                            try
                            {
                                tbl_KHOAHOC_NGANH_CTIET knct = new tbl_KHOAHOC_NGANH_CTIET();
                                knct.ID_MONHOC = Convert.ToInt32(r["ID_MONHOC"].ToString());
                                knct.ID_HE_DAOTAO = Convert.ToInt32(r["ID_HE_DAOTAO"].ToString());
                                knct.ID_KHOAHOC_NGANH = Convert.ToInt32(r["ID_KHOAHOC_NGANH"].ToString());
                                knct.SO_TC = Convert.ToInt32(r["SO_TC"].ToString());
                                knct.HOCKY = Convert.ToInt32(r["HOCKY"].ToString());
                                knct.SOTIET_LT = Convert.ToInt32(r["SOTIET_LT"].ToString());
                                knct.SOTIET_TH = Convert.ToInt32(r["SOTIET_TH"].ToString());
                                knct.ID_MONHOC_TRUOC = Convert.ToInt32(r["ID_MONHOC_TRUOC"].ToString());
                                knct.ID_MONHOC_SONGHANH = Convert.ToInt32(r["ID_MONHOC_SONGHANH"].ToString());
                                knct.MONHOC_TIENQUYET = Convert.ToInt32(r["MONHOC_TIENQUYET"].ToString());
                                knct.IS_DELETE = 0;
                                knct.CREATE_TIME = System.DateTime.Now;
                                knct.CREATE_USER = pUSER;

                                db.tbl_KHOAHOC_NGANH_CTIETs.InsertOnSubmit(knct);
                                db.SubmitChanges();
                                res++;
                            }
                            catch (Exception err)
                            {
                                return false;
                                throw err;
                            }
                            #endregion
                        }
                       
                    }
                    else
                    {
                        if (r["CHK"].ToString() == "False" || r["CHK"].ToString() == string.Empty)
                        {
                            #region Thực hiện cập nhật
                            try
                            {
                                tbl_KHOAHOC_NGANH_CTIET knct = db.tbl_KHOAHOC_NGANH_CTIETs.Single(t => t.ID_KHOAHOC_NGANH_CTIET == Convert.ToInt32(r["ID_KHOAHOC_NGANH_CTIET"].ToString()));
                                knct.ID_MONHOC = Convert.ToInt32(r["ID_MONHOC"].ToString());
                                knct.ID_HE_DAOTAO = Convert.ToInt32(r["ID_HE_DAOTAO"].ToString());
                                knct.ID_KHOAHOC_NGANH = Convert.ToInt32(r["ID_KHOAHOC_NGANH"].ToString());
                                knct.SO_TC = Convert.ToInt32(r["SO_TC"].ToString());
                                knct.HOCKY = Convert.ToInt32(r["HOCKY"].ToString());
                                knct.SOTIET_LT = Convert.ToInt32(r["SOTIET_LT"].ToString());
                                knct.SOTIET_TH = Convert.ToInt32(r["SOTIET_TH"].ToString());
                                knct.ID_MONHOC_TRUOC = Convert.ToInt32(r["ID_MONHOC_TRUOC"].ToString());
                                knct.ID_MONHOC_SONGHANH = Convert.ToInt32(r["ID_MONHOC_SONGHANH"].ToString());
                                knct.MONHOC_TIENQUYET = Convert.ToInt32(r["MONHOC_TIENQUYET"].ToString());
                                knct.IS_DELETE = 0;
                                knct.UPDATE_TIME = System.DateTime.Now;
                                knct.UPDATE_USER = pUSER;

                                db.SubmitChanges();

                                res++;
                            }
                            catch (Exception err)
                            {
                                return false;
                                throw err;
                            }
                            #endregion
                        }
                        if (r["CHK"].ToString() == "True")
                        {
                            tbl_KHOAHOC_NGANH_CTIET knct = db.tbl_KHOAHOC_NGANH_CTIETs.Single(t => t.ID_KHOAHOC_NGANH_CTIET == Convert.ToInt32(r["ID_KHOAHOC_NGANH_CTIET"].ToString()));
                            knct.IS_DELETE = 1;
                            knct.UPDATE_TIME = System.DateTime.Now;
                            knct.UPDATE_USER = pUSER;
                            db.SubmitChanges();
                            res++;
                        }
                       
                    }
                }
                if (res > 0)
                    return true;
                return false;
            }
            catch (Exception err)
            {
                throw err;
            }
        }

        public bool Delete_KhoaNganhCT(params object[] oParams)
        {
            try
            {
                int res = 0;
                DataTable dt = (DataTable) oParams[0];
                string pUSER = (string) oParams[1];
                foreach (DataRow r in dt.Rows)
                {
                    #region Xóa 
                    if (r["ID_KHOAHOC_NGANH_CTIET"].ToString() != "0")
                    {
                        if (r["CHK"].ToString() == "True")
                        {
                            try
                            {
                                tbl_KHOAHOC_NGANH_CTIET knct = db.tbl_KHOAHOC_NGANH_CTIETs.Single(t => t.ID_KHOAHOC_NGANH_CTIET == Convert.ToInt32(r["ID_KHOAHOC_NGANH_CTIET"].ToString()));
                                knct.IS_DELETE = 1;
                                knct.UPDATE_TIME = System.DateTime.Now;
                                knct.UPDATE_USER = pUSER;
                                db.SubmitChanges();
                                res++;
                            }
                            catch (Exception err)
                            {
                                return false;
                                throw err;
                            }
                        }
                    }
                    #endregion
                }
                if (res > 0)
                    return true;
                return false;
            }
            catch (Exception err)
            {
                throw err;
            }
        }

        public int GetIDKhoaNganh(int idkhoa, int idnganh)
        {
            try
            {
                int res = 0;
                DataTable dt = null;
                if (idkhoa > 0 && idnganh > 0)
                {
                    var khoanganh = (from k in db.tbl_KHOAHOC_NGANHs
                                     where (k.IS_DELETE != 1 || k.IS_DELETE == null)
                                     && k.ID_KHOAHOC == idkhoa && k.ID_NGANH == idnganh
                                     select new { k.ID_KHOAHOC_NGANH });
                    dt = TableUtil.LinqToDataTable(khoanganh);
                }
                if (dt != null && dt.Rows.Count > 0)
                {
                    res = Convert.ToInt32(dt.Rows[0]["ID_KHOAHOC_NGANH"].ToString());
                }
                return res;
            }
            catch (Exception err)
            {
                throw err;
            } 
        }
    }
}

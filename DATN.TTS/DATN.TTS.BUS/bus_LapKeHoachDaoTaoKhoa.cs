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

        public DataTable GetNganhWhereHDT(int idkhoahoc)
        {
            try
            {
                DataTable dt = new DataTable();
                var nganh = from ng in db.tbl_NGANHs
                            where
                              (ng.IS_DELETE != 1 ||
                              ng.IS_DELETE == null) &&
                              !
                                (from khoahocnganh in db.tbl_KHOAHOC_NGANHs
                                 join khoahoc in db.tbl_KHOAHOCs on new { ID_KHOAHOC = Convert.ToInt32(khoahocnganh.ID_KHOAHOC) } equals new { ID_KHOAHOC = khoahoc.ID_KHOAHOC } into khoahoc_join
                                 from khoahoc in khoahoc_join.DefaultIfEmpty()
                                 where
                                   (khoahocnganh.IS_DELETE != 1 ||
                                   khoahocnganh.IS_DELETE == null) &&
                                   khoahoc.ID_KHOAHOC == idkhoahoc
                                 select new
                                 {
                                     khoahocnganh.ID_NGANH
                                 }).Contains(new { ID_NGANH = (System.Int32?)ng.ID_NGANH })
                            select new
                            {
                                ng.ID_NGANH,
                                ng.ID_KHOA,
                                ng.ID_HE_DAOTAO,
                                ng.MA_NGANH,
                                ng.TEN_NGANH,
                                ng.KYHIEU,
                                ng.GHICHU,
                                ng.TRANGTHAI,
                                ng.IS_DELETE,
                                ng.CREATE_USER,
                                ng.UPDATE_USER,
                                ng.CREATE_TIME,
                                ng.UPDATE_TIME,
                                ng.CAP_NGANH
                            };

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

        public DataTable GetData_1(int idkhoanganh, int idhedaotao)
        {
            try
            {
                DataTable dt = null;
                var mhoc = from mh in db.tbl_MONHOCs
                           where
                             (mh.IS_DELETE != 1 || mh.IS_DELETE == null) && mh.ID_HE_DAOTAO == idhedaotao &&
                             !(from knct in db.tbl_KHOAHOC_NGANH_CTIETs
                                join khoanganh in db.tbl_KHOAHOC_NGANHs on new { ID_KHOAHOC_NGANH = Convert.ToInt32(knct.ID_KHOAHOC_NGANH) } equals new { ID_KHOAHOC_NGANH = khoanganh.ID_KHOAHOC_NGANH }
                                where
                                  (knct.IS_DELETE != 1 || knct.IS_DELETE == null) &&
                                    (from khoanganh0 in db.tbl_KHOAHOC_NGANHs
                                     where (khoanganh0.IS_DELETE != 1 || khoanganh0.IS_DELETE == null) && khoanganh0.ID_KHOAHOC_NGANH == idkhoanganh
                                     select new
                                     {
                                         khoanganh0.ID_KHOAHOC_NGANH
                                     }).Contains(new { ID_KHOAHOC_NGANH = (System.Int32)knct.ID_KHOAHOC_NGANH })
                                select new
                                {
                                    knct.ID_MONHOC
                                }).Contains(new { ID_MONHOC = (System.Int32?)mh.ID_MONHOC })
                           select new
                           {
                               mh.ID_MONHOC,
                               mh.ID_LOAI_MONHOC,
                               mh.ID_BOMON,
                               mh.MA_MONHOC,
                               mh.TEN_MONHOC,
                               mh.KY_HIEU,
                               mh.SO_TC,
                               mh.IS_THUHOCPHI,
                               mh.IS_THUCHANH,
                               mh.IS_LYTHUYET,
                               mh.IS_TINHDIEM,
                               mh.TRANGTHAI,
                               mh.IS_DELETE,
                               mh.CREATE_USER,
                               mh.UPDATE_USER,
                               mh.CREATE_TIME,
                               mh.UPDATE_TIME,
                               mh.ISBATBUOC,
                               mh.ID_MONHOC_SONGHANH,
                               mh.GHICHU,
                               SOTIET_LT =(int?)mh.SOTIET_LT,
                               SOTIET_TH=(int?)mh.SOTIET_TH,
                               mh.CACH_TINHDIEM
                           };
                
                dt = TableUtil.LinqToDataTable(mhoc);
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
                    select new
                    {
                        ID_MONHOC_TRUOC = x.ID_MONHOC,
                        x.TEN_MONHOC
                    };
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
                    select new
                    {
                        ID_MONHOC_SONGHANH = x.ID_MONHOC,
                        x.TEN_MONHOC
                    };
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
                                  select new
                                  {
                                      MONHOC_TIENQUYET = x.ID_MONHOC,
                                      x.TEN_MONHOC
                                  };
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

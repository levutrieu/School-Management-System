using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DATN.TTS.DATA;

namespace DATN.TTS.BUS
{
    public class bus_NhapDiemSV
    {
        db_ttsDataContext db = new db_ttsDataContext();
        public DataTable GetAll_DiemSV()
        {
            try
            {
                DataTable dt = null;
                var query = from Tbl_DIEM_SINHVIEN in db.tbl_DIEM_SINHVIENs
                            where
                                Tbl_DIEM_SINHVIEN.IS_DELETE != 1 ||
                                Tbl_DIEM_SINHVIEN.IS_DELETE == null
                            select new
                            {
                                Tbl_DIEM_SINHVIEN.ID_KETQUA,
                                Tbl_DIEM_SINHVIEN.ID_SINHVIEN,
                                TEN_SINHVIEN =
                                    ((from m in db.tbL_SINHVIENs
                                      where
                                          m.ID_SINHVIEN == Tbl_DIEM_SINHVIEN.ID_SINHVIEN
                                      select new
                                      {
                                          m.TEN_SINHVIEN
                                      }).First().TEN_SINHVIEN),
                                MA_SINHVIEN =
                                    ((from m in db.tbL_SINHVIENs
                                      where
                                          m.ID_SINHVIEN == Tbl_DIEM_SINHVIEN.ID_SINHVIEN
                                      select new
                                      {
                                          m.MA_SINHVIEN
                                      }).First().MA_SINHVIEN),
                                Tbl_DIEM_SINHVIEN.ID_LOPHOCPHAN,
                                MA_LOP_HOCPHAN =
                                    ((from m in db.tbl_LOP_HOCPHANs
                                      where
                                          m.ID_LOPHOCPHAN == Tbl_DIEM_SINHVIEN.ID_LOPHOCPHAN
                                      select new
                                      {
                                          m.MA_LOP_HOCPHAN
                                      }).First().MA_LOP_HOCPHAN),
                                TEN_LOP_HOCPHAN =
                                    ((from m in db.tbl_LOP_HOCPHANs
                                      where
                                          m.ID_LOPHOCPHAN == Tbl_DIEM_SINHVIEN.ID_LOPHOCPHAN
                                      select new
                                      {
                                          m.TEN_LOP_HOCPHAN
                                      }).First().TEN_LOP_HOCPHAN),
                                Tbl_DIEM_SINHVIEN.ID_KHOAHOC,
                                TEN_KHOAHOC =
                                    ((from m in db.tbl_KHOAHOCs
                                      where
                                          m.ID_KHOAHOC == Tbl_DIEM_SINHVIEN.ID_KHOAHOC
                                      select new
                                      {
                                          m.TEN_KHOAHOC
                                      }).First().TEN_KHOAHOC),
                                HOCKY = Tbl_DIEM_SINHVIEN.ID_HOCKY,
                                Tbl_DIEM_SINHVIEN.DIEM_BT,
                                Tbl_DIEM_SINHVIEN.DIEM_GK,
                                Tbl_DIEM_SINHVIEN.DIEM_CK,
                                Tbl_DIEM_SINHVIEN.DIEM_TONG,
                                Tbl_DIEM_SINHVIEN.DIEM_HE4,
                                Tbl_DIEM_SINHVIEN.DIEM_CHU,
                                Tbl_DIEM_SINHVIEN.GHICHU
                            };
                dt = TableUtil.LinqToDataTable(query);
                return dt;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public int InsertObject_Excel(DataTable idatasource, string pUser)
        {
            try
            {
                int i = 0;
                foreach (DataRow dr in idatasource.Rows)
                {
                    double? diembt;
                    double? diemgk;
                    double? diemck;
                    double? diemtk;
                    double? diemhe4;
                    if (!string.IsNullOrEmpty(dr["f_diembt"].ToString()))
                    {
                        diembt = Convert.ToDouble(dr["f_diembt"]);
                    }
                    else
                    {
                        diembt = null;
                    }
                    if (!string.IsNullOrEmpty(dr["f_diem1"].ToString()))
                    {
                        diemgk = Convert.ToDouble(dr["f_diem1"]);
                    }
                    else
                    {
                        diemgk = null;
                    }
                    if (!string.IsNullOrEmpty(dr["f_diem2"].ToString()))
                    {
                        diemck = Convert.ToDouble(dr["f_diem2"]);
                    }
                    else
                    {
                        diemck = null;
                    }
                    if (!string.IsNullOrEmpty(dr["f_diemtk1"].ToString()))
                    {
                        diemtk = Convert.ToDouble(dr["f_diemtk1"]);
                    }
                    else
                    {
                        diemtk = null;
                    }
                    if (!string.IsNullOrEmpty(dr["f_diemstk1"].ToString()))
                    {
                        diemhe4 = Convert.ToDouble(dr["f_diemstk1"]);
                    }
                    else
                    {
                        diemhe4 = null;
                    }
                    tbl_DIEM_SINHVIEN query = new tbl_DIEM_SINHVIEN
                    {
                        ID_SINHVIEN = Convert.ToInt32(dr["ID_SINHVIEN"]),
                        ID_LOPHOCPHAN = Convert.ToInt32(dr["ID_LOPHOCPHAN"]),
                        DIEM_BT = diembt,
                        DIEM_GK = diemgk,
                        DIEM_CK = diemck,
                        DIEM_TONG = diemtk,
                        DIEM_HE4 = diemhe4,
                        DIEM_CHU = dr["f_diemch1"].ToString(),
                        CREATE_USER = pUser,
                        CREATE_TIME = DateTime.Now,
                    };
                    db.tbl_DIEM_SINHVIENs.InsertOnSubmit(query);
                    db.SubmitChanges();
                    i = query.ID_KETQUA;
                }
                return i;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        //private double ConvertToDouble(string ivalue)
        //{
        //    double? ireturn = null;
        //    if (!string.IsNullOrEmpty(ivalue))
        //    {
        //        ireturn = Convert.ToDouble(ivalue);
        //    }
        //    else
        //    {
        //        ireturn = null;
        //    }
        //    return ireturn;
        //}

        public DataTable GetAllKhoaNganh_ForDiem(int pID_KHOAHOC)
        {
            try
            {
                DataTable dt = new DataTable();
                if (pID_KHOAHOC != 0 || !string.IsNullOrEmpty(pID_KHOAHOC.ToString()))
                {
                    var khoanganh = from khoanganhs in db.tbl_KHOAHOC_NGANHs
                                    where (khoanganhs.IS_DELETE != 1 || khoanganhs.IS_DELETE == null) && khoanganhs.ID_KHOAHOC == pID_KHOAHOC
                                    join k in db.tbl_KHOAHOCs on khoanganhs.ID_KHOAHOC equals k.ID_KHOAHOC
                                    where (k.IS_DELETE != 1 || k.IS_DELETE == null)
                                    join ng in db.tbl_NGANHs on khoanganhs.ID_NGANH equals ng.ID_NGANH
                                    where (ng.IS_DELETE != 1 || ng.IS_DELETE == null)
                                    select new
                                    {
                                        khoanganhs.ID_KHOAHOC_NGANH,
                                        ng.TEN_NGANH
                                    };
                    dt = TableUtil.LinqToDataTable(khoanganh);
                }
                return dt;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DataTable GetMonHocWhereKhoaNganh(int idkhoanganh)
        {
            try
            {
                DataTable dt = null;
                var monhoc = from knct in db.tbl_KHOAHOC_NGANH_CTIETs
                             join mh in db.tbl_MONHOCs on new { ID_MONHOC = Convert.ToInt32(knct.ID_MONHOC) } equals
                                 new { ID_MONHOC = mh.ID_MONHOC } into mh_join
                             from mh in mh_join.DefaultIfEmpty()
                             where
                                 (knct.IS_DELETE != 1 ||
                                  knct.IS_DELETE == null) &&
                                 (mh.IS_DELETE != 1 ||
                                  mh.IS_DELETE == null) &&
                                 knct.ID_KHOAHOC_NGANH == idkhoanganh
                             select new
                             {
                                 ID_MONHOC = (int)knct.ID_MONHOC,
                                 TEN_MONHOC = mh.TEN_MONHOC
                             };
                dt = TableUtil.LinqToDataTable(monhoc);
                return dt;
            }
            catch (Exception err)
            {
                throw err;
            }
        }

        public DataTable GetLopHocPhanWhereIdMonHoc(int idmonhoc)
        {
            try
            {
                DataTable dt = null;
                if (idmonhoc == 0)
                {
                    var lophocphan = from hp in db.tbl_LOP_HOCPHANs
                                     join hkht in db.tbl_NAMHOC_HKY_HTAIs on
                                         new { ID_NAMHOC_HKY_HTAI = Convert.ToInt32(hp.ID_NAMHOC_HKY_HTAI) } equals
                                         new { ID_NAMHOC_HKY_HTAI = hkht.ID_NAMHOC_HKY_HTAI } into hkht_join
                                     from hkht in hkht_join.DefaultIfEmpty()
                                     where
                                         (hp.IS_DELETE != 1 ||
                                          hp.IS_DELETE == null) &&
                                         (hkht.IS_DELETE != 1 ||
                                          hkht.IS_DELETE == null) &&
                                         hkht.IS_HIENTAI == 1
                                     select new
                                     {
                                         hp.ID_LOPHOCPHAN,
                                         TEN_LOP_HOCPHAN = hp.MA_LOP_HOCPHAN.Trim() + "_" + hp.TEN_LOP_HOCPHAN
                                     };
                    dt = TableUtil.LinqToDataTable(lophocphan);
                }
                else
                {
                    var lophocphan = from hp in db.tbl_LOP_HOCPHANs
                                     join hkht in db.tbl_NAMHOC_HKY_HTAIs on
                                         new { ID_NAMHOC_HKY_HTAI = Convert.ToInt32(hp.ID_NAMHOC_HKY_HTAI) } equals
                                         new { ID_NAMHOC_HKY_HTAI = hkht.ID_NAMHOC_HKY_HTAI } into hkht_join
                                     from hkht in hkht_join.DefaultIfEmpty()
                                     where
                                         (hp.IS_DELETE != 1 ||
                                          hp.IS_DELETE == null) &&
                                         (hkht.IS_DELETE != 1 ||
                                          hkht.IS_DELETE == null) &&
                                         hkht.IS_HIENTAI == 1 &&
                                         hp.ID_MONHOC == idmonhoc
                                     select new
                                     {
                                         hp.ID_LOPHOCPHAN,
                                         hp.TEN_LOP_HOCPHAN
                                     };
                    dt = TableUtil.LinqToDataTable(lophocphan);
                }

                return dt;
            }
            catch (Exception err)
            {
                throw err;
            }
        }

        public DataTable GetDanhSachSinhVienDK(int idlophocphan)
        {
            try
            {
                DataTable dt = new DataTable();
                var danhsachsvien = from dk in db.tbl_HP_DANGKies
                                    join diem in db.tbl_DIEM_SINHVIENs on new { ID_DANGKY = dk.ID_DANGKY } equals new { ID_DANGKY = Convert.ToInt32(diem.ID_DANGKY) } into diem_join
                                    from diem in diem_join.DefaultIfEmpty()
                                    join hp in db.tbl_LOP_HOCPHANs on new { ID_LOPHOCPHAN = Convert.ToInt32(dk.ID_LOPHOCPHAN) } equals new { ID_LOPHOCPHAN = hp.ID_LOPHOCPHAN }
                                    join mh in db.tbl_MONHOCs on new { ID_MONHOC = Convert.ToInt32(hp.ID_MONHOC) } equals new { ID_MONHOC = mh.ID_MONHOC }
                                    join hkht in db.tbl_NAMHOC_HKY_HTAIs on new { ID_NAMHOC_HKY_HTAI = Convert.ToInt32(hp.ID_NAMHOC_HKY_HTAI) } equals new { ID_NAMHOC_HKY_HTAI = hkht.ID_NAMHOC_HKY_HTAI }
                                    join sv in db.tbL_SINHVIENs on new { ID_SINHVIEN = Convert.ToInt32(dk.ID_SINHVIEN) } equals new { ID_SINHVIEN = sv.ID_SINHVIEN }
                                    join l in db.tbl_LOPHOCs on new { ID_LOPHOC = Convert.ToInt32(sv.ID_LOPHOC) } equals new { ID_LOPHOC = l.ID_LOPHOC }
                                    where
                                      dk.ID_LOPHOCPHAN == idlophocphan
                                    select new
                                    {
                                        dk.ID_DANGKY,
                                        ID_KETQUA = (int?)diem.ID_KETQUA,
                                        ID_SINHVIEN = (int?)dk.ID_SINHVIEN,
                                        ID_LOPHOCPHAN = (int?)dk.ID_LOPHOCPHAN,
                                        ID_KHOAHOC = (int?)diem.ID_KHOAHOC,
                                        sv.MA_SINHVIEN,
                                        sv.TEN_SINHVIEN,
                                        l.TEN_LOP,
                                        mh.MA_MONHOC,
                                        mh.TEN_MONHOC,
                                        mh.SO_TC,
                                        DIEM_BT = (double?)diem.DIEM_BT,
                                        DIEM_GK = (double?)diem.DIEM_GK,
                                        DIEM_CK = (double?)diem.DIEM_CK,
                                        DIEM_TONG = (double?)diem.DIEM_TONG,
                                        DIEM_CHU = diem.DIEM_CHU,
                                        DIEM_HE4 = (double?)diem.DIEM_HE4,
                                        hp.CACH_TINHDIEM
                                    };

                dt = TableUtil.LinqToDataTable(danhsachsvien);

                return dt;
            }
            catch (Exception err)
            {
                throw err;
            }
        }

        public bool InsertDiemSinhVien(DataTable dt, string pUser)
        {
            try
            {
                int count = 0;
                foreach (DataRow r in dt.Rows)
                {
                    if (r["ID_KETQUA"].ToString() == "0"|| r["ID_KETQUA"].ToString() == string.Empty)
                    {
                        #region Insert

                        try
                        {
                            tbl_DIEM_SINHVIEN diem = new tbl_DIEM_SINHVIEN();
                            diem.ID_SINHVIEN = Convert.ToInt32(r["ID_SINHVIEN"].ToString());
                            diem.ID_LOPHOCPHAN = Convert.ToInt32(r["ID_LOPHOCPHAN"].ToString());
                            diem.ID_KHOAHOC = Convert.ToInt32(r["ID_KHOAHOC"].ToString());
                            diem.ID_HOCKY = Convert.ToInt32(r["ID_HOCKY"].ToString());
                            diem.ID_DANGKY = Convert.ToInt32(r["ID_DANGKY"].ToString());
                            if (r["DIEM_BT"].ToString() == string.Empty)
                            {
                                diem.DIEM_BT = null;
                            }
                            else
                            {
                                diem.DIEM_BT = (double?)Convert.ToDouble(r["DIEM_BT"].ToString());
                            }
                            if (r["DIEM_GK"].ToString() == string.Empty)
                            {
                                diem.DIEM_GK = null;
                            }
                            else
                            {
                                diem.DIEM_GK = (double?)Convert.ToDouble(r["DIEM_GK"].ToString());
                            }
                            if (r["DIEM_CK"].ToString() == string.Empty)
                            {
                                diem.DIEM_CK = null;
                            }
                            else
                            {
                                diem.DIEM_CK = (double?)Convert.ToDouble(r["DIEM_CK"].ToString());
                            }
                            diem.DIEM_TONG = (double?)Convert.ToDouble(r["DIEM_TONG"].ToString());
                            diem.DIEM_HE4 = (double?)Convert.ToDouble(r["DIEM_HE4"].ToString());
                            diem.DIEM_CHU = r["DIEM_CHU"].ToString();
                            diem.IS_DELETE = 0;
                            diem.CREATE_TIME = DateTime.Now;
                            diem.CREATE_USER = pUser;

                            db.tbl_DIEM_SINHVIENs.InsertOnSubmit(diem);
                            db.SubmitChanges();
                            count++;
                        }
                        catch
                        {
                            db.Transaction.Rollback();
                            return false;
                        }

                        #endregion
                    }
                    else
                    {
                        try
                        {
                            tbl_DIEM_SINHVIEN diem = db.tbl_DIEM_SINHVIENs.Single(t => t.ID_KETQUA == Convert.ToInt32(r["ID_KETQUA"].ToString()));
                            diem.ID_SINHVIEN = Convert.ToInt32(r["ID_SINHVIEN"].ToString());
                            diem.ID_LOPHOCPHAN = Convert.ToInt32(r["ID_LOPHOCPHAN"].ToString());
                            diem.ID_KHOAHOC = Convert.ToInt32(r["ID_KHOAHOC"].ToString());
                            diem.ID_HOCKY = Convert.ToInt32(r["ID_HOCKY"].ToString());
                            diem.ID_DANGKY = Convert.ToInt32(r["ID_DANGKY"].ToString());
                            if (r["DIEM_BT"].ToString() == string.Empty)
                            {
                                diem.DIEM_BT = null;
                            }
                            else
                            {
                                diem.DIEM_BT = (double?)Convert.ToDouble(r["DIEM_BT"].ToString());
                            }
                            if (r["DIEM_GK"].ToString() == string.Empty)
                            {
                                diem.DIEM_GK = null;
                            }
                            else
                            {
                                diem.DIEM_GK = (double?)Convert.ToDouble(r["DIEM_GK"].ToString());
                            }
                            if (r["DIEM_CK"].ToString() == string.Empty)
                            {
                                diem.DIEM_CK = null;
                            }
                            else
                            {
                                diem.DIEM_CK = (double?)Convert.ToDouble(r["DIEM_CK"].ToString());
                            }
                            diem.DIEM_TONG = (double?)Convert.ToDouble(r["DIEM_TONG"].ToString());
                            diem.DIEM_HE4 = (double?)Convert.ToDouble(r["DIEM_HE4"].ToString());
                            diem.DIEM_CHU = r["DIEM_CHU"].ToString();
                            diem.IS_DELETE = 0;
                            diem.CREATE_TIME = DateTime.Now;
                            diem.CREATE_USER = pUser;
                            db.SubmitChanges();
                            count++;
                        }
                        catch
                        {
                            db.Transaction.Rollback();
                            return false;
                        }
                    }
                }
                if (count > 0)
                    return true;
                return false;
            }
            catch (Exception err)
            {
                db.Transaction.Rollback();
                return false;
            }
        }
    }
}

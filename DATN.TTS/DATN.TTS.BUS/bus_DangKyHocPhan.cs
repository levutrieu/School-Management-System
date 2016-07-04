using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using DATN.TTS.DATA;
using Microsoft.Win32.SafeHandles;

namespace DATN.TTS.BUS
{
    public class bus_DangKyHocPhan
    {
        db_ttsDataContext db = new db_ttsDataContext();

        public int GetIDKhoaNganh(int idkhoa, int idnganh)
        {
            try
            {
                int res = 0;
                var tblKhoahocNganh = (from kn in db.tbl_KHOAHOC_NGANHs
                                       where (kn.IS_DELETE != 1 || kn.IS_DELETE == null)
                                             && kn.ID_KHOAHOC == idkhoa && kn.ID_NGANH == idnganh
                                       select kn).FirstOrDefault();
                if (tblKhoahocNganh != null)
                {
                    var khoanganh = tblKhoahocNganh.ID_KHOAHOC_NGANH;
                    res = Convert.ToInt32(khoanganh);
                }
                return res;
            }
            catch (Exception err)
            {
                throw err;
            }
        }

        public DataTable GetLopHoc(int khoa, int nganh)
        {
            try
            {
                DataTable dt = null;
                var lop = from l in db.tbl_LOPHOCs
                          where (l.IS_DELETE != 1 || l.IS_DELETE == null) && l.ID_KHOAHOC_NGANH == GetIDKhoaNganh(khoa, nganh)
                          select new
                          {
                              l.ID_LOPHOC,
                              l.TEN_LOP
                          };
                dt = TableUtil.LinqToDataTable(lop);
                return dt;
            }
            catch (Exception err)
            {
                throw err;
            }
        }

        public DataTable GetDSLopHocWhereKhoaHoc(int idKhoahoc)
        {
            try
            {
                DataTable dt = new DataTable();
                var lophoc = from lop in db.tbl_LOPHOCs
                             join khoanganh in db.tbl_KHOAHOC_NGANHs on lop.ID_KHOAHOC_NGANH equals khoanganh.ID_KHOAHOC_NGANH
                             join khoa in db.tbl_KHOAHOCs on khoanganh.ID_KHOAHOC equals khoa.ID_KHOAHOC
                             join nganh in db.tbl_NGANHs on khoanganh.ID_NGANH equals nganh.ID_NGANH
                             where (lop.IS_DELETE != 1 || lop.IS_DELETE == null) &&
                                   (khoanganh.IS_DELETE != 1 || khoanganh.IS_DELETE == null) &&
                                   (khoa.IS_DELETE != 1 || khoa.IS_DELETE == null) &&
                                   (nganh.IS_DELETE != 1 || nganh.IS_DELETE == null) &&
                                   khoanganh.ID_KHOAHOC == idKhoahoc
                             select new
                             {
                                 lop.ID_LOPHOC,
                                 TEN_LOP = khoa.TEN_KHOAHOC + " - " + nganh.TEN_NGANH + " - " + lop.TEN_LOP + " - " + lop.NGAY_MOLOP.Year.ToString()
                             };
                dt = TableUtil.LinqToDataTable(lophoc);
                return dt;
            }
            catch (Exception err)
            {
                throw err;
            }
        }

        public DataTable GetMonHoc()
        {
            try
            {
                DataTable dt = new DataTable();
                var hpmh = (from hp in db.tbl_LOP_HOCPHANs
                            join mh in db.tbl_MONHOCs on hp.ID_MONHOC equals mh.ID_MONHOC
                            join hkht in db.tbl_NAMHOC_HKY_HTAIs on hp.ID_NAMHOC_HKY_HTAI equals hkht.ID_NAMHOC_HKY_HTAI
                            where (hp.IS_DELETE != 1 || hp.IS_DELETE == null) &&
                                  (mh.IS_DELETE != 1 || mh.IS_DELETE == null) &&
                                  hkht.IS_HIENTAI == 1
                            select new
                            {
                                hp.ID_MONHOC,
                                mh.TEN_MONHOC
                            }).Distinct();
                dt = TableUtil.LinqToDataTable(hpmh);
                return dt;
            }
            catch (Exception err)
            {
                throw err;
            }
        }

        public DataTable GetLopHPDK(int id_sinhvien)
        {
            try
            {
                DataTable dt = null;
                var hpdk = from dkhp in db.tbl_HP_DANGKies
                           join hp in db.tbl_LOP_HOCPHANs on dkhp.ID_LOPHOCPHAN equals hp.ID_LOPHOCPHAN
                           join hk in db.tbl_NAMHOC_HKY_HTAIs on hp.ID_NAMHOC_HKY_HTAI equals hk.ID_NAMHOC_HKY_HTAI
                           join nh in db.tbl_NAMHOC_HIENTAIs on hk.ID_NAMHOC_HIENTAI equals nh.ID_NAMHOC_HIENTAI
                           join mh in db.tbl_MONHOCs on new { ID_MONHOC = Convert.ToInt32(hp.ID_MONHOC) } equals
                               new { ID_MONHOC = mh.ID_MONHOC }
                           where
                               (dkhp.IS_DELETE != 1 || dkhp.IS_DELETE == null) &&
                               (hp.IS_DELETE != 1 || hp.IS_DELETE == null) &&
                               (mh.IS_DELETE != 1 || mh.IS_DELETE == null) &&
                                (hk.IS_DELETE != 1 || hk.IS_DELETE == null) &&
                                (nh.IS_DELETE != 1 || nh.IS_DELETE == null) &&
                                hk.IS_HIENTAI == 1 &&
                               dkhp.ID_SINHVIEN == id_sinhvien
                           select new
                           {
                               dkhp.ID_DANGKY,
                               dkhp.ID_SINHVIEN,
                               dkhp.ID_LOPHOCPHAN,
                               dkhp.ID_THAMSO,
                               mh.MA_MONHOC,
                               mh.TEN_MONHOC,
                               dkhp.NGAY_DANGKY,
                               dkhp.GIO_DANGKY,
                               mh.SO_TC,
                               dkhp.DON_GIA,
                               dkhp.THANH_TIEN
                           };
                dt = TableUtil.LinqToDataTable(hpdk);
                dt.Columns.Add("TRANGTHAI");
                foreach (DataRow r in dt.Rows)
                {
                    r["TRANGTHAI"] = "Đã lưu vào cơ sở dữ liệu";
                }
                return dt;
            }
            catch (Exception err)
            {
                throw err;
            }
        }

        public bool Insert_HocPhanDK(DataTable iDataSoure, string pUser)
        {
            try
            {
                int count = 0;
                foreach (DataRow r in iDataSoure.Rows)
                {
                    if (r["ID_DANGKY"].ToString().Equals("0"))
                    {
                        tbl_HP_DANGKY dkhp = new tbl_HP_DANGKY();
                        dkhp.ID_THAMSO = 1;
                        dkhp.ID_SINHVIEN = Convert.ToInt32(r["ID_SINHVIEN"].ToString());
                        dkhp.ID_LOPHOCPHAN = Convert.ToInt32(r["ID_LOPHOCPHAN"].ToString());
                        dkhp.NGAY_DANGKY = DateTime.Now;
                        dkhp.GIO_DANGKY = DateTime.Now.ToString("HH:mm:ss");
                        dkhp.DON_GIA = Convert.ToDouble(r["DON_GIA"].ToString());
                        dkhp.THANH_TIEN = Convert.ToDouble(r["THANH_TIEN"].ToString());
                        dkhp.CREATE_USER = pUser;
                        dkhp.SO_TC = Convert.ToInt32(r["SO_TC"].ToString());
                        dkhp.CREATE_TIME = DateTime.Now;
                        dkhp.IS_DELETE = 0;

                        db.tbl_HP_DANGKies.InsertOnSubmit(dkhp);
                        db.SubmitChanges();
                        count++;
                    }

                }

                if (count > 0)
                {
                    return true;
                }
                return false;
            }
            catch (Exception err)
            {
                throw err;
            }
        }

        public int GetIDSinhVien(string pMASINHVIEN)
        {
            try
            {
                int id = 0;
                var tbLSinhvien = (from idsv in db.tbL_SINHVIENs
                                   where (idsv.IS_DELETE != 1 || idsv.IS_DELETE == null) && idsv.MA_SINHVIEN == pMASINHVIEN
                                   select idsv).FirstOrDefault();
                if (tbLSinhvien != null)
                {
                    var idsinnhvien = tbLSinhvien.ID_SINHVIEN;
                    id = Convert.ToInt32(idsinnhvien);
                }
                return id;
            }
            catch (Exception err)
            {
                throw err;
            }
        }

        public int GetNamHocHienTai()
        {
            try
            {
                int res = 0;
                var tblNamhocHientai = (from nh in db.tbl_NAMHOC_HIENTAIs where (nh.IS_DELETE != 1 || nh.IS_DELETE == null) && nh.IS_HIENTAI == 1 select nh).FirstOrDefault();
                if (tblNamhocHientai != null)
                {
                    var NamHocHT = tblNamhocHientai.ID_NAMHOC_HIENTAI;

                    res = Convert.ToInt32(NamHocHT);
                }

                return res;
            }
            catch (Exception err)
            {
                throw err;
            }
        }

        public int GetHocKyHienTai()
        {
            try
            {
                int res = 0;
                var tblNamhocHkyHtai = (from hkht in db.tbl_NAMHOC_HKY_HTAIs
                                        join namhoc in db.tbl_NAMHOC_HIENTAIs on hkht.ID_NAMHOC_HIENTAI equals namhoc.ID_NAMHOC_HIENTAI
                                        where (hkht.IS_DELETE != 1 || hkht.IS_DELETE == null) && hkht.IS_HIENTAI == 1 && (namhoc.IS_DELETE != 1 || namhoc.IS_DELETE == null) && namhoc.IS_HIENTAI == 1
                                        select hkht).FirstOrDefault();
                if (tblNamhocHkyHtai != null)
                {
                    var hockyHT = tblNamhocHkyHtai.ID_NAMHOC_HKY_HTAI;
                    res = Convert.ToInt32(hockyHT);
                }
                return res;
            }
            catch (Exception err)
            {
                throw err;
            }
        }

        public bool Delete_HocPhanDK(int Id_DangKy, string pUser)
        {
            try
            {
                bool res;

                tbl_HP_DANGKY dkhp = db.tbl_HP_DANGKies.Single(t => t.ID_DANGKY == Id_DangKy);
                dkhp.IS_DELETE = 1;
                dkhp.UPDATE_TIME = DateTime.Now;
                dkhp.CREATE_USER = pUser;
                db.SubmitChanges();
                if (dkhp.ID_DANGKY != 0)
                {
                    res = true;
                }
                else
                {
                    res = false;
                }
                return res;
            }
            catch (Exception err)
            {
                throw err;
            }
        }

        public bool Insert_DangKyHuy(DataTable iDataSoure, string pUser)
        {
            try
            {
                int count = 0;
                if (iDataSoure.Rows.Count > 0)
                {
                    foreach (DataRow r in iDataSoure.Rows)
                    {
                        if (Convert.ToInt32(r["ID_DANGKY"].ToString()) > 0)
                        {
                            try
                            {
                                tbl_HP_DANGKY_HUY huydk = new tbl_HP_DANGKY_HUY();
                                huydk.ID_LOPHOCPHAN = Convert.ToInt32(r["ID_LOPHOCPHAN"].ToString());
                                huydk.ID_SINHVIEN = Convert.ToInt32(r["ID_SINHVIEN"].ToString());
                                huydk.ID_THAMSO = Convert.ToInt32(r["ID_THAMSO"].ToString());
                                huydk.NGAY_HUY = DateTime.Now;
                                huydk.GIO_HUY = DateTime.Now.ToString("HH:mm:ss");
                                huydk.DON_GIA = Convert.ToInt32(r["DON_GIA"].ToString());
                                huydk.THANH_TIEN = Convert.ToInt32(r["THANH_TIEN"].ToString());
                                huydk.IS_DELETE = 0;
                                huydk.CREATE_USER = pUser;
                                huydk.CREATE_TIME = DateTime.Now;

                                db.tbl_HP_DANGKY_HUYs.InsertOnSubmit(huydk);
                                db.SubmitChanges();
                                count++;
                            }
                            catch (Exception e)
                            {
                                throw e;
                            }
                            bool res = Delete_HocPhanDK(Convert.ToInt32(r["ID_DANGKY"].ToString()), pUser);
                        }
                    }
                }
                if (count > 0)
                    return true;
                return false;
            }
            catch (Exception err)
            {
                throw err;
            }
        }

        bool CheckTrungDangKy(int ID_SINHVIEN, int ID_LOPHOCPHAN)
        {
            try
            {
                var hpdk = from temp in db.tbl_HP_DANGKies
                           where
                               (temp.IS_DELETE != 1 || temp.IS_DELETE == null) && temp.ID_SINHVIEN == ID_SINHVIEN &&
                               temp.ID_LOPHOCPHAN == ID_LOPHOCPHAN
                           select temp;
                DataTable dt = TableUtil.LinqToDataTable(hpdk);
                if (dt.Rows.Count > 0)
                    return false;
                return true;
            }
            catch (Exception err)
            {
                throw err;
            }
        }

        public int GetSSDK(int ID_LOPHOCPHAN)
        {
            return db.SoSVDaDangKy(ID_LOPHOCPHAN).GetValueOrDefault();
        }

        public DataTable GetLopHP()
        {
            try
            {
                DataTable dt = null;
                var hpdky = from hp in db.tbl_LOP_HOCPHANs
                            join mh in db.tbl_MONHOCs on new { ID_MONHOC = Convert.ToInt32(hp.ID_MONHOC) } equals
                                new { ID_MONHOC = mh.ID_MONHOC }
                            join thu in db.tbl_LOP_HOCPHAN_CTs on hp.ID_LOPHOCPHAN equals thu.ID_LOPHOCPHAN
                            where
                                (hp.IS_DELETE != 1 ||
                                 hp.IS_DELETE == null) &&
                                (from hkht in db.tbl_NAMHOC_HKY_HTAIs
                                 where hkht.IS_HIENTAI == 1
                                 select new
                                 {
                                     hkht.ID_NAMHOC_HKY_HTAI
                                 }).Contains(new { ID_NAMHOC_HKY_HTAI = (System.Int32)hp.ID_NAMHOC_HKY_HTAI }) &&
                                (thu.IS_DELETE != 1 || thu.IS_DELETE == null)
                            select new
                            {
                                hp.ID_LOPHOCPHAN,
                                hp.ID_KHOAHOC_NGANH_CTIET,
                                hp.ID_NAMHOC_HKY_HTAI,
                                hp.ID_HEDAOTAO,
                                ID_MONHOC = (int?)hp.ID_MONHOC,
                                ISBATBUOC =
                                    ((from n in db.tbl_MONHOCs
                                      where
                                          n.ID_MONHOC == hp.ID_MONHOC
                                      select new
                                      {
                                          n.ISBATBUOC
                                      }).First().ISBATBUOC),
                                hp.ID_LOPHOC,
                                hp.ID_GIANGVIEN,
                                hp.MA_LOP_HOCPHAN,
                                hp.TEN_LOP_HOCPHAN,
                                mh.MA_MONHOC,
                                mh.TEN_MONHOC,
                                mh.SO_TC,
                                mh.IS_LYTHUYET,
                                hp.SOLUONG,
                                hp.TUAN_BD,
                                hp.TUAN_KT,
                                SOSVDKY = db.SoSVDaDangKy(hp.ID_LOPHOCPHAN),
                                TEN_GIANGVIEN = (from gv in db.tbl_GIANGVIENs
                                                 where gv.ID_GIANGVIEN == hp.ID_GIANGVIEN && (gv.IS_DELETE != 1 || gv.IS_DELETE == null)
                                                 select new { gv.TEN_GIANGVIEN }).FirstOrDefault().TEN_GIANGVIEN.ToString(),
                                thu.THU,
                                thu.TIET_BD,
                                thu.TIET_KT,
                                thu.SO_TIET,
                                TEN_PHONG =
                                    (from p in db.tbl_PHONGHOCs
                                     where p.ID_PHONG == thu.ID_PHONG && (p.IS_DELETE != 1 || p.IS_DELETE == null)
                                     select p).FirstOrDefault().TEN_PHONG.ToString()
                            };
                dt = TableUtil.LinqToDataTable(hpdky);
                return dt;
            }
            catch (Exception err)
            {
                throw err;
            }
        }

        DataTable GetLopHPWhereHDT(int ID_HEDAOTAO)
        {
            DataTable dt = null;
            var hpdky = from hp in db.tbl_LOP_HOCPHANs
                        join mh in db.tbl_MONHOCs on new { ID_MONHOC = Convert.ToInt32(hp.ID_MONHOC) } equals new { ID_MONHOC = mh.ID_MONHOC }
                        join thu in db.tbl_LOP_HOCPHAN_CTs on hp.ID_LOPHOCPHAN equals thu.ID_LOPHOCPHAN
                        where
                            (hp.IS_DELETE != 1 ||
                             hp.IS_DELETE == null) &&
                            (from hkht in db.tbl_NAMHOC_HKY_HTAIs
                             where hkht.IS_HIENTAI == 1
                             select new
                             {
                                 hkht.ID_NAMHOC_HKY_HTAI
                             }).Contains(new { ID_NAMHOC_HKY_HTAI = (System.Int32)hp.ID_NAMHOC_HKY_HTAI }) &&
                                (thu.IS_DELETE != 1 || thu.IS_DELETE == null)
                                &&
                                hp.ID_HEDAOTAO == ID_HEDAOTAO
                        #region Select
                        select new
                                        {
                                            hp.ID_LOPHOCPHAN,
                                            hp.ID_KHOAHOC_NGANH_CTIET,
                                            hp.ID_NAMHOC_HKY_HTAI,
                                            hp.ID_HEDAOTAO,
                                            ID_MONHOC = (int?)hp.ID_MONHOC,
                                            hp.ID_LOPHOC,
                                            hp.ID_GIANGVIEN,
                                            hp.MA_LOP_HOCPHAN,
                                            hp.TEN_LOP_HOCPHAN,
                                            mh.MA_MONHOC,
                                            mh.TEN_MONHOC,
                                            mh.SO_TC,
                                            mh.IS_LYTHUYET,
                                            ISBATBUOC =
                                           ((from n in db.tbl_MONHOCs
                                             where
                                                 n.ID_MONHOC == hp.ID_MONHOC
                                             select new
                                             {
                                                 n.ISBATBUOC
                                             }).First().ISBATBUOC),
                                            hp.SOLUONG,
                                            hp.TUAN_BD,
                                            hp.TUAN_KT,
                                            SOSVDKY = db.SoSVDaDangKy(hp.ID_LOPHOCPHAN),
                                            TEN_GIANGVIEN = (from gv in db.tbl_GIANGVIENs where gv.ID_GIANGVIEN == hp.ID_GIANGVIEN && (gv.IS_DELETE != 1 || gv.IS_DELETE == null) select new { gv.TEN_GIANGVIEN }).FirstOrDefault().TEN_GIANGVIEN.ToString(),
                                            thu.THU,
                                            thu.TIET_BD,
                                            thu.TIET_KT,
                                            thu.SO_TIET,
                                            TEN_PHONG = (from p in db.tbl_PHONGHOCs where p.ID_PHONG == thu.ID_PHONG && (p.IS_DELETE != 1 || p.IS_DELETE == null) select p).FirstOrDefault().TEN_PHONG.ToString()
                                        };
                        #endregion
            dt = TableUtil.LinqToDataTable(hpdky);
            return dt;
        }

        DataTable GetLopHPWhereThu(int zthu)
        {
            DataTable dt = null;
            var hpdky = from hp in db.tbl_LOP_HOCPHANs
                        join mh in db.tbl_MONHOCs on new { ID_MONHOC = Convert.ToInt32(hp.ID_MONHOC) } equals new { ID_MONHOC = mh.ID_MONHOC }
                        join thu in db.tbl_LOP_HOCPHAN_CTs on hp.ID_LOPHOCPHAN equals thu.ID_LOPHOCPHAN
                        where
                            (hp.IS_DELETE != 1 ||
                             hp.IS_DELETE == null) &&
                            (from hkht in db.tbl_NAMHOC_HKY_HTAIs
                             where hkht.IS_HIENTAI == 1
                             select new
                             {
                                 hkht.ID_NAMHOC_HKY_HTAI
                             }).Contains(new { ID_NAMHOC_HKY_HTAI = (System.Int32)hp.ID_NAMHOC_HKY_HTAI }) &&
                                (thu.IS_DELETE != 1 || thu.IS_DELETE == null)
                                && thu.THU == zthu
                        #region Select
                        select new
                        {
                            hp.ID_LOPHOCPHAN,
                            hp.ID_KHOAHOC_NGANH_CTIET,
                            hp.ID_NAMHOC_HKY_HTAI,
                            hp.ID_HEDAOTAO,
                            ID_MONHOC = (int?)hp.ID_MONHOC,
                            hp.ID_LOPHOC,
                            hp.ID_GIANGVIEN,
                            hp.MA_LOP_HOCPHAN,
                            hp.TEN_LOP_HOCPHAN,
                            mh.MA_MONHOC,
                            mh.TEN_MONHOC,
                            mh.SO_TC,
                            mh.IS_LYTHUYET,
                            ISBATBUOC =
                           ((from n in db.tbl_MONHOCs
                             where
                                 n.ID_MONHOC == hp.ID_MONHOC
                             select new
                             {
                                 n.ISBATBUOC
                             }).First().ISBATBUOC),
                            hp.SOLUONG,
                            hp.TUAN_BD,
                            hp.TUAN_KT,
                            SOSVDKY = db.SoSVDaDangKy(hp.ID_LOPHOCPHAN),
                            TEN_GIANGVIEN = (from gv in db.tbl_GIANGVIENs where gv.ID_GIANGVIEN == hp.ID_GIANGVIEN && (gv.IS_DELETE != 1 || gv.IS_DELETE == null) select new { gv.TEN_GIANGVIEN }).FirstOrDefault().TEN_GIANGVIEN.ToString(),
                            thu.THU,
                            thu.TIET_BD,
                            thu.TIET_KT,
                            thu.SO_TIET,
                            TEN_PHONG = (from p in db.tbl_PHONGHOCs where p.ID_PHONG == thu.ID_PHONG && (p.IS_DELETE != 1 || p.IS_DELETE == null) select p).FirstOrDefault().TEN_PHONG.ToString()
                        };
                        #endregion
            dt = TableUtil.LinqToDataTable(hpdky);
            return dt;
        }

        DataTable GetLopHPWhereMonHoc(int idmonhoc)
        {
            DataTable dt = null;
            var hpdky = from hp in db.tbl_LOP_HOCPHANs
                        join mh in db.tbl_MONHOCs on new { ID_MONHOC = Convert.ToInt32(hp.ID_MONHOC) } equals new { ID_MONHOC = mh.ID_MONHOC }
                        join thu in db.tbl_LOP_HOCPHAN_CTs on hp.ID_LOPHOCPHAN equals thu.ID_LOPHOCPHAN
                        where
                            (hp.IS_DELETE != 1 ||
                             hp.IS_DELETE == null) &&
                            (from hkht in db.tbl_NAMHOC_HKY_HTAIs
                             where hkht.IS_HIENTAI == 1
                             select new
                             {
                                 hkht.ID_NAMHOC_HKY_HTAI
                             }).Contains(new { ID_NAMHOC_HKY_HTAI = (System.Int32)hp.ID_NAMHOC_HKY_HTAI }) &&
                                (thu.IS_DELETE != 1 || thu.IS_DELETE == null)
                                && hp.ID_MONHOC == idmonhoc
                        #region Select
                        select new
                        {
                            hp.ID_LOPHOCPHAN,
                            hp.ID_KHOAHOC_NGANH_CTIET,
                            hp.ID_NAMHOC_HKY_HTAI,
                            hp.ID_HEDAOTAO,
                            ID_MONHOC = (int?)hp.ID_MONHOC,
                            hp.ID_LOPHOC,
                            hp.ID_GIANGVIEN,
                            hp.MA_LOP_HOCPHAN,
                            hp.TEN_LOP_HOCPHAN,
                            mh.MA_MONHOC,
                            mh.TEN_MONHOC,
                            mh.SO_TC,
                            mh.IS_LYTHUYET,
                            ISBATBUOC =
                           ((from n in db.tbl_MONHOCs
                             where
                                 n.ID_MONHOC == hp.ID_MONHOC
                             select new
                             {
                                 n.ISBATBUOC
                             }).First().ISBATBUOC),
                            hp.SOLUONG,
                            hp.TUAN_BD,
                            hp.TUAN_KT,
                            SOSVDKY = db.SoSVDaDangKy(hp.ID_LOPHOCPHAN),
                            TEN_GIANGVIEN = (from gv in db.tbl_GIANGVIENs where gv.ID_GIANGVIEN == hp.ID_GIANGVIEN && (gv.IS_DELETE != 1 || gv.IS_DELETE == null) select new { gv.TEN_GIANGVIEN }).FirstOrDefault().TEN_GIANGVIEN.ToString(),
                            thu.THU,
                            thu.TIET_BD,
                            thu.TIET_KT,
                            thu.SO_TIET,
                            TEN_PHONG = (from p in db.tbl_PHONGHOCs where p.ID_PHONG == thu.ID_PHONG && (p.IS_DELETE != 1 || p.IS_DELETE == null) select p).FirstOrDefault().TEN_PHONG.ToString()
                        };
                        #endregion
            dt = TableUtil.LinqToDataTable(hpdky);
            return dt;
        }

        DataTable GetLopHPWherMonHoc_Thu(int idmonhoc, int zthu)
        {
            DataTable dt = null;
            var hpdky = from hp in db.tbl_LOP_HOCPHANs
                        join mh in db.tbl_MONHOCs on new { ID_MONHOC = Convert.ToInt32(hp.ID_MONHOC) } equals new { ID_MONHOC = mh.ID_MONHOC }
                        join thu in db.tbl_LOP_HOCPHAN_CTs on hp.ID_LOPHOCPHAN equals thu.ID_LOPHOCPHAN
                        where
                            (hp.IS_DELETE != 1 ||
                             hp.IS_DELETE == null) &&
                            (from hkht in db.tbl_NAMHOC_HKY_HTAIs
                             where hkht.IS_HIENTAI == 1
                             select new
                             {
                                 hkht.ID_NAMHOC_HKY_HTAI
                             }).Contains(new { ID_NAMHOC_HKY_HTAI = (System.Int32)hp.ID_NAMHOC_HKY_HTAI }) &&
                                (thu.IS_DELETE != 1 || thu.IS_DELETE == null)
                                && hp.ID_MONHOC == idmonhoc && thu.THU == zthu
                        #region Select
                        select new
                        {
                            hp.ID_LOPHOCPHAN,
                            hp.ID_KHOAHOC_NGANH_CTIET,
                            hp.ID_NAMHOC_HKY_HTAI,
                            hp.ID_HEDAOTAO,
                            ID_MONHOC = (int?)hp.ID_MONHOC,
                            hp.ID_LOPHOC,
                            hp.ID_GIANGVIEN,
                            hp.MA_LOP_HOCPHAN,
                            hp.TEN_LOP_HOCPHAN,
                            mh.MA_MONHOC,
                            mh.TEN_MONHOC,
                            mh.SO_TC,
                            mh.IS_LYTHUYET,
                            ISBATBUOC =
                           ((from n in db.tbl_MONHOCs
                             where
                                 n.ID_MONHOC == hp.ID_MONHOC
                             select new
                             {
                                 n.ISBATBUOC
                             }).First().ISBATBUOC),
                            hp.SOLUONG,
                            hp.TUAN_BD,
                            hp.TUAN_KT,
                            SOSVDKY = db.SoSVDaDangKy(hp.ID_LOPHOCPHAN),
                            TEN_GIANGVIEN = (from gv in db.tbl_GIANGVIENs where gv.ID_GIANGVIEN == hp.ID_GIANGVIEN && (gv.IS_DELETE != 1 || gv.IS_DELETE == null) select new { gv.TEN_GIANGVIEN }).FirstOrDefault().TEN_GIANGVIEN.ToString(),
                            thu.THU,
                            thu.TIET_BD,
                            thu.TIET_KT,
                            thu.SO_TIET,
                            TEN_PHONG = (from p in db.tbl_PHONGHOCs where p.ID_PHONG == thu.ID_PHONG && (p.IS_DELETE != 1 || p.IS_DELETE == null) select p).FirstOrDefault().TEN_PHONG.ToString()
                        };
                        #endregion
            dt = TableUtil.LinqToDataTable(hpdky);
            return dt;
        }

        DataTable GetLopHPWhereHDT_Thu(int idhedaotao, int zthu)
        {
            DataTable dt = null;
            var hpdky = from hp in db.tbl_LOP_HOCPHANs
                        join mh in db.tbl_MONHOCs on new { ID_MONHOC = Convert.ToInt32(hp.ID_MONHOC) } equals new { ID_MONHOC = mh.ID_MONHOC }
                        join thu in db.tbl_LOP_HOCPHAN_CTs on hp.ID_LOPHOCPHAN equals thu.ID_LOPHOCPHAN
                        where
                            (hp.IS_DELETE != 1 ||
                             hp.IS_DELETE == null) &&
                            (from hkht in db.tbl_NAMHOC_HKY_HTAIs
                             where hkht.IS_HIENTAI == 1
                             select new
                             {
                                 hkht.ID_NAMHOC_HKY_HTAI
                             }).Contains(new { ID_NAMHOC_HKY_HTAI = (System.Int32)hp.ID_NAMHOC_HKY_HTAI }) &&
                                (thu.IS_DELETE != 1 || thu.IS_DELETE == null)
                                &&
                                hp.ID_HEDAOTAO == idhedaotao && thu.THU == zthu
                        #region Select
                        select new
                        {
                            hp.ID_LOPHOCPHAN,
                            hp.ID_KHOAHOC_NGANH_CTIET,
                            hp.ID_NAMHOC_HKY_HTAI,
                            hp.ID_HEDAOTAO,
                            ID_MONHOC = (int?)hp.ID_MONHOC,
                            hp.ID_LOPHOC,
                            hp.ID_GIANGVIEN,
                            hp.MA_LOP_HOCPHAN,
                            hp.TEN_LOP_HOCPHAN,
                            mh.MA_MONHOC,
                            mh.TEN_MONHOC,
                            mh.SO_TC,
                            mh.IS_LYTHUYET,
                            ISBATBUOC =
                           ((from n in db.tbl_MONHOCs
                             where
                                 n.ID_MONHOC == hp.ID_MONHOC
                             select new
                             {
                                 n.ISBATBUOC
                             }).First().ISBATBUOC),
                            hp.SOLUONG,
                            hp.TUAN_BD,
                            hp.TUAN_KT,
                            SOSVDKY = db.SoSVDaDangKy(hp.ID_LOPHOCPHAN),
                            TEN_GIANGVIEN = (from gv in db.tbl_GIANGVIENs where gv.ID_GIANGVIEN == hp.ID_GIANGVIEN && (gv.IS_DELETE != 1 || gv.IS_DELETE == null) select new { gv.TEN_GIANGVIEN }).FirstOrDefault().TEN_GIANGVIEN.ToString(),
                            thu.THU,
                            thu.TIET_BD,
                            thu.TIET_KT,
                            thu.SO_TIET,
                            TEN_PHONG = (from p in db.tbl_PHONGHOCs where p.ID_PHONG == thu.ID_PHONG && (p.IS_DELETE != 1 || p.IS_DELETE == null) select p).FirstOrDefault().TEN_PHONG.ToString()
                        };
                        #endregion
            dt = TableUtil.LinqToDataTable(hpdky);
            return dt;
        }

        DataTable GetLopHPHDT_MonHoc(int idhedaotao, int idmonhoc)
        {
            DataTable dt = null;
            var hpdky = from hp in db.tbl_LOP_HOCPHANs
                        join mh in db.tbl_MONHOCs on new { ID_MONHOC = Convert.ToInt32(hp.ID_MONHOC) } equals new { ID_MONHOC = mh.ID_MONHOC }
                        join thu in db.tbl_LOP_HOCPHAN_CTs on hp.ID_LOPHOCPHAN equals thu.ID_LOPHOCPHAN
                        where
                            (hp.IS_DELETE != 1 ||
                             hp.IS_DELETE == null) &&
                            (from hkht in db.tbl_NAMHOC_HKY_HTAIs
                             where hkht.IS_HIENTAI == 1
                             select new
                             {
                                 hkht.ID_NAMHOC_HKY_HTAI
                             }).Contains(new { ID_NAMHOC_HKY_HTAI = (System.Int32)hp.ID_NAMHOC_HKY_HTAI }) &&
                                (thu.IS_DELETE != 1 || thu.IS_DELETE == null)
                                &&
                                hp.ID_HEDAOTAO == idhedaotao && hp.ID_MONHOC == idmonhoc
                        #region Select
                        select new
                        {
                            hp.ID_LOPHOCPHAN,
                            hp.ID_KHOAHOC_NGANH_CTIET,
                            hp.ID_NAMHOC_HKY_HTAI,
                            hp.ID_HEDAOTAO,
                            ID_MONHOC = (int?)hp.ID_MONHOC,
                            hp.ID_LOPHOC,
                            hp.ID_GIANGVIEN,
                            hp.MA_LOP_HOCPHAN,
                            hp.TEN_LOP_HOCPHAN,
                            mh.MA_MONHOC,
                            mh.TEN_MONHOC,
                            mh.SO_TC,
                            mh.IS_LYTHUYET,
                            ISBATBUOC =
                           ((from n in db.tbl_MONHOCs
                             where
                                 n.ID_MONHOC == hp.ID_MONHOC
                             select new
                             {
                                 n.ISBATBUOC
                             }).First().ISBATBUOC),
                            hp.SOLUONG,
                            hp.TUAN_BD,
                            hp.TUAN_KT,
                            SOSVDKY = db.SoSVDaDangKy(hp.ID_LOPHOCPHAN),
                            TEN_GIANGVIEN = (from gv in db.tbl_GIANGVIENs where gv.ID_GIANGVIEN == hp.ID_GIANGVIEN && (gv.IS_DELETE != 1 || gv.IS_DELETE == null) select new { gv.TEN_GIANGVIEN }).FirstOrDefault().TEN_GIANGVIEN.ToString(),
                            thu.THU,
                            thu.TIET_BD,
                            thu.TIET_KT,
                            thu.SO_TIET,
                            TEN_PHONG = (from p in db.tbl_PHONGHOCs where p.ID_PHONG == thu.ID_PHONG && (p.IS_DELETE != 1 || p.IS_DELETE == null) select p).FirstOrDefault().TEN_PHONG.ToString()
                        };
                        #endregion
            dt = TableUtil.LinqToDataTable(hpdky);
            return dt;
        }

        DataTable GetLopHPWhereMonHoc_LOP(int idmonhoc, int idlophoc)
        {
            DataTable dt = null;
            var hpdky = from hp in db.tbl_LOP_HOCPHANs
                        join mh in db.tbl_MONHOCs on new { ID_MONHOC = Convert.ToInt32(hp.ID_MONHOC) } equals new { ID_MONHOC = mh.ID_MONHOC }
                        join thu in db.tbl_LOP_HOCPHAN_CTs on hp.ID_LOPHOCPHAN equals thu.ID_LOPHOCPHAN
                        where
                            (hp.IS_DELETE != 1 ||
                             hp.IS_DELETE == null) &&
                            (from hkht in db.tbl_NAMHOC_HKY_HTAIs
                             where hkht.IS_HIENTAI == 1
                             select new
                             {
                                 hkht.ID_NAMHOC_HKY_HTAI
                             }).Contains(new { ID_NAMHOC_HKY_HTAI = (System.Int32)hp.ID_NAMHOC_HKY_HTAI }) &&
                                (thu.IS_DELETE != 1 || thu.IS_DELETE == null)
                                &&
                                hp.ID_MONHOC == idmonhoc && hp.ID_LOPHOC == idlophoc
                        #region Select
                        select new
                        {
                            hp.ID_LOPHOCPHAN,
                            hp.ID_KHOAHOC_NGANH_CTIET,
                            hp.ID_NAMHOC_HKY_HTAI,
                            hp.ID_HEDAOTAO,
                            ID_MONHOC = (int?)hp.ID_MONHOC,
                            hp.ID_LOPHOC,
                            hp.ID_GIANGVIEN,
                            hp.MA_LOP_HOCPHAN,
                            hp.TEN_LOP_HOCPHAN,
                            mh.MA_MONHOC,
                            mh.TEN_MONHOC,
                            mh.SO_TC,
                            mh.IS_LYTHUYET,
                            ISBATBUOC =
                           ((from n in db.tbl_MONHOCs
                             where
                                 n.ID_MONHOC == hp.ID_MONHOC
                             select new
                             {
                                 n.ISBATBUOC
                             }).First().ISBATBUOC),
                            hp.SOLUONG,
                            hp.TUAN_BD,
                            hp.TUAN_KT,
                            SOSVDKY = db.SoSVDaDangKy(hp.ID_LOPHOCPHAN),
                            TEN_GIANGVIEN = (from gv in db.tbl_GIANGVIENs where gv.ID_GIANGVIEN == hp.ID_GIANGVIEN && (gv.IS_DELETE != 1 || gv.IS_DELETE == null) select new { gv.TEN_GIANGVIEN }).FirstOrDefault().TEN_GIANGVIEN.ToString(),
                            thu.THU,
                            thu.TIET_BD,
                            thu.TIET_KT,
                            thu.SO_TIET,
                            TEN_PHONG = (from p in db.tbl_PHONGHOCs where p.ID_PHONG == thu.ID_PHONG && (p.IS_DELETE != 1 || p.IS_DELETE == null) select p).FirstOrDefault().TEN_PHONG.ToString()
                        };
                        #endregion
            dt = TableUtil.LinqToDataTable(hpdky);
            return dt;
        }

        DataTable GetLopHocHPWhereThu_Lop(int zthu, int idlophoc)
        {
            try
            {
                DataTable dt = null;
                var hpdky = from hp in db.tbl_LOP_HOCPHANs
                            join mh in db.tbl_MONHOCs on new { ID_MONHOC = Convert.ToInt32(hp.ID_MONHOC) } equals new { ID_MONHOC = mh.ID_MONHOC }
                            join thu in db.tbl_LOP_HOCPHAN_CTs on hp.ID_LOPHOCPHAN equals thu.ID_LOPHOCPHAN
                            where
                                (hp.IS_DELETE != 1 ||
                                 hp.IS_DELETE == null) &&
                                (from hkht in db.tbl_NAMHOC_HKY_HTAIs
                                 where hkht.IS_HIENTAI == 1
                                 select new
                                 {
                                     hkht.ID_NAMHOC_HKY_HTAI
                                 }).Contains(new { ID_NAMHOC_HKY_HTAI = (System.Int32)hp.ID_NAMHOC_HKY_HTAI }) &&
                                    (thu.IS_DELETE != 1 || thu.IS_DELETE == null)
                                    &&
                                    thu.THU == zthu && hp.ID_LOPHOC == idlophoc
                            #region Select
                            select new
                            {
                                hp.ID_LOPHOCPHAN,
                                hp.ID_KHOAHOC_NGANH_CTIET,
                                hp.ID_NAMHOC_HKY_HTAI,
                                hp.ID_HEDAOTAO,
                                ID_MONHOC = (int?)hp.ID_MONHOC,
                                hp.ID_LOPHOC,
                                hp.ID_GIANGVIEN,
                                hp.MA_LOP_HOCPHAN,
                                hp.TEN_LOP_HOCPHAN,
                                mh.MA_MONHOC,
                                mh.TEN_MONHOC,
                                mh.SO_TC,
                                mh.IS_LYTHUYET,
                                ISBATBUOC =
                               ((from n in db.tbl_MONHOCs
                                 where
                                     n.ID_MONHOC == hp.ID_MONHOC
                                 select new
                                 {
                                     n.ISBATBUOC
                                 }).First().ISBATBUOC),
                                hp.SOLUONG,
                                hp.TUAN_BD,
                                hp.TUAN_KT,
                                SOSVDKY = db.SoSVDaDangKy(hp.ID_LOPHOCPHAN),
                                TEN_GIANGVIEN = (from gv in db.tbl_GIANGVIENs where gv.ID_GIANGVIEN == hp.ID_GIANGVIEN && (gv.IS_DELETE != 1 || gv.IS_DELETE == null) select new { gv.TEN_GIANGVIEN }).FirstOrDefault().TEN_GIANGVIEN.ToString(),
                                thu.THU,
                                thu.TIET_BD,
                                thu.TIET_KT,
                                thu.SO_TIET,
                                TEN_PHONG = (from p in db.tbl_PHONGHOCs where p.ID_PHONG == thu.ID_PHONG && (p.IS_DELETE != 1 || p.IS_DELETE == null) select p).FirstOrDefault().TEN_PHONG.ToString()
                            };
                            #endregion
                dt = TableUtil.LinqToDataTable(hpdky);
                return dt;
            }
            catch (Exception err)
            {
                throw err;
            }
        }

        DataTable GetLopHPWhereLop_Monhoc_Thu(int idlophoc, int idmonhoc, int zthu)
        {
            DataTable dt = null;
            var hpdky = from hp in db.tbl_LOP_HOCPHANs
                        join mh in db.tbl_MONHOCs on new { ID_MONHOC = Convert.ToInt32(hp.ID_MONHOC) } equals new { ID_MONHOC = mh.ID_MONHOC }
                        join thu in db.tbl_LOP_HOCPHAN_CTs on hp.ID_LOPHOCPHAN equals thu.ID_LOPHOCPHAN
                        where
                            (hp.IS_DELETE != 1 ||
                                hp.IS_DELETE == null) &&
                            (from hkht in db.tbl_NAMHOC_HKY_HTAIs
                             where hkht.IS_HIENTAI == 1
                             select new
                             {
                                 hkht.ID_NAMHOC_HKY_HTAI
                             }).Contains(new { ID_NAMHOC_HKY_HTAI = (System.Int32)hp.ID_NAMHOC_HKY_HTAI }) &&
                                (thu.IS_DELETE != 1 || thu.IS_DELETE == null)
                                &&
                                thu.THU == zthu && hp.ID_LOPHOC == idlophoc && hp.ID_MONHOC == idmonhoc
                        #region Select
                        select new
                        {
                            hp.ID_LOPHOCPHAN,
                            hp.ID_KHOAHOC_NGANH_CTIET,
                            hp.ID_NAMHOC_HKY_HTAI,
                            hp.ID_HEDAOTAO,
                            ID_MONHOC = (int?)hp.ID_MONHOC,
                            hp.ID_LOPHOC,
                            hp.ID_GIANGVIEN,
                            hp.MA_LOP_HOCPHAN,
                            hp.TEN_LOP_HOCPHAN,
                            mh.MA_MONHOC,
                            mh.TEN_MONHOC,
                            mh.SO_TC,
                            mh.IS_LYTHUYET,
                            ISBATBUOC =
                           ((from n in db.tbl_MONHOCs
                             where
                                 n.ID_MONHOC == hp.ID_MONHOC
                             select new
                             {
                                 n.ISBATBUOC
                             }).First().ISBATBUOC),
                            hp.SOLUONG,
                            hp.TUAN_BD,
                            hp.TUAN_KT,
                            SOSVDKY = db.SoSVDaDangKy(hp.ID_LOPHOCPHAN),
                            TEN_GIANGVIEN = (from gv in db.tbl_GIANGVIENs where gv.ID_GIANGVIEN == hp.ID_GIANGVIEN && (gv.IS_DELETE != 1 || gv.IS_DELETE == null) select new { gv.TEN_GIANGVIEN }).FirstOrDefault().TEN_GIANGVIEN.ToString(),
                            thu.THU,
                            thu.TIET_BD,
                            thu.TIET_KT,
                            thu.SO_TIET,
                            TEN_PHONG = (from p in db.tbl_PHONGHOCs where p.ID_PHONG == thu.ID_PHONG && (p.IS_DELETE != 1 || p.IS_DELETE == null) select p).FirstOrDefault().TEN_PHONG.ToString()
                        };
                        #endregion
            dt = TableUtil.LinqToDataTable(hpdky);
            return dt;
        }

        DataTable GetLopHPWhereLop(int idlophoc)
        {
            DataTable dt = null;
            var hpdky = from hp in db.tbl_LOP_HOCPHANs
                        join mh in db.tbl_MONHOCs on new { ID_MONHOC = Convert.ToInt32(hp.ID_MONHOC) } equals new { ID_MONHOC = mh.ID_MONHOC }
                        join thu in db.tbl_LOP_HOCPHAN_CTs on hp.ID_LOPHOCPHAN equals thu.ID_LOPHOCPHAN
                        where
                            (hp.IS_DELETE != 1 ||
                                hp.IS_DELETE == null) &&
                            (from hkht in db.tbl_NAMHOC_HKY_HTAIs
                             where hkht.IS_HIENTAI == 1
                             select new
                             {
                                 hkht.ID_NAMHOC_HKY_HTAI
                             }).Contains(new { ID_NAMHOC_HKY_HTAI = (System.Int32)hp.ID_NAMHOC_HKY_HTAI }) &&
                                (thu.IS_DELETE != 1 || thu.IS_DELETE == null)
                                && hp.ID_LOPHOC == idlophoc
                        #region Select
                        select new
                        {
                            hp.ID_LOPHOCPHAN,
                            hp.ID_KHOAHOC_NGANH_CTIET,
                            hp.ID_NAMHOC_HKY_HTAI,
                            hp.ID_HEDAOTAO,
                            ID_MONHOC = (int?)hp.ID_MONHOC,
                            hp.ID_LOPHOC,
                            hp.ID_GIANGVIEN,
                            hp.MA_LOP_HOCPHAN,
                            hp.TEN_LOP_HOCPHAN,
                            mh.MA_MONHOC,
                            mh.TEN_MONHOC,
                            mh.SO_TC,
                            mh.IS_LYTHUYET,
                            ISBATBUOC =
                           ((from n in db.tbl_MONHOCs
                             where
                                 n.ID_MONHOC == hp.ID_MONHOC
                             select new
                             {
                                 n.ISBATBUOC
                             }).First().ISBATBUOC),
                            hp.SOLUONG,
                            hp.TUAN_BD,
                            hp.TUAN_KT,
                            SOSVDKY = db.SoSVDaDangKy(hp.ID_LOPHOCPHAN),
                            TEN_GIANGVIEN = (from gv in db.tbl_GIANGVIENs where gv.ID_GIANGVIEN == hp.ID_GIANGVIEN && (gv.IS_DELETE != 1 || gv.IS_DELETE == null) select new { gv.TEN_GIANGVIEN }).FirstOrDefault().TEN_GIANGVIEN.ToString(),
                            thu.THU,
                            thu.TIET_BD,
                            thu.TIET_KT,
                            thu.SO_TIET,
                            TEN_PHONG = (from p in db.tbl_PHONGHOCs where p.ID_PHONG == thu.ID_PHONG && (p.IS_DELETE != 1 || p.IS_DELETE == null) select p).FirstOrDefault().TEN_PHONG.ToString()
                        };
                        #endregion
            dt = TableUtil.LinqToDataTable(hpdky);
            return dt;
        }

        public DataTable GetLopHocPhan(int idhedaotao, int idmonhoc, int zthu, int idlophoc)
        {
            try
            {
                DataTable dt = new DataTable();
                if (idhedaotao == 0 && idmonhoc == 0 && zthu == 0 && idlophoc == 0)
                {
                    dt = GetLopHP();
                }
                if (idmonhoc == 0 && zthu == 0 && idlophoc == 0)
                {
                    dt = GetLopHPWhereHDT(idhedaotao);
                }
                if (idmonhoc != 0 && zthu == 0 && idlophoc == 0)
                {
                    dt = GetLopHPWhereMonHoc(idmonhoc);
                }
                if (idmonhoc == 0 && zthu != 0 && idlophoc == 0)
                {
                    dt = GetLopHPWhereThu(zthu);
                }
                if (idmonhoc != 0 && zthu != 0 && idlophoc == 0)
                {
                    dt = GetLopHPWherMonHoc_Thu(idmonhoc, zthu);
                }
                if (idmonhoc == 0 && zthu != 0 && idlophoc == 0)
                {
                    dt = GetLopHPWhereHDT_Thu(idhedaotao, zthu);
                }
                if (idmonhoc != 0 && zthu == 0 && idlophoc == 0)
                {
                    dt = GetLopHPHDT_MonHoc(idhedaotao, idmonhoc);
                }
                if (idmonhoc != 0 && idlophoc != 0 && zthu == 0)
                {
                    dt = GetLopHPWhereMonHoc_LOP(idmonhoc, idlophoc);
                }
                if (zthu != 0 && idlophoc != 0 && idmonhoc == 0)
                {
                    dt = GetLopHocHPWhereThu_Lop(zthu, idlophoc);
                }

                if (idlophoc != 0 && idmonhoc != 0 && zthu != 0)
                {
                    dt = GetLopHPWhereLop_Monhoc_Thu(idlophoc, idmonhoc, zthu);
                }

                if (idlophoc != 0 && idmonhoc == 0 && zthu == 0)
                {
                    dt = GetLopHPWhereLop(idlophoc);
                }
                return dt;
            }
            catch (Exception err)
            {
                throw err;
            }
        }

        public DataTable GetHocKyNamHoc()
        {
            try
            {
                DataTable dt = null;
                var hockynamhoc = from hkht in db.tbl_NAMHOC_HKY_HTAIs
                                  join nhht in db.tbl_NAMHOC_HIENTAIs on
                                      new { ID_NAMHOC_HIENTAI = Convert.ToInt32(hkht.ID_NAMHOC_HIENTAI) } equals
                                      new { ID_NAMHOC_HIENTAI = nhht.ID_NAMHOC_HIENTAI }
                                  where
                                      nhht.IS_HIENTAI == 1
                                  select new
                                  {
                                      hkht.ID_NAMHOC_HKY_HTAI,
                                      TEN_HOKY_NH =
                                          ("Học kỳ " + "" + Convert.ToString(hkht.HOCKY) + " " + "Năm " +
                                           Convert.ToString(nhht.NAMHOC_TU) + " - " + Convert.ToString(nhht.NAMHOC_DEN))
                                  };
                dt = TableUtil.LinqToDataTable(hockynamhoc);
                return dt;
            }
            catch (Exception err)
            {
                throw err;
            }
        }

        public DataTable GetNgay_SoTuan()
        {
            try
            {
                DataTable dt = null;
                var NgayTuan = from nhht in db.tbl_NAMHOC_HIENTAIs
                               where
                                   (nhht.IS_DELETE != 1 ||
                                    nhht.IS_DELETE == null) &&
                                   nhht.IS_HIENTAI == 1
                               select new
                               {
                                   nhht.NGAY_BATDAU,
                                   nhht.SO_TUAN
                               };
                dt = TableUtil.LinqToDataTable(NgayTuan);

                return dt;
            }
            catch (Exception err)
            {
                throw err;
            }
        }

        public int GetHocKy(int id_hocky_htai)
        {
            try
            {
                var hocky = (from hk in db.tbl_NAMHOC_HKY_HTAIs
                             where hk.ID_NAMHOC_HKY_HTAI == id_hocky_htai
                             select new { hk.HOCKY }).First().HOCKY.ToString();
                return Convert.ToInt32(hocky.ToString());
            }
            catch (Exception err)
            {
                throw err;
            }
        }

        public DataTable GetTKB(int id_hocky_hientai, int id_sinhvien, int tuan)
        {
            try
            {
                DataTable dt = null;
                var tkb = from dkhp in db.tbl_HP_DANGKies
                          join hp in db.tbl_LOP_HOCPHANs on new { ID_LOPHOCPHAN = Convert.ToInt32(dkhp.ID_LOPHOCPHAN) } equals new { ID_LOPHOCPHAN = hp.ID_LOPHOCPHAN }
                          join hpct in db.tbl_LOP_HOCPHAN_CTs on new { ID_LOPHOCPHAN = hp.ID_LOPHOCPHAN } equals new { ID_LOPHOCPHAN = Convert.ToInt32(hpct.ID_LOPHOCPHAN) }
                          join mh in db.tbl_MONHOCs on new { ID_MONHOC = Convert.ToInt32(hp.ID_MONHOC) } equals new { ID_MONHOC = mh.ID_MONHOC }
                          join p in db.tbl_PHONGHOCs on new { ID_PHONG = Convert.ToInt32(hpct.ID_PHONG) } equals new { ID_PHONG = p.ID_PHONG }
                          join gv in db.tbl_GIANGVIENs on new { ID_GIANGVIEN = Convert.ToInt32(hp.ID_GIANGVIEN) } equals new { ID_GIANGVIEN = gv.ID_GIANGVIEN }
                          where
                            (dkhp.IS_DELETE != 1 ||
                            dkhp.IS_DELETE == null) &&
                            (hp.IS_DELETE != 1 ||
                            hp.IS_DELETE == null) &&
                            (hpct.IS_DELETE != 1 ||
                            hpct.IS_DELETE == null) &&
                            (mh.IS_DELETE != 1 ||
                            mh.IS_DELETE == null) &&
                            (p.IS_DELETE != 1 ||
                            p.IS_DELETE == null) &&
                            (gv.IS_DELETE != 1 ||
                            p.IS_DELETE == null) &&
                            dkhp.ID_SINHVIEN == id_sinhvien &&
                            hp.ID_NAMHOC_HKY_HTAI == id_hocky_hientai &&
                            tuan >= hp.TUAN_BD && tuan <= hp.TUAN_KT
                          select new
                          {
                              hp.TEN_LOP_HOCPHAN,
                              hp.ID_LOPHOCPHAN,
                              hp.SOTIET,
                              hp.MA_LOP_HOCPHAN,
                              hp.TUAN_BD,
                              hp.TUAN_KT,
                              hpct.THU,
                              hpct.TIET_BD,
                              hpct.TIET_KT,
                              p.TEN_PHONG,
                              gv.TEN_GIANGVIEN,
                              mh.TEN_MONHOC
                          };
                dt = TableUtil.LinqToDataTable(tkb);
                return dt;
            }
            catch (Exception err)
            {
                throw err;
            }
        }

        public DataTable GetThongTinSinhVien(int id_sinhvien)
        {
            try
            {
                DataTable dt = null;
                var thongtinsinhvien = from sv in db.tbL_SINHVIENs
                                       join l in db.tbl_LOPHOCs on new { ID_LOPHOC = Convert.ToInt32(sv.ID_LOPHOC) } equals
                                           new { ID_LOPHOC = l.ID_LOPHOC }
                                       join khngang in db.tbl_KHOAHOC_NGANHs on
                                           new { ID_KHOAHOC_NGANH = Convert.ToInt32(l.ID_KHOAHOC_NGANH) } equals
                                           new { ID_KHOAHOC_NGANH = khngang.ID_KHOAHOC_NGANH }
                                       join kh in db.tbl_KHOAHOCs on new { ID_KHOAHOC = Convert.ToInt32(khngang.ID_KHOAHOC) } equals
                                           new { ID_KHOAHOC = kh.ID_KHOAHOC }
                                       join ng in db.tbl_NGANHs on new { ID_NGANH = Convert.ToInt32(khngang.ID_NGANH) } equals
                                           new { ID_NGANH = ng.ID_NGANH }
                                       join hdt in db.tbl_HEDAOTAOs on new { ID_HE_DAOTAO = Convert.ToInt32(kh.ID_HE_DAOTAO) } equals
                                           new { ID_HE_DAOTAO = hdt.ID_HE_DAOTAO }
                                       where
                                           (sv.IS_DELETE != 1 ||
                                            sv.IS_DELETE == null) &&
                                           (l.IS_DELETE != 1 ||
                                            l.IS_DELETE == null) &&
                                           (khngang.IS_DELETE != 1 ||
                                            khngang.IS_DELETE == null) &&
                                           (kh.IS_DELETE != 1 ||
                                            kh.IS_DELETE == null) &&
                                           (ng.IS_DELETE != 1 ||
                                            ng.IS_DELETE == null) &&
                                           (hdt.IS_DELETE != 1 ||
                                            hdt.IS_DELETE == null) &&
                                           sv.ID_SINHVIEN == id_sinhvien
                                       select new
                                       {
                                           sv.ID_SINHVIEN,
                                           sv.MA_SINHVIEN,
                                           sv.TEN_SINHVIEN,
                                           sv.NGAYSINH,
                                           sv.NOISINH,
                                           sv.DIACHI,
                                           sv.CMND,
                                           sv.NGAYCAP,
                                           sv.NOICAP,
                                           sv.GIOITINH,
                                           sv.DIENTHOAI,
                                           sv.EMAIL,
                                           sv.THONGTIN_NGOAITRU,
                                           hdt.ID_HE_DAOTAO,
                                           l.ID_LOPHOC,
                                           l.TEN_LOP,
                                           ng.ID_NGANH,
                                           ng.TEN_NGANH,
                                           kh.TEN_KHOAHOC,
                                           hdt.TEN_HE_DAOTAO,
                                           kh.ID_KHOAHOC,
                                           KHOAHOC = kh.NAM_BD + "-" + kh.NAM_KT
                                       };
                dt = TableUtil.LinqToDataTable(thongtinsinhvien);
                return dt;
            }
            catch (Exception err)
            {
                throw err;
            }
        }

        public DataTable GetDanhSachDiem(int id_sinhvien)
        {
            try
            {
                DataTable dt = null;
                var xxxx = (
                    from hkht in db.tbl_NAMHOC_HKY_HTAIs
                    join nhht in db.tbl_NAMHOC_HIENTAIs on new { ID_NAMHOC_HIENTAI = Convert.ToInt32(hkht.ID_NAMHOC_HIENTAI) } equals new { ID_NAMHOC_HIENTAI = nhht.ID_NAMHOC_HIENTAI }
                    where
                        (hkht.IS_DELETE == null || hkht.IS_DELETE != 1) && (nhht.IS_DELETE != 1 || nhht.IS_DELETE == null) &&
                        (from hp in db.tbl_LOP_HOCPHANs
                         where (hp.IS_DELETE != 1 || hp.IS_DELETE == null) && hkht.ID_NAMHOC_HKY_HTAI == hp.ID_NAMHOC_HKY_HTAI &&
                             (from diem in db.tbl_DIEM_SINHVIENs
                              where diem.IS_DELETE != 1 || diem.IS_DELETE == null
                              select new
                              {
                                  diem.ID_LOPHOCPHAN
                              }).Contains(new { ID_LOPHOCPHAN = (System.Int32?)hp.ID_LOPHOCPHAN })
                         select new
                         {
                             hp.ID_NAMHOC_HKY_HTAI
                         }).Contains(new { ID_NAMHOC_HKY_HTAI = (System.Int32?)hkht.ID_NAMHOC_HKY_HTAI })
                    select new
                    {
                        ID = ("HK" + Convert.ToString(hkht.ID_NAMHOC_HKY_HTAI)),
                        NAME = ("Học kỳ" + " " + Convert.ToString(hkht.HOCKY) + " Năm " + Convert.ToString(nhht.NAMHOC_TU) + "-" + Convert.ToString(nhht.NAMHOC_DEN)),
                        MA_MONHOC = "",
                        SO_TC = 0,
                        DIEM_BT = (double?)0,
                        DIEM_GK = (double?)0,
                        DIEM_CK = (double?)0,
                        DIEM_TONG = (double?)0,
                        DIEM_HE4 = (double?)0,
                        DIEM_CHU = "",
                        CACH_TINHDIEM = "",
                        ID_PARENT = ""
                    }).Concat
                    (
                        from diem in db.tbl_DIEM_SINHVIENs
                        join hp in db.tbl_LOP_HOCPHANs on
                            new { ID_LOPHOCPHAN = Convert.ToInt32(diem.ID_LOPHOCPHAN) } equals new { ID_LOPHOCPHAN = hp.ID_LOPHOCPHAN }

                        join mh in db.tbl_MONHOCs on
                            new { ID_MONHOC = Convert.ToInt32(hp.ID_MONHOC) } equals new { ID_MONHOC = mh.ID_MONHOC }
                        join hkht in db.tbl_NAMHOC_HKY_HTAIs on
                            new { ID_NAMHOC_HKY_HTAI = Convert.ToInt32(hp.ID_NAMHOC_HKY_HTAI) } equals new { ID_NAMHOC_HKY_HTAI = hkht.ID_NAMHOC_HKY_HTAI }

                        join nhht in db.tbl_NAMHOC_HIENTAIs on
                        new { ID_NAMHOC_HIENTAI = Convert.ToInt32(hkht.ID_NAMHOC_HIENTAI) } equals new { ID_NAMHOC_HIENTAI = nhht.ID_NAMHOC_HIENTAI }

                        where
                            (diem.IS_DELETE != 1 || diem.IS_DELETE == null) && diem.ID_SINHVIEN == id_sinhvien
                        select new
                        {
                            ID = ("D" + Convert.ToString(diem.ID_KETQUA)),
                            NAME = mh.TEN_MONHOC,
                            MA_MONHOC = mh.MA_MONHOC,
                            SO_TC = (int)mh.SO_TC,
                            DIEM_BT = (double?)diem.DIEM_BT,
                            DIEM_GK = (double?)diem.DIEM_GK,
                            DIEM_CK = (double?)diem.DIEM_CK,
                            DIEM_TONG = (double?)diem.DIEM_TONG,
                            DIEM_HE4 = (double?)diem.DIEM_HE4,
                            DIEM_CHU = diem.DIEM_CHU,
                            CACH_TINHDIEM = mh.CACH_TINHDIEM,
                            ID_PARENT = ("HK" + Convert.ToString(hp.ID_NAMHOC_HKY_HTAI))
                        }
                    );

                dt = TableUtil.LinqToDataTable(xxxx);
                return dt;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public double GetHocPhi_LT(int id_hedaotao, int LT_Or_TH)
        {
            double dongia = 0;
            if (LT_Or_TH == 1)// hoc phi lt
            {
                var dongiatc = (from gia in db.tbl_HP_CAUHINH_HOCPHIs
                                join hknh in db.tbl_NAMHOC_HKY_HTAIs on gia.ID_NAMHOC_HKY_HTAI equals hknh.ID_NAMHOC_HKY_HTAI
                                where (gia.IS_DELETE != 1 || gia.IS_DELETE == null) && (hknh.IS_DELETE != 1 || hknh.IS_DELETE == null)
                                && hknh.IS_HIENTAI == 1
                                && gia.IS_LYTHUYET == 1
                                && gia.ID_HE_DAOTAO == id_hedaotao
                                select new
                                {
                                    gia.DON_GIA
                                }).First().DON_GIA;

                dongia = Convert.ToDouble(dongiatc);
            }
            if (LT_Or_TH == 0)// hoc phi lt
            {
                var dongiatc = (from gia in db.tbl_HP_CAUHINH_HOCPHIs
                                join hknh in db.tbl_NAMHOC_HKY_HTAIs on gia.ID_NAMHOC_HKY_HTAI equals hknh.ID_NAMHOC_HKY_HTAI
                                where (gia.IS_DELETE != 1 || gia.IS_DELETE == null) && (hknh.IS_DELETE != 1 || hknh.IS_DELETE == null)
                                && hknh.IS_HIENTAI == 1
                                && gia.IS_LYTHUYET == 0
                                && gia.ID_HE_DAOTAO == id_hedaotao
                                select new
                                {
                                    gia.DON_GIA
                                }).First().DON_GIA;

                dongia = Convert.ToDouble(dongiatc);
            }
            return dongia;
        }

    }
}

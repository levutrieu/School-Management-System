using System;
using System.CodeDom;
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

        public DataTable GetLopHoc(int idkhoahoc, int idnganh)
        {
            try
            {
                DataTable dt = null;
                var lop = (from l in db.tbl_LOPHOCs
                           join kn in db.tbl_KHOAHOC_NGANHs on new { ID_KHOAHOC_NGANH = Convert.ToInt32(l.ID_KHOAHOC_NGANH) } equals new { ID_KHOAHOC_NGANH = kn.ID_KHOAHOC_NGANH }
                           where
                             (l.IS_DELETE != 1 ||
                             l.IS_DELETE == null) &&
                             (kn.IS_DELETE != 1 ||
                             kn.IS_DELETE == null) &&
                             kn.ID_KHOAHOC == idkhoahoc &&
                             kn.ID_NGANH == idnganh
                           select new
                           {
                               l.ID_LOPHOC,
                               l.TEN_LOP
                           }).Distinct();
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
                               mh.MA_MONHOC,
                               mh.TEN_MONHOC,
                               hp.MA_LOP_HOCPHAN,
                               hp.TEN_LOP_HOCPHAN,
                               mh.SO_TC,
                               dkhp.DON_GIA,
                               dkhp.THANH_TIEN,
                               dkhp.NGAY_DANGKY,
                               dkhp.GIO_DANGKY,
                               dkhp.ID_DANGKY,
                               dkhp.ID_SINHVIEN,
                               dkhp.ID_LOPHOCPHAN,
                               dkhp.ID_THAMSO
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

        //int GetThamSo()
        //{
        //    int res = 0;
        //    var thamso = from 
        //}
        public DataTable GetALLSinhVien()
        {
            DataTable dt = null;
            var sv = from svien in db.tbL_SINHVIENs select new {svien.ID_SINHVIEN, svien.MA_SINHVIEN};
            dt = TableUtil.LinqToDataTable(sv);
            return dt;
        }

        public bool Insert_HocPhanDK_All(DataTable iDataSoure, string pUser, int pID_SINHVIEN)
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
                        dkhp.ID_SINHVIEN = pID_SINHVIEN;
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
                                   where (idsv.IS_DELETE != 1 || idsv.IS_DELETE == null) && idsv.MA_SINHVIEN.Trim() == pMASINHVIEN
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

        public int GetSSDK(int ID_LOPHOCPHAN)
        {
            return db.SoSVDaDangKy(ID_LOPHOCPHAN).GetValueOrDefault();
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

        public DataTable GetThamSoDangDotDangKy(int id_hedaotao)
        {
            try
            {
                DataTable dt = null;
                var dotdangkyhocphan = from dotDKHP in db.tbl_HP_DOTDKs
                    join hkht in db.tbl_NAMHOC_HKY_HTAIs on
                        new {ID_NAMHOC_HKY_HTAI = Convert.ToInt32(dotDKHP.ID_NAMHOC_HKY_HTAI)} equals
                        new {ID_NAMHOC_HKY_HTAI = hkht.ID_NAMHOC_HKY_HTAI}
                    join nhht in db.tbl_NAMHOC_HIENTAIs on
                        new {ID_NAMHOC_HIENTAI = Convert.ToInt32(hkht.ID_NAMHOC_HIENTAI)} equals
                        new {ID_NAMHOC_HIENTAI = nhht.ID_NAMHOC_HIENTAI}
                    where
                        (dotDKHP.ISDELETE != 1 ||
                         dotDKHP.ISDELETE == null) &&
                        (hkht.IS_DELETE != 1 ||
                         hkht.IS_DELETE == null) &&
                        hkht.IS_HIENTAI == 1 &&
                        (nhht.IS_DELETE != 1 ||
                         nhht.IS_DELETE == null) &&
                        nhht.IS_HIENTAI == 1 &&
                        Convert.ToDateTime(DateTime.Now) >= Convert.ToDateTime(dotDKHP.NGAY_BDAU) &&
                        Convert.ToDateTime(DateTime.Now) <= Convert.ToDateTime(dotDKHP.NGAY_KTHUC) &&
                        dotDKHP.ID_HE_DAOTAO == id_hedaotao
                    select new
                    {
                        dotDKHP.ID_DOTDK,
                        dotDKHP.ID_NAMHOC_HKY_HTAI,
                        dotDKHP.ID_HE_DAOTAO,
                        dotDKHP.MA_DOT_DK,
                        dotDKHP.TEN_DOT_DK,
                        dotDKHP.NGAY_BDAU,
                        dotDKHP.NGAY_KTHUC
                    };
                dt = TableUtil.LinqToDataTable(dotdangkyhocphan);
                return dt;
            }
            catch (Exception err)
            {
                throw err;
            }
        }

        public DataTable GetDanhSachLopHocPhan(int idkhoahoc, int idnganh)
        {
            DataTable dt = null;
            if (idkhoahoc == 0 && idnganh == 0)
            {
                dt = GetDanhSachLopHocPhan_3();
            }
            if (idkhoahoc == 0 && idnganh != 1)
            {
                dt = GetDanhSachLopHocPhan_2(idnganh);
            }
            if (idkhoahoc != 0 && idnganh == 0)
            {
                dt = GetDanhSachLopHocPhan_1(idkhoahoc);
            }
            if (idkhoahoc != 0 && idnganh != 0)
            {
                dt = GetDanhSachLopHocPhan_4(idkhoahoc, idnganh);
            }
            return dt;
        }

         DataTable GetDanhSachLopHocPhan_1(int idkhoahoc)
        {
            try
            {
                DataTable dt = null;
                var danhsachlophocphan = from hp in db.tbl_LOP_HOCPHANs
                    join mh in db.tbl_MONHOCs on new {ID_MONHOC = Convert.ToInt32(hp.ID_MONHOC)} equals
                        new {ID_MONHOC = mh.ID_MONHOC}
                    join knct in db.tbl_KHOAHOC_NGANH_CTIETs on
                        new {ID_KHOAHOC_NGANH_CTIET = Convert.ToInt32(hp.ID_KHOAHOC_NGANH_CTIET)} equals
                        new {ID_KHOAHOC_NGANH_CTIET = knct.ID_KHOAHOC_NGANH_CTIET}
                    join hkht in db.tbl_NAMHOC_HKY_HTAIs on
                        new {ID_NAMHOC_HKY_HTAI = Convert.ToInt32(hp.ID_NAMHOC_HKY_HTAI)} equals
                        new {ID_NAMHOC_HKY_HTAI = hkht.ID_NAMHOC_HKY_HTAI}
                    join nhht in db.tbl_NAMHOC_HIENTAIs on
                        new {ID_NAMHOC_HIENTAI = Convert.ToInt32(hkht.ID_NAMHOC_HIENTAI)} equals
                        new {ID_NAMHOC_HIENTAI = nhht.ID_NAMHOC_HIENTAI}
                    join kn in db.tbl_KHOAHOC_NGANHs on knct.ID_KHOAHOC_NGANH equals kn.ID_KHOAHOC_NGANH
                    //join l in db.tbl_LOPHOCs on hp.ID_LOPHOC  equals l.ID_LOPHOC
                    where
                        kn.ID_KHOAHOC == idkhoahoc &&
                        (hp.IS_DELETE != 1 || hp.IS_DELETE == null) &&
                        (mh.IS_DELETE != 1 || mh.IS_DELETE == null) &&
                        (knct.IS_DELETE != 1 || knct.IS_DELETE == null) &&
                        (hkht.IS_DELETE != 1 || hkht.IS_DELETE == null) &&
                        (nhht.IS_DELETE != 1 ||nhht.IS_DELETE == null) &&
                        hkht.IS_HIENTAI == 1 && nhht.IS_HIENTAI == 1 &&
                        (kn.IS_DELETE != 1 || kn.IS_DELETE == null)&&
                        //(l.IS_DELETE != 1 || l.IS_DELETE == null) &&
                        (from hpct in db.tbl_LOP_HOCPHAN_CTs
                            where hpct.IS_DELETE != 1 || hpct.IS_DELETE == null
                            select new
                            {
                                hpct.ID_LOPHOCPHAN
                            }).Contains(new {ID_LOPHOCPHAN = (System.Int32?) hp.ID_LOPHOCPHAN})
                    select new
                    {
                        hp.ID_LOPHOCPHAN,
                        ID_KHOAHOC_NGANH_CTIET = (int?) hp.ID_KHOAHOC_NGANH_CTIET,
                        ID_NAMHOC_HKY_HTAI = (int?) hp.ID_NAMHOC_HKY_HTAI,
                        hp.ID_HEDAOTAO,
                        ID_MONHOC = (int?) hp.ID_MONHOC,
                        mh.ISBATBUOC,
                        hp.ID_LOPHOC,
                        //l.TEN_LOP,
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
                        SOSVDKY = (int?) db.SoSVDaDangKy(hp.ID_LOPHOCPHAN),
                        TEN_GIANGVIEN = (from gv in db.tbl_GIANGVIENs where gv.ID_GIANGVIEN == hp.ID_GIANGVIEN && (gv.IS_DELETE != 1 || gv.IS_DELETE == null) select new { gv.TEN_GIANGVIEN }).FirstOrDefault().TEN_GIANGVIEN.ToString()
                    };
                dt = TableUtil.LinqToDataTable(danhsachlophocphan);
                dt.Columns.Add("THOIKHOABIEU");
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow r in dt.Rows)
                    {
                        int idlophocphan = Convert.ToInt32(r["ID_LOPHOCPHAN"].ToString());
                        r["THOIKHOABIEU"] = GetChiTietLopHocPhan(idlophocphan);
                    }
                }
                return dt;
            }
            catch (Exception err)
            {
                throw err;
            }
        }

         DataTable GetDanhSachLopHocPhan_2(int idnganh)
        {
            try
            {
                DataTable dt = null;
                var danhsachlophocphan = from hp in db.tbl_LOP_HOCPHANs
                                         join mh in db.tbl_MONHOCs on new { ID_MONHOC = Convert.ToInt32(hp.ID_MONHOC) } equals
                                             new { ID_MONHOC = mh.ID_MONHOC }
                                         join knct in db.tbl_KHOAHOC_NGANH_CTIETs on
                                             new { ID_KHOAHOC_NGANH_CTIET = Convert.ToInt32(hp.ID_KHOAHOC_NGANH_CTIET) } equals
                                             new { ID_KHOAHOC_NGANH_CTIET = knct.ID_KHOAHOC_NGANH_CTIET }
                                         join hkht in db.tbl_NAMHOC_HKY_HTAIs on
                                             new { ID_NAMHOC_HKY_HTAI = Convert.ToInt32(hp.ID_NAMHOC_HKY_HTAI) } equals
                                             new { ID_NAMHOC_HKY_HTAI = hkht.ID_NAMHOC_HKY_HTAI }
                                         join nhht in db.tbl_NAMHOC_HIENTAIs on
                                             new { ID_NAMHOC_HIENTAI = Convert.ToInt32(hkht.ID_NAMHOC_HIENTAI) } equals
                                             new { ID_NAMHOC_HIENTAI = nhht.ID_NAMHOC_HIENTAI }
                                         join kn in db.tbl_KHOAHOC_NGANHs on knct.ID_KHOAHOC_NGANH equals kn.ID_KHOAHOC_NGANH
                                         //join l in db.tbl_LOPHOCs on hp.ID_LOPHOC  equals l.ID_LOPHOC
                                         where
                                             kn.ID_NGANH == idnganh &&
                                             (hp.IS_DELETE != 1 || hp.IS_DELETE == null) &&
                                             (mh.IS_DELETE != 1 || mh.IS_DELETE == null) &&
                                             (knct.IS_DELETE != 1 || knct.IS_DELETE == null) &&
                                             (hkht.IS_DELETE != 1 || hkht.IS_DELETE == null) &&
                                             (nhht.IS_DELETE != 1 || nhht.IS_DELETE == null) &&
                                             hkht.IS_HIENTAI == 1 && nhht.IS_HIENTAI == 1 &&
                                             (kn.IS_DELETE != 1 || kn.IS_DELETE == null) &&
                                             //(l.IS_DELETE != 1 || l.IS_DELETE == null) &&
                                             (from hpct in db.tbl_LOP_HOCPHAN_CTs
                                              where hpct.IS_DELETE != 1 || hpct.IS_DELETE == null
                                              select new
                                              {
                                                  hpct.ID_LOPHOCPHAN
                                              }).Contains(new { ID_LOPHOCPHAN = (System.Int32?)hp.ID_LOPHOCPHAN })
                                         select new
                                         {
                                             hp.ID_LOPHOCPHAN,
                                             ID_KHOAHOC_NGANH_CTIET = (int?)hp.ID_KHOAHOC_NGANH_CTIET,
                                             ID_NAMHOC_HKY_HTAI = (int?)hp.ID_NAMHOC_HKY_HTAI,
                                             hp.ID_HEDAOTAO,
                                             ID_MONHOC = (int?)hp.ID_MONHOC,
                                             mh.ISBATBUOC,
                                             hp.ID_LOPHOC,
                                             //l.TEN_LOP,
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
                                             SOSVDKY = (int?)db.SoSVDaDangKy(hp.ID_LOPHOCPHAN),
                                             TEN_GIANGVIEN = (from gv in db.tbl_GIANGVIENs where gv.ID_GIANGVIEN == hp.ID_GIANGVIEN && (gv.IS_DELETE != 1 || gv.IS_DELETE == null) select new { gv.TEN_GIANGVIEN }).FirstOrDefault().TEN_GIANGVIEN.ToString()
                                         };
                dt = TableUtil.LinqToDataTable(danhsachlophocphan);
                dt.Columns.Add("THOIKHOABIEU");
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow r in dt.Rows)
                    {
                        int idlophocphan = Convert.ToInt32(r["ID_LOPHOCPHAN"].ToString());
                        r["THOIKHOABIEU"] = GetChiTietLopHocPhan(idlophocphan);
                    }
                }
                return dt;
            }
            catch (Exception err)
            {
                throw err;
            }
        }

         DataTable GetDanhSachLopHocPhan_3()
        {
            try
            {
                DataTable dt = null;
                var danhsachlophocphan = from hp in db.tbl_LOP_HOCPHANs
                                         join mh in db.tbl_MONHOCs on new { ID_MONHOC = Convert.ToInt32(hp.ID_MONHOC) } equals
                                             new { ID_MONHOC = mh.ID_MONHOC }
                                         join knct in db.tbl_KHOAHOC_NGANH_CTIETs on
                                             new { ID_KHOAHOC_NGANH_CTIET = Convert.ToInt32(hp.ID_KHOAHOC_NGANH_CTIET) } equals
                                             new { ID_KHOAHOC_NGANH_CTIET = knct.ID_KHOAHOC_NGANH_CTIET }
                                         join hkht in db.tbl_NAMHOC_HKY_HTAIs on
                                             new { ID_NAMHOC_HKY_HTAI = Convert.ToInt32(hp.ID_NAMHOC_HKY_HTAI) } equals
                                             new { ID_NAMHOC_HKY_HTAI = hkht.ID_NAMHOC_HKY_HTAI }
                                         join nhht in db.tbl_NAMHOC_HIENTAIs on
                                             new { ID_NAMHOC_HIENTAI = Convert.ToInt32(hkht.ID_NAMHOC_HIENTAI) } equals
                                             new { ID_NAMHOC_HIENTAI = nhht.ID_NAMHOC_HIENTAI }
                                         join kn in db.tbl_KHOAHOC_NGANHs on knct.ID_KHOAHOC_NGANH equals kn.ID_KHOAHOC_NGANH
                                         //join l in db.tbl_LOPHOCs on hp.ID_LOPHOC  equals l.ID_LOPHOC
                                         where
                                             (hp.IS_DELETE != 1 || hp.IS_DELETE == null) &&
                                             (mh.IS_DELETE != 1 || mh.IS_DELETE == null) &&
                                             (knct.IS_DELETE != 1 || knct.IS_DELETE == null) &&
                                             (hkht.IS_DELETE != 1 || hkht.IS_DELETE == null) &&
                                             (nhht.IS_DELETE != 1 || nhht.IS_DELETE == null) &&
                                             hkht.IS_HIENTAI == 1 && nhht.IS_HIENTAI == 1 &&
                                             (kn.IS_DELETE != 1 || kn.IS_DELETE == null) &&
                                             //(l.IS_DELETE != 1 || l.IS_DELETE == null) &&
                                             (from hpct in db.tbl_LOP_HOCPHAN_CTs
                                              where hpct.IS_DELETE != 1 || hpct.IS_DELETE == null
                                              select new
                                              {
                                                  hpct.ID_LOPHOCPHAN
                                              }).Contains(new { ID_LOPHOCPHAN = (System.Int32?)hp.ID_LOPHOCPHAN })
                                         select new
                                         {
                                             hp.ID_LOPHOCPHAN,
                                             ID_KHOAHOC_NGANH_CTIET = (int?)hp.ID_KHOAHOC_NGANH_CTIET,
                                             ID_NAMHOC_HKY_HTAI = (int?)hp.ID_NAMHOC_HKY_HTAI,
                                             hp.ID_HEDAOTAO,
                                             ID_MONHOC = (int?)hp.ID_MONHOC,
                                             mh.ISBATBUOC,
                                             hp.ID_LOPHOC,
                                             //l.TEN_LOP,
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
                                             SOSVDKY = (int?)db.SoSVDaDangKy(hp.ID_LOPHOCPHAN),
                                             TEN_GIANGVIEN = (from gv in db.tbl_GIANGVIENs where gv.ID_GIANGVIEN == hp.ID_GIANGVIEN && (gv.IS_DELETE != 1 || gv.IS_DELETE == null) select new { gv.TEN_GIANGVIEN }).FirstOrDefault().TEN_GIANGVIEN.ToString()
                                         };
                dt = TableUtil.LinqToDataTable(danhsachlophocphan);
                dt.Columns.Add("THOIKHOABIEU");
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow r in dt.Rows)
                    {
                        int idlophocphan = Convert.ToInt32(r["ID_LOPHOCPHAN"].ToString());
                        r["THOIKHOABIEU"] = GetChiTietLopHocPhan(idlophocphan);
                    }
                }
                return dt;
            }
            catch (Exception err)
            {
                throw err;
            }
        }

         DataTable GetDanhSachLopHocPhan_4(int idkhoahoc, int idnganh)
         {
             try
             {
                 DataTable dt = null;
                 var danhsachlophocphan = from hp in db.tbl_LOP_HOCPHANs
                                          join mh in db.tbl_MONHOCs on new { ID_MONHOC = Convert.ToInt32(hp.ID_MONHOC) } equals
                                              new { ID_MONHOC = mh.ID_MONHOC }
                                          join knct in db.tbl_KHOAHOC_NGANH_CTIETs on
                                              new { ID_KHOAHOC_NGANH_CTIET = Convert.ToInt32(hp.ID_KHOAHOC_NGANH_CTIET) } equals
                                              new { ID_KHOAHOC_NGANH_CTIET = knct.ID_KHOAHOC_NGANH_CTIET }
                                          join hkht in db.tbl_NAMHOC_HKY_HTAIs on
                                              new { ID_NAMHOC_HKY_HTAI = Convert.ToInt32(hp.ID_NAMHOC_HKY_HTAI) } equals
                                              new { ID_NAMHOC_HKY_HTAI = hkht.ID_NAMHOC_HKY_HTAI }
                                          join nhht in db.tbl_NAMHOC_HIENTAIs on
                                              new { ID_NAMHOC_HIENTAI = Convert.ToInt32(hkht.ID_NAMHOC_HIENTAI) } equals
                                              new { ID_NAMHOC_HIENTAI = nhht.ID_NAMHOC_HIENTAI }
                                          join kn in db.tbl_KHOAHOC_NGANHs on knct.ID_KHOAHOC_NGANH equals kn.ID_KHOAHOC_NGANH
                                          //join l in db.tbl_LOPHOCs on hp.ID_LOPHOC  equals l.ID_LOPHOC
                                          where
                                              kn.ID_KHOAHOC == idkhoahoc && kn.ID_NGANH == idnganh &&
                                              (hp.IS_DELETE != 1 || hp.IS_DELETE == null) &&
                                              (mh.IS_DELETE != 1 || mh.IS_DELETE == null) &&
                                              (knct.IS_DELETE != 1 || knct.IS_DELETE == null) &&
                                              (hkht.IS_DELETE != 1 || hkht.IS_DELETE == null) &&
                                              (nhht.IS_DELETE != 1 || nhht.IS_DELETE == null) &&
                                              hkht.IS_HIENTAI == 1 && nhht.IS_HIENTAI == 1 &&
                                              (kn.IS_DELETE != 1 || kn.IS_DELETE == null) &&
                                              //(l.IS_DELETE != 1 || l.IS_DELETE == null) &&
                                              (from hpct in db.tbl_LOP_HOCPHAN_CTs
                                               where hpct.IS_DELETE != 1 || hpct.IS_DELETE == null
                                               select new
                                               {
                                                   hpct.ID_LOPHOCPHAN
                                               }).Contains(new { ID_LOPHOCPHAN = (System.Int32?)hp.ID_LOPHOCPHAN })
                                          select new
                                          {
                                              hp.ID_LOPHOCPHAN,
                                              ID_KHOAHOC_NGANH_CTIET = (int?)hp.ID_KHOAHOC_NGANH_CTIET,
                                              ID_NAMHOC_HKY_HTAI = (int?)hp.ID_NAMHOC_HKY_HTAI,
                                              hp.ID_HEDAOTAO,
                                              ID_MONHOC = (int?)hp.ID_MONHOC,
                                              mh.ISBATBUOC,
                                              hp.ID_LOPHOC,
                                              //l.TEN_LOP,
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
                                              SOSVDKY = (int?)db.SoSVDaDangKy(hp.ID_LOPHOCPHAN),
                                              TEN_GIANGVIEN = (from gv in db.tbl_GIANGVIENs where gv.ID_GIANGVIEN == hp.ID_GIANGVIEN && (gv.IS_DELETE != 1 || gv.IS_DELETE == null) select new { gv.TEN_GIANGVIEN }).FirstOrDefault().TEN_GIANGVIEN.ToString()
                                          };
                 dt = TableUtil.LinqToDataTable(danhsachlophocphan);
                 dt.Columns.Add("THOIKHOABIEU");
                 if (dt.Rows.Count > 0)
                 {
                     foreach (DataRow r in dt.Rows)
                     {
                         int idlophocphan = Convert.ToInt32(r["ID_LOPHOCPHAN"].ToString());
                         r["THOIKHOABIEU"] = GetChiTietLopHocPhan(idlophocphan);
                     }
                 }
                 return dt;
             }
             catch (Exception err)
             {
                 throw err;
             }
         }

        string GetChiTietLopHocPhan(int id_lophocphan)
        {
            string ketqua="";
            try
            {
                DataTable dt = null;
                var dslophocphanchitiet = from hpct in db.tbl_LOP_HOCPHAN_CTs
                                            join p in db.tbl_PHONGHOCs on hpct.ID_PHONG equals p.ID_PHONG
                                            where (hpct.IS_DELETE != 1 || hpct.IS_DELETE == null) &&
                                                  (p.IS_DELETE != 1 || p.IS_DELETE == null) &&
                                                  hpct.ID_LOPHOCPHAN == id_lophocphan
                                            select new
                                            {
                                                THOIKHOABIEU = "Thứ:" + hpct.THU.ToString() +",\tTiết: " + hpct.TIET_BD.ToString()+" đến " + hpct.TIET_KT.ToString() + "\tPhòng: " +p.MA_PHONG.ToString()
                                            };
                dt = TableUtil.LinqToDataTable(dslophocphanchitiet);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow r in dt.Rows)
                    {
                        ketqua =ketqua +"\n" + r["THOIKHOABIEU"].ToString()+"\n";
                    }
                }
                return ketqua;
            }
            catch (Exception err)
            {
                throw err;
            }
        }

        public DataTable GetNganhWhereKhoaHoc(int idkhoahoc)
        {
            DataTable dt = null;
            var nganh = from kn in db.tbl_KHOAHOC_NGANHs
                join kh in db.tbl_KHOAHOCs on new {ID_KHOAHOC = Convert.ToInt32(kn.ID_KHOAHOC)} equals
                    new {ID_KHOAHOC = kh.ID_KHOAHOC}
                join ng in db.tbl_NGANHs on new {ID_NGANH = Convert.ToInt32(kn.ID_NGANH)} equals
                    new {ID_NGANH = ng.ID_NGANH}
                where
                    (kn.IS_DELETE != 1 ||
                     kn.IS_DELETE == null) &&
                    (kh.IS_DELETE != 1 ||
                     kh.IS_DELETE == null) &&
                    (ng.IS_DELETE != 1 ||
                     ng.IS_DELETE == null) &&
                    kn.ID_KHOAHOC == idkhoahoc
                select new
                {
                    ID_NGANH = (int?) kn.ID_NGANH,
                    ng.TEN_NGANH
                };
            dt = TableUtil.LinqToDataTable(nganh);
            return dt;
        }

        public DataTable GetMonHoc(int idkhoahoc, int idnganh)
        {
            try
            {
                DataTable dt = null;
                var dsmonhoc = (from knct in db.tbl_KHOAHOC_NGANH_CTIETs
                                join mh in db.tbl_MONHOCs on new { ID_MONHOC = Convert.ToInt32(knct.ID_MONHOC) } equals new { ID_MONHOC = mh.ID_MONHOC }
                                join kn in db.tbl_KHOAHOC_NGANHs on new { ID_KHOAHOC_NGANH = Convert.ToInt32(knct.ID_KHOAHOC_NGANH) } equals new { ID_KHOAHOC_NGANH = kn.ID_KHOAHOC_NGANH }
                                join hp in db.tbl_LOP_HOCPHANs on new { ID_KHOAHOC_NGANH_CTIET = knct.ID_KHOAHOC_NGANH_CTIET } equals new { ID_KHOAHOC_NGANH_CTIET = Convert.ToInt32(hp.ID_KHOAHOC_NGANH_CTIET) }
                                join hkht in db.tbl_NAMHOC_HKY_HTAIs on new { ID_NAMHOC_HKY_HTAI = Convert.ToInt32(hp.ID_NAMHOC_HKY_HTAI) } equals new { ID_NAMHOC_HKY_HTAI = hkht.ID_NAMHOC_HKY_HTAI }
                                where
                                  (knct.IS_DELETE != 1 ||
                                  knct.IS_DELETE == null) &&
                                  (mh.IS_DELETE != 1 ||
                                  mh.IS_DELETE == null) &&
                                  (hp.IS_DELETE != 1 ||
                                  hp.IS_DELETE == null) &&
                                  (hkht.IS_DELETE != 1 ||
                                  hkht.IS_DELETE == null) &&
                                  hkht.IS_HIENTAI == 1 &&
                                  kn.ID_KHOAHOC == idkhoahoc &&
                                  kn.ID_NGANH == idnganh
                                select new
                                {
                                    mh.ID_MONHOC,
                                    mh.TEN_MONHOC
                                }).Distinct();
                dt = TableUtil.LinqToDataTable(dsmonhoc);
                return dt;
            }
            catch (Exception err)
            {
                throw err;
            }
        }
    }
}

﻿using System;
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
                var khoanganh = from kn in db.tbl_KHOAHOC_NGANHs
                    where
                        (kn.IS_DELETE != 1 || kn.IS_DELETE == null) && kn.ID_KHOAHOC == idkhoa && kn.ID_NGANH == idnganh
                    select new {kn.ID_KHOAHOC_NGANH};
                DataTable dt = TableUtil.LinqToDataTable(khoanganh);
                if (dt.Rows.Count > 0)
                    res = Convert.ToInt32(dt.Rows[0]["ID_KHOAHOC_NGANH"].ToString());
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

        public DataTable GetMonHoc()
        {
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("ID_MONHOC", typeof (Decimal));
                dt.Columns.Add("TEN_MONHOC", typeof (string));
                DataRow r = dt.NewRow();
                r["ID_MONHOC"] = 0;
                r["TEN_MONHOC"] = "--------------------------------------Chọn----------------------------------------";
                dt.Rows.Add(r);
                var mh = from m in db.tbl_MONHOCs
                    where (m.IS_DELETE != 1 || m.IS_DELETE == null)
                    select new {m.ID_MONHOC, m.TEN_MONHOC};
                foreach (var m in mh)
                {
                    DataRow dr = dt.NewRow();
                    dr["ID_MONHOC"] = m.ID_MONHOC;
                    dr["TEN_MONHOC"] = m.TEN_MONHOC;
                    dt.Rows.Add(dr);
                    dt.AcceptChanges();
                }
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
                    join hp in db.tbl_LOP_HOCPHANs on new {ID_LOPHOCPHAN = Convert.ToInt32(dkhp.ID_LOPHOCPHAN)} equals
                        new {ID_LOPHOCPHAN = hp.ID_LOPHOCPHAN}
                    join hk in db.tbl_NAMHOC_HKY_HTAIs on hp.ID_NAMHOC_HKY_HTAI equals hk.ID_NAMHOC_HKY_HTAI
                    join nh in db.tbl_NAMHOC_HIENTAIs on hk.ID_NAMHOC_HIENTAI equals nh.ID_NAMHOC_HIENTAI
                    join mh in db.tbl_MONHOCs on new {ID_MONHOC = Convert.ToInt32(hp.ID_MONHOC)} equals
                        new {ID_MONHOC = mh.ID_MONHOC}
                    where
                        (dkhp.IS_DELETE != 1 ||dkhp.IS_DELETE == null) &&
                        (hp.IS_DELETE != 1 ||hp.IS_DELETE == null) &&
                        (mh.IS_DELETE != 1 ||mh.IS_DELETE == null) &&
                         (hk.IS_DELETE != 1 || hk.IS_DELETE == null) &&
                         (nh.IS_DELETE != 1 || nh.IS_DELETE == null) &&
                         hk.IS_HIENTAI == 1 && nh.IS_HIENTAI == 1 &&
                        dkhp.ID_SINHVIEN == id_sinhvien
                    select new
                    {
                        dkhp.ID_DANGKY,
                        dkhp.ID_SINHVIEN,
                        dkhp.ID_LOPHOCPHAN,
                        mh.MA_MONHOC,
                        mh.TEN_MONHOC,
                        dkhp.NGAY_DANGKY,
                        dkhp.GIO_DANGKY,
                        mh.SO_TC,
                        dkhp.THANH_TIEN
                    };
                dt = TableUtil.LinqToDataTable(hpdk);
                return dt;
            }
            catch (Exception err)
            {
                throw err;
            }
        }

        public DataTable GetLopHP()
        {
            try
            {
                DataTable dt = null;
                var hpdky = from hp in db.tbl_LOP_HOCPHANs
                    join mh in db.tbl_MONHOCs on new {ID_MONHOC = Convert.ToInt32(hp.ID_MONHOC)} equals
                        new {ID_MONHOC = mh.ID_MONHOC}
                    where
                        (hp.IS_DELETE != 1 ||
                         hp.IS_DELETE == null) &&
                        (from hkht in db.tbl_NAMHOC_HKY_HTAIs
                            where
                                hkht.IS_HIENTAI == 1
                            select new
                            {
                                hkht.ID_NAMHOC_HKY_HTAI
                            }).Contains(new {ID_NAMHOC_HKY_HTAI = (System.Int32) hp.ID_NAMHOC_HKY_HTAI})
                    select new
                    {
                        hp.ID_LOPHOCPHAN,
                        hp.ID_KHOAHOC_NGANH_CTIET,
                        hp.ID_NAMHOC_HKY_HTAI,
                        hp.ID_HEDAOTAO,
                        ID_MONHOC = (int?) hp.ID_MONHOC,
                        hp.ID_LOPHOC,
                        hp.ID_GIANGVIEN,
                        hp.MA_LOP_HOCPHAN,
                        hp.TEN_LOP_HOCPHAN,
                        mh.MA_MONHOC,
                        mh.TEN_MONHOC,
                        mh.SO_TC,
                        hp.SOLUONG,
                        hp.TUAN_BD,
                        hp.TUAN_KT,
                        SOSVDKY = db.SoSVDaDangKy(hp.ID_LOPHOCPHAN)
                    };
                dt = TableUtil.LinqToDataTable(hpdky);
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
                    where (hkht.IS_DELETE != 1 || hkht.IS_DELETE == null) && hkht.IS_HIENTAI == 1 && (namhoc.IS_DELETE != 1 || namhoc.IS_DELETE == null) && namhoc.IS_HIENTAI ==1
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
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using DATN.TTS.DATA;

namespace DATN.TTS.BUS
{
    public class bus_PhanCong_TKB
    {
        db_ttsDataContext db = new db_ttsDataContext();

        public DataTable GetAllHDT()
        {
            try
            {
                DataTable dt = new DataTable();
                var hdt = from dtao in db.tbl_HEDAOTAOs where dtao.IS_DELETE == 0
                    select new
                    {
                        dtao.ID_HE_DAOTAO,
                        dtao.TEN_HE_DAOTAO
                    };
                dt = TableUtil.LinqToDataTable(hdt);
                return dt;
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        public DataTable GetKhoaHoc(int pID_HE_DATAO)
        {
            try
            {
                DataTable dt = new DataTable();
                var khoahoc = from k in db.tbl_KHOAHOCs
                    where k.IS_DELETE == 0 && k.ID_HE_DAOTAO.Equals(pID_HE_DATAO)
                    select new
                    {
                        k.ID_KHOAHOC,
                        k.TEN_KHOAHOC
                    };
                dt = TableUtil.LinqToDataTable(khoahoc);
                return dt;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DataTable GetAllLopForKhoaHoc(int pID_HE_DATAO, int pID_KHOAHOC)
        {
            try
            {
                DataTable dt = new DataTable();
                var lop = from hp in db.tbl_LOP_HOCPHANs
                          join knct in db.tbl_KHOAHOC_NGANH_CTIETs on new { ID_KHOAHOC_NGANH_CTIET = Convert.ToInt32(hp.ID_KHOAHOC_NGANH_CTIET) } 
                            equals new { ID_KHOAHOC_NGANH_CTIET = knct.ID_KHOAHOC_NGANH_CTIET } into knct_join
                          from knct in knct_join.DefaultIfEmpty()

                          join khn in db.tbl_KHOAHOC_NGANHs on new { ID_KHOAHOC_NGANH = Convert.ToInt32(knct.ID_KHOAHOC_NGANH) } 
                            equals new { ID_KHOAHOC_NGANH = khn.ID_KHOAHOC_NGANH } into khn_join
                          from khn in khn_join.DefaultIfEmpty()

                          join kh in db.tbl_KHOAHOCs on new { ID_KHOAHOC = Convert.ToInt32(khn.ID_KHOAHOC), Column1 = pID_KHOAHOC, ID_HE_DAOTAO = pID_HE_DATAO }
                            equals new { ID_KHOAHOC = kh.ID_KHOAHOC, Column1 = kh.ID_KHOAHOC, ID_HE_DAOTAO = Convert.ToInt32(kh.ID_HE_DAOTAO) } into kh_join
                          from kh in kh_join.DefaultIfEmpty()

                          join mh in db.tbl_MONHOCs on new { ID_MONHOC = Convert.ToInt32(knct.ID_MONHOC) } equals new { ID_MONHOC = mh.ID_MONHOC } into mh_join
                          from mh in mh_join.DefaultIfEmpty()

                          join gv in db.tbl_GIANGVIENs on new { ID_GIANGVIEN = Convert.ToInt32(hp.ID_GIANGVIEN) } equals new { ID_GIANGVIEN = gv.ID_GIANGVIEN } into gv_join
                          from gv in gv_join.DefaultIfEmpty()
                          where
                            hp.IS_DELETE == 0 &&
                            hp.ID_HEDAOTAO == pID_HE_DATAO &&
                            knct.IS_DELETE == 0 &&
                            khn.IS_DELETE == 0 &&
                            kh.IS_DELETE == 0 &&
                            gv.IS_DELETE == 0
                          select new
                          {
                              hp.ID_LOPHOCPHAN,
                              hp.ID_KHOAHOC_NGANH_CTIET,
                              hp.ID_HOCKY_NAMHOC,
                              hp.ID_HEDAOTAO,
                              hp.ID_MONHOC,
                              hp.ID_LOPHOC,
                              hp.ID_GIANGVIEN,
                              gv.MA_GIANGVIEN,
                              gv.TEN_GIANGVIEN,
                              hp.MA_LOP_HOCPHAN,
                              hp.TEN_LOP_HOCPHAN,
                              hp.SOTIET,
                              hp.NGAY_BD,
                              hp.NGAY_KT,
                              hp.TUAN_BD,
                              hp.TUAN_KT,
                              hp.SO_TUAN,
                              hp.SOLUONG,
                              knct.ID_KHOAHOC_NGANH,
                              knct.SO_TC,
                              knct.ID_HOCKY,
                              knct.SOTIET_LT,
                              knct.SOTIET_TH,
                              knct.ID_MONHOC_TRUOC,
                              knct.ID_MONHOC_SONGHANH,
                              knct.MONHOC_TIENQUYET,
                              khn.ID_NGANH,
                              khn.SO_HKY,
                              khn.SO_SINHVIEN_DK,
                              khn.SO_LOP,
                              khn.HOCKY_TRONGKHOA,
                              mh.MA_MONHOC,
                              mh.TEN_MONHOC,
                              mh.KY_HIEU,
                              //mh.SO_TC,
                              mh.IS_THUHOCPHI,
                              mh.IS_THUCHANH,
                              mh.IS_LYTHUYET,
                              mh.IS_TINHDIEM,
                              mh.ISBATBUOC
                          };
                dt = TableUtil.LinqToDataTable(lop);
                return dt;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DataTable GetHPCTByLHP(int pID_LOPHOCPHAN)
        {
            try
            {
                DataTable dt = new DataTable();
                var lhpct = from hpct in db.tbl_LOP_HOCPHAN_CTs
                            join p in db.tbl_PHONGHOCs on hpct.ID_PHONG equals p.ID_PHONG
                            join h in db.tbl_LOP_HOCPHANs on hpct.ID_LOPHOCPHAN equals h.ID_LOPHOCPHAN
                            join gv in db.tbl_GIANGVIENs on h.ID_GIANGVIEN equals gv.ID_GIANGVIEN
                    where
                        hpct.IS_DELETE == 0 &&
                        hpct.ID_LOPHOCPHAN == pID_LOPHOCPHAN &&
                        p.IS_DELETE == 0 &&
                        h.IS_DELETE==0 &&
                        gv.IS_DELETE == 0
                    select new
                    {
                        hpct.ID_LOP_HOCPHAN_CTIET,
                        hpct.ID_LOPHOCPHAN,
                        hpct.IS_DELETE,
                        hpct.THU,
                        hpct.TIET_BD,
                        hpct.TIET_KT,
                        hpct.ID_PHONG,
                        p.TEN_PHONG,
                        h.TEN_LOP_HOCPHAN,
                        h.NGAY_BD,
                        h.NGAY_KT,
                        gv.TEN_GIANGVIEN
                    };
                dt = TableUtil.LinqToDataTable(lhpct);
                return dt;
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        public bool CheckTrungGio()
        {

            return true;
        }

        public bool Insert_LopHocPhan_CT(DataTable iDataSoure)
        {
            try
            {
                if (iDataSoure != null)
                {
                    DataRow r = iDataSoure.Rows[0];
                    tbl_LOP_HOCPHAN_CT hpct = new tbl_LOP_HOCPHAN_CT();
                    hpct.ID_LOPHOCPHAN = Convert.ToInt32(r["ID_LOPHOCPHAN"].ToString());
                    hpct.ID_PHONG = Convert.ToInt32(r["ID_PHONG"].ToString());
                    hpct.THU = Convert.ToInt32(r["THU"].ToString());
                    hpct.TIET_BD = Convert.ToInt32(r["TIET_BD"].ToString());
                    hpct.TIET_KT = Convert.ToInt32(r["TIET_KT"].ToString());
                    hpct.SO_TIET = Convert.ToInt32(r["SO_TIET"].ToString());
                    hpct.CREATE_USER = r["USER"].ToString();
                    hpct.IS_DELETE = 0;
                    hpct.CREATE_TIME = System.DateTime.Today;

                    db.tbl_LOP_HOCPHAN_CTs.InsertOnSubmit(hpct);
                    db.SubmitChanges();
                    if (!hpct.ID_LOPHOCPHAN.ToString().GetTypeCode().Equals(TypeCode.DBNull))
                        return true;
                }
                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool Update_LopHocPhan_CT(DataTable iDataSoure)
        {
            try
            {
                if (iDataSoure != null)
                {
                    DataRow r = iDataSoure.Rows[0];
                    tbl_LOP_HOCPHAN_CT hpct = db.tbl_LOP_HOCPHAN_CTs.Single(t => t.ID_LOP_HOCPHAN_CTIET == Convert.ToInt32(r["ID_LOP_HOCPHAN_CTIET"].ToString()));
                    hpct.ID_PHONG = Convert.ToInt32(r["ID_PHONG"].ToString());
                    hpct.THU = Convert.ToInt32(r["THU"].ToString());
                    hpct.TIET_BD = Convert.ToInt32(r["TIET_BD"].ToString());
                    hpct.TIET_KT = Convert.ToInt32(r["TIET_KT"].ToString());
                    hpct.SO_TIET = Convert.ToInt32(r["SO_TIET"].ToString());
                    hpct.UPDATE_USER = r["USER"].ToString();
                    hpct.UPDATE_TIME = System.DateTime.Today;

                    db.SubmitChanges();
                    if (!hpct.ID_LOPHOCPHAN.ToString().GetTypeCode().Equals(TypeCode.DBNull))
                        return true;
                }
                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool Delete_LopHocPhan_CT(int pID_LOP_HOCPHAN_CTIET, string pUser)
        {
            try
            {
                if (pID_LOP_HOCPHAN_CTIET != 0)
                {
                    tbl_LOP_HOCPHAN_CT hpct = db.tbl_LOP_HOCPHAN_CTs.Single(t => t.ID_LOP_HOCPHAN_CTIET == pID_LOP_HOCPHAN_CTIET);
                    hpct.IS_DELETE = 1;
                    hpct.UPDATE_USER = pUser;
                    hpct.UPDATE_TIME = System.DateTime.Today;
                    db.SubmitChanges();
                    if (!hpct.ID_LOPHOCPHAN.ToString().GetTypeCode().Equals(TypeCode.DBNull))
                        return true;
                }
                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int SoTietDaSep(int pID_LOPHOCPHAN)
        {
            try
            {
                var sotiet = from hpct in
                    (from hpct in db.tbl_LOP_HOCPHAN_CTs
                        where
                            hpct.IS_DELETE == 0 &&
                            hpct.ID_LOPHOCPHAN == pID_LOPHOCPHAN
                        select new
                        {
                            hpct.SO_TIET,
                            Dummy = "x"
                        })
                    group hpct by new {hpct.Dummy}
                    into g
                    select new
                    {
                        TietDaSep = (int?) g.Sum(p => p.SO_TIET)
                    };

                DataTable dt = TableUtil.LinqToDataTable(sotiet);
                int res = 0;
                if (dt.Rows.Count > 0)
                {
                   res  = Convert.ToInt32(dt.Rows[0]["TietDaSep"].ToString());
                }
                return res;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //lay danh sách phòng rỗi bận
        public DataTable GetPhongTrong()
        {
            try
            {
                DataTable dt = new DataTable();
                var phong = from p in db.tbl_PHONGHOCs where p.IS_DELETE == 0 && 
                                (from hpct in db.tbl_LOP_HOCPHAN_CTs
                                 where hpct.IS_DELETE == 0 && p.ID_PHONG == hpct.ID_PHONG
                                select new
                                {
                                    hpct.ID_PHONG
                                }).Single() == null
                            select new
                            {
                                p.ID_PHONG,
                                p.TEN_PHONG
                            };
                dt = TableUtil.LinqToDataTable(phong);
                return dt;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #region chưa dùng đến
        public DataTable GetPhongWhereThu(List<int> thu)
        {
            try
            {
                DataTable dt = null;

                return dt;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DataTable GetPhongWhereTiet(int tietbd, int tietkt)
        {
            try
            {
                DataTable dt = null;

                return dt;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DataTable GetPhongWhereTuan(int tuanbd, int tuankt)
        {
            try
            {
                DataTable dt = null;

                return dt;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        public DataTable GetPhongWhereThuTiet(int thu, int tietbd, int tietkt)
        {
            try
            {
                DataTable dt = null;
                var phong = from p in db.tbl_PHONGHOCs
                    where p.IS_DELETE == 0 &&
                          (from hpct in db.tbl_LOP_HOCPHAN_CTs
                              where
                                  hpct.IS_DELETE == 0 &&
                                  (hpct.TIET_BD >= tietbd && hpct.TIET_KT <= tietkt) &&
                                  hpct.THU == thu &&
                                  p.ID_PHONG == hpct.ID_PHONG
                                  select new
                                  {
                                      hpct.ID_PHONG
                                  }).Single() == null                   
                            select new
                            {
                                p.ID_PHONG,
                                p.MA_PHONG,
                                p.TEN_PHONG,
                                p.SUCCHUA,
                                p.LOAIPHONG,
                                p.IS_MAYCHIEU,
                                p.IS_DELETE,
                                p.IS_MAYTINH,
                                p.DAY,
                                p.TANG,
                                p.TINHTRANG
                            };
                dt = TableUtil.LinqToDataTable(phong);
                return dt;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
 
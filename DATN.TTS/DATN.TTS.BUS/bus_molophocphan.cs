using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DATN.TTS.DATA;

namespace DATN.TTS.BUS
{
    public class bus_molophocphan
    {
        db_ttsDataContext db = new db_ttsDataContext();

        public DataTable GetAll_Tso_Hocky()
        {
            var query = from d in db.tbl_NAMHOC_HIENTAIs
                where
                    (d.IS_DELETE != 1 ||
                     d.IS_DELETE == null) &&
                    d.IS_HIENTAI == 1
                select new
                {
                    d.ID_NAMHOC_HIENTAI,
                    d.NAMHOC_TU,
                    d.NAMHOC_DEN,
                    d.NGAY_BATDAU,
                    d.SO_TUAN,
                    d.SO_HKY_TRONGNAM,
                    d.IS_HIENTAI,
                    ID_NAMHOC_HKY_HTAI =
                        ((from m in db.tbl_NAMHOC_HKY_HTAIs
                            where
                                (m.IS_DELETE != 1 ||
                                 m.IS_DELETE == null) &&
                                Convert.ToInt64(m.IS_HIENTAI) == 1 &&
                                m.ID_NAMHOC_HIENTAI == d.ID_NAMHOC_HIENTAI
                            select new
                            {
                                m.ID_NAMHOC_HKY_HTAI
                            }).First().ID_NAMHOC_HKY_HTAI),
                    HOCKY =
                        ((from m in db.tbl_NAMHOC_HKY_HTAIs
                            where
                                (m.IS_DELETE != 1 ||
                                 m.IS_DELETE == null) &&
                                Convert.ToInt64(m.IS_HIENTAI) == 1 &&
                                m.ID_NAMHOC_HIENTAI == d.ID_NAMHOC_HIENTAI
                            select new
                            {
                                m.HOCKY
                            }).First().HOCKY)
                };
            DataTable xdt = null;
            xdt = TableUtil.LinqToDataTable(query);
            return xdt;
        }

        public DataTable GetAll_HDT()
        {
            var query = from d in db.tbl_HEDAOTAOs
                        where
                          d.IS_DELETE != 1 ||
                          d.IS_DELETE == null
                        select new
                        {
                            d.ID_HE_DAOTAO,
                            d.ID_BAC_DAOTAO,
                            d.ID_LOAIHINH_DTAO,
                            d.MA_HE_DAOTAO,
                            d.TEN_HE_DAOTAO,
                            d.SO_NAMHOC,
                            d.TRANGTHAI,
                        };
            DataTable xdt = null;
            xdt = TableUtil.LinqToDataTable(query);
            return xdt;
        }

        public DataTable GetAll_KhoaHoc(int id_hedaotao)
        {
            var query = from d in db.tbl_KHOAHOCs
                        where
                          (d.IS_DELETE != 1 ||
                          d.IS_DELETE == null) &&
                          d.ID_HE_DAOTAO == id_hedaotao
                        select new
                        {
                            d.ID_KHOAHOC,
                            d.MA_KHOAHOC,
                            d.TEN_KHOAHOC,
                            d.NAM_BD,
                            d.NAM_KT,
                            d.SO_HKY_1NAM,
                            d.SO_HKY,
                            d.KYHIEU,
                            d.TRANGTHAI,
                        };
            DataTable xdt = null;
            xdt = TableUtil.LinqToDataTable(query);
            return xdt;
        }

        public DataTable GetAll_Khoa_Nganh(int ID_KHOAHOC)
        {
            var query = from d in db.tbl_KHOAHOC_NGANHs
                        where
                          (d.IS_DELETE != 1 ||
                          d.IS_DELETE == null) &&
                          d.ID_KHOAHOC == ID_KHOAHOC
                        select new
                        {
                            d.ID_KHOAHOC_NGANH,
                            d.ID_NGANH,
                            TEN_NGANH =
                              ((from m in db.tbl_NGANHs
                                where
                                  m.ID_NGANH == d.ID_NGANH
                                select new
                                {
                                    m.TEN_NGANH
                                }).First().TEN_NGANH),
                            d.SO_HKY,
                            d.SO_SINHVIEN_DK,
                            d.SO_LOP,
                            d.HOCKY_TRONGKHOA,
                            d.GHICHU,
                        };
            DataTable xdt = null;
            xdt = TableUtil.LinqToDataTable(query);
            return xdt;
        }

        public DataTable GetAll_monhoc(int ID_KHOAHOC_NGANH)
        {
            var query = from d in db.tbl_KHOAHOC_NGANH_CTIETs
                        join m in db.tbl_MONHOCs on new { ID_MONHOC = Convert.ToInt32(d.ID_MONHOC) } equals new { ID_MONHOC = m.ID_MONHOC } into m_join
                        from m in m_join.DefaultIfEmpty()
                        where
                          (d.IS_DELETE != 1 ||
                          d.IS_DELETE == null) &&
                          (m.IS_DELETE != 1 ||
                          m.IS_DELETE == null) &&
                          d.ID_KHOAHOC_NGANH == ID_KHOAHOC_NGANH
                        select new
                        {
                            d.ID_KHOAHOC_NGANH_CTIET,
                            d.HOCKY,
                            m.ID_MONHOC,
                            m.ID_LOAI_MONHOC,
                            m.ID_BOMON,
                            TEN_BM =
                              ((from n in db.tbl_BOMONs
                                where
                                  n.ID_BOMON == m.ID_BOMON
                                select new
                                {
                                    n.TEN_BM
                                }).First().TEN_BM),
                            m.MA_MONHOC,
                            m.TEN_MONHOC,
                            m.KY_HIEU,
                            m.SO_TC,
                            SOTIET = (int?)(d.SOTIET_LT ?? 0 + d.SOTIET_TH ?? 0),
                            m.IS_THUHOCPHI,
                            m.IS_THUCHANH,
                            m.IS_LYTHUYET,
                            m.IS_TINHDIEM,
                            m.TRANGTHAI,
                            m.ISBATBUOC,
                            m.ID_MONHOC_SONGHANH,
                            m.GHICHU
                        };
            DataTable xdt = null;
            xdt = TableUtil.LinqToDataTable(query);
            return xdt;
        }

        public DataTable GetAll_Lop(int pID_KHOAHOC)
        {
            var query = from lh in db.tbl_LOPHOCs
                        join kn in db.tbl_KHOAHOC_NGANHs on new { ID_KHOAHOC_NGANH = Convert.ToInt32(lh.ID_KHOAHOC_NGANH) } equals new { ID_KHOAHOC_NGANH = kn.ID_KHOAHOC_NGANH } into kn_join
                        from kn in kn_join.DefaultIfEmpty()
                        where
                          kn.ID_KHOAHOC == 1 &&
                          (kn.IS_DELETE != 1 ||
                          kn.IS_DELETE == null) &&
                          (lh.IS_DELETE != 1 ||
                          lh.IS_DELETE == null)
                select new
                {
                    ID_KHOAHOC_NGANH = (int?) kn.ID_KHOAHOC_NGANH,
                    ID_LOPHOC = (int?) lh.ID_LOPHOC,
                    MA_LOP = lh.MA_LOP,
                    TEN_LOP = lh.TEN_LOP,
                    GHICHU = lh.GHICHU,
                    TEN_KHOAHOC =
                        ((from m in db.tbl_KHOAHOCs
                            where
                                m.ID_KHOAHOC == pID_KHOAHOC
                            select new
                            {
                                m.TEN_KHOAHOC
                            }).First().TEN_KHOAHOC),
                    ten_nganh =
                        ((from n in db.tbl_NGANHs
                            where
                                n.ID_NGANH == kn.ID_NGANH
                            select new
                            {
                                n.TEN_NGANH
                            }).First().TEN_NGANH)
                };
            DataTable xdt = null;
            xdt = TableUtil.LinqToDataTable(query);
            return xdt;
        }

        public DataTable GetAll_MH_byKhoaHoc(int pID_KHOAHOC_NGANH)
        {
            var query = from ct in db.tbl_KHOAHOC_NGANH_CTIETs
                join mh in db.tbl_MONHOCs on new {ID_MONHOC = Convert.ToInt32(ct.ID_MONHOC)} equals
                    new {ID_MONHOC = mh.ID_MONHOC} into mh_join
                from mh in mh_join.DefaultIfEmpty()
                where
                    ct.ID_KHOAHOC_NGANH == pID_KHOAHOC_NGANH &&
                    (ct.IS_DELETE != 1 ||
                     ct.IS_DELETE == null) &&
                    (mh.IS_DELETE != 1 ||
                     mh.IS_DELETE == null)
                select new
                {
                    ct.ID_KHOAHOC_NGANH_CTIET,
                    ID_MONHOC = (int?) mh.ID_MONHOC,
                    MA_MONHOC = mh.MA_MONHOC,
                    TEN_MONHOC = mh.TEN_MONHOC,
                    SO_TC = (int?) mh.SO_TC,
                    SOTIET = (int?)(ct.SOTIET_LT ?? 0 + ct.SOTIET_TH ?? 0),
                    ten_bm =
                        ((from m in db.tbl_BOMONs
                            where
                                m.ID_BOMON == mh.ID_BOMON
                            select new
                            {
                                m.TEN_BM
                            }).First().TEN_BM),
                    Ten_loai_monhoc =
                        ((from n in db.tbl_LOAI_MONHOCs
                            where
                                n.ID_LOAI_MONHOC == mh.ID_LOAI_MONHOC
                            select new
                            {
                                n.TEN_LOAI_MONHOC
                            }).First().TEN_LOAI_MONHOC),
                    mh.IS_LYTHUYET,
                    mh.IS_THUCHANH
                };
            DataTable xdt = null;
            xdt = TableUtil.LinqToDataTable(query);
            return xdt;
        }

        public DataTable GetAll_hocphan_bymh(int ID_MONHOC, int ID_NAMHOC_HKY_HTAI, int ID_KHOAHOC_NGANH_CTIET)
        {
            var query = from Tbl_LOP_HOCPHAN in db.tbl_LOP_HOCPHANs
                where
                    (Tbl_LOP_HOCPHAN.IS_DELETE != 1 ||
                     Tbl_LOP_HOCPHAN.IS_DELETE == null) &&
                    Tbl_LOP_HOCPHAN.ID_MONHOC == ID_MONHOC &&
                    Tbl_LOP_HOCPHAN.ID_NAMHOC_HKY_HTAI == ID_NAMHOC_HKY_HTAI &&
                    Tbl_LOP_HOCPHAN.ID_KHOAHOC_NGANH_CTIET == ID_KHOAHOC_NGANH_CTIET
                select new
                {
                    Tbl_LOP_HOCPHAN.ID_LOPHOCPHAN,
                    Tbl_LOP_HOCPHAN.ID_LOPHOC,
                    Tbl_LOP_HOCPHAN.ID_KHOAHOC_NGANH_CTIET,
                    Tbl_LOP_HOCPHAN.ID_NAMHOC_HKY_HTAI,
                    Tbl_LOP_HOCPHAN.ID_HEDAOTAO,
                    Tbl_LOP_HOCPHAN.ID_MONHOC,
                    Tbl_LOP_HOCPHAN.MA_LOP_HOCPHAN,
                    Tbl_LOP_HOCPHAN.TEN_LOP_HOCPHAN,
                    Tbl_LOP_HOCPHAN.TUAN_BD,
                    Tbl_LOP_HOCPHAN.TUAN_KT,
                    Tbl_LOP_HOCPHAN.SOTIET,
                    Tbl_LOP_HOCPHAN.SOLUONG,
                    Tbl_LOP_HOCPHAN.ID_GIANGVIEN,
                    TEN_GIANGVIEN = (from Tbl_GIANGVIEN in db.tbl_GIANGVIENs
                        where
                            (Tbl_GIANGVIEN.IS_DELETE != 1 ||
                             Tbl_GIANGVIEN.IS_DELETE == null) &&
                            Tbl_GIANGVIEN.ID_GIANGVIEN == 1
                        select new
                        {
                            Tbl_GIANGVIEN.TEN_GIANGVIEN
                        }).First().TEN_GIANGVIEN,
                    Tbl_LOP_HOCPHAN.CACH_TINHDIEM
                };
            DataTable xdt = null;
            xdt = TableUtil.LinqToDataTable(query);
            return xdt;
        }

        public int InsertObject(DataTable xdt)
        {
            try
            {
                int i = 0;
                foreach (DataRow dt in xdt.Rows)
                {
                    if (string.IsNullOrEmpty(dt["ID_LOPHOC"].ToString()))
                    {
                        dt["ID_LOPHOC"] = 0;
                    }
                    tbl_LOP_HOCPHAN query = new tbl_LOP_HOCPHAN
                    {
                        ID_LOPHOC = Convert.ToInt32(dt["ID_LOPHOC"]),
                        ID_KHOAHOC_NGANH_CTIET = Convert.ToInt32(dt["ID_KHOAHOC_NGANH_CTIET"]),
                        ID_NAMHOC_HKY_HTAI = Convert.ToInt32(dt["ID_NAMHOC_HKY_HTAI"]),
                        ID_HEDAOTAO = Convert.ToInt32(dt["ID_HE_DAOTAO"]),
                        ID_MONHOC = Convert.ToInt32(dt["ID_MONHOC"]),
                        MA_LOP_HOCPHAN = dt["MA_LOP_HOCPHAN"].ToString(),
                        TEN_LOP_HOCPHAN = dt["TEN_LOP_HOCPHAN"].ToString(),
                        TUAN_BD = Convert.ToInt32(dt["TUAN_BD"]),
                        TUAN_KT = Convert.ToInt32(dt["TUAN_KT"]),
                        SOTIET = Convert.ToInt32(dt["SOTIET"]),
                        SOLUONG = Convert.ToInt32(dt["SOLUONG"]),
                        CACH_TINHDIEM = dt["CACH_TINHDIEM"].ToString(),
                        CREATE_USER = dt["USER"].ToString(),
                        CREATE_TIME = DateTime.Now,
                        IS_DELETE = 0
                    };
                    db.tbl_LOP_HOCPHANs.InsertOnSubmit(query);
                    db.SubmitChanges();
                    i = query.ID_LOPHOCPHAN;
                }
                return i;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public int UpdateObject(DataTable xdt)
        {
            try
            {
                int i = 0;
                tbl_LOP_HOCPHAN query = (from Tbl_LOP_HOCPHAN in db.tbl_LOP_HOCPHANs
                                   where
                                     Tbl_LOP_HOCPHAN.ID_LOPHOCPHAN == Convert.ToInt32(xdt.Rows[0]["ID_LOPHOCPHAN"])
                                   select Tbl_LOP_HOCPHAN).FirstOrDefault();
                query.ID_NAMHOC_HKY_HTAI = Convert.ToInt32(xdt.Rows[0]["ID_NAMHOC_HKY_HTAI"]);
                query.TEN_LOP_HOCPHAN = xdt.Rows[0]["TEN_LOP_HOCPHAN"].ToString();
                query.TUAN_BD = Convert.ToInt32(xdt.Rows[0]["TUAN_BD"]);
                query.TUAN_KT = Convert.ToInt32(xdt.Rows[0]["TUAN_KT"]);
                query.SOLUONG = Convert.ToInt32(xdt.Rows[0]["SOLUONG"]);
                query.CACH_TINHDIEM = xdt.Rows[0]["CACH_TINHDIEM"].ToString();
                query.UPDATE_USER = xdt.Rows[0]["USER"].ToString();
                query.UPDATE_TIME = DateTime.Now;
                db.SubmitChanges();
                i = query.ID_LOPHOCPHAN;
                return i;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public int DeleteObject(int pID_LOPHOCPHAN, string user)
        {
            try
            {
                int i = 0;
                tbl_LOP_HOCPHAN query = (from d in db.tbl_LOP_HOCPHANs
                                   where
                                       d.ID_LOPHOCPHAN == pID_LOPHOCPHAN
                                   select d).FirstOrDefault();
                query.IS_DELETE = 1;
                query.UPDATE_USER = user;
                query.UPDATE_TIME = DateTime.Now;
                db.SubmitChanges();
                i = query.ID_LOPHOCPHAN;
                return i;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public bool Check_MaHocPhan(string pMA_LOP_HOCPHAN)
        {
            try
            {
                var query = (from d in db.tbl_LOP_HOCPHANs
                    where d.MA_LOP_HOCPHAN == pMA_LOP_HOCPHAN && (d.IS_DELETE !=1 || d.IS_DELETE == null)
                    select new {d.ID_LOPHOCPHAN});
                DataTable xdt = TableUtil.LinqToDataTable(query);
                if (xdt.Rows.Count > 0)
                    return true;
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int Get_Thso_MaHP(int pID_MONHOC, int pID_NAMHOC_HKY_HTAI)
        {
            try
            {
                var m = from Tbl_LOP_HOCPHAN in
                    (from Tbl_LOP_HOCPHAN in db.tbl_LOP_HOCPHANs
                        where
                            (Tbl_LOP_HOCPHAN.IS_DELETE != 1 ||
                             Tbl_LOP_HOCPHAN.IS_DELETE == null) &&
                            Convert.ToInt64(Tbl_LOP_HOCPHAN.ID_NAMHOC_HKY_HTAI) == pID_NAMHOC_HKY_HTAI &&
                            Tbl_LOP_HOCPHAN.ID_MONHOC == pID_MONHOC
                        select new
                        {
                            tmp = "x"
                        })
                        group Tbl_LOP_HOCPHAN by new { Tbl_LOP_HOCPHAN.tmp }
                    into g
                    select new
                    {
                        n = g.Count()
                    };
                DataTable xdt = TableUtil.LinqToDataTable(m);
                if(xdt!=null && xdt.Rows.Count>0)
                    return Convert.ToInt32(xdt.Rows[0]["n"]);
                return 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable Getall_NamHocHT()
        {
            var query = from d in db.tbl_NAMHOC_HIENTAIs
                        where
                          d.IS_DELETE != 1 ||
                          d.IS_DELETE == null
                        select new
                        {
                            d.ID_NAMHOC_HIENTAI,
                            d.NAMHOC_TU,
                            d.NAMHOC_DEN,
                            d.NGAY_BATDAU,
                            d.SO_TUAN,
                            d.SO_HKY_TRONGNAM,
                            d.IS_HIENTAI,
                        };
            DataTable xdt = null;
            xdt = TableUtil.LinqToDataTable(query);
            return xdt;
        }

        public DataTable Getall_NamHoc_hkyHT()
        {
            var query = from d in db.tbl_NAMHOC_HKY_HTAIs
                        join m in db.tbl_NAMHOC_HIENTAIs on new { ID_NAMHOC_HIENTAI = Convert.ToInt32(d.ID_NAMHOC_HIENTAI) } equals new { ID_NAMHOC_HIENTAI = m.ID_NAMHOC_HIENTAI } into m_join
                        from m in m_join.DefaultIfEmpty()
                        where
                          d.IS_DELETE != 1 ||
                          d.IS_DELETE == null
                        select new
                        {
                            d.ID_NAMHOC_HKY_HTAI,
                            d.HOCKY,
                            NAMHOC_TU = (int?)m.NAMHOC_TU,
                            NAMHOC_DEN = (int?)m.NAMHOC_DEN,
                            SO_TUAN = (int?)m.SO_TUAN,
                            SO_HKY_TRONGNAM = (int?)m.SO_HKY_TRONGNAM
                        };
            DataTable xdt = null;
            xdt = TableUtil.LinqToDataTable(query);
            return xdt;
        }

        public DataTable Getall_lopHocPhan()
        {
            var query = from d in db.tbl_LOP_HOCPHANs
                        where
                          d.IS_DELETE != 1 ||
                          d.IS_DELETE == null
                        select new
                        {
                            d.ID_LOPHOCPHAN,
                            d.ID_LOPHOC,
                            d.ID_KHOAHOC_NGANH_CTIET,
                            d.ID_NAMHOC_HKY_HTAI,
                            d.ID_HEDAOTAO,
                            d.ID_MONHOC,
                            d.MA_LOP_HOCPHAN,
                            d.TEN_LOP_HOCPHAN,
                            d.TUAN_BD,
                            d.TUAN_KT,
                            d.SOTIET,
                            d.SOLUONG,
                            d.ID_GIANGVIEN,
                            d.CACH_TINHDIEM
                        };
            DataTable xdt = null;
            xdt = TableUtil.LinqToDataTable(query);
            return xdt;
        }

        public DataTable GetAll_hocphan_ds(int pID_NAMHOC_HKY_HTAI)
        {
            var query = from Tbl_LOP_HOCPHAN in db.tbl_LOP_HOCPHANs
                        where
                            (Tbl_LOP_HOCPHAN.IS_DELETE != 1 ||
                             Tbl_LOP_HOCPHAN.IS_DELETE == null)
                             && Tbl_LOP_HOCPHAN.ID_NAMHOC_HKY_HTAI == pID_NAMHOC_HKY_HTAI
                        orderby Tbl_LOP_HOCPHAN.ID_MONHOC
                        select new
                        {
                            Tbl_LOP_HOCPHAN.ID_LOPHOCPHAN,
                            Tbl_LOP_HOCPHAN.ID_LOPHOC,
                            Tbl_LOP_HOCPHAN.ID_KHOAHOC_NGANH_CTIET,
                            Tbl_LOP_HOCPHAN.ID_NAMHOC_HKY_HTAI,
                            Tbl_LOP_HOCPHAN.ID_HEDAOTAO,
                            Tbl_LOP_HOCPHAN.ID_MONHOC,
                            Tbl_LOP_HOCPHAN.MA_LOP_HOCPHAN,
                            Tbl_LOP_HOCPHAN.TEN_LOP_HOCPHAN,
                            Tbl_LOP_HOCPHAN.TUAN_BD,
                            Tbl_LOP_HOCPHAN.TUAN_KT,
                            Tbl_LOP_HOCPHAN.SOTIET,
                            Tbl_LOP_HOCPHAN.SOLUONG,
                            Tbl_LOP_HOCPHAN.ID_GIANGVIEN,
                            TEN_GIANGVIEN = (from Tbl_GIANGVIEN in db.tbl_GIANGVIENs
                                             where
                                                 (Tbl_GIANGVIEN.IS_DELETE != 1 ||
                                                  Tbl_GIANGVIEN.IS_DELETE == null) &&
                                                 Tbl_GIANGVIEN.ID_GIANGVIEN == 1
                                             select new
                                             {
                                                 Tbl_GIANGVIEN.TEN_GIANGVIEN
                                             }).First().TEN_GIANGVIEN,
                            Tbl_LOP_HOCPHAN.CACH_TINHDIEM
                        }
                        ;
            DataTable xdt = null;
            xdt = TableUtil.LinqToDataTable(query);
            return xdt;
        }

        public DataTable GetAll_monhoc_all()
        {
            var query = from d in db.tbl_MONHOCs
                        where
                          d.IS_DELETE != 1 ||
                          d.IS_DELETE == null
                        select new
                        {
                            d.ID_MONHOC,
                            d.ID_LOAI_MONHOC,
                            d.ID_BOMON,
                            d.MA_MONHOC,
                            d.TEN_MONHOC,
                            d.KY_HIEU,
                            d.SO_TC,
                            d.IS_THUHOCPHI,
                            d.IS_THUCHANH,
                            d.IS_LYTHUYET,
                            d.IS_TINHDIEM,
                            d.TRANGTHAI,
                            d.ISBATBUOC,
                            d.ID_MONHOC_SONGHANH,
                            d.GHICHU
                        };
            DataTable xdt = null;
            xdt = TableUtil.LinqToDataTable(query);
            return xdt;
        }

        public DataTable GetAll_TUANHOC()
        {
            var query = from Tbl_NAMHOC_HKY_HTAI in db.tbl_NAMHOC_HKY_HTAIs
                        where
                          Tbl_NAMHOC_HKY_HTAI.ID_NAMHOC_HIENTAI ==
                            ((from Tbl_NAMHOC_HIENTAI in db.tbl_NAMHOC_HIENTAIs
                              where
                                Tbl_NAMHOC_HIENTAI.IS_HIENTAI == 1 &&
                                (Tbl_NAMHOC_HIENTAI.IS_DELETE != 1 ||
                                Tbl_NAMHOC_HIENTAI.IS_DELETE == null)
                              select new
                              {
                                  Tbl_NAMHOC_HIENTAI.ID_NAMHOC_HIENTAI
                              }).First().ID_NAMHOC_HIENTAI) &&
                          Tbl_NAMHOC_HKY_HTAI.HOCKY == 2
                        select new
                        {
                            Tbl_NAMHOC_HKY_HTAI.SO_TUAN,
                            Tbl_NAMHOC_HKY_HTAI.TUAN_BDAU_HKY
                        };
            DataTable xdt = null;
            xdt = TableUtil.LinqToDataTable(query);
            return xdt;
        }

        public DataTable GetAll_NAMHOC()
        {
            var query = from Tbl_NAMHOC_HIENTAI in db.tbl_NAMHOC_HIENTAIs
                        where
                          Tbl_NAMHOC_HIENTAI.IS_DELETE != 1 ||
                          Tbl_NAMHOC_HIENTAI.IS_DELETE == null
                        select new
                        {
                            NAMHOC_CHU = Tbl_NAMHOC_HIENTAI.NAMHOC_TU + "-" +  Tbl_NAMHOC_HIENTAI.NAMHOC_DEN,
                            Tbl_NAMHOC_HIENTAI.NAMHOC_TU
                        };
            DataTable xdt = null;
            xdt = TableUtil.LinqToDataTable(query);
            return xdt;
        }

        public DataTable GetAll_HOCKY_ByNH( int namhoc)
        {
            var query = from d in db.tbl_NAMHOC_HKY_HTAIs
                        join m in db.tbl_NAMHOC_HIENTAIs on new { ID_NAMHOC_HIENTAI = Convert.ToInt32(d.ID_NAMHOC_HIENTAI) } equals new { ID_NAMHOC_HIENTAI = m.ID_NAMHOC_HIENTAI } into m_join
                        from m in m_join.DefaultIfEmpty()
                        where
                          (d.IS_DELETE != 1 ||
                          d.IS_DELETE == null) &&
                          (m.IS_DELETE != 1 ||
                          m.IS_DELETE == null) &&
                          m.NAMHOC_TU == namhoc
                        select new
                        {
                            d.HOCKY,
                            TENHOCKY = d.HOCKY
                        };
            DataTable xdt = null;
            xdt = TableUtil.LinqToDataTable(query);
            return xdt;
        }

        public DataTable GetAll_HKYHT(int namhoc, int hocky)
        {
            var query = from d in db.tbl_NAMHOC_HKY_HTAIs
                        join m in db.tbl_NAMHOC_HIENTAIs on new { ID_NAMHOC_HIENTAI = Convert.ToInt32(d.ID_NAMHOC_HIENTAI) } equals new { ID_NAMHOC_HIENTAI = m.ID_NAMHOC_HIENTAI } into m_join
                        from m in m_join.DefaultIfEmpty()
                        where
                          (d.IS_DELETE != 1 ||
                          d.IS_DELETE == null) &&
                          (m.IS_DELETE != 1 ||
                          m.IS_DELETE == null) &&
                          m.NAMHOC_TU == namhoc &&
                          d.HOCKY == hocky
                        select new
                        {
                            d.ID_NAMHOC_HKY_HTAI,
                            d.HOCKY,
                            d.TUAN_BDAU_HKY,
                            d.SO_TUAN,
                            m.NAMHOC_TU
                        };
            DataTable xdt = null;
            xdt = TableUtil.LinqToDataTable(query);
            return xdt;
        }

        public int Insert_NamHocHT_Excel(DataTable idatasource, string pUser)
        {
            try
            {
                int i = 0;
                foreach (DataRow dr in idatasource.Rows)
                {
                    tbl_NAMHOC_HIENTAI query = new tbl_NAMHOC_HIENTAI
                    {
                        NAMHOC_TU = Convert.ToInt32(dr["f_namhoc0"].ToString()),
                        NAMHOC_DEN = Convert.ToInt32(dr["f_namhoc0"].ToString()) + 1 ,
                        CREATE_USER = pUser,
                        CREATE_TIME = DateTime.Now,
                        IS_DELETE = 0
                    };
                    db.tbl_NAMHOC_HIENTAIs.InsertOnSubmit(query);
                    db.SubmitChanges();
                    i = query.ID_NAMHOC_HIENTAI;
                }
                return i;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public int Insert_HkyHT_Excel(DataTable idatasource, string pUser)
        {
            try
            {
                int i = 0;
                DataTable xdt = Getall_NamHocHT();
                foreach (DataRow dr in idatasource.Rows)
                {
                    tbl_NAMHOC_HKY_HTAI query = new tbl_NAMHOC_HKY_HTAI
                    {
                        HOCKY = Convert.ToInt32(dr["f_hockythu"].ToString()),
                        ID_NAMHOC_HIENTAI = Convert.ToInt32((xdt.Select("NAMHOC_TU = " + dr["f_namhoc0"].ToString()))[0]["ID_NAMHOC_HIENTAI"].ToString()),
                        CREATE_USER = pUser,
                        CREATE_TIME = DateTime.Now,
                        IS_DELETE = 0
                    };
                    db.tbl_NAMHOC_HKY_HTAIs.InsertOnSubmit(query);
                    db.SubmitChanges();
                    i = query.ID_NAMHOC_HKY_HTAI;
                }
                return i;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public int Insert_lhp_Excel(DataTable idatasource, string pUser)
        {
            try
            {
                int i = 0;
                DataTable xdt = Getall_NamHoc_hkyHT();
                DataTable xdt_monhoc = GetAll_monhoc_all();
                foreach (DataRow dr in idatasource.Rows)
                {
                    string cachtinhdiem = "";
                    if (Convert.ToInt32(dr["f_phtrambt"].ToString()) == 0 &&
                        Convert.ToInt32(dr["f_phtramkt"].ToString()) == 0)
                    {
                        cachtinhdiem = "0-0-100";
                    }
                    else
                    {
                        cachtinhdiem = dr["f_phtrambt"].ToString() + "-" + dr["f_phtramkt"].ToString() + ( 100 - ( Convert.ToInt32(dr["f_phtrambt"].ToString()) + Convert.ToInt32(dr["f_phtramkt"].ToString()) ) ).ToString();
                    }
                    int id_namhoc_hky = 0;
                    if (!string.IsNullOrEmpty(dr["f_namhoc0"].ToString()))
                    {
                        id_namhoc_hky =
                            Convert.ToInt32(
                                (xdt.Select("NAMHOC_TU = " + dr["f_namhoc0"].ToString() + " and HOCKY = " +
                                            dr["f_hockythu"].ToString()))[0]["ID_NAMHOC_HKY_HTAI"].ToString());
                    }
                    tbl_LOP_HOCPHAN query = new tbl_LOP_HOCPHAN
                    {
                        ID_NAMHOC_HKY_HTAI = id_namhoc_hky,
                        ID_MONHOC = Convert.ToInt32((xdt_monhoc.Select("MA_MONHOC = '" + dr["f_mamh"].ToString() + "'"))[0]["ID_MONHOC"].ToString()),
                        MA_LOP_HOCPHAN = dr["f_mamhhtd"].ToString(),
                        TEN_LOP_HOCPHAN = dr["f_tenmhvn"].ToString(),
                        CACH_TINHDIEM = cachtinhdiem,
                        CREATE_USER = pUser,
                        CREATE_TIME = DateTime.Now,
                        IS_DELETE = 0
                    };
                    db.tbl_LOP_HOCPHANs.InsertOnSubmit(query);
                    db.SubmitChanges();
                    i = query.ID_LOPHOCPHAN;
                }
                return i;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public DataTable GetAll_TT_byHP(int pID_LOPHOCPHAN)
        {
            var query = from hp in db.tbl_LOP_HOCPHANs
                        join ct in db.tbl_KHOAHOC_NGANH_CTIETs on new { ID_KHOAHOC_NGANH_CTIET = Convert.ToInt32(hp.ID_KHOAHOC_NGANH_CTIET) } equals new { ID_KHOAHOC_NGANH_CTIET = ct.ID_KHOAHOC_NGANH_CTIET } into ct_join
                        from ct in ct_join.DefaultIfEmpty()
                        join khn in db.tbl_KHOAHOC_NGANHs on new { ID_KHOAHOC_NGANH = Convert.ToInt32(ct.ID_KHOAHOC_NGANH) } equals new { ID_KHOAHOC_NGANH = khn.ID_KHOAHOC_NGANH } into khn_join
                        from khn in khn_join.DefaultIfEmpty()
                        join kh in db.tbl_KHOAHOCs on new { ID_KHOAHOC = Convert.ToInt32(khn.ID_KHOAHOC) } equals new { ID_KHOAHOC = kh.ID_KHOAHOC } into kh_join
                        from kh in kh_join.DefaultIfEmpty()
                        join n in db.tbl_NGANHs on new { ID_NGANH = Convert.ToInt32(khn.ID_NGANH) } equals new { ID_NGANH = n.ID_NGANH } into n_join
                        from n in n_join.DefaultIfEmpty()
                        join hdt in db.tbl_HEDAOTAOs on new { ID_HE_DAOTAO = Convert.ToInt32(hp.ID_HEDAOTAO) } equals new { ID_HE_DAOTAO = hdt.ID_HE_DAOTAO } into hdt_join
                        from hdt in hdt_join.DefaultIfEmpty()
                        join mh in db.tbl_MONHOCs on new { ID_MONHOC = Convert.ToInt32(hp.ID_MONHOC) } equals new { ID_MONHOC = mh.ID_MONHOC } into mh_join
                        from mh in mh_join.DefaultIfEmpty()
                        join hk in db.tbl_NAMHOC_HKY_HTAIs on new { ID_NAMHOC_HKY_HTAI = Convert.ToInt32(hp.ID_NAMHOC_HKY_HTAI) } equals new { ID_NAMHOC_HKY_HTAI = hk.ID_NAMHOC_HKY_HTAI } into hk_join
                        from hk in hk_join.DefaultIfEmpty()
                        join nh in db.tbl_NAMHOC_HIENTAIs on new { ID_NAMHOC_HIENTAI = Convert.ToInt32(hk.ID_NAMHOC_HIENTAI) } equals new { ID_NAMHOC_HIENTAI = nh.ID_NAMHOC_HIENTAI } into nh_join
                        from nh in nh_join.DefaultIfEmpty()
                        where hp.ID_LOPHOCPHAN == pID_LOPHOCPHAN
                        select new
                        {
                            hp.ID_LOPHOCPHAN,
                            hp.ID_LOPHOC,
                            hp.ID_KHOAHOC_NGANH_CTIET,
                            hp.ID_NAMHOC_HKY_HTAI,
                            hp.ID_HEDAOTAO,
                            hp.ID_MONHOC,
                            hp.MA_LOP_HOCPHAN,
                            hp.TEN_LOP_HOCPHAN,
                            hp.TUAN_BD,
                            hp.TUAN_KT,
                            hp.SOTIET,
                            hp.SOLUONG,
                            hp.CACH_TINHDIEM,
                            hp.SO_TUAN,
                            SO_TC = (int?)ct.SO_TC,
                            SOTIET_LT = (int?)ct.SOTIET_LT,
                            SOTIET_TH = (int?)ct.SOTIET_TH,
                            ID_KHOAHOC = (int?)khn.ID_KHOAHOC,
                            ID_NGANH = (int?)khn.ID_NGANH,
                            SO_HKY = (int?)khn.SO_HKY,
                            SO_SINHVIEN_DK = (int?)khn.SO_SINHVIEN_DK,
                            SO_LOP = (int?)khn.SO_LOP,
                            TEN_KHOAHOC = kh.TEN_KHOAHOC,
                            NAM_BD = (int?)kh.NAM_BD,
                            NAM_KT = (int?)kh.NAM_KT,
                            ID_KHOA = (int?)n.ID_KHOA,
                            MA_NGANH = n.MA_NGANH,
                            TEN_NGANH = n.TEN_NGANH,
                            ID_BAC_DAOTAO = (int?)hdt.ID_BAC_DAOTAO,
                            ID_LOAIHINH_DTAO = (int?)hdt.ID_LOAIHINH_DTAO,
                            TEN_HEDAOTAO = hdt.TEN_HE_DAOTAO,
                            ID_LOAI_MONHOC = (int?)mh.ID_LOAI_MONHOC,
                            ID_BOMON = (int?)mh.ID_BOMON,
                            MA_MONHOC = mh.MA_MONHOC,
                            TEN_MONHOC = mh.TEN_MONHOC,
                            IS_THUHOCPHI = (int?)mh.IS_THUHOCPHI,
                            IS_THUCHANH = (int?)mh.IS_THUCHANH,
                            IS_LYTHUYET = (int?)mh.IS_LYTHUYET,
                            IS_TINHDIEM = (int?)mh.IS_TINHDIEM,
                            hk.HOCKY,
                            NAMHOC = nh.NAMHOC_TU
                        };
            DataTable xdt = null;
            xdt = TableUtil.LinqToDataTable(query);
            return xdt;
        }
    }
}

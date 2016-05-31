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
    }
}

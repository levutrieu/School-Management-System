using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DATN.TTS.DATA;

namespace DATN.TTS.BUS
{
    public class bus_ID3
    {
        db_ttsDataContext db = new db_ttsDataContext();

        public DataTable GetAllDiem(int id_sinhvien)
        {
            try
            {
                DataTable dtRes = null;
                var query = from d in db.tbl_DIEM_SINHVIENs
                            where
                                d.IS_DELETE != 1 ||
                                d.IS_DELETE == null
                            select new
                            {
                                d.ID_KETQUA,
                                d.ID_SINHVIEN,
                                d.ID_LOPHOCPHAN,
                                TEN_MONHOC = ((from n in db.tbl_LOP_HOCPHANs
                                               join m in db.tbl_MONHOCs on new { ID_MONHOC = Convert.ToInt32(n.ID_MONHOC) } equals
                                                   new { ID_MONHOC = m.ID_MONHOC } into m_join
                                               from m in m_join.DefaultIfEmpty()
                                               where
                                                   (n.IS_DELETE != 1 ||
                                                    n.IS_DELETE == null) &&
                                                   (m.IS_DELETE != 1 ||
                                                    m.IS_DELETE == null) &&
                                                    (n.ID_LOPHOCPHAN == 329 || n.ID_LOPHOCPHAN == 330 || n.ID_LOPHOCPHAN == 331)
                                               select new
                                               {
                                                   TEN_MONHOC = m.TEN_MONHOC
                                               }).First().TEN_MONHOC),
                                d.DIEM_TONG
                            };
                dtRes = TableUtil.LinqToDataTable(query);
                return dtRes;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable Get_TTSV(string masinhvien)
        {
            try
            {
                DataTable dtRes = null;
                var query = from SV in db.tbL_SINHVIENs
                            join LH in db.tbl_LOPHOCs on new { ID_LOPHOC = Convert.ToInt32(SV.ID_LOPHOC) } equals new { ID_LOPHOC = LH.ID_LOPHOC } into LH_join
                            from LH in LH_join.DefaultIfEmpty()
                            join N in db.tbl_KHOAHOC_NGANHs on new { ID_KHOAHOC_NGANH = Convert.ToInt32(LH.ID_KHOAHOC_NGANH) } equals new { ID_KHOAHOC_NGANH = N.ID_KHOAHOC_NGANH } into N_join
                            from N in N_join.DefaultIfEmpty()
                            join KH in db.tbl_KHOAHOCs on new { ID_KHOAHOC = Convert.ToInt32(N.ID_KHOAHOC) } equals new { ID_KHOAHOC = KH.ID_KHOAHOC } into KH_join
                            from KH in KH_join.DefaultIfEmpty()
                            where
                              SV.MA_SINHVIEN == masinhvien && (SV.IS_DELETE != 1 || SV.IS_DELETE == null)
                            select new
                            {
                                ID_KHOAHOC_NGANH = (int?)LH.ID_KHOAHOC_NGANH,
                                NAM_BD = (int?)KH.NAM_BD,
                                ID_HE_DAOTAO = (int?)KH.ID_HE_DAOTAO,
                                ID_NGANH = (int?)N.ID_NGANH
                            };
                dtRes = TableUtil.LinqToDataTable(query);
                return dtRes;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetAll_SVKhoaTruoc(DataTable iDataSource)
        {
            try
            {
                DataTable dtRes = null;
                var query = from sv in db.tbL_SINHVIENs
                            join lh in db.tbl_LOPHOCs on new { ID_LOPHOC = Convert.ToInt32(sv.ID_LOPHOC) } equals new { ID_LOPHOC = lh.ID_LOPHOC } into lh_join
                            from lh in lh_join.DefaultIfEmpty()
                            where
                                (from n in db.tbl_KHOAHOC_NGANHs
                                 join kh in db.tbl_KHOAHOCs on new { ID_KHOAHOC = Convert.ToInt32(n.ID_KHOAHOC) } equals new { ID_KHOAHOC = kh.ID_KHOAHOC } into kh_join
                                 from kh in kh_join.DefaultIfEmpty()
                                 where
                                   kh.ID_HE_DAOTAO == Convert.ToInt32(iDataSource.Rows[0]["ID_HE_DAOTAO"]) &&
                                   kh.NAM_BD < Convert.ToInt32(iDataSource.Rows[0]["NAM_BD"]) &&
                                   n.ID_NGANH == Convert.ToInt32(iDataSource.Rows[0]["ID_NGANH"]) &&
                                   (n.IS_DELETE != 1 ||
                                   n.IS_DELETE == null) &&
                                   (kh.IS_DELETE != 1 ||
                                   kh.IS_DELETE == null)
                                 select new
                                 {
                                     n.ID_KHOAHOC_NGANH
                                 }).Contains(new { ID_KHOAHOC_NGANH = (System.Int32)lh.ID_KHOAHOC_NGANH })
                            select new
                            {
                                sv.ID_SINHVIEN
                            };
                dtRes = TableUtil.LinqToDataTable(query);
                return dtRes;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetAll_MH_Mau(int pID_KHOAHOC_NGANH, int pHOCKY)
        {
            try
            {
                DataTable dtRes = null;
                var query = from Tbl_KHOAHOC_NGANH_CTIET in db.tbl_KHOAHOC_NGANH_CTIETs
                            where
                              Tbl_KHOAHOC_NGANH_CTIET.ID_KHOAHOC_NGANH == pID_KHOAHOC_NGANH &&
                              Convert.ToInt64(Tbl_KHOAHOC_NGANH_CTIET.HOCKY) == pHOCKY - 1
                            select new
                            {
                                Tbl_KHOAHOC_NGANH_CTIET.ID_MONHOC
                            };
                dtRes = TableUtil.LinqToDataTable(query);
                return dtRes;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetAll_KhoaHoc_nganh_mau(DataTable iDataSource)
        {
            try
            {
                DataTable dtRes = null;
                var query = from n in db.tbl_KHOAHOC_NGANHs
                            join kh in db.tbl_KHOAHOCs on new { ID_KHOAHOC = Convert.ToInt32(n.ID_KHOAHOC) } equals new { ID_KHOAHOC = kh.ID_KHOAHOC } into kh_join
                            from kh in kh_join.DefaultIfEmpty()
                            where
                              n.ID_NGANH == Convert.ToInt32(iDataSource.Rows[0]["ID_NGANH"]) &&
                              kh.ID_HE_DAOTAO == Convert.ToInt32(iDataSource.Rows[0]["ID_HE_DAOTAO"]) &&
                              kh.NAM_BD < Convert.ToInt32(iDataSource.Rows[0]["NAM_BD"]) &&
                              (kh.IS_DELETE != 1 ||
                              kh.IS_DELETE == null) &&
                              (n.IS_DELETE != 1 ||
                              n.IS_DELETE == null)
                            select new
                            {
                                n.ID_KHOAHOC_NGANH
                            };
                dtRes = TableUtil.LinqToDataTable(query);
                return dtRes;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetAll_Lop_hocphan_mau(DataTable iDataSource, int pHOCKY)
        {
            try
            {
                DataTable dsmh = GetAll_MH_Mau(Convert.ToInt32(iDataSource.Rows[0]["ID_KHOAHOC_NGANH"]), 3);
                DataTable dsKhN = GetAll_KhoaHoc_nganh_mau(iDataSource);
                DataTable dtRes = null;
                foreach (DataRow dr in dsKhN.Rows)
                {
                    var query =
                        from n in db.tbl_LOP_HOCPHANs
                        where
                            (from m in db.tbl_KHOAHOC_NGANH_CTIETs
                                where
                                    m.ID_KHOAHOC_NGANH == 1 &&
                                    (from d in db.tbl_KHOAHOC_NGANH_CTIETs
                                        where
                                            d.ID_KHOAHOC_NGANH ==
                                            Convert.ToInt32(iDataSource.Rows[0]["ID_KHOAHOC_NGANH"]) &&
                                            Convert.ToInt64(m.HOCKY) == pHOCKY - 1
                                        select new
                                        {
                                            d.ID_MONHOC
                                        }).Contains(new {ID_MONHOC = m.ID_MONHOC})
                                select new
                                {
                                    m.ID_KHOAHOC_NGANH_CTIET
                                }).Contains(new {ID_KHOAHOC_NGANH_CTIET = (int)n.ID_KHOAHOC_NGANH_CTIET})
                        select new
                        {
                            n.ID_LOPHOCPHAN
                        };
                    DataTable idata = TableUtil.LinqToDataTable(query);
                    dtRes = idata.Copy();
                }
                return dtRes;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetDiemMau()
        {
            try
            {
                DataTable dt_result = new DataTable();
                DataTable dtRes_sv = null;
                DataTable dtRes_mh = null;
                //var query = from d in db.tbL_SINHVIENs
                //            where
                //                d.IS_DELETE != 1 ||
                //                d.IS_DELETE == null
                //            select new
                //            {
                //                d.ID_SINHVIEN,
                //            };
                //dtRes_sv = TableUtil.LinqToDataTable(query);
                dtRes_sv = GetAll_SVKhoaTruoc(Get_TTSV("2001120021"));

                var query_mh = from d in db.tbl_LOP_HOCPHANs
                               where

                                   (from Tbl_KHOAHOC_NGANH_CTIET in db.tbl_KHOAHOC_NGANH_CTIETs
                                    where
                                        Tbl_KHOAHOC_NGANH_CTIET.ID_KHOAHOC_NGANH == 1 &&
                                        Convert.ToInt64(Tbl_KHOAHOC_NGANH_CTIET.HOCKY) == 2
                                    select new
                                    {
                                        Tbl_KHOAHOC_NGANH_CTIET.ID_MONHOC
                                    }).Contains(new { ID_MONHOC = d.ID_MONHOC })
                               select new
                               {
                                   d.ID_LOPHOCPHAN
                               };
                dtRes_mh = TableUtil.LinqToDataTable(query_mh);
                dtRes_mh = GetAll_Lop_hocphan_mau(Get_TTSV("2001120021"), 3);
                foreach (DataRow dr in dtRes_mh.Rows)
                {
                    dt_result.Columns.Add(dr[0].ToString(), typeof(double));
                }


                foreach (DataRow dr in dtRes_sv.Rows)
                {
                    var get_kq = from m in db.tbl_DIEM_SINHVIENs
                                 where
                                     m.ID_SINHVIEN == Convert.ToInt32(dr["ID_SINHVIEN"]) &&

                                     (from d in db.tbl_LOP_HOCPHANs
                                      where

                                          (from Tbl_KHOAHOC_NGANH_CTIET in db.tbl_KHOAHOC_NGANH_CTIETs
                                           where
                                               Tbl_KHOAHOC_NGANH_CTIET.ID_KHOAHOC_NGANH == 1 &&
                                               Convert.ToInt64(Tbl_KHOAHOC_NGANH_CTIET.HOCKY) == 2
                                           select new
                                           {
                                               Tbl_KHOAHOC_NGANH_CTIET.ID_MONHOC
                                           }).Contains(new { ID_MONHOC = d.ID_MONHOC })
                                      select new
                                      {
                                          d.ID_LOPHOCPHAN
                                      }).Contains(new { ID_LOPHOCPHAN = (System.Int32)m.ID_LOPHOCPHAN })
                                 select new
                                 {
                                     m.ID_LOPHOCPHAN,
                                     m.DIEM_TONG
                                 };
                    DataTable ximport = TableUtil.LinqToDataTable(get_kq);

                    DataRow dtrow = dt_result.NewRow();
                    foreach (DataRow xdr in ximport.Rows)
                    {
                        for (int i = 0; i < dt_result.Columns.Count; i++)
                        {
                            if (xdr["ID_LOPHOCPHAN"].ToString().Trim().Equals(dt_result.Columns[i].ColumnName))
                            {
                                dtrow[dt_result.Columns[i].ColumnName] = Convert.ToDouble(xdr["DIEM_TONG"]);
                            }
                        }
                    }
                    for (int n = 0; n < dt_result.Columns.Count; n++)
                    {
                        if (string.IsNullOrEmpty(dtrow[n].ToString()))
                        {
                            dtrow[n] = 0;
                        }
                    }
                    dt_result.Rows.Add(dtrow);
                }
                return dt_result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

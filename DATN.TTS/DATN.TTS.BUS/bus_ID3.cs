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
                                SV.ID_SINHVIEN,
                                ID_KHOAHOC_NGANH = (int?)LH.ID_KHOAHOC_NGANH,
                                NAM_BD = (int?)KH.NAM_BD,
                                ID_HE_DAOTAO = (int?)KH.ID_HE_DAOTAO,
                                ID_NGANH = (int?)N.ID_NGANH,
                                ID_KHOAHOC = N.ID_KHOAHOC
                            };
                dtRes = TableUtil.LinqToDataTable(query);

                #region Lay hoc ky hien tai (1 -> 8) theo khoa hoc
                var query1 = from Tbl_KHOAHOC in db.tbl_KHOAHOCs
                             where
                                 Tbl_KHOAHOC.ID_KHOAHOC == Convert.ToInt32(dtRes.Rows[0]["ID_KHOAHOC"])
                             select new
                             {
                                 Tbl_KHOAHOC.NAM_BD
                             };
                DataTable xdt = null;
                xdt = TableUtil.LinqToDataTable(query1);
                DataTable iGridData_TTHK = null;
                iGridData_TTHK = GetAll_Tso_Hocky();
                int hkht = 0;
                if (xdt != null && xdt.Rows.Count > 0)
                {
                    int hk = Convert.ToInt32(iGridData_TTHK.Rows[0]["HOCKY"]);
                    if (hk == 3)
                    {
                        hk = 2;
                    }
                    hkht = (Convert.ToInt32(iGridData_TTHK.Rows[0]["NAMHOC_TU"]) -
                                Convert.ToInt32(xdt.Rows[0]["NAM_BD"])) * 2 + hk;
                    dtRes.Columns.Add("HOCKY", typeof(int));
                    dtRes.Rows[0]["HOCKY"] = hkht;
                }
                #endregion

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
                                  }).Contains(new { ID_MONHOC = m.ID_MONHOC })
                             select new
                             {
                                 m.ID_KHOAHOC_NGANH_CTIET
                             }).Contains(new { ID_KHOAHOC_NGANH_CTIET = (int)n.ID_KHOAHOC_NGANH_CTIET })
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

        public DataTable GetDiemMau(string Masinhvien, int IDMonTuVan)
        {
            try
            {
                DataTable dt_result = new DataTable(); // Bang diem mau
                DataTable dtRes_sv = null; // Danh sach sinh vien nhung khoa truoc
                DataTable dtRes_mh = null; // Danh sach mon hoc cua hoc ky truoc
                DataTable dt_TTSV = null;  // Thong tin sinh vien
                dt_TTSV = Get_TTSV(Masinhvien);
                dtRes_sv = GetAll_SVKhoaTruoc(dt_TTSV);
                dtRes_mh = GetAll_MH_Mau(Convert.ToInt32(dt_TTSV.Rows[0]["ID_KHOAHOC_NGANH"]),
                    Convert.ToInt32(dt_TTSV.Rows[0]["HOCKY"]));

                #region Them cot diem mon dang xet

                DataRow idr = dtRes_mh.NewRow();
                idr["ID_MONHOC"] = IDMonTuVan;
                dtRes_mh.Rows.Add(idr);

                #endregion

                foreach (DataRow dr in dtRes_mh.Rows)
                {
                    dt_result.Columns.Add(dr[0].ToString(), typeof(double));
                }


                foreach (DataRow dr in dtRes_sv.Rows)
                {
                    #region Ket qua nhung mon hoc mau

                    var get_kq = from diem in db.tbl_DIEM_SINHVIENs
                                 join hp in db.tbl_LOP_HOCPHANs on new { ID_LOPHOCPHAN = Convert.ToInt32(diem.ID_LOPHOCPHAN) }
                                     equals
                                     new { ID_LOPHOCPHAN = hp.ID_LOPHOCPHAN } into hp_join
                                 from hp in hp_join.DefaultIfEmpty()
                                 join ct in db.tbl_KHOAHOC_NGANH_CTIETs on
                                     new { ID_KHOAHOC_NGANH_CTIET = Convert.ToInt32(hp.ID_KHOAHOC_NGANH_CTIET) } equals
                                     new { ID_KHOAHOC_NGANH_CTIET = ct.ID_KHOAHOC_NGANH_CTIET } into ct_join
                                 from ct in ct_join.DefaultIfEmpty()
                                 where
                                     diem.ID_SINHVIEN == Convert.ToInt32(dr["ID_SINHVIEN"]) &&
                                 #region Ds mon hoc cua ky truoc
                                    (
                                        (from Tbl_KHOAHOC_NGANH_CTIET in db.tbl_KHOAHOC_NGANH_CTIETs
                                         where
                                             Tbl_KHOAHOC_NGANH_CTIET.ID_KHOAHOC_NGANH ==
                                             Convert.ToInt32(dt_TTSV.Rows[0]["ID_KHOAHOC_NGANH"]) &&
                                             Tbl_KHOAHOC_NGANH_CTIET.HOCKY == (Convert.ToInt32(dt_TTSV.Rows[0]["HOCKY"]) - 1) &&
                                             (Tbl_KHOAHOC_NGANH_CTIET.IS_DELETE != 1 || Tbl_KHOAHOC_NGANH_CTIET.IS_DELETE == null)
                                         select new
                                         {
                                             Tbl_KHOAHOC_NGANH_CTIET.ID_MONHOC
                                         }).Distinct().Contains(new { ID_MONHOC = hp.ID_MONHOC }))
                                 #endregion
                                     && ct.ID_KHOAHOC_NGANH ==

                                                                     #region Khoa_nganh cua khoa truoc

                                     ((from kn in db.tbl_KHOAHOC_NGANHs
                                       join kh in db.tbl_KHOAHOCs on new { ID_KHOAHOC = Convert.ToInt32(kn.ID_KHOAHOC) } equals
                                           new { ID_KHOAHOC = kh.ID_KHOAHOC } into kh_join
                                       from kh in kh_join.DefaultIfEmpty()
                                       where
                                           kn.ID_NGANH == Convert.ToInt32(dt_TTSV.Rows[0]["ID_NGANH"]) &&
                                           kh.NAM_BD == (Convert.ToInt32(dt_TTSV.Rows[0]["NAM_BD"]) - 1)
                                       select new
                                       {
                                           kn.ID_KHOAHOC_NGANH
                                       }).First().ID_KHOAHOC_NGANH)

                                 #endregion

                                    && ct.HOCKY == (Convert.ToInt32(dt_TTSV.Rows[0]["HOCKY"]) - 1) &&
                                     (hp.IS_DELETE != 1 ||
                                      hp.IS_DELETE == null)
                                 select new
                                 {
                                     hp.ID_MONHOC,
                                     diem.DIEM_TONG
                                 };
                    DataTable ximport = TableUtil.LinqToDataTable(get_kq); 
                    #endregion

                    #region Ket qua mon hoc can tu van

                    var get_kq_tuvan = from diem in db.tbl_DIEM_SINHVIENs
                                 join hp in db.tbl_LOP_HOCPHANs on new { ID_LOPHOCPHAN = Convert.ToInt32(diem.ID_LOPHOCPHAN) }
                                     equals
                                     new { ID_LOPHOCPHAN = hp.ID_LOPHOCPHAN } into hp_join
                                 from hp in hp_join.DefaultIfEmpty()
                                 join ct in db.tbl_KHOAHOC_NGANH_CTIETs on
                                     new { ID_KHOAHOC_NGANH_CTIET = Convert.ToInt32(hp.ID_KHOAHOC_NGANH_CTIET) } equals
                                     new { ID_KHOAHOC_NGANH_CTIET = ct.ID_KHOAHOC_NGANH_CTIET } into ct_join
                                 from ct in ct_join.DefaultIfEmpty()
                                 where
                                     diem.ID_SINHVIEN == Convert.ToInt32(dr["ID_SINHVIEN"]) && hp.ID_MONHOC == IDMonTuVan

                                     && ct.ID_KHOAHOC_NGANH ==

                                 #region Khoa_nganh cua khoa truoc

                                     ((from kn in db.tbl_KHOAHOC_NGANHs
                                       join kh in db.tbl_KHOAHOCs on new { ID_KHOAHOC = Convert.ToInt32(kn.ID_KHOAHOC) } equals
                                           new { ID_KHOAHOC = kh.ID_KHOAHOC } into kh_join
                                       from kh in kh_join.DefaultIfEmpty()
                                       where
                                           kn.ID_NGANH == Convert.ToInt32(dt_TTSV.Rows[0]["ID_NGANH"]) &&
                                           kh.NAM_BD == (Convert.ToInt32(dt_TTSV.Rows[0]["NAM_BD"]) - 1)
                                       select new
                                       {
                                           kn.ID_KHOAHOC_NGANH
                                       }).First().ID_KHOAHOC_NGANH)

                                 #endregion

                                    && ct.HOCKY == Convert.ToInt32(dt_TTSV.Rows[0]["HOCKY"]) &&
                                     (hp.IS_DELETE != 1 ||
                                      hp.IS_DELETE == null)
                                 select new
                                 {
                                     hp.ID_MONHOC,
                                     diem.DIEM_TONG
                                 };
                    DataTable ximport_tuvan = TableUtil.LinqToDataTable(get_kq_tuvan);
                    if (ximport_tuvan != null && ximport_tuvan.Rows.Count > 0)
                    {
                        DataRow ndr = ximport.NewRow();
                        ndr["ID_MONHOC"] = ximport_tuvan.Rows[0]["ID_MONHOC"];
                        ndr["DIEM_TONG"] = ximport_tuvan.Rows[0]["DIEM_TONG"];
                        ximport.Rows.Add(ndr);
                    }
                    #endregion

                    DataRow dtrow = dt_result.NewRow();

                    #region Chuyen ket qua diem cua 1 sinh vien thanh 1 dong trong bang du lieu mau

                    foreach (DataRow xdr in ximport.Rows)
                    {
                        for (int i = 0; i < dt_result.Columns.Count; i++)
                        {
                            if (xdr["ID_MONHOC"].ToString().Trim().Equals(dt_result.Columns[i].ColumnName))
                            {
                                dtrow[dt_result.Columns[i].ColumnName] = Convert.ToDouble(xdr["DIEM_TONG"]);
                            }
                        }
                    }

                    #endregion

                    #region Neu khong có diem => 0

                    for (int n = 0; n < dt_result.Columns.Count; n++)
                    {
                        if (string.IsNullOrEmpty(dtrow[n].ToString()))
                        {
                            dtrow[n] = 0;
                        }
                    }

                    #endregion

                    #region Nếu điểm môn cần tư vấn có tồn tại => import vào bảng dữ liệu mẫu

                    if (Convert.ToInt32(dtrow[IDMonTuVan.ToString()]) != 0)
                    {
                        dt_result.Rows.Add(dtrow);
                    }

                    #endregion
                }
                return dt_result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetDiem_SV(string masinhvien, int IDMonTuVan)
        {
            try
            {
                DataTable dt_result = new DataTable(); // Bang diem mau
                DataTable dtRes_sv = null; // Danh sach sinh vien nhung khoa truoc
                DataTable dtRes_mh = null; // Danh sach mon hoc cua hoc ky truoc
                DataTable dt_TTSV = null;  // Thong tin sinh vien
                dt_TTSV = Get_TTSV(masinhvien);
                dtRes_sv = GetAll_SVKhoaTruoc(dt_TTSV);
                dtRes_mh = GetAll_MH_Mau(Convert.ToInt32(dt_TTSV.Rows[0]["ID_KHOAHOC_NGANH"]),
                    Convert.ToInt32(dt_TTSV.Rows[0]["HOCKY"]));
                foreach (DataRow dr in dtRes_mh.Rows)
                {
                    dt_result.Columns.Add(dr[0].ToString(), typeof(double));
                }
                dt_result.Columns.Add(IDMonTuVan.ToString(), typeof(double));

                var get_kq = from diem in db.tbl_DIEM_SINHVIENs
                             join hp in db.tbl_LOP_HOCPHANs on new { ID_LOPHOCPHAN = Convert.ToInt32(diem.ID_LOPHOCPHAN) }
                                 equals
                                 new { ID_LOPHOCPHAN = hp.ID_LOPHOCPHAN } into hp_join
                             from hp in hp_join.DefaultIfEmpty()
                             join ct in db.tbl_KHOAHOC_NGANH_CTIETs on
                                 new { ID_KHOAHOC_NGANH_CTIET = Convert.ToInt32(hp.ID_KHOAHOC_NGANH_CTIET) } equals
                                 new { ID_KHOAHOC_NGANH_CTIET = ct.ID_KHOAHOC_NGANH_CTIET } into ct_join
                             from ct in ct_join.DefaultIfEmpty()
                             where
                                 diem.ID_SINHVIEN == Convert.ToInt32(dt_TTSV.Rows[0]["ID_SINHVIEN"]) &&
                                 (

                             #region Ds mon hoc cua ky truoc

                            from Tbl_KHOAHOC_NGANH_CTIET in db.tbl_KHOAHOC_NGANH_CTIETs
                            where
                                Tbl_KHOAHOC_NGANH_CTIET.ID_KHOAHOC_NGANH ==
                                Convert.ToInt32(dt_TTSV.Rows[0]["ID_KHOAHOC_NGANH"]) &&
                                Tbl_KHOAHOC_NGANH_CTIET.HOCKY == (Convert.ToInt32(dt_TTSV.Rows[0]["HOCKY"]) - 1)
                            select new
                            {
                                Tbl_KHOAHOC_NGANH_CTIET.ID_MONHOC
                            }

                                                         #endregion

                            ).Contains(new { ID_MONHOC = hp.ID_MONHOC })
                            && ct.ID_KHOAHOC_NGANH == Convert.ToInt32(dt_TTSV.Rows[0]["ID_KHOAHOC_NGANH"])
                            && ct.HOCKY == (Convert.ToInt32(dt_TTSV.Rows[0]["HOCKY"]) - 1) &&
                                 (hp.IS_DELETE != 1 ||
                                  hp.IS_DELETE == null)
                             select new
                             {
                                 hp.ID_MONHOC,
                                 diem.DIEM_TONG
                             };
                DataTable ximport = TableUtil.LinqToDataTable(get_kq);

                DataRow dtrow = dt_result.NewRow();

                #region Chuyen ket qua diem cua 1 sinh vien thanh 1 dong trong bang du lieu mau

                foreach (DataRow xdr in ximport.Rows)
                {
                    for (int i = 0; i < dt_result.Columns.Count; i++)
                    {
                        if (xdr["ID_MONHOC"].ToString().Trim().Equals(dt_result.Columns[i].ColumnName))
                        {
                            dtrow[dt_result.Columns[i].ColumnName] = Convert.ToDouble(xdr["DIEM_TONG"]);
                        }
                    }
                }

                #endregion

                dt_result.Rows.Add(dtrow);
                return dt_result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

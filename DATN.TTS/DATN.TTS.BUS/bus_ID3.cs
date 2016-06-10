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
                            join m in db.tbl_MONHOCs on new {ID_MONHOC = Convert.ToInt32(n.ID_MONHOC)} equals
                                new {ID_MONHOC = m.ID_MONHOC} into m_join
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

        public DataTable GetDiemMau()
        {
            try
            {
                DataTable dt_result = new DataTable();
                DataTable dtRes_sv = null;
                DataTable dtRes_mh = null;
                var query = from d in db.tbL_SINHVIENs
                            where
                                d.IS_DELETE != 1 ||
                                d.IS_DELETE == null
                            select new
                            {
                                d.ID_SINHVIEN,
                            };
                dtRes_sv = TableUtil.LinqToDataTable(query);

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
                                        }).Contains(new {ID_MONHOC = d.ID_MONHOC})
                                select new
                                {
                                    d.ID_LOPHOCPHAN
                                }).Contains(new {ID_LOPHOCPHAN = (System.Int32) m.ID_LOPHOCPHAN})
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
                    for(int n=0;n<dt_result.Columns.Count;n++)
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

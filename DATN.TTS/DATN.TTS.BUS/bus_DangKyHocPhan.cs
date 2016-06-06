using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using DATN.TTS.DATA;

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
    }
}

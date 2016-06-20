using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DATN.TTS.DATA;

namespace DATN.TTS.BUS
{
    public class bus_C45
    {
        db_ttsDataContext db = new db_ttsDataContext();
        public DataTable GetMonHocForC45()
        {
            try
            {
                DataTable dt = null;
                var monhoc = (from dsv in db.tbl_DIEM_SINHVIENs
                    join hp in db.tbl_LOP_HOCPHANs on new {ID_LOPHOCPHAN = Convert.ToInt32(dsv.ID_LOPHOCPHAN)} equals
                        new {ID_LOPHOCPHAN = hp.ID_LOPHOCPHAN}
                    join mh in db.tbl_MONHOCs on new {ID_MONHOC = Convert.ToInt32(hp.ID_MONHOC)} equals
                        new {ID_MONHOC = mh.ID_MONHOC}
                    where
                        (dsv.IS_DELETE != 1 ||
                         dsv.IS_DELETE == null) &&
                        (hp.IS_DELETE != 1 ||
                         hp.IS_DELETE == null) &&
                        (mh.IS_DELETE != 1 ||
                         mh.IS_DELETE == null) &&
                        mh.ISBATBUOC == 0  &&
                        dsv.ID_SINHVIEN ==1
                        //&& mh.ID_MONHOC ==  7
                    select new
                    {
                        //dsv.ID_SINHVIEN,
                        mh.TEN_MONHOC,
                        dsv.DIEM_TONG
                    }).Distinct();
                dt = TableUtil.LinqToDataTable(monhoc);
                return dt;
            }
            catch (Exception err)
            {
                throw err;
            }
        }

        public DataTable GetSinhVien()
        {
            try
            {
                DataTable dt = null;
                var svien = from sv in db.tbL_SINHVIENs
                    where
                        sv.IS_DELETE != 1 ||
                        sv.IS_DELETE == null
                    select new
                    {
                        sv.ID_SINHVIEN,
                        sv.MA_SINHVIEN
                    };
                dt = TableUtil.LinqToDataTable(svien);
                return dt;
            }
            catch (Exception err)
            {
                throw err;
            }
        }
    }
}

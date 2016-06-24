using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DATN.TTS.DATA;

namespace DATN.TTS.BUS
{
    public class bus_NhapDiemSV
    {
        db_ttsDataContext db = new db_ttsDataContext();
        public DataTable GetAll_DiemSV()
        {
            try
            {
                DataTable dt = null;
                var query = from Tbl_DIEM_SINHVIEN in db.tbl_DIEM_SINHVIENs
                    where
                        Tbl_DIEM_SINHVIEN.IS_DELETE != 1 ||
                        Tbl_DIEM_SINHVIEN.IS_DELETE == null
                    select new
                    {
                        Tbl_DIEM_SINHVIEN.ID_KETQUA,
                        Tbl_DIEM_SINHVIEN.ID_SINHVIEN,
                        TEN_SINHVIEN =
                            ((from m in db.tbL_SINHVIENs
                                where
                                    m.ID_SINHVIEN == Tbl_DIEM_SINHVIEN.ID_SINHVIEN
                                select new
                                {
                                    m.TEN_SINHVIEN
                                }).First().TEN_SINHVIEN),
                        MA_SINHVIEN =
                            ((from m in db.tbL_SINHVIENs
                                where
                                    m.ID_SINHVIEN == Tbl_DIEM_SINHVIEN.ID_SINHVIEN
                                select new
                                {
                                    m.MA_SINHVIEN
                                }).First().MA_SINHVIEN),
                        Tbl_DIEM_SINHVIEN.ID_LOPHOCPHAN,
                        MA_LOP_HOCPHAN =
                            ((from m in db.tbl_LOP_HOCPHANs
                                where
                                    m.ID_LOPHOCPHAN == Tbl_DIEM_SINHVIEN.ID_LOPHOCPHAN
                                select new
                                {
                                    m.MA_LOP_HOCPHAN
                                }).First().MA_LOP_HOCPHAN),
                        TEN_LOP_HOCPHAN =
                            ((from m in db.tbl_LOP_HOCPHANs
                                where
                                    m.ID_LOPHOCPHAN == Tbl_DIEM_SINHVIEN.ID_LOPHOCPHAN
                                select new
                                {
                                    m.TEN_LOP_HOCPHAN
                                }).First().TEN_LOP_HOCPHAN),
                        Tbl_DIEM_SINHVIEN.ID_KHOAHOC,
                        TEN_KHOAHOC =
                            ((from m in db.tbl_KHOAHOCs
                                where
                                    m.ID_KHOAHOC == Tbl_DIEM_SINHVIEN.ID_KHOAHOC
                                select new
                                {
                                    m.TEN_KHOAHOC
                                }).First().TEN_KHOAHOC),
                        HOCKY = Tbl_DIEM_SINHVIEN.ID_HOCKY,
                        Tbl_DIEM_SINHVIEN.DIEM_BT,
                        Tbl_DIEM_SINHVIEN.DIEM_GK,
                        Tbl_DIEM_SINHVIEN.DIEM_CK,
                        Tbl_DIEM_SINHVIEN.DIEM_TONG,
                        Tbl_DIEM_SINHVIEN.DIEM_HE4,
                        Tbl_DIEM_SINHVIEN.DIEM_CHU,
                        Tbl_DIEM_SINHVIEN.GHICHU
                    };
                dt = TableUtil.LinqToDataTable(query);
                return dt;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public int InsertObject_Excel(DataTable idatasource, string pUser)
        {
            try
            {
                int i = 0;
                foreach (DataRow dr in idatasource.Rows)
                {
                    double? diembt;
                    double? diemgk;
                    double? diemck;
                    double? diemtk;
                    double? diemhe4;
                    if (!string.IsNullOrEmpty(dr["f_diembt"].ToString()))
                    {
                        diembt = Convert.ToDouble(dr["f_diembt"]);
                    }
                    else
                    {
                        diembt = null;
                    }
                    if (!string.IsNullOrEmpty(dr["f_diem1"].ToString()))
                    {
                        diemgk = Convert.ToDouble(dr["f_diem1"]);
                    }
                    else
                    {
                        diemgk = null;
                    }
                    if (!string.IsNullOrEmpty(dr["f_diem2"].ToString()))
                    {
                        diemck = Convert.ToDouble(dr["f_diem2"]);
                    }
                    else
                    {
                        diemck = null;
                    }
                    if (!string.IsNullOrEmpty(dr["f_diemtk1"].ToString()))
                    {
                        diemtk = Convert.ToDouble(dr["f_diemtk1"]);
                    }
                    else
                    {
                        diemtk = null;
                    }
                    if (!string.IsNullOrEmpty(dr["f_diemstk1"].ToString()))
                    {
                        diemhe4 = Convert.ToDouble(dr["f_diemstk1"]);
                    }
                    else
                    {
                        diemhe4 = null;
                    }
                    tbl_DIEM_SINHVIEN query = new tbl_DIEM_SINHVIEN
                    {
                        ID_SINHVIEN =Convert.ToInt32(dr["ID_SINHVIEN"]),
                        ID_LOPHOCPHAN = Convert.ToInt32(dr["ID_LOPHOCPHAN"]),
                        DIEM_BT = diembt,
                        DIEM_GK = diemgk,
                        DIEM_CK = diemck,
                        DIEM_TONG = diemtk,
                        DIEM_HE4 = diemhe4,
                        DIEM_CHU = dr["f_diemch1"].ToString(),
                        CREATE_USER = pUser,
                        CREATE_TIME = DateTime.Now,
                    };
                    db.tbl_DIEM_SINHVIENs.InsertOnSubmit(query);
                    db.SubmitChanges();
                    i = query.ID_KETQUA;
                }
                return i;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        //private double ConvertToDouble(string ivalue)
        //{
        //    double? ireturn = null;
        //    if (!string.IsNullOrEmpty(ivalue))
        //    {
        //        ireturn = Convert.ToDouble(ivalue);
        //    }
        //    else
        //    {
        //        ireturn = null;
        //    }
        //    return ireturn;
        //}

        public DataTable GetAllKhoaNganh_ForDiem(int pID_KHOAHOC)
        {
            try
            {
                DataTable dt = new DataTable();
                if (pID_KHOAHOC != 0 || !string.IsNullOrEmpty(pID_KHOAHOC.ToString()))
                {
                    var khoanganh = from khoanganhs in db.tbl_KHOAHOC_NGANHs
                                    where (khoanganhs.IS_DELETE != 1 || khoanganhs.IS_DELETE == null) && khoanganhs.ID_KHOAHOC == pID_KHOAHOC
                                    join k in db.tbl_KHOAHOCs on khoanganhs.ID_KHOAHOC equals k.ID_KHOAHOC
                                    where (k.IS_DELETE != 1 || k.IS_DELETE == null)
                                    join ng in db.tbl_NGANHs on khoanganhs.ID_NGANH equals ng.ID_NGANH
                                    where (ng.IS_DELETE != 1 || ng.IS_DELETE == null)
                                    select new
                                    {
                                        khoanganhs.ID_KHOAHOC_NGANH,
                                        ng.TEN_NGANH
                                    };
                    dt = TableUtil.LinqToDataTable(khoanganh);
                }
                return dt;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}

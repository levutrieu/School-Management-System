using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Linq;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DATN.TTS.DATA;

namespace DATN.TTS.BUS
{

    public class bus_MonHoc
    {
        db_ttsDataContext db = new db_ttsDataContext();

        //public DataTable GetAll_MonHoc()
        //{

        //    var query = (from MONHOC in db.tbl_MONHOCs
        //                 where
        //                     (MONHOC.IS_DELETE != 1 ||
        //                      MONHOC.IS_DELETE == null) &&
        //                     MONHOC.ISBATBUOC == 1
        //                 select new
        //                 {
        //                     ID = MONHOC.ID_MONHOC,
        //                     ID_NAME = MONHOC.ID_MONHOC,
        //                     MA_MONHOC = MONHOC.MA_MONHOC,
        //                     NAME = MONHOC.TEN_MONHOC,
        //                     ID_PARENT = 0
        //                 }).Concat
        //        (from MONHOC in db.tbl_MONHOCs
        //         where
        //             (MONHOC.IS_DELETE != 1 ||
        //              MONHOC.IS_DELETE == null) &&
        //             MONHOC.ISBATBUOC == 0
        //         select new
        //         {
        //             ID = MONHOC.ID_MONHOC,
        //             ID_NAME = MONHOC.ID_MONHOC,
        //             MA_MONHOC = MONHOC.MA_MONHOC,
        //             NAME = MONHOC.TEN_MONHOC,
        //             ID_PARENT = (int)MONHOC.ID_MONHOC_CHA
        //         }
        //        );
        //    DataTable xdt = null;
        //    xdt =TableUtil.LinqToDataTable(query);
        //    return xdt;
        //}

        //public DataTable GetAllMonHoc()
        //{
        //    var query = from MH in db.tbl_MONHOCs
        //        join TQ in db.tbl_MONHOC_TQs on new {ID_MONHOC = MH.ID_MONHOC} equals
        //            new {ID_MONHOC = Convert.ToInt32(TQ.ID_MONHOC)} into TQ_join
        //        from TQ in TQ_join.DefaultIfEmpty()
        //        orderby
        //            MH.ID_MONHOC
        //        select new
        //        {
        //            MH.ID_MONHOC,
        //            MH.ID_LOAI_MONHOC,
        //            TEN_LOAIMH =
        //                ((from m in db.tbl_LOAI_MONHOCs
        //                  where
        //                      m.ID_LOAI_MONHOC == MH.ID_LOAI_MONHOC
        //                  select new
        //                  {
        //                      m.TEN_LOAI_MONHOC
        //                  }).First().TEN_LOAI_MONHOC),
        //            MH.ID_BOMON,
        //            MH.TEN_MONHOC,
        //            MH.MA_MONHOC,
        //            MH.KY_HIEU,
        //            MH.SO_TC,
        //            MH.IS_THUHOCPHI,
        //            MH.IS_THUCHANH,
        //            MH.IS_LYTHUYET,
        //            MH.IS_TINHDIEM,
        //            MH.TRANGTHAI,
        //            MH.ISBATBUOC,
        //            ID_MONHOC_TQ = (int?) TQ.ID_MONHOC_TQ,
        //            TEN_MONHOC_TQ =
        //                ((from m in db.tbl_MONHOCs
        //                    where
        //                        m.ID_MONHOC == TQ.ID_MONHOC_TQ
        //                    select new
        //                    {
        //                        m.TEN_MONHOC
        //                    }).First().TEN_MONHOC),
        //            MH.ID_MONHOC_SONGHANH,
        //            TEN_MONHOC_SONGHANH =
        //                ((from n in db.tbl_MONHOCs
        //                  where
        //                      n.ID_MONHOC == MH.ID_MONHOC_SONGHANH
        //                  select new
        //                  {
        //                      n.TEN_MONHOC
        //                  }).First().TEN_MONHOC),
        //        };
        //    DataTable xdt = null;
        //    xdt = TableUtil.LinqToDataTable(query);
        //    return xdt;
        //}

        public DataTable GetAllMonHoc()
        {
            var query = from MH in db.tbl_MONHOCs
                        where (MH.IS_DELETE != 1 || MH.IS_DELETE == null)
                        select new
                        {
                            MH.ID_MONHOC,
                            MH.ID_LOAI_MONHOC,
                            TEN_LOAIMH =
                                ((from m in db.tbl_LOAI_MONHOCs
                                  where
                                      m.ID_LOAI_MONHOC == MH.ID_LOAI_MONHOC
                                  select new
                                  {
                                      m.TEN_LOAI_MONHOC
                                  }).First().TEN_LOAI_MONHOC),
                            MH.ID_BOMON,
                            TEN_BOMON=
                                ((from m in db.tbl_BOMONs
                                  where
                                      m.ID_BOMON == MH.ID_BOMON
                                  select new
                                  {
                                      m.TEN_BM
                                  }).First().TEN_BM),
                            MH.TEN_MONHOC,
                            MH.MA_MONHOC,
                            MH.KY_HIEU,
                            MH.SO_TC,
                            MH.IS_THUHOCPHI,
                            MH.IS_THUCHANH,
                            MH.IS_LYTHUYET,
                            MH.IS_TINHDIEM,
                            MH.TRANGTHAI,
                            MH.ISBATBUOC,
                            MH.GHICHU,
                            //ID_MONHOC_TQ = (int?)TQ.ID_MONHOC_TQ,
                            //TEN_MONHOC_TQ =
                            //    ((from m in db.tbl_MONHOCs
                            //      where
                            //          m.ID_MONHOC == TQ.ID_MONHOC_TQ
                            //      select new
                            //      {
                            //          m.TEN_MONHOC
                            //      }).First().TEN_MONHOC),
                            MH.ID_MONHOC_SONGHANH,
                            TEN_MONHOC_SONGHANH =
                                ((from n in db.tbl_MONHOCs
                                  where
                                      n.ID_MONHOC == MH.ID_MONHOC_SONGHANH
                                  select new
                                  {
                                      n.TEN_MONHOC
                                  }).First().TEN_MONHOC),
                        };
            DataTable xdt = null;
            xdt = TableUtil.LinqToDataTable(query);
            return xdt;
        }

        public int InsertObject(DataTable idatasource)
        {
            int i = 0;
            tbl_MONHOC query = new tbl_MONHOC
            {
                ID_LOAI_MONHOC = Convert.ToInt32(idatasource.Rows[0]["ID_LOAI_MONHOC"]),
                ID_BOMON = Convert.ToInt32(idatasource.Rows[0]["ID_BOMON"]),
                MA_MONHOC = idatasource.Rows[0]["MA_MONHOC"].ToString(),
                TEN_MONHOC = idatasource.Rows[0]["TEN_MONHOC"].ToString(),
                KY_HIEU = idatasource.Rows[0]["KY_HIEU"].ToString(),
                SO_TC = Convert.ToInt32(idatasource.Rows[0]["SO_TC"]),
                IS_THUHOCPHI = Convert.ToInt32(idatasource.Rows[0]["IS_THUHOCPHI"]),
                IS_THUCHANH = Convert.ToInt32(idatasource.Rows[0]["IS_THUCHANH"]),
                IS_LYTHUYET = Convert.ToInt32(idatasource.Rows[0]["IS_LYTHUYET"]),
                IS_TINHDIEM = Convert.ToInt32(idatasource.Rows[0]["IS_TINHDIEM"]),
                TRANGTHAI = Convert.ToInt32(idatasource.Rows[0]["TRANGTHAI"]),
                CREATE_USER = idatasource.Rows[0]["USER"].ToString(),
                CREATE_TIME = DateTime.Now,
                ISBATBUOC = Convert.ToInt32(idatasource.Rows[0]["ISBATBUOC"]),
                ID_MONHOC_SONGHANH = Convert.ToInt32(idatasource.Rows[0]["ID_MONHOC_SONGHANH"]),
                GHICHU = idatasource.Rows[0]["GHICHU"].ToString(),
                IS_DELETE = 0
            };
            db.tbl_MONHOCs.InsertOnSubmit(query);
            db.SubmitChanges();
            i = query.ID_MONHOC;
            return i;
        }

        public int UpdateObject(DataTable idatasource)
        {
            int i = 0;
            tbl_MONHOC query = (from Tbl_MONHOC in db.tbl_MONHOCs
                where
                    Tbl_MONHOC.ID_MONHOC == Convert.ToInt32(idatasource.Rows[0]["ID_MONHOC"])
                select Tbl_MONHOC).FirstOrDefault();
            query.ID_LOAI_MONHOC = Convert.ToInt32(idatasource.Rows[0]["ID_LOAI_MONHOC"]);
            query.ID_BOMON = Convert.ToInt32(idatasource.Rows[0]["ID_BOMON"]);
            query.MA_MONHOC = idatasource.Rows[0]["MA_MONHOC"].ToString();
            query.TEN_MONHOC = idatasource.Rows[0]["TEN_MONHOC"].ToString();
            query.KY_HIEU = idatasource.Rows[0]["KY_HIEU"].ToString();
            query.SO_TC = Convert.ToInt32(idatasource.Rows[0]["SO_TC"]);
            query.IS_THUHOCPHI = Convert.ToInt32(idatasource.Rows[0]["IS_THUHOCPHI"]);
            query.IS_THUCHANH = Convert.ToInt32(idatasource.Rows[0]["IS_THUCHANH"]);
            query.IS_LYTHUYET = Convert.ToInt32(idatasource.Rows[0]["IS_LYTHUYET"]);
            query.IS_TINHDIEM = Convert.ToInt32(idatasource.Rows[0]["IS_TINHDIEM"]);
            query.TRANGTHAI = Convert.ToInt32(idatasource.Rows[0]["TRANGTHAI"]);
            query.UPDATE_USER = idatasource.Rows[0]["USER"].ToString();
            query.UPDATE_TIME = DateTime.Now;
            query.ISBATBUOC = Convert.ToInt32(idatasource.Rows[0]["ISBATBUOC"]);
            query.ID_MONHOC_SONGHANH = Convert.ToInt32(idatasource.Rows[0]["ID_MONHOC_SONGHANH"]);
            query.GHICHU = idatasource.Rows[0]["GHICHU"].ToString();
            db.SubmitChanges();
            i = query.ID_MONHOC;
            return i;
        }

        public int DeleteObject(int id_monhoc, string user)
        {
            int i = 0;
            tbl_MONHOC query = (from Tbl_MONHOC in db.tbl_MONHOCs
                                where
                                    Tbl_MONHOC.ID_MONHOC == id_monhoc
                                select Tbl_MONHOC).FirstOrDefault();
            query.IS_DELETE = 1;
            query.UPDATE_USER = user;
            query.UPDATE_TIME = DateTime.Now;
            db.SubmitChanges();
            i = query.ID_MONHOC;
            return i;
        }

        public int InsertObject_Excel(DataTable idatasource, string pUser)
        {
            try
            {
                int i = 0;
                foreach (DataRow dr in idatasource.Rows)
                {
                    tbl_MONHOC query = new tbl_MONHOC
                    {
                        MA_MONHOC = dr["f_mamh"].ToString(),
                        TEN_MONHOC = dr["f_tenmhvn"].ToString(),
                        SO_TC = Convert.ToInt32(dr["f_dvht"]),
                        IS_THUHOCPHI = 1,
                        IS_TINHDIEM = 1,
                        TRANGTHAI = 1,
                        CREATE_USER = pUser,
                        CREATE_TIME = DateTime.Now,
                        IS_DELETE = 0
                    };
                    db.tbl_MONHOCs.InsertOnSubmit(query);
                    db.SubmitChanges();
                    i = query.ID_MONHOC;
                }
                return i;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
    }
}

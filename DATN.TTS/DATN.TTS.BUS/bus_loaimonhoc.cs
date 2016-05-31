using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DATN.TTS.DATA;

namespace DATN.TTS.BUS
{
    public class bus_loaimonhoc
    {
        db_ttsDataContext db = new db_ttsDataContext();

        public DataTable GetAll()
        {
            try
            {
                DataTable dtRes = null;
                var query = from d in db.tbl_LOAI_MONHOCs
                            where
                                d.IS_DELETE != 1 ||
                                d.IS_DELETE == null
                            select new
                            {
                                d.ID_LOAI_MONHOC,
                                d.MA_LOAI_MONHOC,
                                d.TEN_LOAI_MONHOC,
                                d.TRANGTHAI
                            };
                dtRes = TableUtil.LinqToDataTable(query);
                return dtRes;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int InsertObject(DataTable xdt)
        {
            try
            {
                int i = 0;
                tbl_LOAI_MONHOC query = new tbl_LOAI_MONHOC()
                {
                    MA_LOAI_MONHOC = xdt.Rows[0]["MA_LOAI"].ToString(),
                    TEN_LOAI_MONHOC = xdt.Rows[0]["TENLOAI"].ToString(),
                    TRANGTHAI = xdt.Rows[0]["TRANGTHAI"].ToString(),
                    CREATE_USER = xdt.Rows[0]["USER"].ToString(),
                    CREATE_TIME = DateTime.Now
                };

                db.tbl_LOAI_MONHOCs.InsertOnSubmit(query);
                db.SubmitChanges();
                i = query.ID_LOAI_MONHOC;
                return i;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public int DeleteObject(int id_loai_monhoc, string user)
        {
            try
            {
                int i = 0;
                tbl_LOAI_MONHOC query = (from d in db.tbl_LOAI_MONHOCs
                                         where
                                             d.ID_LOAI_MONHOC == id_loai_monhoc
                                         select d).FirstOrDefault();

                query.IS_DELETE = 1;
                query.UPDATE_USER = user;
                query.UPDATE_TIME = DateTime.Now;
                db.SubmitChanges();
                i = query.ID_LOAI_MONHOC;
                return i;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public int UpdateObject(DataTable idatasource)
        {
            try
            {
                int i = 0;
                tbl_LOAI_MONHOC query = (from d in db.tbl_LOAI_MONHOCs
                    where
                        d.ID_LOAI_MONHOC == Convert.ToInt32(idatasource.Rows[0]["ID_LOAI"]) 
                                         select d).FirstOrDefault();
                query.TEN_LOAI_MONHOC = idatasource.Rows[0]["TENLOAI"].ToString();
                query.TRANGTHAI = idatasource.Rows[0]["TRANGTHAI"].ToString();
                query.UPDATE_USER = idatasource.Rows[0]["USER"].ToString();
                query.UPDATE_TIME = DateTime.Now;
                db.SubmitChanges();
                i = query.ID_LOAI_MONHOC;
                return i;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}

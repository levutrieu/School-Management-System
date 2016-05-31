using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DATN.TTS.DATA;

namespace DATN.TTS.BUS
{
    public class bus_bomon
    {
        db_ttsDataContext db = new db_ttsDataContext();

        public DataTable GetAllBoMon()
        {
            try
            {
                DataTable dtRes = null;
                var query = from d in db.tbl_BOMONs
                            where
                                d.ISDELETE != 1 ||
                                d.ISDELETE == null
                            select new
                            {
                                d.ID_BOMON,
                                d.MA_BM,
                                d.TEN_BM,
                            };
                dtRes = TableUtil.LinqToDataTable(query);
                return dtRes;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int InsertObject(string Ma_bm, string Ten_bm, string user)
        {
            try
            {
                int i = 0;
                tbl_BOMON query = new tbl_BOMON
                {
                    MA_BM = Ma_bm,
                    TEN_BM = Ten_bm,
                    CREATE_TIME = DateTime.Now,
                    CREATE_USER = user
                };
                db.tbl_BOMONs.InsertOnSubmit(query);
                db.SubmitChanges();
                i = query.ID_BOMON;
                return i;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public int DeleteObject(int id_bomon, string user)
        {
            try
            {
                int i = 0;
                tbl_BOMON query = (from d in db.tbl_BOMONs
                    where
                        d.ID_BOMON == id_bomon
                    select d).FirstOrDefault();
                query.ISDELETE = 1;
                query.UPDATE_USER = user;
                query.UPDATE_TIME = DateTime.Now;
                db.SubmitChanges();
                i = query.ID_BOMON;
                return i;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public int UpdateObject(int id_bomon, string ten_bm, string user)
        {
            try
            {
                int i = 0;
                tbl_BOMON query = (from d in db.tbl_BOMONs
                                   where
                                       d.ID_BOMON == id_bomon
                                   select d).FirstOrDefault();
                query.TEN_BM = ten_bm;
                query.UPDATE_USER = user;
                query.UPDATE_TIME = DateTime.Now;
                db.SubmitChanges();
                i = query.ID_BOMON;
                return i;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}

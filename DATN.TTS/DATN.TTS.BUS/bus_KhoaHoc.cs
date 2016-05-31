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
    public class bus_KhoaHoc
    {
        db_ttsDataContext db = new db_ttsDataContext();

        public DataTable GetAllHeDaoTao()
        {
            try
            {
                DataTable dt = null;
                var hdt = from hdtao in db.tbl_HEDAOTAOs where hdtao.IS_DELETE == 0 select hdtao;

                dt = TableUtil.LinqToDataTable(hdt);

                return dt;
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        public DataTable GetAllKhoaHoc()
        {
            try
            {
                DataTable dt = null;
                var kh = from khoc in db.tbl_KHOAHOCs
                    join hdt in db.tbl_HEDAOTAOs on khoc.ID_HE_DAOTAO equals hdt.ID_HE_DAOTAO
                    where khoc.IS_DELETE == 0 && hdt.IS_DELETE == 0
                    select new
                    {
                        khoc.ID_KHOAHOC,
                        khoc.MA_KHOAHOC,
                        khoc.ID_HE_DAOTAO,
                        khoc.KYHIEU,
                        khoc.NAM_BD,
                        khoc.NAM_KT,
                        khoc.SO_HKY_1NAM,
                        khoc.SO_HKY,
                        khoc.TEN_KHOAHOC,
                        khoc.TRANGTHAI,
                        hdt.TEN_HE_DAOTAO

                    };
                dt = TableUtil.LinqToDataTable(kh);
                return dt;

            }
            catch (Exception)
            {
                
                throw;
            }
        }

        public int GetLastId()
        {
            try
            {
                int lasts = 0;
                var last = (from ns in db.tbl_KHOAHOCs select ns).OrderByDescending(t => t.ID_KHOAHOC).FirstOrDefault();

                if (last != null)
                {
                    lasts = int.Parse(last.ID_KHOAHOC.ToString());
                }
                return lasts + 1;
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        public bool Insert_KhoaHoc(params object[] param)
        {
            try
            {
                DataTable dt = (DataTable)param[0];
                DataRow r = dt.Rows[0];
                tbl_KHOAHOC kh = new tbl_KHOAHOC();
                //kh.ID_KHOAHOC = GetLastId();
                kh.ID_HE_DAOTAO = int.Parse(r["ID_HE_DAOTAO"].ToString().Trim());
                kh.IS_DELETE = 0;
                kh.KYHIEU = r["KYHIEU"].ToString();
                kh.MA_KHOAHOC = r["MA_KHOAHOC"].ToString();
                kh.NAM_BD = int.Parse(r["NAM_BD"].ToString());
                kh.NAM_KT = int.Parse(r["NAM_KT"].ToString());
                kh.SO_HKY_1NAM = int.Parse(r["SO_HKY_1NAM"].ToString());
                kh.SO_HKY = int.Parse(r["SO_HKY"].ToString());
                kh.TEN_KHOAHOC = r["TEN_KHOAHOC"].ToString();
                kh.TRANGTHAI = r["TRANGTHAI"].ToString();
                kh.CREATE_TIME = System.DateTime.Now;
                kh.CREATE_USER = r["USER"].ToString();

                db.tbl_KHOAHOCs.InsertOnSubmit(kh);
                db.SubmitChanges();

                if (kh.ID_KHOAHOC.GetTypeCode() != TypeCode.DBNull)
                {
                    db.Transaction.Commit();
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                db.Transaction.Rollback();
                throw;
            }
        }

        public void Delete_KhoaHoc(int pId, string pUser)
        {
            try
            {
                //db.Connection.Open();
                tbl_KHOAHOC kh = db.tbl_KHOAHOCs.Single(t => t.ID_KHOAHOC == pId);
                using (db.Transaction = db.Connection.BeginTransaction())
                {
                    kh.IS_DELETE = 1;
                    kh.UPDATE_USER = pUser;
                    kh.UPDATE_TIME = System.DateTime.Now;

                    db.SubmitChanges();

                    db.Transaction.Commit();
                }
            }
            catch (Exception)
            {
                db.Transaction.Rollback();
            }
            //db.Connection.Close();
        }

        public void Update_KhoaHoc(params object[] param)
        {
            try
            {
                DataTable dt = (DataTable)param[0];
                DataRow r = dt.Rows[0];
                db.Connection.Open();
                using (db.Transaction = db.Connection.BeginTransaction())
                {
                    tbl_KHOAHOC kh = db.tbl_KHOAHOCs.Single(t => t.ID_KHOAHOC == int.Parse(r["ID_KHOAHOC"].ToString()));

                    kh.ID_HE_DAOTAO = int.Parse(r["ID_HE_DAOTAO"].ToString().Trim());
                    kh.IS_DELETE = 0;
                    kh.KYHIEU = r["KYHIEU"].ToString();
                    kh.MA_KHOAHOC = r["MA_KHOAHOC"].ToString();
                    kh.NAM_BD = int.Parse(r["NAM_BD"].ToString());
                    kh.NAM_KT = int.Parse(r["NAM_KT"].ToString());
                    kh.SO_HKY_1NAM = int.Parse(r["SO_HKY_1NAM"].ToString());
                    kh.SO_HKY = int.Parse(r["SO_HKY"].ToString());
                    kh.TEN_KHOAHOC = r["TEN_KHOAHOC"].ToString();
                    kh.TRANGTHAI = r["TRANGTHAI"].ToString();
                    kh.UPDATE_TIME = System.DateTime.Now;
                    kh.UPDATE_USER = r["USER"].ToString();

                    db.SubmitChanges();

                    db.Transaction.Commit();
                }
            }
            catch (Exception)
            {
                db.Transaction.Rollback();
            }
            db.Connection.Close();
        }
    }
}

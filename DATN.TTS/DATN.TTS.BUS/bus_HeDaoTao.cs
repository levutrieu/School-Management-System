using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DATN.TTS.DATA;

namespace DATN.TTS.BUS
{
    public class bus_HeDaoTao
    {
        db_ttsDataContext db = new db_ttsDataContext();

         public int GetLastId()
        {
            try
            {
                int lasts = 0;
                var last = (from ns in db.tbl_HEDAOTAOs select ns).OrderByDescending(t => t.ID_HE_DAOTAO).FirstOrDefault();

                if (last != null)
                {
                    lasts = int.Parse(last.ID_HE_DAOTAO.ToString());
                }
                return lasts + 1;
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        public bool KiemTrungMa(string pMA_HE_DAOTAO)
        {
            try
            {
                var hdt = from hdtao in db.tbl_HEDAOTAOs.Where(t => t.MA_HE_DAOTAO == pMA_HE_DAOTAO) select hdtao;

                DataTable dt = TableUtil.LinqToDataTable(hdt);
                if (dt.Rows.Count > 0)
                {
                    return false;
                }
                return true;
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        public DataTable GetAllHeDaoTao()
        {
            try
            {
                DataTable dt = null;
                var hdt = from h in db.tbl_HEDAOTAOs where h.IS_DELETE == 0
                          join bac in db.tbl_BACDAOTAOs on h.ID_BAC_DAOTAO equals bac.ID_BAC_DAOTAO where bac.IS_DELETE == 0
                          join lhinh in db.tbl_LOAIHINH_DTAOs on h.ID_LOAIHINH_DTAO equals lhinh.ID_LOAIHINH_DTAO where lhinh.IS_DELETE ==0
                          select new {h.ID_HE_DAOTAO, h.ID_BAC_DAOTAO, h.ID_LOAIHINH_DTAO, h.MA_HE_DAOTAO, h.TEN_HE_DAOTAO, h.TRANGTHAI, h.SO_NAMHOC, bac.TEN_BAC_DAOTAO, lhinh.TEN_LOAIHINH_DTAO };
                dt = TableUtil.LinqToDataTable(hdt);
                return dt;
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        public DataTable GetAllBacDaoTao()
        {
            try
            {
                DataTable dt = null;
                var bac = from b in db.tbl_BACDAOTAOs where b.IS_DELETE == 0 select b;
                dt = TableUtil.LinqToDataTable(bac);
                return dt;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DataTable GetAllLoaiHinhDaoTao()
        {
            try
            {
                DataTable dt = null;
                var bac = from lhinhDt in db.tbl_LOAIHINH_DTAOs where lhinhDt.IS_DELETE == 0 select lhinhDt;
                dt = TableUtil.LinqToDataTable(bac);
                return dt;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool Insert_HeDaoTao(params object[] param)
        {
            try
            {
                DataTable dt = (DataTable) param[0];
                DataRow r = dt.Rows[0];

                tbl_HEDAOTAO hdt = new tbl_HEDAOTAO();
                //hdt.ID_HE_DAOTAO = GetLastId();
                hdt.ID_BAC_DAOTAO = int.Parse(r["ID_BAC_DAOTAO"].ToString());
                hdt.ID_LOAIHINH_DTAO = int.Parse(r["ID_LOAIHINH_DTAO"].ToString());
                hdt.MA_HE_DAOTAO = r["MA_HE_DAOTAO"].ToString();
                hdt.TEN_HE_DAOTAO = r["TEN_HE_DAOTAO"].ToString();
                hdt.TRANGTHAI = r["TRANGTHAI"].ToString();
                hdt.CREATE_USER = r["USER"].ToString();
                hdt.CREATE_TIME = System.DateTime.Today;
                hdt.IS_DELETE = 0;

                db.tbl_HEDAOTAOs.InsertOnSubmit(hdt);
                db.SubmitChanges();
                if (!string.IsNullOrEmpty(hdt.ID_HE_DAOTAO.ToString()))
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

        public void Update_HeDaoTao(params object[] param)
        {
            try
            {
                DataTable dt = (DataTable) param[0];
                DataRow r = dt.Rows[0];
                tbl_HEDAOTAO hdt = db.tbl_HEDAOTAOs.Single(t => t.ID_HE_DAOTAO == int.Parse(r["ID_HE_DAOTAO"].ToString()));
                hdt.ID_BAC_DAOTAO = int.Parse(r["ID_BAC_DAOTAO"].ToString());
                hdt.ID_LOAIHINH_DTAO = int.Parse(r["ID_LOAIHINH_DTAO"].ToString());
                hdt.MA_HE_DAOTAO = r["MA_HE_DAOTAO"].ToString();
                hdt.TEN_HE_DAOTAO = r["TEN_HE_DAOTAO"].ToString();
                hdt.TRANGTHAI = r["TRANGTHAI"].ToString();
                hdt.UPDATE_USER = r["USER"].ToString();
                hdt.UPDATE_TIME = System.DateTime.Today;
                hdt.IS_DELETE = 0;

                db.SubmitChanges();

                //db.Transaction.Commit();
            }
            catch (Exception)
            {
                //db.Transaction.Rollback();
                throw;
            }
        }

        public void Delete_HeDaoTao(int pId, string pUser)
        {
            try
            {
                tbl_HEDAOTAO hdt = db.tbl_HEDAOTAOs.Single(t => t.ID_HE_DAOTAO == pId);
                hdt.UPDATE_USER = pUser;
                hdt.UPDATE_TIME = System.DateTime.Today;
                hdt.IS_DELETE = 1;
                //db.Transaction.Commit();
                db.SubmitChanges();
            }
            catch (Exception)
            {
                //db.Transaction.Rollback();
                throw;
            }
        }
    }
}

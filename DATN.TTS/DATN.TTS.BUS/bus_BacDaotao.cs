using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DATN.TTS.DATA;

namespace DATN.TTS.BUS
{
    public class bus_BacDaotao
    {
        db_ttsDataContext db = new db_ttsDataContext();

        public DataTable GetAllBac()
        {
            try
            {
                var bac = from b in db.tbl_BACDAOTAOs where b.IS_DELETE == 0 select b;
                DataTable dt = TableUtil.LinqToDataTable(bac);
                return dt;
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        public bool KiemTratrungMa(string pMa)
        {
            try
            {
                var bac = from b in db.tbl_BACDAOTAOs.Where(t => t.MA_BAC_DAOTAO == pMa) select b;

                DataTable dt = TableUtil.LinqToDataTable(bac);
                if (dt.Rows.Count <= 0)
                {
                    return true;
                }
                return false;

            }
            catch (Exception)
            {
                
                throw;
            }
        }

        public bool Delete_BacDaoTao(int pId, string pUser)
        {
            try
            {
                tbl_BACDAOTAO bac = db.tbl_BACDAOTAOs.Single(t => t.ID_BAC_DAOTAO == pId);
                bac.UPDATE_USER = pUser;
                bac.UPDATE_TIME = System.DateTime.Today;
                bac.IS_DELETE = 1;
                db.SubmitChanges();
                if (!string.IsNullOrEmpty(bac.ID_BAC_DAOTAO.ToString()))
                {
                    return true;
                }
                return false;

            }
            catch (Exception err)
            {
                throw err;
            }
        }

        public bool Insert_BacDaotao(params object[] param)
        {
            try
            {
                DataTable dt = (DataTable) param[0];
                DataRow r = dt.Rows[0];

                tbl_BACDAOTAO bac = new tbl_BACDAOTAO();
                bac.MA_BAC_DAOTAO = r["MA_BAC_DAOTAO"].ToString();
                bac.TEN_BAC_DAOTAO = r["TEN_BAC_DAOTAO"].ToString();
                bac.CREATE_USER = r["USER"].ToString();
                bac.CREATE_TIME = System.DateTime.Today;
                bac.IS_DELETE = 0;

                db.tbl_BACDAOTAOs.InsertOnSubmit(bac);
                db.SubmitChanges();
                if (!string.IsNullOrEmpty(bac.ID_BAC_DAOTAO.ToString()))
                {
                    return true;
                }
                return false;
            }
            catch (Exception err)
            {
                throw err;
            }
        }

        public bool Update_BacDaoTao(params object[] param)
        {
            try
            {
                DataTable dt = (DataTable) param[0];
                DataRow r = dt.Rows[0];
                tbl_BACDAOTAO bac = db.tbl_BACDAOTAOs.Single(t => t.ID_BAC_DAOTAO == int.Parse(r["ID_BAC_DAOTAO"].ToString()));
                bac.MA_BAC_DAOTAO = r["MA_BAC_DAOTAO"].ToString();
                bac.TEN_BAC_DAOTAO = r["TEN_BAC_DAOTAO"].ToString();
                bac.UPDATE_USER = r["USER"].ToString();
                bac.UPDATE_TIME = System.DateTime.Today;
                bac.IS_DELETE = 0;
                db.SubmitChanges();
                if (!string.IsNullOrEmpty(bac.ID_BAC_DAOTAO.ToString()))
                {
                    return true;
                }
                return false;
            }
            catch (Exception err)
            {
                throw err;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DATN.TTS.DATA;

namespace DATN.TTS.BUS
{
    public class bus_Cauhinhhocphi
    {
        db_ttsDataContext db = new db_ttsDataContext();

        public DataTable GetAll_Namhoc()
        {
            try
            {
                DataTable dtRes = null;
                var query = from Tbl_NAMHOC_HIENTAI in db.tbl_NAMHOC_HIENTAIs
                            where
                              Tbl_NAMHOC_HIENTAI.IS_DELETE != 1 ||
                              Tbl_NAMHOC_HIENTAI.IS_DELETE == null
                            select new
                            {
                                Tbl_NAMHOC_HIENTAI.ID_NAMHOC_HIENTAI,
                                NAMHOC = Tbl_NAMHOC_HIENTAI.NAMHOC_TU.ToString() + '-' + (Tbl_NAMHOC_HIENTAI.NAMHOC_TU + 1).ToString(),
                                Tbl_NAMHOC_HIENTAI.SO_HKY_TRONGNAM
                            };
                dtRes = TableUtil.LinqToDataTable(query);
                return dtRes;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetAll_Namhoc_HKY(int pID_NAMHOC_HIENTAI)
        {
            try
            {
                DataTable dtRes = null;
                var query = from Tbl_NAMHOC_HKY_HTAI in db.tbl_NAMHOC_HKY_HTAIs
                            where
                              (Tbl_NAMHOC_HKY_HTAI.IS_DELETE != 1 ||
                              Tbl_NAMHOC_HKY_HTAI.IS_DELETE == null) &&
                              Tbl_NAMHOC_HKY_HTAI.ID_NAMHOC_HIENTAI == pID_NAMHOC_HIENTAI
                            select new
                            {
                                Tbl_NAMHOC_HKY_HTAI.ID_NAMHOC_HKY_HTAI,
                                HOCKY = Tbl_NAMHOC_HKY_HTAI.HOCKY.ToString()
                            };
                dtRes = TableUtil.LinqToDataTable(query);
                return dtRes;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetAll_HP(int pID_NAMHOC_HIENTAI)
        {
            try
            {
                DataTable dtRes = null;
                var query = from hp in db.tbl_HP_CAUHINH_HOCPHIs
                            join hk in db.tbl_NAMHOC_HKY_HTAIs on new { ID_NAMHOC_HKY_HTAI = Convert.ToInt32(hp.ID_NAMHOC_HKY_HTAI) } equals new { ID_NAMHOC_HKY_HTAI = hk.ID_NAMHOC_HKY_HTAI } into hk_join
                            from hk in hk_join.DefaultIfEmpty()
                            join nh in db.tbl_NAMHOC_HIENTAIs on new { ID_NAMHOC_HIENTAI = Convert.ToInt32(hk.ID_NAMHOC_HIENTAI) } equals new { ID_NAMHOC_HIENTAI = nh.ID_NAMHOC_HIENTAI } into nh_join
                            from nh in nh_join.DefaultIfEmpty()
                            where
                              (hp.IS_DELETE != 1 ||
                              hp.IS_DELETE == null) &&
                              (hk.IS_DELETE != 1 ||
                              hk.IS_DELETE == null) &&
                              (nh.IS_DELETE != 1 ||
                              nh.IS_DELETE == null) &&
                              nh.ID_NAMHOC_HIENTAI == pID_NAMHOC_HIENTAI
                            select new
                            {
                                hp.ID_CAUHINH_HOCPHI,
                                hp.ID_HE_DAOTAO,
                                TEN_HE_DAOTAO =
                                  ((from d in db.tbl_HEDAOTAOs
                                    where
                                      d.ID_HE_DAOTAO == hp.ID_HE_DAOTAO
                                    select new
                                    {
                                        d.TEN_HE_DAOTAO
                                    }).First().TEN_HE_DAOTAO),
                                hp.IS_LYTHUYET,
                                hp.DON_GIA,
                                hp.ID_NAMHOC_HKY_HTAI,
                                HOCKY = (int?)hk.HOCKY
                            };
                dtRes = TableUtil.LinqToDataTable(query);
                return dtRes;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetAll_HKyHientai(int pID_NAMHOC_HIENTAI)
        {
            try
            {
                DataTable dtRes = null;
                var query = from hk in db.tbl_NAMHOC_HKY_HTAIs
                            join nh in db.tbl_NAMHOC_HIENTAIs on new { ID_NAMHOC_HIENTAI = Convert.ToInt32(hk.ID_NAMHOC_HIENTAI) } equals new { ID_NAMHOC_HIENTAI = nh.ID_NAMHOC_HIENTAI } into nh_join
                            from nh in nh_join.DefaultIfEmpty()
                            where
                              (hk.IS_DELETE != 1 ||
                              hk.IS_DELETE == null) &&
                              (nh.IS_DELETE != 1 ||
                              nh.IS_DELETE == null) &&
                              nh.ID_NAMHOC_HIENTAI == pID_NAMHOC_HIENTAI &&
                              hk.IS_HIENTAI == 1
                            select new
                            {
                                hk.ID_NAMHOC_HKY_HTAI,
                                hk.HOCKY
                            };
                dtRes = TableUtil.LinqToDataTable(query);
                return dtRes;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetAll_NhocHientai()
        {
            try
            {
                DataTable dtRes = null;
                var query = from Tbl_NAMHOC_HIENTAI in db.tbl_NAMHOC_HIENTAIs
                            where
                              (Tbl_NAMHOC_HIENTAI.IS_DELETE != 1 ||
                              Tbl_NAMHOC_HIENTAI.IS_DELETE == null) &&
                              Tbl_NAMHOC_HIENTAI.IS_HIENTAI == 1
                            select new
                            {
                                Tbl_NAMHOC_HIENTAI.ID_NAMHOC_HIENTAI
                            };
                dtRes = TableUtil.LinqToDataTable(query);
                return dtRes;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int SaveObject(DataTable iDataSource, string pUser)
        {
            try
            {
                int i = 0;
                foreach (DataRow dr in iDataSource.Rows)
                {
                    if (!string.IsNullOrEmpty(dr["ID_CAUHINH_HOCPHI"].ToString()) && Convert.ToInt32(dr["ID_CAUHINH_HOCPHI"]) != 0)
                    {
                        #region Update
                        tbl_HP_CAUHINH_HOCPHI query = (from d in db.tbl_HP_CAUHINH_HOCPHIs
                                                 where
                                                   d.ID_CAUHINH_HOCPHI == Convert.ToInt32(dr["ID_CAUHINH_HOCPHI"])
                                                 select d).FirstOrDefault();
                        query.DON_GIA = Convert.ToDouble(dr["DON_GIA"]);
                        query.UPDATE_USER = pUser;
                        query.UPDATE_TIME = DateTime.Now;
                        db.SubmitChanges();
                        i = query.ID_CAUHINH_HOCPHI;
                        #endregion
                    }
                    else
                    {
                        #region Insert
                        tbl_HP_CAUHINH_HOCPHI query = new tbl_HP_CAUHINH_HOCPHI
                        {
                            ID_HE_DAOTAO = Convert.ToInt32(dr["ID_HE_DAOTAO"]),
                            ID_NAMHOC_HKY_HTAI = Convert.ToInt32(dr["ID_NAMHOC_HKY_HTAI"]),
                            IS_LYTHUYET = Convert.ToInt32(dr["IS_LYTHUYET"]),
                            DON_GIA = Convert.ToDouble(dr["DON_GIA"]),
                            CREATE_USER = pUser,
                            CREATE_TIME = DateTime.Now
                        };
                        db.tbl_HP_CAUHINH_HOCPHIs.InsertOnSubmit(query);
                        db.SubmitChanges();
                        i = query.ID_CAUHINH_HOCPHI;
                        #endregion
                    }
                }
                return i;
            }
            catch (Exception err)
            {

                throw err;
            }
        }
    }
}

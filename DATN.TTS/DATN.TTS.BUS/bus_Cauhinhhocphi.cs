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

        #region frm_CauHinhDotDKMH

        public DataTable GetAll_DOT_DK()
        {
            try
            {
                DataTable dtRes = null;
                var query = from d in db.tbl_HP_DOTDKs
                            join hk in db.tbl_NAMHOC_HKY_HTAIs on new { ID_NAMHOC_HKY_HTAI = Convert.ToInt32(d.ID_NAMHOC_HKY_HTAI) } equals new { ID_NAMHOC_HKY_HTAI = hk.ID_NAMHOC_HKY_HTAI } into hk_join
                            from hk in hk_join.DefaultIfEmpty()
                            join nh in db.tbl_NAMHOC_HIENTAIs on new { ID_NAMHOC_HIENTAI = Convert.ToInt32(hk.ID_NAMHOC_HIENTAI) } equals new { ID_NAMHOC_HIENTAI = nh.ID_NAMHOC_HIENTAI } into nh_join
                            from nh in nh_join.DefaultIfEmpty()
                            where
                              (d.ISDELETE != 1 ||
                              d.ISDELETE == null) &&
                              (hk.IS_DELETE != 1 ||
                              hk.IS_DELETE == null) &&
                              (nh.IS_DELETE != 1 ||
                              nh.IS_DELETE == null)
                            orderby
                              nh.NAMHOC_TU
                            select new
                            {
                                d.ID_DOTDK,
                                d.ID_NAMHOC_HKY_HTAI,
                                nh.ID_NAMHOC_HIENTAI,
                                hk.HOCKY,
                                d.ID_HE_DAOTAO,
                                d.MA_DOT_DK,
                                d.TEN_DOT_DK,
                                d.NGAY_BDAU,
                                d.NGAY_KTHUC,
                                NAMHOC_TU = (int?)nh.NAMHOC_TU,
                                NAMHOC = nh.NAMHOC_TU.ToString() + "-" + nh.NAMHOC_DEN.ToString()
                            };
                dtRes = TableUtil.LinqToDataTable(query);
                return dtRes;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int InsertObject(DataTable iDataSource)
        {
            try
            {
                int i = 0;
                tbl_HP_DOTDK iTbl_HP_DOTDK = new tbl_HP_DOTDK
                {
                    ID_NAMHOC_HKY_HTAI = Convert.ToInt32(iDataSource.Rows[0]["ID_NAMHOC_HKY_HTAI"]),
                    ID_HE_DAOTAO = Convert.ToInt32(iDataSource.Rows[0]["ID_HE_DAOTAO"]),
                    MA_DOT_DK = iDataSource.Rows[0]["MA_DOT_DK"].ToString(),
                    TEN_DOT_DK = iDataSource.Rows[0]["TEN_DOT_DK"].ToString(),
                    NGAY_BDAU = Convert.ToDateTime(iDataSource.Rows[0]["NGAY_BDAU"]),
                    NGAY_KTHUC = Convert.ToDateTime(iDataSource.Rows[0]["NGAY_KTHUC"]),
                    CREATE_USER = iDataSource.Rows[0]["USER"].ToString(),
                    CREATE_TIME = DateTime.Now
                };
                db.tbl_HP_DOTDKs.InsertOnSubmit(iTbl_HP_DOTDK);
                db.SubmitChanges();
                i = iTbl_HP_DOTDK.ID_DOTDK;
                return i;
            }
            catch (Exception err)
            {
                throw err;
            }
        }

        public int UpdateObject(DataTable iDataSource)
        {
            try
            {
                int i = 0;
                tbl_HP_DOTDK query =
                    (from Tbl_HP_DOTDK in db.tbl_HP_DOTDKs
                     where
                       Tbl_HP_DOTDK.ID_DOTDK == Convert.ToInt32(iDataSource.Rows[0]["ID_DOTDK"].ToString())
                     select Tbl_HP_DOTDK).FirstOrDefault();
                query.MA_DOT_DK = iDataSource.Rows[0]["MA_DOT_DK"].ToString();
                query.TEN_DOT_DK = iDataSource.Rows[0]["TEN_DOT_DK"].ToString();
                query.NGAY_BDAU = Convert.ToDateTime(iDataSource.Rows[0]["NGAY_BDAU"]);
                query.NGAY_KTHUC = Convert.ToDateTime(iDataSource.Rows[0]["NGAY_KTHUC"]);
                query.UPDATE_USER = iDataSource.Rows[0]["USER"].ToString();
                query.UPDATE_TIME = DateTime.Now;
                db.SubmitChanges();
                i = query.ID_DOTDK;
                return i;
            }
            catch (Exception err)
            {
                throw err;
            }
        }

        public int DeleteObject(int pID_DOTDK, string pUser)
        {
            try
            {
                int i = 0;
                tbl_HP_DOTDK query =
                    (from Tbl_HP_DOTDK in db.tbl_HP_DOTDKs
                     where
                       Tbl_HP_DOTDK.ID_DOTDK == pID_DOTDK
                     select Tbl_HP_DOTDK).FirstOrDefault();
                query.ISDELETE = 1;
                query.UPDATE_USER = pUser;
                query.UPDATE_TIME = DateTime.Now;
                db.SubmitChanges();
                i = query.ID_DOTDK;
                return i;
            }
            catch (Exception err)
            {
                throw err;
            }
        }

        public string New_MaAuto(int pID_HE_DAOTAO, int pID_NAMHOC_HKY_HTAI)
        {
            try
            {
                var query = from Tbl_HP_DOTDK in db.tbl_HP_DOTDKs
                    where
                        (Tbl_HP_DOTDK.ISDELETE != 1 ||
                         Tbl_HP_DOTDK.ISDELETE == null) &&
                        Tbl_HP_DOTDK.ID_NAMHOC_HKY_HTAI == pID_NAMHOC_HKY_HTAI &&
                        Tbl_HP_DOTDK.ID_HE_DAOTAO == pID_HE_DAOTAO
                    select new
                    {
                        tong = Tbl_HP_DOTDK.ID_DOTDK
                    };
                var namhoc = (from nh in db.tbl_NAMHOC_HIENTAIs
                    join hk in db.tbl_NAMHOC_HKY_HTAIs on new {ID_NAMHOC_HIENTAI = nh.ID_NAMHOC_HIENTAI}
                        equals new {ID_NAMHOC_HIENTAI = Convert.ToInt32(hk.ID_NAMHOC_HIENTAI)} into hk_join
                    from hk in hk_join.DefaultIfEmpty()
                    where
                        hk.ID_NAMHOC_HKY_HTAI == pID_NAMHOC_HKY_HTAI
                    select new
                    {
                        nhhk = nh.NAMHOC_TU.ToString() +"_" + hk.HOCKY.ToString()
                    }).First().nhhk;
                var hedaotao = (from Tbl_HEDAOTAO in db.tbl_HEDAOTAOs
                    where
                        Tbl_HEDAOTAO.ID_HE_DAOTAO == pID_HE_DAOTAO
                    select new
                    {
                        Tbl_HEDAOTAO.MA_HE_DAOTAO
                    }).First().MA_HE_DAOTAO;
                int soluong = query.Count() + 1;
                string xreturn = "DKMH_" + namhoc.ToString().Trim() + "_" + hedaotao.ToString().Trim() + "_" +
                                 soluong.ToString().Trim();
                while (!CheckTrung(xreturn))
                {
                     soluong += 1;
                     xreturn = "DKMH_" + namhoc.ToString().Trim() + "_" + hedaotao.ToString().Trim() + "_" +
                                     soluong.ToString().Trim();
                }
                return xreturn;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool CheckTrung(string pMA_DOT_DK)
        {
            try
            {
                var query = from dk in db.tbl_HP_DOTDKs
                            where
                              (dk.ISDELETE != 1 ||
                              dk.ISDELETE == null) &&
                              dk.MA_DOT_DK == pMA_DOT_DK
                            select new
                            {
                                dk.ID_DOTDK
                            };
                if (query.Count() < 1)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}

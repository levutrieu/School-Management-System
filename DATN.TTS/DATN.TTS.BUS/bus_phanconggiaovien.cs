using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DATN.TTS.DATA;

namespace DATN.TTS.BUS
{
    public class bus_phanconggiaovien
    {
        db_ttsDataContext db = new db_ttsDataContext();
        public DataTable GetAll_Hocphan()
        {
            try
            {
                DataTable dtRes = null;
                var query = from d in db.tbl_LOP_HOCPHANs
                            where
                                d.IS_DELETE != 1 ||
                                d.IS_DELETE == null
                            select new
                            {
                                d.ID_LOPHOCPHAN,
                                d.ID_LOPHOC,
                                TEN_LOP =
                                        ((from m in db.tbl_LOPHOCs
                                          where
                                              m.ID_LOPHOC == d.ID_LOPHOC
                                          select new
                                          {
                                              m.TEN_LOP
                                          }).First().TEN_LOP),
                                d.ID_KHOAHOC_NGANH_CTIET,
                                d.ID_NAMHOC_HKY_HTAI,
                                d.ID_HEDAOTAO,
                                TEN_HE_DAOTAO =
                                        ((from m in db.tbl_HEDAOTAOs
                                          where
                                              m.ID_HE_DAOTAO == d.ID_HEDAOTAO
                                          select new
                                          {
                                              m.TEN_HE_DAOTAO
                                          }).First().TEN_HE_DAOTAO),
                                //nganh
                                TEN_NGANH =
                                        ((from m in db.tbl_NGANHs
                                          where
                                              m.ID_NGANH == ((from h in db.tbl_KHOAHOC_NGANH_CTIETs
                                                              join k in db.tbl_KHOAHOC_NGANHs on new { ID_KHOAHOC_NGANH = Convert.ToInt32(h.ID_KHOAHOC_NGANH) } equals new { ID_KHOAHOC_NGANH = k.ID_KHOAHOC_NGANH } into k_join
                                                              from k in k_join.DefaultIfEmpty()
                                                              where
                                                                h.ID_KHOAHOC_NGANH_CTIET == 1
                                                              select new
                                                              {
                                                                  ID_NGANH = (int?)k.ID_NGANH
                                                              }).First().ID_NGANH)
                                          select new
                                          {
                                              m.TEN_NGANH
                                          }).First().TEN_NGANH),
                                d.ID_MONHOC,
                                //mon hoc
                                TEN_MONHOC =
                                        ((from m in db.tbl_MONHOCs
                                          where
                                              m.ID_MONHOC == d.ID_MONHOC
                                          select new
                                          {
                                              m.TEN_MONHOC
                                          }).First().TEN_MONHOC),
                                //bomon
                                TEN_BM =
                                        ((from m in db.tbl_MONHOCs
                                          join n in db.tbl_BOMONs on new { ID_BOMON = Convert.ToInt32(m.ID_BOMON) } equals new { ID_BOMON = n.ID_BOMON } into n_join
                                          from n in n_join.DefaultIfEmpty()
                                          where
                                            m.ID_MONHOC == 1 &&
                                            (n.ISDELETE != 1 ||
                                            n.ISDELETE == null)
                                          select new
                                          {
                                              TEN_BM = n.TEN_BM
                                          }).First().TEN_BM),
                                d.MA_LOP_HOCPHAN,
                                d.TEN_LOP_HOCPHAN,
                                d.TUAN_BD,
                                d.TUAN_KT,
                                d.SOTIET,
                                d.SOLUONG,
                                d.ID_GIANGVIEN,
                                TEN_GIANGVIEN =
                                        ((from m in db.tbl_GIANGVIENs
                                          where
                                              m.ID_GIANGVIEN == d.ID_GIANGVIEN
                                          select new
                                          {
                                              m.TEN_GIANGVIEN
                                          }).First().TEN_GIANGVIEN),
                                d.IS_DELETE,
                                d.CREATE_USER,
                                d.UPDATE_USER,
                                d.CREATE_TIME,
                                d.UPDATE_TIME
                            };
                dtRes = TableUtil.LinqToDataTable(query);
                return dtRes;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetGV_tree()
        {

            var query = (from d in db.tbl_KHOAs
                         where
                             (d.IS_DELETE != 1 ||
                              d.IS_DELETE == null)
                         select new
                         {
                             ID = string.Concat("K", d.ID_KHOA),
                             MA = d.ID_KHOA,
                             NAME = d.TEN_KHOA,
                             ID_PARENT = ""
                         }).Concat
                (from m in db.tbl_GIANGVIENs
                 where
                     (m.IS_DELETE != 1 ||
                      m.IS_DELETE == null)
                 select new
                 {
                     ID = string.Concat("G", m.ID_GIANGVIEN),
                     MA = m.ID_GIANGVIEN,
                     NAME = m.TEN_GIANGVIEN,
                     ID_PARENT = string.Concat("K", ((int?)m.ID_KHOA ?? 0))
                 }
                );
            DataTable xdt = null;
            xdt = TableUtil.LinqToDataTable(query);
            return xdt;
        }

        public int UpdateObject(int id_hocphan, int id_giangvien, string user)
        {
            try
            {
                int i = 0;
                tbl_LOP_HOCPHAN query = (from d in db.tbl_LOP_HOCPHANs
                                         where
                                             d.ID_LOPHOCPHAN == id_hocphan
                                         select d).FirstOrDefault();
                query.ID_GIANGVIEN = id_giangvien;
                query.UPDATE_USER = user;
                query.UPDATE_TIME = DateTime.Now;
                db.SubmitChanges();
                i = query.ID_LOPHOCPHAN;
                return i;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}

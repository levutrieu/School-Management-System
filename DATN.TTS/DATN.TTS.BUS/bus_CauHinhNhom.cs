using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DATN.TTS.DATA;

namespace DATN.TTS.BUS
{
    public class bus_CauHinhNhom:bus_CauHinhNguoiDung
    {
        db_ttsDataContext db = new db_ttsDataContext();

        public DataTable GetAllManHinh()
        {
            try
            {
                DataTable dtRes = new DataTable();
                dtRes.Columns.Add("MaManHinh");
                dtRes.Columns.Add("TenManHinh");
                dtRes.Columns.Add("GhiChu");
                dtRes.Columns.Add("IsNew");
                dtRes.Columns.Add("CoQuyen");
                var mh = from mhinh in db.tbl_ManHinhs select mhinh;
                foreach (var mhinh in mh)
                {
                    DataRow dr = dtRes.NewRow();
                    dr["MaManHinh"] = mhinh.MaManHinh;
                    dr["TenManHinh"] = mhinh.TenManHinh;
                    dr["GhiChu"] = mhinh.GhiChu;
                    dr["IsNew"] = "False";
                    dr["CoQuyen"] = "0";
                    dtRes.Rows.Add(dr);
                    dtRes.AcceptChanges();
                }
                return dtRes;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DataTable GetAllPhanQuyenUi()
        {
            try
            {
                var phanquyen = from pq in db.tbl_PhanQuyens select pq;

                return TableUtil.LinqToDataTable(phanquyen);
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        public DataTable GetPhanQuyenByWhere(string pMaNhom)
        {
            try
            {
                var phanquyen = (from pq in db.tbl_PhanQuyens where pq.MaNhomNguoiDung ==pMaNhom select pq).Distinct();

                return TableUtil.LinqToDataTable(phanquyen);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public DataTable GetPhanQuyenOne(string pMaNhom, string pMaMH)
        {
            try
            {
                DataTable dt = null;
                var ndn = (from nd in db.tbl_PhanQuyens where nd.MaManHinh == pMaMH && nd.MaNhomNguoiDung == pMaNhom select new { nd.MaManHinh, nd.MaNhomNguoiDung, nd.CoQuyen }).Distinct();

                dt = TableUtil.LinqToDataTable(ndn);
                return dt;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string ThemMaNhom()
        {
            ArrayList _MangPBH = new ArrayList();
            var _MaNhom = from _PBH in db.tbl_NhomNguoiDungs select _PBH;
            foreach (tbl_NhomNguoiDung bh in _MaNhom)
            {
                _MangPBH.Add(int.Parse(bh.MaNhomNguoiDung.Substring(3)));
            }
            return TableUtil.KiemTra(_MangPBH, "N");
        }

        public bool Insert_Nhom(params object[] param)
        {
            try
            {
                DataTable dt = (DataTable) param[0];
                DataRow dr = dt.Rows[0];

                tbl_NhomNguoiDung mh = new tbl_NhomNguoiDung();
                mh.MaNhomNguoiDung = dr["MaNhomNguoiDung"].ToString();
                mh.TenNhomNguoiDUng = dr["TenNhomNguoiDUng"].ToString();
                mh.GhiChu = dr["GhiChu"].ToString();

                db.tbl_NhomNguoiDungs.InsertOnSubmit(mh);
                db.SubmitChanges();
                if (!string.IsNullOrEmpty(mh.MaNhomNguoiDung))
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

        public void Delete_Nhom(string pMaNhom)
        {
            try
            {
                tbl_NhomNguoiDung nhom = db.tbl_NhomNguoiDungs.Single(t => t.MaNhomNguoiDung == pMaNhom);

                db.tbl_NhomNguoiDungs.DeleteOnSubmit(nhom);
                db.SubmitChanges();
            }
            catch
            {
                throw;
            }
        }

        public void Update_Nhom(params object[] param)
        {
            try
            {
                DataTable dt = (DataTable) param[0];
                DataRow dr = dt.Rows[0];
                tbl_NhomNguoiDung mh = db.tbl_NhomNguoiDungs.Single(t => t.MaNhomNguoiDung == dr["MaNhomNguoiDung"].ToString().Trim());
                mh.TenNhomNguoiDUng = dr["TenNhomNguoiDUng"].ToString();
                mh.GhiChu = dr["GhiChu"].ToString();
                db.SubmitChanges();
            }
            catch
            {
                throw;
            }
        }

        public bool Insert_PhanQuyenUI(params object[] param)
        {
            try
            {
                DataTable dt = (DataTable)param[0];
                DataRow dr = dt.Rows[0];

                tbl_PhanQuyen phanQuyen = new tbl_PhanQuyen();
                phanQuyen.MaManHinh = dr["MaManHinh"].ToString();
                phanQuyen.MaNhomNguoiDung = dr["MaNhomNguoiDung"].ToString();
                phanQuyen.CoQuyen = int.Parse(dr["CoQuyen"].ToString());

                db.tbl_PhanQuyens.InsertOnSubmit(phanQuyen);
                db.SubmitChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void Delete_PhanQuyenUI(params object[] param)
        {
            try
            {
                DataTable dt = (DataTable)param[0];
                DataRow dr = dt.Rows[0];
                tbl_PhanQuyen phanQuyen = db.tbl_PhanQuyens.Single(t => t.MaManHinh == dr["MaManHinh"].ToString().Trim() 
                                                                     && t.MaNhomNguoiDung == dr["MaNhomNguoiDung"].ToString().Trim());

                db.tbl_PhanQuyens.DeleteOnSubmit(phanQuyen);
                db.SubmitChanges();
            }
            catch
            {
                throw;
            }
        }
    }
}

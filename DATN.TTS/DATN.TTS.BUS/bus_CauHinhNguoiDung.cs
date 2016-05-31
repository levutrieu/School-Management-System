using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DATN.TTS.DATA;

namespace DATN.TTS.BUS
{
    public class bus_CauHinhNguoiDung
    {
        db_ttsDataContext db = new db_ttsDataContext();

        public DataTable GetAllNhom()
        {
            DataTable dtRes = new DataTable();
            dtRes.Columns.Add("MaNhomNguoiDung");
            dtRes.Columns.Add("TenNhomNguoiDUng");
            dtRes.Columns.Add("GhiChu");
            dtRes.Columns.Add("IsNew");
            try
            {
                var nhom = from nhomnd in db.tbl_NhomNguoiDungs select nhomnd;
                foreach (var nhomn in nhom)
                {
                    DataRow dr = dtRes.NewRow();
                    dr["MaNhomNguoiDung"] = nhomn.MaNhomNguoiDung;
                    dr["TenNhomNguoiDUng"] = nhomn.TenNhomNguoiDUng;
                    dr["IsNew"] = "False";
                    dr["GhiChu"] = nhomn.GhiChu;
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

        public DataTable GetNhomWhere(string User)
        {
            try
            {
                DataTable dt = null;

                var nhom = from nhomnd in db.tbl_NDung_NhomNDungs where nhomnd.UserName.Equals(User) && nhomnd.MaNhomNguoiDung !=string.Empty 
                           select new{ nhomnd.MaNhomNguoiDung};

                dt = TableUtil.LinqToDataTable(nhom);

                return dt;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DataTable GetAllNhanVien()
        {
            DataTable dt = null;
            //DataTable dt = new DataTable();
            //dt.Columns.Add("ID_NhanVien");
            //dt.Columns.Add("HOTEN");
            //dt.Columns.Add("NhanVien");
            try
            {
                var nvien = from nv in db.tbl_NhanSus where nv.IS_DELETE ==0 select nv;
                dt = TableUtil.LinqToDataTable(nvien);
                //foreach (var nv in nvien)
                //{
                //    DataRow dr = dt.NewRow();
                //    dr["ID_NhanVien"] = nv.ID_NHANVIEN;
                //    //dr["NhanVien"] = nv.MA_NHANVIEN + "-" + nv.HOTEN;
                //    dr["HOTEN"] = nv.HOTEN;
                //    dt.Rows.Add(dr);
                //    dt.AcceptChanges();
                //}
                return dt;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool InsertNguoiDung(params object[] oparam)
        {
            bool res = true;
            try
            {
                DataTable dt = (DataTable) oparam[0];
                DataRow dr = dt.Rows[0];
                tbl_NguoiDung tblNguoiDung = new tbl_NguoiDung();
                tblNguoiDung.UserName = dr["UserName"].ToString();
                tblNguoiDung.Pass = dr["Pass"].ToString();
                tblNguoiDung.HoatDong = int.Parse(dr["HoatDong"].ToString());
                tblNguoiDung.GhiChu = dr["GhiChu"].ToString();
                tblNguoiDung.ID_NhanVien = int.Parse(dr["ID_NHANVIEN"].ToString());
                tblNguoiDung.IS_DELETE = 0;
                db.tbl_NguoiDungs.InsertOnSubmit(tblNguoiDung);
                db.SubmitChanges();

                string strRes = tblNguoiDung.UserName;

                if (string.IsNullOrEmpty(strRes))
                {
                    res = false;
                }
                else
                {
                    res = true;
                }
                return res;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateNguoiDung(params object[] param)
        {
            try
            {
                DataTable dt = (DataTable)param[0];
                DataRow dr = dt.Rows[0];
                tbl_NguoiDung tblNguoiDung = db.tbl_NguoiDungs.Single(t => t.UserName == dr["UserName"].ToString().Trim());
                tblNguoiDung.Pass = dr["Pass"].ToString();
                tblNguoiDung.HoatDong = int.Parse(dr["HoatDong"].ToString());
                tblNguoiDung.GhiChu = dr["GhiChu"].ToString();
                tblNguoiDung.ID_NhanVien = int.Parse(dr["ID_NHANVIEN"].ToString());

                db.SubmitChanges();
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        public void Delete_NhanSu(params object[] param)
        {
            try
            {
                DataTable dt = (DataTable)param[0];
                DataRow dr = dt.Rows[0];
                tbl_NguoiDung ns = db.tbl_NguoiDungs.Single(t => t.UserName.Trim() == dr["UserName"].ToString().Trim());
                ns.IS_DELETE = 1;
                db.SubmitChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DataTable GetAllNDungNhom(string pUserName, string pMaNhomND)
        {
            try
            {
                DataTable dt = null;
                var ndn = (from nd in db.tbl_NDung_NhomNDungs where nd.UserName == pUserName && nd.MaNhomNguoiDung == pMaNhomND select new{nd.UserName, nd.MaNhomNguoiDung}).Distinct();

                dt = TableUtil.LinqToDataTable(ndn);
                return dt;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool InsertNDungVaoNhom(string pUserName, string pMaNhomNguoiDung)
        {
            bool res = true;
            try
            {
                tbl_NDung_NhomNDung tblNguoiDung = new tbl_NDung_NhomNDung();
                tblNguoiDung.UserName = pUserName;
                tblNguoiDung.MaNhomNguoiDung = pMaNhomNguoiDung;
                db.tbl_NDung_NhomNDungs.InsertOnSubmit(tblNguoiDung);
                db.SubmitChanges();

                string strRes = tblNguoiDung.UserName.ToString() +""+tblNguoiDung.MaNhomNguoiDung.ToString();

                if (string.IsNullOrEmpty(strRes))
                {
                    res = false;
                }
                else
                {
                    res = true;
                }
                return res;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //public void UpdateNDungVaoNhom(string pUserName, string pMaNhomNguoiDung)
        //{
        //    try
        //    {
        //        tbl_NDung_NhomNDung up =
        //            db.tbl_NDung_NhomNDungs.Single(t => t.UserName.Trim() == pUserName.Trim() && t.MaNhomNguoiDung.Trim() == pMaNhomNguoiDung.Trim());
        //        db.SubmitChanges();
        //    }
        //    catch (Exception)
        //    {
                
        //        throw;
        //    }
        //}

        public void DeleteNDungVaoNhom(string pUserName, string pMaNhomNguoiDung)
        {
            try
            {
                tbl_NDung_NhomNDung up =
                    db.tbl_NDung_NhomNDungs.Single(t=>t.UserName.Trim() == pUserName.Trim() && t.MaNhomNguoiDung.Trim() == pMaNhomNguoiDung.Trim());

                db.tbl_NDung_NhomNDungs.DeleteOnSubmit(up);
                db.SubmitChanges();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public DataTable GetAllNguoiDung()
        {
            try
            {
                var ndung = from nd in db.tbl_NguoiDungs where nd.IS_DELETE ==0
                    join nv in db.tbl_NhanSus on nd.ID_NhanVien equals nv.ID_NHANVIEN
                 select new{nd.UserName, nd.HoatDong, nd.Pass, nd.GhiChu, nv.HOTEN, nd.ID_NhanVien};
                return TableUtil.LinqToDataTable(ndung);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool KiemTraUse(string pUser)
        {
            try
            {
                var user = (from us in db.tbl_NguoiDungs where us.UserName == pUser select us).Distinct();

                DataTable dt = TableUtil.LinqToDataTable(user);
                if (dt.Rows.Count <=0)
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
    }
}

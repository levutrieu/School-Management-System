using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using DATN.TTS.DATA;

namespace DATN.TTS.BUS
{
    public class bus_namhoc
    {
        db_ttsDataContext db = new db_ttsDataContext();

        public DataTable GetAll()
        {
            try
            {
                DataTable dt = new DataTable();
                var namhoc = from nh in db.tbl_NAMHOC_HIENTAIs where nh.IS_DELETE == 0 select nh;
                dt = TableUtil.LinqToDataTable(namhoc);
                return dt;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool SetNamHocHienTai(int pID_NAMHOC_HIENTAI)
        {
            try
            {
                tbl_NAMHOC_HIENTAI namhoc = db.tbl_NAMHOC_HIENTAIs.Single(t => t.ID_NAMHOC_HIENTAI == pID_NAMHOC_HIENTAI);
                namhoc.IS_HIENTAI = 1;
                db.SubmitChanges();
                var nhoc = from nam in db.tbl_NAMHOC_HIENTAIs.Where(t => t.ID_NAMHOC_HIENTAI != pID_NAMHOC_HIENTAI && (t.IS_DELETE != 1 || t.IS_DELETE == null)) select nam;
                DataTable dt = new DataTable();
                dt = TableUtil.LinqToDataTable(nhoc);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow r in dt.Rows)
                    {
                        tbl_NAMHOC_HIENTAI sethien = db.tbl_NAMHOC_HIENTAIs.Single(t => t.ID_NAMHOC_HIENTAI == Convert.ToInt32(r["ID_NAMHOC_HIENTAI"].ToString()));
                        sethien.IS_HIENTAI = 0;
                        db.SubmitChanges();
                    }
                }
                db.SubmitChanges();
                if (namhoc.ID_NAMHOC_HIENTAI > 0)
                    return true;
                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool Insert_NamHocHienTai(DataTable iDataSoure)
        {
            try
            {
                DataRow r = iDataSoure.Rows[0];
                tbl_NAMHOC_HIENTAI namhoc = new tbl_NAMHOC_HIENTAI();
                namhoc.NAMHOC_TU = Convert.ToInt32(r["NAMHOC_TU"]);
                namhoc.NAMHOC_DEN = Convert.ToInt32(r["NAMHOC_DEN"]);
                namhoc.NGAY_BATDAU = Convert.ToDateTime(r["NGAY_BATDAU"]);
                namhoc.SO_TUAN = Convert.ToInt32(r["SO_TUAN"]);
                namhoc.SO_HKY_TRONGNAM = Convert.ToInt32(r["SO_HKY_TRONGNAM"]);
                //namhoc.IS_HIENTAI = Convert.ToInt32(r[""]);
                //namhoc.IS_DELETE = Convert.ToInt32(r[""]);
                namhoc.CREATE_USER = r["USER"].ToString();
                namhoc.CREATE_TIME = DateTime.Now;

                db.tbl_NAMHOC_HIENTAIs.InsertOnSubmit(namhoc);
                db.SubmitChanges();
                if (!namhoc.ID_NAMHOC_HIENTAI.GetTypeCode().Equals(TypeCode.DBNull))
                    return true;
                return false;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public bool Delete_NamHocHienTai(int pID_NAMHOC_HIENTAI, string pUSER)
        {
            try
            {
                tbl_NAMHOC_HIENTAI namhoc = db.tbl_NAMHOC_HIENTAIs.Single(t => t.ID_NAMHOC_HIENTAI == pID_NAMHOC_HIENTAI);
                namhoc.IS_DELETE = 1;
                namhoc.UPDATE_USER = pUSER;
                namhoc.UPDATE_TIME = DateTime.Now;
                db.SubmitChanges();
                db.SubmitChanges();
                if (namhoc.ID_NAMHOC_HIENTAI > 0)
                    return true;
                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool Update_NamHocHienTai(DataTable iDataSoure)
        {
            try
            {
                DataRow r = iDataSoure.Rows[0];
                tbl_NAMHOC_HIENTAI namhoc = db.tbl_NAMHOC_HIENTAIs.Single(t => t.ID_NAMHOC_HIENTAI == Convert.ToInt32(r["ID_NAMHOC_HIENTAI"].ToString()));
                namhoc.NAMHOC_TU = Convert.ToInt32(r["NAMHOC_TU"]);
                namhoc.NAMHOC_DEN = Convert.ToInt32(r["NAMHOC_DEN"]);
                namhoc.NGAY_BATDAU = Convert.ToDateTime(r["NGAY_BATDAU"]);
                namhoc.SO_TUAN = Convert.ToInt32(r["SO_TUAN"]);
                namhoc.SO_HKY_TRONGNAM = Convert.ToInt32(r["SO_HKY_TRONGNAM"]);
                namhoc.UPDATE_USER = r["USER"].ToString();
                namhoc.UPDATE_TIME = DateTime.Now;

                db.SubmitChanges();
                if (namhoc.ID_NAMHOC_HIENTAI > 0)
                    return true;
                return false;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public DataTable GetAllNamHoc()
        {
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("NAMHOC", typeof(string));
                dt.Columns.Add("ID_NAMHOC_HIENTAI", typeof(Decimal));
                DataRow dr = dt.NewRow();
                dr["NAMHOC"] = "----------------Chọn-------------------";
                dr["ID_NAMHOC_HIENTAI"] = 0;
                dt.Rows.Add(dr);
                var namhoc = from nh in db.tbl_NAMHOC_HIENTAIs where (nh.IS_DELETE != 1 || nh.IS_DELETE == null) select nh;
                foreach (var nh in namhoc)
                {
                    DataRow r = dt.NewRow();
                    r["NAMHOC"] = nh.NAMHOC_TU + "_" + nh.NAMHOC_DEN;
                    r["ID_NAMHOC_HIENTAI"] = nh.ID_NAMHOC_HIENTAI;

                    dt.Rows.Add(r);
                    dt.AcceptChanges();
                }
                return dt;
            }
            catch (Exception err)
            {
                throw err;
            }
        }

        public DataTable GetData(int pID_NAMHOC_HIENTAI, int pHOCKY)
        {
            try
            {
                DataTable dt = null;
                if (pID_NAMHOC_HIENTAI != 0 && pHOCKY != 0)
                {
                    #region Truong hop 1 "Có điều kiện giữa hocky và nam hoc hiện tại"

                    dt = GetData_2(pID_NAMHOC_HIENTAI, pHOCKY);

                    #endregion
                }
                else
                {
                    if (pID_NAMHOC_HIENTAI == 0 && pHOCKY != 0)
                    {
                        #region Truong hop 2 chỉ có điều kiện ở học kỳ

                        dt = GetData_3(pHOCKY);

                        #endregion
                    }
                    else
                    {
                        if (pID_NAMHOC_HIENTAI != 0 && pHOCKY == 0)
                        {
                            #region Truong hop 3 chỉ có điều kiện ở năm học hiện tại

                            dt = GetData_4(pID_NAMHOC_HIENTAI);

                            #endregion
                        }
                        else
                        {
                            if (pID_NAMHOC_HIENTAI == 0 && pHOCKY == 0)
                            {
                                #region Truong hop 4 "Không có điều kiện

                                dt = GetData_1();

                                #endregion
                            }
                        }
                    }
                    
                }
                return dt;
            }
            catch (Exception er)
            {
                throw er;
            }   
        }

        private DataTable GetData_1()
        {
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("ID_NAMHOC_HKY_HTAI", typeof(Decimal));
                dt.Columns.Add("ID_NAMHOC_HIENTAI", typeof(Decimal));
                dt.Columns.Add("NAMHOC_TU", typeof(Decimal));
                dt.Columns.Add("NAMHOC", typeof(string));
                dt.Columns.Add("HOCKY", typeof(Decimal));
                dt.Columns.Add("IS_HIENTAI", typeof(Decimal));
                var hockynamhoc = from a in db.tbl_NAMHOC_HKY_HTAIs
                                  join b in db.tbl_NAMHOC_HIENTAIs on new { ID_NAMHOC_HIENTAI = Convert.ToInt32(a.ID_NAMHOC_HIENTAI) } equals new { ID_NAMHOC_HIENTAI = b.ID_NAMHOC_HIENTAI }
                                  where
                                    a.IS_DELETE == 0 
                                  select new
                                  {
                                      ID_NAMHOC_HIENTAI = (int?)a.ID_NAMHOC_HIENTAI,
                                      a.ID_NAMHOC_HKY_HTAI,
                                      a.IS_HIENTAI,
                                      a.HOCKY,
                                      b.NAMHOC_TU,
                                      b.NAMHOC_DEN
                                  };
                foreach (var hk in hockynamhoc)
                {
                    DataRow r = dt.NewRow();
                    r["ID_NAMHOC_HKY_HTAI"] = hk.ID_NAMHOC_HKY_HTAI;
                    r["ID_NAMHOC_HIENTAI"] = hk.ID_NAMHOC_HIENTAI;
                    r["NAMHOC_TU"] = hk.NAMHOC_TU;
                    r["NAMHOC"] = hk.NAMHOC_TU + "_" + hk.NAMHOC_DEN;
                    r["HOCKY"] = hk.HOCKY;
                    r["IS_HIENTAI"] = hk.IS_HIENTAI;

                    dt.Rows.Add(r);
                    dt.AcceptChanges();
                }

                return dt;
            }
            catch (Exception err)
            {
                throw err;
            }
        }

        private DataTable GetData_2(int pID_NAMHOC_HIENTAI, int pHOCKY)
        {
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("ID_NAMHOC_HKY_HTAI", typeof(Decimal));
                dt.Columns.Add("ID_NAMHOC_HIENTAI", typeof(Decimal));
                dt.Columns.Add("NAMHOC_TU", typeof(Decimal));
                dt.Columns.Add("NAMHOC", typeof(string));
                dt.Columns.Add("HOCKY", typeof(Decimal));
                dt.Columns.Add("IS_HIENTAI", typeof(Decimal));
                var hockynamhoc = from a in db.tbl_NAMHOC_HKY_HTAIs
                                  join b in db.tbl_NAMHOC_HIENTAIs on new { ID_NAMHOC_HIENTAI = Convert.ToInt32(a.ID_NAMHOC_HIENTAI) } equals new { ID_NAMHOC_HIENTAI = b.ID_NAMHOC_HIENTAI }
                                  where
                                    a.IS_DELETE == 0 &&
                                    a.ID_NAMHOC_HIENTAI == pID_NAMHOC_HIENTAI &&
                                    a.HOCKY == pHOCKY
                                  select new
                                  {
                                      ID_NAMHOC_HIENTAI = (int?)a.ID_NAMHOC_HIENTAI,
                                      a.ID_NAMHOC_HKY_HTAI,
                                      a.IS_HIENTAI,
                                      a.HOCKY,
                                      b.NAMHOC_TU,
                                      b.NAMHOC_DEN
                                  };
                foreach (var hk in hockynamhoc)
                {
                    DataRow r = dt.NewRow();
                    r["ID_NAMHOC_HKY_HTAI"] = hk.ID_NAMHOC_HKY_HTAI;
                    r["ID_NAMHOC_HIENTAI"] = hk.ID_NAMHOC_HIENTAI;
                    r["NAMHOC_TU"] = hk.NAMHOC_TU;
                    r["NAMHOC"] = hk.NAMHOC_TU + "_" + hk.NAMHOC_DEN;
                    r["HOCKY"] = hk.HOCKY;
                    r["IS_HIENTAI"] = hk.IS_HIENTAI;

                    dt.Rows.Add(r);
                    dt.AcceptChanges();
                }

                return dt;
            }
            catch (Exception err)
            {
                throw err;
            }
        }

        private DataTable GetData_3(int pHOCKY)
        {
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("ID_NAMHOC_HKY_HTAI", typeof(Decimal));
                dt.Columns.Add("ID_NAMHOC_HIENTAI", typeof(Decimal));
                dt.Columns.Add("NAMHOC_TU", typeof(Decimal));
                dt.Columns.Add("NAMHOC", typeof(string));
                dt.Columns.Add("HOCKY", typeof(Decimal));
                dt.Columns.Add("IS_HIENTAI", typeof(Decimal));
                var hockynamhoc = from a in db.tbl_NAMHOC_HKY_HTAIs
                                  join b in db.tbl_NAMHOC_HIENTAIs on new { ID_NAMHOC_HIENTAI = Convert.ToInt32(a.ID_NAMHOC_HIENTAI) } equals new { ID_NAMHOC_HIENTAI = b.ID_NAMHOC_HIENTAI }
                                  where
                                    a.IS_DELETE == 0 &&
                                    a.HOCKY == pHOCKY
                                  select new
                                  {
                                      ID_NAMHOC_HIENTAI = (int?)a.ID_NAMHOC_HIENTAI,
                                      a.ID_NAMHOC_HKY_HTAI,
                                      a.IS_HIENTAI,
                                      a.HOCKY,
                                      b.NAMHOC_TU,
                                      b.NAMHOC_DEN
                                  };
                foreach (var hk in hockynamhoc)
                {
                    DataRow r = dt.NewRow();
                    r["ID_NAMHOC_HKY_HTAI"] = hk.ID_NAMHOC_HKY_HTAI;
                    r["ID_NAMHOC_HIENTAI"] = hk.ID_NAMHOC_HIENTAI;
                    r["NAMHOC_TU"] = hk.NAMHOC_TU;
                    r["NAMHOC"] = hk.NAMHOC_TU + "_" + hk.NAMHOC_DEN;
                    r["HOCKY"] = hk.HOCKY;
                    r["IS_HIENTAI"] = hk.IS_HIENTAI;

                    dt.Rows.Add(r);
                    dt.AcceptChanges();
                }

                return dt;
            }
            catch (Exception err)
            {
                throw err;
            }
        }

        private DataTable GetData_4(int pID_NAMHOC_HIENTAI)
        {
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("ID_NAMHOC_HKY_HTAI", typeof(Decimal));
                dt.Columns.Add("ID_NAMHOC_HIENTAI", typeof(Decimal));
                dt.Columns.Add("NAMHOC_TU", typeof(Decimal));
                dt.Columns.Add("NAMHOC", typeof(string));
                dt.Columns.Add("HOCKY", typeof(Decimal));
                dt.Columns.Add("IS_HIENTAI", typeof(Decimal));
                var hockynamhoc = from a in db.tbl_NAMHOC_HKY_HTAIs
                                  join b in db.tbl_NAMHOC_HIENTAIs on new { ID_NAMHOC_HIENTAI = Convert.ToInt32(a.ID_NAMHOC_HIENTAI) } equals new { ID_NAMHOC_HIENTAI = b.ID_NAMHOC_HIENTAI }
                                  where
                                    a.IS_DELETE == 0 &&
                                    a.ID_NAMHOC_HIENTAI == pID_NAMHOC_HIENTAI
                                  select new
                                  {
                                      ID_NAMHOC_HIENTAI = (int?)a.ID_NAMHOC_HIENTAI,
                                      a.ID_NAMHOC_HKY_HTAI,
                                      a.IS_HIENTAI,
                                      a.HOCKY,
                                      b.NAMHOC_TU,
                                      b.NAMHOC_DEN
                                  };
                foreach (var hk in hockynamhoc)
                {
                    DataRow r = dt.NewRow();
                    r["ID_NAMHOC_HKY_HTAI"] = hk.ID_NAMHOC_HKY_HTAI;
                    r["ID_NAMHOC_HIENTAI"] = hk.ID_NAMHOC_HIENTAI;
                    r["NAMHOC_TU"] = hk.NAMHOC_TU;
                    r["NAMHOC"] = hk.NAMHOC_TU + "_" + hk.NAMHOC_DEN;
                    r["HOCKY"] = hk.HOCKY;
                    r["IS_HIENTAI"] = hk.IS_HIENTAI;

                    dt.Rows.Add(r);
                    dt.AcceptChanges();
                }

                return dt;
            }
            catch (Exception err)
            {
                throw err;
            }
        }

        public bool Insert_HocKyNamHoc(int pID_NAMHOC_HIENTAI, int pHOCKY, string pUser)
        {
            try
            {
                // thực hiện thêm mới học kỳ năm học
                tbl_NAMHOC_HKY_HTAI hknamhoc = new tbl_NAMHOC_HKY_HTAI();
                hknamhoc.ID_NAMHOC_HIENTAI = pID_NAMHOC_HIENTAI;
                hknamhoc.HOCKY = pHOCKY;
                hknamhoc.CREATE_USER = pUser;
                hknamhoc.CREATE_TIME = System.DateTime.Now;
                hknamhoc.IS_DELETE = 0;
                hknamhoc.IS_HIENTAI = 1;
                db.tbl_NAMHOC_HKY_HTAIs.InsertOnSubmit(hknamhoc);
                db.SubmitChanges();
                //kết thúc thực hiện thêm mới

                //Thực hiện cập nhật
                var hknamhocUp = from hk in db.tbl_NAMHOC_HKY_HTAIs
                               where (hk.IS_DELETE != 1 || hk.IS_DELETE == null)
                                     && hk.ID_NAMHOC_HIENTAI == pID_NAMHOC_HIENTAI && hk.HOCKY != pHOCKY
                               select hk;
                DataTable dt = new DataTable();
                dt = TableUtil.LinqToDataTable(hknamhocUp);
                if (dt.Rows.Count > 0)
                {
                    //duyệt tìm để set lại is_hientai = 0 cho năm học trong học kỳ đó
                    foreach (DataRow r in dt.Rows)
                    {
                        tbl_NAMHOC_HKY_HTAI hkyHtai = db.tbl_NAMHOC_HKY_HTAIs.Single(t => t.ID_NAMHOC_HKY_HTAI == Convert.ToInt32(r["ID_NAMHOC_HKY_HTAI"].ToString()));
                        hkyHtai.IS_HIENTAI = 0;
                        hkyHtai.UPDATE_USER = pUser;
                        hkyHtai.UPDATE_TIME = System.DateTime.Now;
                        db.SubmitChanges();
                    }
                }
                // kết thúc cập nhật
                if (!hknamhoc.ID_NAMHOC_HKY_HTAI.GetTypeCode().Equals(TypeCode.DBNull))
                    return true;
                return false;
            }
            catch (Exception err)
            {
                throw err;
            }
        }

        public bool Update_IsHienTai(int pID_NAMHOC_HIENTAI, int pHOCKY, string pUser)
        {
            try
            {
                // thực hiện cập nhật khi trùng dữ liệu
                tbl_NAMHOC_HKY_HTAI hky = db.tbl_NAMHOC_HKY_HTAIs.Single(t => t.ID_NAMHOC_HIENTAI == pID_NAMHOC_HIENTAI && t.HOCKY == pHOCKY);
                hky.IS_HIENTAI = 1;
                hky.UPDATE_USER = pUser;
                hky.UPDATE_TIME = System.DateTime.Now;
                db.SubmitChanges();
                //Lấy danh sách học kỳ năm học có ID_NAMHOC_HIENTAI = vs tham số năm học và học kỳ khác vs tham số học kỳ
                var hknamhoc = from hk in db.tbl_NAMHOC_HKY_HTAIs
                    where (hk.IS_DELETE != 1 || hk.IS_DELETE == null) 
                          && hk.ID_NAMHOC_HIENTAI == pID_NAMHOC_HIENTAI && hk.HOCKY != pHOCKY
                    select hk;
                DataTable dt = new DataTable();
                dt = TableUtil.LinqToDataTable(hknamhoc);
                if (dt.Rows.Count > 0)
                {
                    //duyệt tìm để set lại is_hientai = 0 cho năm học trong học kỳ đó
                    foreach (DataRow r in dt.Rows)
                    {
                        tbl_NAMHOC_HKY_HTAI hkyHtai = db.tbl_NAMHOC_HKY_HTAIs.Single(t => t.ID_NAMHOC_HKY_HTAI == Convert.ToInt32(r["ID_NAMHOC_HKY_HTAI"].ToString()));
                        hkyHtai.IS_HIENTAI = 0;
                        hkyHtai.UPDATE_USER = pUser;
                        hkyHtai.UPDATE_TIME = System.DateTime.Now;
                        db.SubmitChanges();
                    }
                }
                if (hky.ID_NAMHOC_HKY_HTAI > 0)
                    return true;
                return false;

            }
            catch (Exception err)
            {
                throw err;
            }
        }

        public bool CheckTrungHocKyNamHoc(int pID_NAMHOC_HIENTAI, int pHOCKY)
        {
            try
            {
                var hknamhoc = from hk in db.tbl_NAMHOC_HKY_HTAIs
                    where (hk.IS_DELETE != 1 || hk.IS_DELETE == null) 
                          && hk.ID_NAMHOC_HIENTAI == pID_NAMHOC_HIENTAI && hk.HOCKY == pHOCKY
                    select hk;
                DataTable dt = new DataTable();
                dt = TableUtil.LinqToDataTable(hknamhoc);
                if (dt.Rows.Count > 0)
                {
                    return false;
                }
                return true;
            }
            catch (Exception err)
            {
                throw err;
            }
        }
    }
}

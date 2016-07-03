using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Linq;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DATN.TTS.BUS.Resource;
using DATN.TTS.DATA;

namespace DATN.TTS.BUS
{
    public class bus_login
    {
        db_ttsDataContext db = new db_ttsDataContext();

        public int Check_Config()
        {
            if (DATN.TTS.DATA.Properties.Settings.Default.ctr == string.Empty)
                return 1;// Chuỗi cấu hình không tồn tại
            SqlConnection _Sqlconn = new SqlConnection(DATN.TTS.DATA.Properties.Settings.Default.ctr);
            try
            {
                if (_Sqlconn.State == System.Data.ConnectionState.Closed)
                    _Sqlconn.Open();
                return 0;// Kết nối thành công chuỗi cấu hình hợp lệ
            }
            catch
            {
                return 2;// Chuỗi cấu hình không phù hợp.
            }
        }

        public int CheckLogin(string account, string pass)
        {
            var xcheck = from nd in db.tbl_NguoiDungs
                where nd.UserName.Equals(account) && nd.Pass.Equals(pass)
                select nd;
            DataTable xdt = TableUtil.LinqToDataTable(xcheck);
            if (xdt != null && xdt.Rows.Count > 0)
            {
                if (Convert.ToInt32(xdt.Rows[0]["HoatDong"]) == 1)
                {
                    if (!string.IsNullOrEmpty(xdt.Rows[0]["ID_NhanVien"].ToString()))
                    {
                        UserCommon.IdNhanVien = Convert.ToInt32(xdt.Rows[0]["ID_NhanVien"].ToString());
                        UserCommon.TenNhanVien =
                            (from d in db.tbl_NhanSus
                                where d.ID_NHANVIEN == Convert.ToInt32(xdt.Rows[0]["ID_NhanVien"].ToString())
                                select d).First().HOTEN;
                        UserCommon.UserName = account;
                    }
                    return 1; // dang nhap thanh cong
                }
                else
                {
                    return 2;// tai khoan bi khoa
                }
            }
            return 0; // tai khoan khong dung
        }

        public bool ChangeConnectionString(string pServerName, string pDataBase, string pUser, string pPass)
        {
            try
            {
                if (string.IsNullOrEmpty(pServerName)) return false;
                else
                {
                    if (!string.IsNullOrEmpty(pUser))
                    {
                        DATN.TTS.DATA.Properties.Settings.Default.ctr = "Data Source=" + pServerName +
                                                                        ";Initial Catalog=" +
                                                                        pDataBase + ";User ID=" + pUser + ";pwd = " +
                                                                        pPass + "";
                    }
                    else
                    {
                        DATN.TTS.DATA.Properties.Settings.Default.ctr = "Data Source=" + pServerName +
                                                                        ";Initial Catalog=" +
                                                                        pDataBase + ";Integrated Security=True;";
                    }
                    DATN.TTS.DATA.Properties.Settings.Default.Save();
                    DATN.TTS.DATA.Properties.Settings.Default.Reload();
                }
                return true;
            }
            catch (Exception ex) { return false;}
        }

        public void Load_Information(params object[] oparam)
        {
            //DataTable xdt = (DataTable)oparam[0];

            //var query = from
        }

        public DataTable Load_manhinh(string user)
        {
            var query = (from ndn in db.tbl_NDung_NhomNDungs
                join pq in db.tbl_PhanQuyens on ndn.MaNhomNguoiDung equals pq.MaNhomNguoiDung into pq_join
                from pq in pq_join.DefaultIfEmpty()
                join mh in db.tbl_ManHinhs on pq.MaManHinh equals mh.MaManHinh into mh_join
                from mh in mh_join.DefaultIfEmpty()
                where
                    ndn.UserName == user &&
                    pq.CoQuyen == 1
                select new
                {
                    MaManHinh = mh.MaManHinh,
                    TenManHinh = mh.TenManHinh
                });
            DataTable xdt = TableUtil.LinqToDataTable(query);
            return xdt;
        }

        public int UpdatePass(string account, string pass)
        {
            try
            {
                string i = "";
                tbl_NguoiDung query =
                    (from nd in db.tbl_NguoiDungs
                     where nd.UserName.Equals(account.Trim())
                     select nd).FirstOrDefault();
                query.Pass = pass;
                db.SubmitChanges();
                i = query.UserName;
                if (!string.IsNullOrEmpty(i))
                {
                    return 1;
                }
                return 0;
            }
            catch (Exception err)
            {
                throw err;
            }
        }

    }
}

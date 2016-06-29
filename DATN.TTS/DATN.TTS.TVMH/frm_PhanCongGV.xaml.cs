using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CustomMessage;
using DATN.TTS.BUS;
using DATN.TTS.BUS.Resource;
using DevExpress.Utils;
using DevExpress.Xpf.Editors.Settings;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Editors;

namespace DATN.TTS.TVMH
{
    /// <summary>
    /// Interaction logic for frm_PhanCongGV.xaml
    /// </summary>
    public partial class frm_PhanCongGV : Page
    {
        private DataTable iDataSource = null;
        private DataTable iGridDataSource = null;
        private DataTable iGridDataSource_Search = null;
        private DataTable iDataSource_MH = null;
        private ArrayList iDataChange = new ArrayList();

        public static int rowfocus = -1;
        public static int id_giaovien = 0;
        public static string ten_giaovien = "";

        bus_phanconggiaovien bus = new bus_phanconggiaovien();

        public frm_PhanCongGV()
        {
            InitializeComponent();
            iDataSource = TableChelmabinding();
            this.DataContext = this.iDataSource;
            this.iDataSource.Rows[0]["USER"] = UserCommon.UserName;
            Init_Grid();
            Load_cbo();
            //Load_data();
        }

        private DataTable TableChelmabinding()
        {
            DataTable dtaTable = null;
            try
            {
                Dictionary<string, Type> xDicUser = new Dictionary<string, Type>();
                xDicUser.Add("USER", typeof(string));
                xDicUser.Add("ID_BOMON", typeof(int));
                xDicUser.Add("MA_BM", typeof(string));
                xDicUser.Add("TEN_BM", typeof(string));
                

                xDicUser.Add("ID_HE_DAOTAO", typeof(int));
                xDicUser.Add("ID_KHOAHOC", typeof(int));
                xDicUser.Add("ID_KHOAHOC_NGANH", typeof(int));
                xDicUser.Add("HOCKY", typeof(int));
                //xDicUser.Add("ID_MONHOC", typeof(int));
                xDicUser.Add("ID_KHOAHOC_NGANH_CTIET", typeof(int));

                dtaTable = TableUtil.ConvertToTable(xDicUser);
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return dtaTable;
        }

        private void Init_Grid()
        {
            try
            {
                GridColumn xcolumn;

                xcolumn = new GridColumn();
                xcolumn.FieldName = "ID_LOPHOCPHAN";
                xcolumn.Visible = false;
                grd.Columns.Add(xcolumn);

                xcolumn = new GridColumn();
                xcolumn.FieldName = "ID_GIANGVIEN";
                xcolumn.Visible = false;
                grd.Columns.Add(xcolumn);

                xcolumn = new GridColumn() { CellTemplate = (DataTemplate)this.Resources["btn"] };
                xcolumn.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                xcolumn.Header = "Xóa GV";
                xcolumn.Width = 90;
                xcolumn.AllowEditing = DefaultBoolean.False;
                xcolumn.Visible = true;
                grd.Columns.Add(xcolumn);

                xcolumn = new GridColumn();
                xcolumn.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                xcolumn.FieldName = "TEN_GIANGVIEN";
                xcolumn.Header = "Tên giảng viên";
                xcolumn.Width = 150;
                xcolumn.AllowEditing = DefaultBoolean.False;
                xcolumn.Visible = true;
                grd.Columns.Add(xcolumn);

                xcolumn = new GridColumn();
                xcolumn.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                xcolumn.FieldName = "MA_LOP_HOCPHAN";
                xcolumn.Header = "Mã học phần";
                xcolumn.Width = 90;
                xcolumn.AllowEditing = DefaultBoolean.False;
                xcolumn.Visible = true;
                grd.Columns.Add(xcolumn);

                xcolumn = new GridColumn();
                xcolumn.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                xcolumn.FieldName = "TEN_LOP_HOCPHAN";
                xcolumn.Header = "Tên học phần";
                xcolumn.Width = 150;
                xcolumn.AllowEditing = DefaultBoolean.False;
                xcolumn.Visible = true;
                grd.Columns.Add(xcolumn);

                xcolumn = new GridColumn();
                xcolumn.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                xcolumn.FieldName = "TEN_LOP";
                xcolumn.Header = "Lớp kiểm soát";
                xcolumn.Width = 150;
                xcolumn.AllowEditing = DefaultBoolean.False;
                xcolumn.Visible = true;
                grd.Columns.Add(xcolumn);

                xcolumn = new GridColumn();
                xcolumn.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                xcolumn.FieldName = "SOTIET";
                xcolumn.Header = "Số tiết";
                xcolumn.Width = 50;
                xcolumn.AllowEditing = DefaultBoolean.False;
                xcolumn.Visible = true;
                grd.Columns.Add(xcolumn);

                xcolumn = new GridColumn();
                xcolumn.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                xcolumn.FieldName = "SOLUONG";
                xcolumn.Header = "Số sinh viên";
                xcolumn.Width = 50;
                xcolumn.AllowEditing = DefaultBoolean.False;
                xcolumn.Visible = true;
                grd.Columns.Add(xcolumn);

                xcolumn = new GridColumn();
                xcolumn.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                xcolumn.FieldName = "TUAN_BD";
                xcolumn.Header = "Tuần bắt đầu";
                xcolumn.Width = 50;
                xcolumn.AllowEditing = DefaultBoolean.False;
                xcolumn.Visible = true;
                grd.Columns.Add(xcolumn);

                xcolumn = new GridColumn();
                xcolumn.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                xcolumn.FieldName = "TUAN_KT";
                xcolumn.Header = "Tuần kết thúc";
                xcolumn.Width = 50;
                xcolumn.AllowEditing = DefaultBoolean.False;
                xcolumn.Visible = true;
                grd.Columns.Add(xcolumn);

                xcolumn = new GridColumn();
                xcolumn.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                xcolumn.FieldName = "TEN_HE_DAOTAO";
                xcolumn.Header = "Hệ đào tạo";
                xcolumn.Width = 90;
                xcolumn.AllowEditing = DefaultBoolean.False;
                xcolumn.Visible = true;
                grd.Columns.Add(xcolumn);

                xcolumn = new GridColumn();
                xcolumn.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                xcolumn.FieldName = "TEN_NGANH";
                xcolumn.Header = "Ngành";
                xcolumn.Width = 150;
                xcolumn.AllowEditing = DefaultBoolean.False;
                xcolumn.Visible = true;
                grd.Columns.Add(xcolumn);

                xcolumn = new GridColumn();
                xcolumn.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                xcolumn.FieldName = "TEN_BM";
                xcolumn.Header = "Bộ môn";
                xcolumn.Width = 150;
                xcolumn.AllowEditing = DefaultBoolean.False;
                xcolumn.Visible = true;
                grd.Columns.Add(xcolumn);

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void Load_data()
        {
            try
            {
                Mouse.OverrideCursor = Cursors.Wait;
                iGridDataSource = bus.GetAll_Hocphan();
                grd.ItemsSource = iGridDataSource;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

        private void Load_cbo()
        {
            try
            {
                Mouse.OverrideCursor = Cursors.Wait;
                bus_molophocphan bus = new bus_molophocphan();
                DataTable xdtbm = bus.GetAll_HDT();
                cboHeDT.ItemsSource = xdtbm;

                #region LOAD HOC KY
                DataTable dt_hocky = new DataTable();
                dt_hocky.Columns.Add("hocky", typeof(int));
                dt_hocky.Columns.Add("tenhocky", typeof(string));

                DataRow xdr = null;
                xdr = dt_hocky.NewRow();
                xdr["hocky"] = 0;
                xdr["tenhocky"] = "---Chọn---";
                dt_hocky.Rows.Add(xdr);
                for (int i = 1; i <= 8; i++)
                {
                    xdr = dt_hocky.NewRow();
                    xdr["hocky"] = i;
                    xdr["tenhocky"] = i.ToString();
                    dt_hocky.Rows.Add(xdr);
                }
                dt_hocky.AcceptChanges();
                cboHocKy.ItemsSource = dt_hocky;
                #endregion
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

        private void btndelete_Click(object sender, RoutedEventArgs e)
        {
            if (rowfocus != -1)
            {
                if (string.IsNullOrEmpty(iGridDataSource_Search.Rows[rowfocus]["ID_GIANGVIEN"].ToString()) ||
                    Convert.ToInt32(iGridDataSource_Search.Rows[rowfocus]["ID_GIANGVIEN"]) == 0) return;
                string gv = iGridDataSource_Search.Rows[rowfocus]["TEN_GIANGVIEN"].ToString();
                if (CTMessagebox.Show("Bạn muốn xóa giảng viên " + gv + " không ?", "Xóa giảng viên", "",
                    CTICON.Question,
                    CTBUTTON.YesNo) == CTRESPONSE.Yes)
                {
                    iGridDataSource_Search.Rows[rowfocus]["TEN_GIANGVIEN"] = "";
                    iGridDataSource_Search.Rows[rowfocus]["ID_GIANGVIEN"] = 0;
                    for (int j = 0; j < iGridDataSource.Rows.Count; j++)
                    {
                        if (Convert.ToInt32(iGridDataSource.Rows[j]["ID_LOPHOCPHAN"]) ==
                            Convert.ToInt32(iGridDataSource_Search.Rows[rowfocus]["ID_LOPHOCPHAN"]))
                        {
                            iGridDataSource.Rows[j]["ID_GIANGVIEN"] = 0;
                            iGridDataSource.Rows[j]["TEN_GIANGVIEN"] = "";
                            break;
                        }
                    }
                    int xcheck = 0;
                    foreach (object d in iDataChange)
                    {
                        int chk = Convert.ToInt32(d.ToString());
                        if (Convert.ToInt32(this.iGridDataSource_Search.Rows[rowfocus]["ID_LOPHOCPHAN"]) == chk)
                        {
                            xcheck = 1;
                        }
                    }
                    if (xcheck != 1)
                    {
                        iDataChange.Add(Convert.ToInt32(this.iGridDataSource_Search.Rows[rowfocus]["ID_LOPHOCPHAN"]));
                    }
                }
            }
        }

        private void Grd_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            int rowHandle = -1;

            if (this.grd.GetFocusedRow() == null) return;
            if (iGridDataSource_Search.Rows.Count == 0) return;
            DataRow RowSelGb = ((DataRowView)this.grd.GetFocusedRow()).Row;
            for (int i = 0; i < iGridDataSource_Search.Rows.Count; i++)
            {
                if (Convert.ToInt32(iGridDataSource_Search.Rows[i]["ID_LOPHOCPHAN"]) ==
                    Convert.ToInt32(RowSelGb["ID_LOPHOCPHAN"]))
                {
                    rowHandle = i;
                    break;
                }
            }
            if (rowHandle != -1)
            {
                frm_PhanCongGV_popup popup = new frm_PhanCongGV_popup();
                popup.ShowDialog();
                if (id_giaovien != 0)
                {
                    this.iGridDataSource_Search.Rows[rowHandle]["ID_GIANGVIEN"] = id_giaovien;
                    this.iGridDataSource_Search.Rows[rowHandle]["TEN_GIANGVIEN"] = ten_giaovien;
                    for (int j = 0; j < iGridDataSource.Rows.Count; j++)
                    {
                        if (Convert.ToInt32(iGridDataSource.Rows[j]["ID_LOPHOCPHAN"]) ==
                            Convert.ToInt32(RowSelGb["ID_LOPHOCPHAN"]))
                        {
                            iGridDataSource.Rows[j]["ID_GIANGVIEN"] = id_giaovien;
                            iGridDataSource.Rows[j]["TEN_GIANGVIEN"] = ten_giaovien;
                            break;
                        }
                    }
                }

                int xcheck = 0;
                foreach (object d in iDataChange)
                {
                    int chk = Convert.ToInt32(d.ToString());
                    if (Convert.ToInt32(this.iGridDataSource_Search.Rows[rowHandle]["ID_LOPHOCPHAN"]) == chk)
                    {
                        xcheck = 1;
                    }
                }
                if (xcheck != 1)
                {
                    iDataChange.Add(Convert.ToInt32(this.iGridDataSource_Search.Rows[rowHandle]["ID_LOPHOCPHAN"]));
                }
            }
        }

        private void BtnSave_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (this.iDataChange != null)
                {
                    int kq = 0;

                    foreach (object id in iDataChange)
                    {
                        foreach (DataRow dtr in iGridDataSource.Rows)
                        {
                            int d = Convert.ToInt32(id);
                            if (Convert.ToInt32(dtr["ID_LOPHOCPHAN"]) == d)
                            {
                                kq = bus.UpdateObject(Convert.ToInt32(dtr["ID_LOPHOCPHAN"].ToString()),
                                    Convert.ToInt32(dtr["ID_GIANGVIEN"].ToString()),
                                    iDataSource.Rows[0]["USER"].ToString());
                            }
                        }
                    }
                    if (kq != 0)
                    {
                        CTMessagebox.Show("Thành công", "Lưu", "", CTICON.Information, CTBUTTON.OK);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void GrdView_OnFocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            try
            {
                DataRow RowSelGb = null;
                if (this.grd.GetFocusedRow() == null) return;
                RowSelGb = ((DataRowView)this.grd.GetFocusedRow()).Row;
                for (int i = 0; i < iGridDataSource_Search.Rows.Count; i++)
                {
                    if (string.IsNullOrEmpty(iGridDataSource_Search.Rows[i]["ID_LOPHOCPHAN"].ToString())) continue;
                    if (Convert.ToInt32(iGridDataSource_Search.Rows[i]["ID_LOPHOCPHAN"]) ==
                        Convert.ToInt32(RowSelGb["ID_LOPHOCPHAN"]))
                    {
                        rowfocus = i;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void Btnrefresh_OnClick(object sender, RoutedEventArgs e)
        {
            Load_data();
        }

        private void CboHeDT_OnEditValueChanged(object sender, EditValueChangedEventArgs e)
        {
            try
            {
                bus_molophocphan bus = new bus_molophocphan();
                DataTable xdtbm = bus.GetAll_KhoaHoc(Convert.ToInt32(iDataSource.Rows[0]["ID_HE_DAOTAO"]));
                cboKhoa.ItemsSource = xdtbm;
                //iDataSource.Rows[0]["ID_KHOAHOC"] = xdtbm.Rows[0]["ID_KHOAHOC"];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void CboKhoa_OnEditValueChanged(object sender, EditValueChangedEventArgs e)
        {
            try
            {
                bus_molophocphan bus = new bus_molophocphan();
                DataTable xdtbm = bus.GetAll_Khoa_Nganh(Convert.ToInt32(iDataSource.Rows[0]["ID_KHOAHOC"]));
                cboNganh.ItemsSource = xdtbm;
                if (xdtbm.Rows.Count > 0)
                {
                    iDataSource.Rows[0]["ID_KHOAHOC_NGANH"] = xdtbm.Rows[0]["ID_KHOAHOC_NGANH"];
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void CboNganh_OnEditValueChanged(object sender, EditValueChangedEventArgs e)
        {
            try
            {
                bus_molophocphan lhp = new bus_molophocphan();

                #region Load mon hoc cua nganh

                iDataSource_MH = lhp.GetAll_monhoc(Convert.ToInt32(iDataSource.Rows[0]["ID_KHOAHOC_NGANH"]));
                cbomonhoc.ItemsSource = iDataSource_MH;

                #endregion

                #region Load tat ca cac lop hoc phan

                iGridDataSource = bus.GetAll_Hocphan();
                iGridDataSource_Search = iGridDataSource.Copy();
                grd.ItemsSource = iGridDataSource_Search;

                #endregion

                #region Load hoc ky hien tai cua khoa hoc

                DataTable iGridData_TTHK = lhp.GetAll_Tso_Hocky();
                DataTable tmp = lhp.Get_HK_HT(Convert.ToInt32(iDataSource.Rows[0]["ID_KHOAHOC"]));
                int hkht = 0;
                if (tmp != null && tmp.Rows.Count > 0)
                {
                    int hk = Convert.ToInt32(iGridData_TTHK.Rows[0]["HOCKY"]);
                    if (hk == 3)
                    {
                        hk = 2;
                    }
                    hkht = (Convert.ToInt32(iGridData_TTHK.Rows[0]["NAMHOC_TU"]) -
                                Convert.ToInt32(tmp.Rows[0]["NAM_BD"])) * 2 + hk;
                    iDataSource.Rows[0]["HOCKY"] = hkht;
                } 

                #endregion

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void CboHocKy_OnEditValueChanged(object sender, EditValueChangedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(iDataSource.Rows[0]["HOCKY"].ToString()))
                {
                    if (Convert.ToInt32(iDataSource.Rows[0]["HOCKY"]) == 0)
                    {
                        cbomonhoc.ItemsSource = iDataSource_MH;
                        iGridDataSource_Search = iGridDataSource.Copy();
                        grd.ItemsSource = iGridDataSource_Search;
                    }
                    else
                    {
                        #region Load mh theo hoc ky

                        DataTable xdt_mh = iDataSource_MH.Clone();
                        DataRow[] xcheck = (from x in
                                                iDataSource_MH.AsEnumerable()
                                                    .Where(
                                                        d =>
                                                            d.Field<int>("HOCKY") ==
                                                            Convert.ToInt32(iDataSource.Rows[0]["HOCKY"].ToString()))
                                            select x).ToArray();
                        if (xcheck.Count() > 0)
                        {
                            xdt_mh = xcheck.CopyToDataTable();
                            DataRow xnew = xdt_mh.NewRow();
                            xnew["ID_KHOAHOC_NGANH_CTIET"] = 0;
                            xnew["HOCKY"] = 0;
                            xnew["TEN_MONHOC"] = "---------Chọn---------";
                            xdt_mh.Rows.InsertAt(xnew, 0);
                        }
                        cbomonhoc.ItemsSource = xdt_mh;
                        iDataSource.Rows[0]["ID_KHOAHOC_NGANH_CTIET"] = 0;

                        #endregion

                        #region Load lop hoc phan theo hoc ky

                        iGridDataSource_Search = iGridDataSource.Clone();
                        if (xdt_mh.Rows.Count > 0)
                        {
                            foreach (DataRow dr in xdt_mh.Rows)
                            {
                                DataRow[] tmp = (from x in
                                                     iGridDataSource.AsEnumerable()
                                                            .Where(
                                                                d =>
                                                                    d.Field<int>("ID_KHOAHOC_NGANH_CTIET") ==
                                                                    Convert.ToInt32(dr["ID_KHOAHOC_NGANH_CTIET"].ToString()))
                                                    select x).ToArray();
                                if (tmp.Count() > 0)
                                {
                                    DataTable xtmp = tmp.CopyToDataTable();
                                    foreach (DataRow drtmp in xtmp.Rows)
                                    {
                                        iGridDataSource_Search.ImportRow(drtmp);
                                    }
                                }
                            }
                            grd.ItemsSource = iGridDataSource_Search;
                        }
                        else
                        {
                            grd.ItemsSource = iGridDataSource_Search;
                        }

                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }

        private void Cbomonhoc_OnEditValueChanged(object sender, EditValueChangedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(iDataSource.Rows[0]["ID_KHOAHOC_NGANH_CTIET"].ToString()))
                {
                    if (Convert.ToInt32(iDataSource.Rows[0]["ID_KHOAHOC_NGANH_CTIET"]) == 0)
                    {
                        CboHocKy_OnEditValueChanged(null, null);
                    }
                    else
                    {
                        DataRow[] tmp = (from x in
                                                iGridDataSource.AsEnumerable()
                                                    .Where(
                                                        d =>
                                                            d.Field<int>("ID_KHOAHOC_NGANH_CTIET") ==
                                                            Convert.ToInt32(iDataSource.Rows[0]["ID_KHOAHOC_NGANH_CTIET"].ToString()))
                                            select x).ToArray();
                        if (tmp.Count() > 0)
                        {
                            DataTable xtmp = tmp.CopyToDataTable();
                            iGridDataSource_Search = xtmp.Copy();
                        }
                        grd.ItemsSource = iGridDataSource_Search;
                    }
                }
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }

        private void BtnAuto_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                DataTable xdt = bus.GetAll_GV();
                DataTable ds_gv = xdt.Copy();
                foreach (DataRow dr in iGridDataSource_Search.Rows)
                {
                    if (string.IsNullOrEmpty(dr["ID_GIANGVIEN"].ToString()) ||
                        Convert.ToInt32(dr["ID_GIANGVIEN"]) == 0)
                    {
                        Random rd = new Random();
                        int i = rd.Next(0, ds_gv.Rows.Count - 1);
                        dr["ID_GIANGVIEN"] = ds_gv.Rows[i]["ID_GIANGVIEN"];
                        dr["TEN_GIANGVIEN"] = ds_gv.Rows[i]["TEN_GIANGVIEN"];
                        for (int j = 0; j < iGridDataSource.Rows.Count; j++)
                        {
                            if (Convert.ToInt32(iGridDataSource.Rows[j]["ID_LOPHOCPHAN"]) ==
                                Convert.ToInt32(dr["ID_LOPHOCPHAN"]))
                            {
                                iGridDataSource.Rows[j]["ID_GIANGVIEN"] = ds_gv.Rows[i]["ID_GIANGVIEN"];
                                iGridDataSource.Rows[j]["TEN_GIANGVIEN"] = ds_gv.Rows[i]["TEN_GIANGVIEN"];
                                int tmp = 0;
                                foreach (object d in iDataChange)
                                {
                                    int chk = Convert.ToInt32(d.ToString());
                                    if (Convert.ToInt32(this.iGridDataSource.Rows[j]["ID_LOPHOCPHAN"]) == chk)
                                    {
                                        tmp = 1;
                                        break;
                                    }
                                }
                                if (tmp != 1)
                                {
                                    iDataChange.Add(Convert.ToInt32(this.iGridDataSource.Rows[j]["ID_LOPHOCPHAN"]));
                                }
                                break;
                            }
                        }
                        ds_gv.Rows.RemoveAt(i);
                        if (ds_gv.Rows.Count < 1)
                        {
                            ds_gv = xdt.Copy();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }
    }
}

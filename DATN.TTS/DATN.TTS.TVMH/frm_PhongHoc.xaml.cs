using System;
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
using DevExpress.Data;
using DevExpress.Utils;
using DevExpress.Xpf.Core.HandleDecorator;
using DevExpress.Xpf.Editors.Settings;
using DevExpress.Xpf.Grid;

namespace DATN.TTS.TVMH
{
    /// <summary>
    /// Interaction logic for frm_PhongHoc.xaml
    /// </summary>
    public partial class frm_PhongHoc : Page
    {
        bus_PhongHoc client = new bus_PhongHoc();
        private DataTable iDataSoure = null;
        private DataTable iGridDataSoure = null;
        private bool flagsave = true;
        public frm_PhongHoc()
        {
            InitializeComponent();
            iDataSoure = TableSchemabinding();
            this.DataContext = iDataSoure;
            this.iDataSoure.Rows[0]["USER"] = UserCommon.UserName;
            InitGrid();

            txtDay.ItemsSource = ItemsSoure();
        }

        private void GetGrid()
        {
            this.iGridDataSoure = client.GetAll();
            grd.ItemsSource = iGridDataSoure;
        }

        private DataTable TableSchemabinding()
        {
            try
            {
                DataTable dt = null;
                Dictionary<string, Type> dic = new Dictionary<string, Type>();
                dic.Add("ID_PHONG", typeof(Decimal));
                dic.Add("MA_PHONG", typeof(string));
                dic.Add("TEN_PHONG", typeof(string));
                dic.Add("SUCCHUA", typeof(Decimal));
                dic.Add("GHICHU", typeof(string));
                dic.Add("LOAIPHONG", typeof(string));
                dic.Add("IS_MAYCHIEU", typeof(Decimal));
                dic.Add("IS_MAYTINH", typeof(Decimal));
                dic.Add("DAY", typeof(string));
                dic.Add("TANG", typeof(string));
                dic.Add("TINHTRANG", typeof(string));
                dic.Add("USER", typeof(string));
                dt = TableUtil.ConvertToTable(dic);
                return dt;
            }
            catch (Exception ex)
            {
                CTMessagebox.Show("Lỗi", "Lỗi", ex.Message, CTICON.Error, CTBUTTON.OK);
            }
            return null;
        }

        private void InitGrid()
        {
            try
            {
                GridColumn col = null;

                col = new GridColumn();
                col.FieldName = "ID_PHONG";
                col.Header = string.Empty;
                col.Visible = false;
                grd.Columns.Add(col);

                col = new GridColumn();
                col.FieldName = "MA_PHONG";
                col.Header = "Mã phòng";
                col.AllowEditing = DefaultBoolean.False;
                col.Visible = true;
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                col.EditSettings = new TextEditSettings();
                col.EditSettings.HorizontalContentAlignment = DevExpress.Xpf.Editors.Settings.EditSettingsHorizontalAlignment.Center;
                grd.Columns.Add(col);

                col = new GridColumn();
                col.FieldName = "TEN_PHONG";
                col.Header = "Phòng";
                col.Visible = true;
                col.AllowEditing = DefaultBoolean.False;
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                grd.Columns.Add(col);

                col = new GridColumn();
                col.FieldName = "LOAIPHONG";
                col.Header = "Loại phòng";
                col.Visible = true;
                col.AllowEditing = DefaultBoolean.False;
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                col.EditSettings = new TextEditSettings();
                col.EditSettings.HorizontalContentAlignment = DevExpress.Xpf.Editors.Settings.EditSettingsHorizontalAlignment.Center;
                grd.Columns.Add(col);

                col = new GridColumn();
                col.FieldName = "SUCCHUA";
                col.Header = "Sức chứa";
                col.Visible = true;
                col.AllowEditing = DefaultBoolean.False;
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                col.EditSettings = new TextEditSettings();
                col.EditSettings.HorizontalContentAlignment = DevExpress.Xpf.Editors.Settings.EditSettingsHorizontalAlignment.Center;
                grd.Columns.Add(col);

                col = new GridColumn();
                col.FieldName = "DAY";
                col.Header = "Dãy";
                col.Visible = true;
                col.AllowEditing = DefaultBoolean.False;
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                col.EditSettings = new TextEditSettings();
                col.EditSettings.HorizontalContentAlignment = DevExpress.Xpf.Editors.Settings.EditSettingsHorizontalAlignment.Center;
                grd.Columns.Add(col);

                col = new GridColumn();
                col.FieldName = "TANG";
                col.Header = "Tầng";
                col.Visible = true;
                col.AllowEditing = DefaultBoolean.False;
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                col.EditSettings = new TextEditSettings();
                col.EditSettings.HorizontalContentAlignment = DevExpress.Xpf.Editors.Settings.EditSettingsHorizontalAlignment.Center;
                grd.Columns.Add(col);

                col = new GridColumn();
                col.FieldName = "IS_MAYTINH";
                col.Header = string.Empty;
                col.Visible = false;
                grd.Columns.Add(col);

                col = new GridColumn();
                col.FieldName = "IS_MAYCHIEU";
                col.Header = string.Empty;
                col.Visible = false;
                grd.Columns.Add(col);

                col = new GridColumn();
                col.FieldName = "MAYCHIEU";
                col.Header = "Máy chiếu";
                col.Visible = true;
                col.EditSettings = new CheckEditSettings();
                col.AutoFilterValue = true;
                col.UnboundType = UnboundColumnType.Integer;
                col.AllowEditing = DefaultBoolean.False;
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                grd.Columns.Add(col);

                col = new GridColumn();
                col.FieldName = "MAYTINH";
                col.Header = "Máy Tính";
                col.Visible = true;
                col.EditSettings = new CheckEditSettings();
                col.AllowEditing = DefaultBoolean.False;
                col.AutoFilterValue = true;
                col.UnboundType = UnboundColumnType.Integer;
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                grd.Columns.Add(col);

                col = new GridColumn();
                col.FieldName = "HIENTRANG";
                col.Header = "Hiện trạng";
                col.Visible = true;
                col.AllowEditing = DefaultBoolean.False;
                col.HorizontalHeaderContentAlignment = HorizontalAlignment.Center;
                grd.Columns.Add(col);

                col = new GridColumn();
                col.FieldName = "GHICHU";
                col.Header = string.Empty;
                col.Visible = false;
                grd.Columns.Add(col);

                GetGrid();

            }
            catch (Exception ex)
            {
                CTMessagebox.Show("Lỗi", "Lỗi", ex.Message, CTICON.Error, CTBUTTON.OK);
            }
        }

        void ValiCheck()
        {
            if (rdLT.IsChecked == true)
            {
                this.iDataSoure.Rows[0]["LOAIPHONG"] = "Lý Thuyết";
            }
            if (rdTH.IsChecked == true)
            {
                this.iDataSoure.Rows[0]["LOAIPHONG"] = "Thực hành";
            }
            if (chkTinhTrang.IsChecked == true)
            {
                this.iDataSoure.Rows[0]["TINHTRANG"] = "1";
            }
            else
            {
                this.iDataSoure.Rows[0]["TINHTRANG"] = "0";
            }

            if (chkMayTinh.IsChecked == true)
            {
                this.iDataSoure.Rows[0]["IS_MAYTINH"] = "1";
            }
            else
            {
                this.iDataSoure.Rows[0]["IS_MAYTINH"] = "0";
            }

            if (chkMayChieu.IsChecked == true)
            {
                this.iDataSoure.Rows[0]["IS_MAYCHIEU"] = "1";
            }
            else
            {
                this.iDataSoure.Rows[0]["IS_MAYCHIEU"] = "0";
            }
        }

        void CheckRadio()
        {
            if (this.iDataSoure.Rows[0]["LOAIPHONG"].ToString().Contains("Lý Thuyết"))
            {
                rdLT.IsChecked = true;
            }
            if (this.iDataSoure.Rows[0]["LOAIPHONG"].ToString().Contains("Thực hành"))
            {
                rdTH.IsChecked = true;
            }
        }

        bool ValiDate()
        {
            try
            {
                if (this.iDataSoure.Rows[0]["MA_PHONG"].ToString() == string.Empty)
                {
                    CTMessagebox.Show("Vui lòng nhập mã phòng.", "Thông báo", "", CTICON.Error, CTBUTTON.OK);
                    txtMaPhong.Focus();
                    return false;
                }
                if (this.iDataSoure.Rows[0]["TEN_PHONG"].ToString() == string.Empty)
                {
                    CTMessagebox.Show("Vui lòng nhập tên phòng.", "Thông báo", "", CTICON.Error, CTBUTTON.OK);
                    txtTenPhong.Focus();
                    return false;
                }
                if (this.iDataSoure.Rows[0]["SUCCHUA"].ToString() == "0")
                {
                    CTMessagebox.Show("Vui lòng nhập sức chứa phòng.", "Thông báo", "", CTICON.Error, CTBUTTON.OK);
                    txtSucChua.Focus();
                    return false;
                }
                if (this.iDataSoure.Rows[0]["LOAIPHONG"].ToString() == string.Empty)
                {
                    CTMessagebox.Show("Vui lòng chọn loại phòng", "Thông báo", "", CTICON.Error, CTBUTTON.OK);
                    rdLT.Focus();
                    return false;
                }
                if (this.iDataSoure.Rows[0]["DAY"].ToString() == string.Empty)
                {
                    CTMessagebox.Show("Vui lòng chọn dãy phòng.", "Thông báo", "", CTICON.Error, CTBUTTON.OK);
                    txtDay.Focus();
                    return false;
                }
                if (this.iDataSoure.Rows[0]["TANG"].ToString() == string.Empty)
                {
                    CTMessagebox.Show("Vui lòng nhập tầng của phòng.", "Thông báo", "", CTICON.Error, CTBUTTON.OK);
                    txtTang.Focus();
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                CTMessagebox.Show("Lỗi", "Lỗi", ex.Message, CTICON.Error, CTBUTTON.OK);
            }
            return false;
        }

        void SetIsNull()
        {
            this.iDataSoure.Rows[0]["MA_PHONG"] = string.Empty;
            this.iDataSoure.Rows[0]["TEN_PHONG"] = string.Empty;
            this.iDataSoure.Rows[0]["SUCCHUA"] = "0";
            this.iDataSoure.Rows[0]["GHICHU"] = string.Empty;
            this.iDataSoure.Rows[0]["LOAIPHONG"] = string.Empty;
            this.iDataSoure.Rows[0]["IS_MAYCHIEU"] = "0";
            this.iDataSoure.Rows[0]["IS_MAYTINH"] = "0";
            this.iDataSoure.Rows[0]["DAY"] = string.Empty;
            this.iDataSoure.Rows[0]["TANG"] = string.Empty;
            this.iDataSoure.Rows[0]["TINHTRANG"] = "0";
        }

        List<string> ItemsSoure()
        {
            List<string> lst = new List<string>();
            lst.Add("A");
            lst.Add("B");
            lst.Add("C");
            lst.Add("D");
            lst.Add("E");
            lst.Add("F");
            lst.Add("G");
            lst.Add("H");
            lst.Add("K");

            return lst;
        }

        private void btnAddNew_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Mouse.OverrideCursor = Cursors.Wait;
                GetGrid();
                SetIsNull();
                flagsave = true;
                txtMaPhong.Focus();

            }
            catch (Exception ex)
            {
                CTMessagebox.Show("Lỗi", "Lỗi", ex.Message, CTICON.Error, CTBUTTON.OK);
            }
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

        private void btnSave_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Mouse.OverrideCursor = Cursors.Wait;
                ValiCheck();
                if (ValiDate())
                {
                    if (flagsave)
                    {
                        bool res = client.Insert_Phong(this.iDataSoure.Copy());
                        if (!res)
                        {
                            CTMessagebox.Show("Thêm mới không thành công", "Thêm mới","", CTICON.Error, CTBUTTON.OK);
                        }
                        else
                        {
                            CTMessagebox.Show("Thêm mới thành công", "Thêm mới", "", CTICON.Information, CTBUTTON.OK);
                            GetGrid();
                            SetIsNull();
                            txtDay.Focus();
                        }
                      
                    }
                    else
                    {
                        client.Update_Phong(this.iDataSoure.Copy());
                        GetGrid();
                        SetIsNull();
                        txtDay.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                CTMessagebox.Show("Lỗi", "Lỗi", ex.Message, CTICON.Error, CTBUTTON.OK);
            }
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

        private void btnDelete_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Mouse.OverrideCursor = Cursors.Wait;
                if (CTMessagebox.Show("Bạn có muốn xóa?", "Xóa", "", CTICON.Information, CTBUTTON.YesNo) == CTRESPONSE.Yes)
                {
                    client.Delete_Phong(this.iDataSoure.Copy());
                    GetGrid();
                    SetIsNull();
                    txtDay.Focus();
                }
            }
            catch (Exception ex)
            {
                CTMessagebox.Show("Lỗi", "Lỗi", ex.Message, CTICON.Error, CTBUTTON.OK);
            }
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

        private void btnRefresh_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Mouse.OverrideCursor = Cursors.Wait;
                GetGrid();
                SetIsNull();
                txtMaPhong.Focus();
                flagsave = true;
            }
            catch (Exception ex)
            {
                CTMessagebox.Show("Lỗi", "Lỗi", ex.Message, CTICON.Error, CTBUTTON.OK);
            }
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

        private void GrdViewNDung_OnFocusedRowChanged(object sender, DevExpress.Xpf.Grid.FocusedRowChangedEventArgs e)
        {
            try
            {
                Mouse.OverrideCursor = Cursors.Wait;

                DataRow row = null;
                if (this.grd.GetFocusedRow() == null)
                    return;
                row = ((DataRowView)this.grd.GetFocusedRow()).Row;
                this.iDataSoure.Rows[0]["ID_PHONG"] = row["ID_PHONG"];
                this.iDataSoure.Rows[0]["MA_PHONG"] = row["MA_PHONG"];
                this.iDataSoure.Rows[0]["TEN_PHONG"] = row["TEN_PHONG"];
                this.iDataSoure.Rows[0]["SUCCHUA"] = row["SUCCHUA"];
                this.iDataSoure.Rows[0]["GHICHU"] = row["GHICHU"];
                this.iDataSoure.Rows[0]["LOAIPHONG"] = row["LOAIPHONG"];
                this.iDataSoure.Rows[0]["IS_MAYCHIEU"] = row["IS_MAYCHIEU"];
                this.iDataSoure.Rows[0]["IS_MAYTINH"] = row["IS_MAYTINH"];
                this.iDataSoure.Rows[0]["DAY"] = row["DAY"];
                this.iDataSoure.Rows[0]["TANG"] = row["TANG"];
                this.iDataSoure.Rows[0]["TINHTRANG"] = row["TINHTRANG"];

                CheckRadio();
                flagsave = false;
            }
            catch (Exception ex)
            {
                CTMessagebox.Show("Lỗi", "Lỗi", ex.Message, CTICON.Error, CTBUTTON.OK);
            }
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }
    }
}
